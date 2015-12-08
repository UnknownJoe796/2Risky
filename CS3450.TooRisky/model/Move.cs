using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS3450.TooRisky.Utils;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// An move that a player might make.
    /// </summary>
    public class Move : IAction
    {
        /// <summary>
        /// The name of the player that could make the move.
        /// </summary>
        public PlayerNumber PlayerNumber = PlayerNumber.None;

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
        public Player GetPlayer()
        {
            return Game.Instance.Players[PlayerNumber];
        }

        /// <summary>
        /// The country that this action originates from.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this attack originates from.</returns>
        public Country GetFrom()
        {
            return Game.Instance.Countries[FromName];
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
            Country from = GetFrom();
            Player player = GetPlayer();

            if (from.OwnedBy != player.PlayerNumber) return false;
            if (to.OwnedBy != player.PlayerNumber) return false;
            if (from.Units < 2) return false;
            if (player.UnitsToMove < 2) return false;
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
            Country from = GetFrom();
            Player player = GetPlayer();

            player.UnitsToMove--;
            from.Units--;
            to.Units++;
            GameLog.AddEvent(player.Name + " moved 1 unit from " + from.Name + " to " + to.Name + ".");
            return true;
        }

        /// <summary>
        /// Equals override
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Move)) return false;
            var other = obj as Move;
            if (PlayerNumber != other.PlayerNumber) return false;
            if (ToName != other.ToName) return false;
            if (FromName != other.FromName) return false;
            return true;
        }
    }
}
