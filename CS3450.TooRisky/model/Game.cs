using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// Represents the entire game state.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The name of the current player.
        /// </summary>
        public string CurrentPlayerName = "";

        /// <summary>
        /// The phase in the current player's turn.
        /// </summary>
        public int TurnPhase = 0;

        /// <summary>
        /// A dictionary of all of the players in the game by name.
        /// </summary>
        public Dictionary<string, Player> Players = new Dictionary<string, Player>();

        /// <summary>
        /// A dictionary of all of the countries in the game by name.
        /// </summary>
        public Dictionary<string, Country> Countries = new Dictionary<string, Country>();

        /// <summary>
        /// A dictionary of all of the continents in the game by name.
        /// </summary>
        public Dictionary<string, Continent> Continents = new Dictionary<string, Continent>();

        /// <summary>
        /// Adds a player to this game.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            Players[player.Name] = player;
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

            var randomizedPlayers = new List<Player>(Players.Values.OrderBy(a => random.Next()));
            var randomizedCountries = Countries.Values.OrderBy(a => random.Next());
            var playerIndex = 0;
            foreach(var country in randomizedCountries)
            {
                country.OwnedByName = randomizedPlayers[playerIndex % randomizedPlayers.Count].Name;
                playerIndex++;
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

        public Game()
        {

        }
    }
}
