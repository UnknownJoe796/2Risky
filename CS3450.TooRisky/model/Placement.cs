using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.model
{
    /// <summary>
    /// An action that a player can take, whether it is a move or an attack.
    /// </summary>
    public class Placement
    {
        /// <summary>
        /// The name of the player that could make the move.
        /// </summary>
        public string PlayerName ="";

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
            Player player = Player(game);
            if (to.OwnedByName == player.Name) return false;
            if (player.UnitsToPlace <= 0) return false;
            player.UnitsToPlace--;
            to.Units++;
            return true;
        }
    }
}
