using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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
        public string Name;

        public PlayerNumber PlayerNumber;

        /// <summary>
        /// The color that represents this player.
        /// </summary>
        public uint Color;

        /// <summary>
        /// The number of units the player can still place this turn.
        /// This will be decremented when a unit is placed and refilled at the beginning of a turn.
        /// </summary>
        public int UnitsToPlace;

        /// <summary>
        /// The number of units the player can still move this turn.
        /// This will be decremented when a unit is placed and refilled at the beginning of a turn.
        /// </summary>
        public int UnitsToMove;

        public bool IsActive = true;
             

        public void  SetReinforcments()
        {
            UnitsToPlace = Math.Max(CountriesOwned.Count / 3, 3);
            foreach(Continent continent in ContinentsOwned)
            {
                UnitsToPlace += continent.Worth;
            }
            UnitsToMove = Constants.UnitsToMove;
        }
        /// <summary>
        /// Returns a list of the countries that this player owns.
        /// </summary>
        /// <returns>A list of the countries that this player owns.</returns>
        public List<Country> CountriesOwned
        {
            get
            {
                List<Country> countries = new List<Country>();
                foreach (Country country in Game.Instance.Countries.Values)
                {
                    if (country.OwnedBy == PlayerNumber)
                    {
                        countries.Add(country);
                    }
                }
                return countries;
            }
        }

        /// <summary>
        /// Returns a list of the continents that this player owns.
        /// </summary>
        /// <returns>A list of the continents that this player owns.</returns>
        public List<Continent> ContinentsOwned
        {
            get
            {
                List<Continent> continents = new List<Continent>();
                foreach (Continent continent in Game.Instance.Continents.Values)
                {
                    if (continent.OwnedByName(Game.Instance, PlayerNumber))
                    {
                        continents.Add(continent);
                    }
                }
                return continents;
            }
        }

        /// <summary>
        /// Returns total num of units owned by player.
        /// </summary>
        public int TotalUnits
        {
            get
            {
                var ct = (from a in CountriesOwned select a.Units).Sum();
                return ct;
            }
        }

    }
}