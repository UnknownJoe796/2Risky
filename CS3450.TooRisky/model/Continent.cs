﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// Represents a continent in the game.
    /// </summary>
    public class Continent
    {
        /// <summary>
        /// The name of the continent.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The number of units a player gains every turn for owning this continent.
        /// </summary>
        public int Worth = 0;

        /// <summary>
        /// A list of the names of the countries that are part of this continent.
        /// </summary>
        public List<string> CountryNames = new List<string>();

        /// <summary>
        /// Retrieves a list of the countries that are a part of this continent.
        /// </summary>
        /// <param name="game">The game that this continent is a part of.</param>
        /// <returns>A list of the countries that are a part of this continent.</returns>
        public List<Country> Countries(Game game)
        {
            List<Country> countries = new List<Country>();
            foreach (string name in CountryNames)
            {
                countries.Add(game.Countries[name]);
            }
            return countries;
        }

        /// <summary>
        /// Returns whether or not the given player owns this continent.
        /// </summary>
        /// <param name="game">The game that this continent is a part of.</param>
        /// <param name="playerName">The name of the player who may own this country.</param>
        /// <returns>Whether or not the given player owns this continent.</returns>
        public bool OwnedByName(Game game, PlayerNumber playerName)
        {
            foreach(Country country in Countries(game))
            {
                if(country.OwnedBy != playerName)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
