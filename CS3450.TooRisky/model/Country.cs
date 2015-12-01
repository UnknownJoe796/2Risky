using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
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
        public int Units = 1;

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
        /// Flag indicating if the country was taken over, in which case the player can move as many units as they want into this country during this turn.
        /// This flag should be reset to false at the end of every turn.
        /// </summary>
        public bool JustTakenOver = false;

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
                countries.Add(game.Countries[name]);
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
            return game.Players[OwnedByName];
        }

        /// <summary>
        /// Returns all of the valid attacks that can be made from this country.
        /// </summary>
        /// <param name="game">The game that this country belongs to.</param>
        /// <returns>A list of the legal attacks that can be made from this country.</returns>
        public List<Attack> Attacks(Game game)
        {
            var actions = new List<Attack>();

            foreach(var country in AdjacentCountries(game))
            {
                if(country.OwnedByName != OwnedByName)
                {
                    var attack = new Attack()
                    {
                        PlayerName = OwnedByName,
                        FromName = Name,
                        ToName = country.Name
                    };
                    if (attack.IsValid(game))
                    {
                        actions.Add(attack);
                    }
                }
            }

            return actions;
        }

        /// <summary>
        /// Returns all of the valid moves that can be made from this country.
        /// </summary>
        /// <param name="game">The game that this country belongs to.</param>
        /// <returns>A list of the legal moves that can be made from this country.</returns>
        public List<Move> Moves(Game game)
        {
            var actions = new List<Move>();

            foreach (var country in AdjacentCountries(game))
            {
                if (country.OwnedByName == OwnedByName)
                {
                    var move = new Move()
                    {
                        PlayerName = OwnedByName,
                        FromName = Name,
                        ToName = country.Name
                    };
                    if (move.IsValid(game))
                    {
                        actions.Add(move);
                    }
                }
            }

            return actions;
        }
    }
}
