using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS3450.TooRisky.Utils;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// An action that a player can take, whether it is a move or an attack.
    /// </summary>
    public class Placement : IAction
    {
        /// <summary>
        /// The name of the player that could make the move.
        /// </summary>
        public PlayerNumber PlayerNumber = PlayerNumber.None;

        /// <summary>
        /// The name of the country that this action is targeting.
        /// </summary>
        public string ToName = "";

        /// <summary>
        /// The player that could make this action.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The player that could make this move.</returns>
        public Player GetPlayer()
        {
            return Game.Instance.Players[PlayerNumber];
        }

        /// <summary>
        /// The country that this action originates from, in this case null.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>null</returns>
        public Country GetFrom()
        {
            return null;
        }

        /// <summary>
        /// The country that this action is targeting.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this action is targeting.</returns>
        public Country GetTo()
        {
            return Game.Instance.Countries[ToName];
        }

        /// <summary>
        /// Returns if the move is valid.
        /// </summary>
        /// <param name="game">The game the move belongs to.</param>
        /// <returns>If the move is valid.</returns>
        public bool IsValid()
        {
            Country to = GetTo();
            Player player = GetPlayer();

            if (to.OwnedBy != player.PlayerNumber) return false;
            if (player.UnitsToPlace <= 0) return false;
            
            return true;
        }

        /// <summary>
        /// Executes the given move.  This should only be called on the server.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>Whether the move is valid and was executed.</returns>
        public bool Execute(Random random)
        {
            if (!IsValid()) return false;

            Country to = GetTo();
            Player player = GetPlayer();

            player.UnitsToPlace--;
            to.Units++;
            GameLog.AddEvent(player.Name + " place 1 unit on " + to.Name + ".");
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Placement)) return false;
            var other = obj as Placement;
            if (PlayerNumber != other.PlayerNumber) return false;
            if (ToName != other.ToName) return false;
            return true;
        }
    }
}
