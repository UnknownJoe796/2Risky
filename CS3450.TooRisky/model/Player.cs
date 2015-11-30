using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// Represents one player in the game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The name of this player.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The color that represents this player.
        /// </summary>
        public uint Color = 0xFF808080;

        /// <summary>
        /// The IP address of this player.
        /// </summary>
        public string IpAddress = "";

        /// <summary>
        /// The number of units the player can still place this turn.
        /// This will be decremented when a unit is placed and refilled at the beginning of a turn.
        /// </summary>
        public int UnitsToPlace = 0;

        /// <summary>
        /// The number of units the player can still move this turn.
        /// This will be decremented when a unit is placed and refilled at the beginning of a turn.
        /// </summary>
        public int UnitsToMove = 0;

        /// <summary>
        /// Returns a list of the countries that this player owns.
        /// </summary>
        /// <param name="game">The game that this player belongs to.</param>
        /// <returns>A list of the countries that this player owns.</returns>
        public List<Country> CountriesOwned(Game game)
        {
            List<Country> countries = new List<Country>();
            foreach (Country country in game.Countries.Values)
            {
                if(country.OwnedByName == Name)
                {
                    countries.Add(country);
                }
            }
            return countries;
        }

        /// <summary>
        /// Returns a list of the continents that this player owns.
        /// </summary>
        /// <param name="game">The game that this player belongs to.</param>
        /// <returns>A list of the continents that this player owns.</returns>
        public List<Continent> ContinentsOwned(Game game)
        {
            List<Continent> continents = new List<Continent>();
            foreach (Continent continent in game.Continents.Values)
            {
                if (continent.OwnedByName(game, Name))
                {
                    continents.Add(continent);
                }
            }
            return continents;
        }

    }
}
