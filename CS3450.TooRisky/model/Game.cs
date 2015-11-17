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
        public Dictionary<string, Player> players = new Dictionary<string, Player>();

        /// <summary>
        /// A dictionary of all of the countries in the game by name.
        /// </summary>
        public Dictionary<string, Country> countries = new Dictionary<string, Country>();

        /// <summary>
        /// A dictionary of all of the continents in the game by name.
        /// </summary>
        public Dictionary<string, Continent> continents = new Dictionary<string, Continent>();

        public Game()
        {

        }
    }
}
