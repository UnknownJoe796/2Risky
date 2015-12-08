using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS3450.TooRisky.Utils;

namespace CS3450.TooRisky.Model
{

    public enum PlayerNumber { None, P1, P2, P3, P4, P5, P6 }
    public enum TurnPhase { Placement, Attack, Move}
    /// <summary>
    /// Represents the entire game state.
    /// </summary>
    public class Game
    {
        #region singleton magic
        //possibly move this to another class to preserve serializationability

        public static Game Instance { get; set; }

        public static Game DisposeAndAcreateNewGame()
        {
            Instance = new Game();
            return Instance;
        }

        #endregion

        /// <summary>
        /// The name of the current player.
        /// </summary>
        public PlayerNumber CurrentPlayerNumber = PlayerNumber.P1;

        /// <summary>
        /// The phase in the current player's turn.
        /// </summary>
        public TurnPhase CurrentPhase = TurnPhase.Placement;


        /// <summary>
        /// A dictionary of adll of the players in the game by name.
        /// </summary>
        public Dictionary<PlayerNumber, Player> Players = new Dictionary<PlayerNumber, Player>();

        /// <summary>
        /// A dictionary of all of the countries in the game by name.
        /// </summary>
        public Dictionary<string, Country> Countries = new Dictionary<string, Country>();

        /// <summary>
        /// A dictionary of all of the continents in the game by name.
        /// </summary>
        public Dictionary<string, Continent> Continents = new Dictionary<string, Continent>();

        /// <summary>
        /// Public constructor
        /// </summary>
        private Game()
        {
            
        }

        /// <summary>
        /// Adds a player to this game.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            GameLog.AddEvent(player.Name + " has joined the battle!");
            var ct = Players.Count +  1;
            player.PlayerNumber = (PlayerNumber) ct;
            Players[(PlayerNumber)ct] = player;
        }

        /// <summary>
        /// Adds a country to this game.
        /// </summary>
        /// <param name="country"></param>
        public void AddCountry(Country country)
        {
            Countries[country.Name] = country;
        }

        /// <summary>
        /// Adds a continent to this game.
        /// </summary>
        /// <param name="continent"></param>
        public void AddContinent(Continent continent)
        {
            Continents[continent.Name] = continent;
        }

        /// <summary>
        /// Randomly assigns all of the countries to different players.
        /// </summary>
        public void RandomlyAssignCountries(Random random = null)
        {
            if (random == null) random = new Random();

            foreach (var country in Countries)
            {
                country.Value.OwnedBy = (PlayerNumber) random.Next(1, Instance.Players.Count + 1);
                country.Value.Units = Constants.InitialNumOfUnits;
            }
        }

        /// <summary>
        /// This must be called at the end of every player turn.
        /// </summary>
        public void CleanUpAfterTurn()
        {
            foreach(var country in Countries.Values)
            {
                country.JustTakenOver = false;
            }
        }
        

        /// <summary>
        /// Checks for win conditions. The UI code asks this every game iteration.
        /// </summary>
        /// <returns></returns>
        public bool GameOver()
        {
            var activePlayers = 0;
            foreach (KeyValuePair<PlayerNumber, Player> entry in Players)
            {
                if(entry.Value.IsActive)
                {
                    activePlayers++;
                }
                
            }
            return activePlayers == 1;
        }

        /// <summary>
        /// Ends current turn phase for current player.
        /// If it's ending move phase (last phase), it goes to next player's placement phase.
        /// </summary>
        public void EndCurrentPhase()
        {
            if (CurrentPhase == TurnPhase.Placement)
            {
                Constants.CurrentHintText = Constants.AttackHint;
                CurrentPhase = TurnPhase.Attack;
                GameLog.AddEvent(Players[CurrentPlayerNumber].Name+" has finished placement and initiated attack phase.");
            }
            else if (CurrentPhase == TurnPhase.Attack)
            {
                CurrentPhase = TurnPhase.Move;
                Constants.CurrentHintText = Constants.MoveHint;
                GameLog.AddEvent(Players[CurrentPlayerNumber].Name + " has finished attacking and initiated move phase.");
            }
            else if (CurrentPhase == TurnPhase.Move)
            {
                Constants.CurrentHintText = Constants.PlacementHint;

                GameLog.AddEvent(Players[CurrentPlayerNumber].Name + " has finished their turn.");
                EndCurrentPlayerTurn();
                CurrentPhase = TurnPhase.Placement;
                
            }
        }

        /// <summary>
        /// Handles forfeiting of the current player. All of forfeiting player's countries will be assigned to None.
        /// </summary>
        public void ForfeitCurrentPlayer()
        {
            GameLog.AddEvent(Players[CurrentPlayerNumber].Name + " has given up.");
            Players[CurrentPlayerNumber].IsActive = false;
            foreach (KeyValuePair<string, Country> entry in Countries.Where(entry => entry.Value.OwnedBy == CurrentPlayerNumber))
            {
                entry.Value.OwnedBy = PlayerNumber.None;
            }
            CurrentPhase = TurnPhase.Placement;
            EndCurrentPlayerTurn();

        }

        /// <summary>
        /// Ends current player's turn and transitions to the next player.
        /// </summary>
        public void EndCurrentPlayerTurn()
        {
            if (CurrentPlayerNumber == PlayerNumber.P1)
            {
                CurrentPlayerNumber = PlayerNumber.P2;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            else if (CurrentPlayerNumber == PlayerNumber.P2 && Players.Count > 2)
            {

                CurrentPlayerNumber = PlayerNumber.P3;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            else if (CurrentPlayerNumber == PlayerNumber.P3 && Players.Count > 3)
            {
                CurrentPlayerNumber = PlayerNumber.P4;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            else if (CurrentPlayerNumber == PlayerNumber.P4 && Players.Count > 4)
            {
                CurrentPlayerNumber = PlayerNumber.P5;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            else if (CurrentPlayerNumber == PlayerNumber.P5 && Players.Count > 5)
            {
                CurrentPlayerNumber = PlayerNumber.P6;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            else
            {
                CurrentPlayerNumber = PlayerNumber.P1;
                if (!Players[CurrentPlayerNumber].IsActive)
                {
                    EndCurrentPlayerTurn();
                    return;
                }
            }
            Players[CurrentPlayerNumber].SetReinforcments();
            GameLog.AddEvent(Players[CurrentPlayerNumber].Name + " has begun their turn with " +
                Players[CurrentPlayerNumber].UnitsToPlace.ToString() + " reinforcements");
        }
    }
}
