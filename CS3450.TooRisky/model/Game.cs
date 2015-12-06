using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{

    public enum PlayerNumber { None, P1, P2, P3, P4, P5, P6 }
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
        public PlayerNumber CurrentPlayerNumber = PlayerNumber.None;

        /// <summary>
        /// The phase in the current player's turn.
        /// </summary>
        public int TurnPhase = 0;

        /// <summary>
        /// A dictionary of all of the players in the game by name.
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

/*            var randomizedPlayers = new List<Player>(Players.Values.OrderBy(a => random.Next()));
            var randomizedCountries = Countries.Values.OrderBy(a => random.Next());
            var playerIndex = 0;
            foreach(var country in randomizedCountries)
            {
                country.OwnedByName = randomizedPlayers[playerIndex % randomizedPlayers.Count].Name;
                playerIndex++;
            }*/
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


    }
}
