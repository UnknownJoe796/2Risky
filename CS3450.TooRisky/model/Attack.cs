using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.model
{
    /// <summary>
    /// An attack that a player could make.
    /// </summary>
    public class Attack
    {
        /// <summary>
        /// The name of the player that could make the move.
        /// </summary>
        public string PlayerName ="";

        /// <summary>
        /// The name of the country that this action originates from.
        /// If this field is empty, the move is a placement.
        /// </summary>
        public string FromName = "";

        /// <summary>
        /// The name of the country that this action is targeting.
        /// </summary>
        public string ToName = "";

        /// <summary>
        /// The player that could make this action.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The player that could make this move.</returns>
        public Player Player(Game game)
        {
            return game.players[PlayerName];
        }

        /// <summary>
        /// The country that this action originates from.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this attack originates from.</returns>
        public Country From(Game game)
        {
            return game.countries[FromName];
        }

        /// <summary>
        /// The country that this action is targeting.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this action is targeting.</returns>
        public Country To(Game game)
        {
            return game.countries[ToName];
        }

        /// <summary>
        /// Executes the given move.  This should only be called on the server.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>Whether the move is valid and was executed.</returns>
        public bool Execute(Game game)
        {
            Country to = To(game);
            Country from = From(game);
            Player player = Player(game);

            if (from.OwnedByName != player.Name) return false;
            if (to.OwnedByName == player.Name) return false;

            Random random = new Random();
            //50% chance
            if (random.Next(0, 1) == 1)
            {
                //Successful attack
                from.Units--;
            }
            else
            {
                //Unsuccessful attack
                to.Units--;
            }

            return true;
        }
    }
}
