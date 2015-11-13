using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.model
{
    /// <summary>
    /// Represents a country in the game.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// The name of this country.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// A list of the names of the adjacent countries.
        /// </summary>
        public List<string> AdjacentCountryNames = new List<string>();

        /// <summary>
        /// The number of the units on this country.
        /// </summary>
        public int Units = 0;

        /// <summary>
        /// The name of the player that owns this country.
        /// </summary>
        public string OwnedByName = "";

        /// <summary>
        /// The graphical X coordinate of the center of this country.
        /// </summary>
        public int X = 0;

        /// <summary>
        /// The graphical Y coordinate of the center of this country.
        /// </summary>
        public int Y = 0;

        /// <summary>
        /// Retrieves a list of countries adjacent to this one.
        /// </summary>
        /// <param name="game">The game that this country is a part of.</param>
        /// <returns>A list of countries adjacent to this one.</returns>
        public List<Country> AdjacentCountries(Game game)
        {
            List<Country> countries = new List<Country>();
            foreach(string name in AdjacentCountryNames)
            {
                countries.Add(game.countries[name]);
            }
            return countries;
        }

        /// <summary>
        /// Retrieves the player object that owns this country.
        /// </summary>
        /// <param name="game">The game that this country is a part of.</param>
        /// <returns>The player that owns this country.</returns>
        public Player OwnedBy(Game game)
        {
            return game.players[OwnedByName];
        }
    }
}
