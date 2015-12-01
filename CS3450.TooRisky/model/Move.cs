using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// An move that a player might make.
    /// </summary>
    public class Move : Action
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
        public Player GetPlayer(Game game)
        {
            return game.Players[PlayerName];
        }

        /// <summary>
        /// The country that this action originates from.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this attack originates from.</returns>
        public Country GetFrom(Game game)
        {
            return game.Countries[FromName];
        }

        /// <summary>
        /// The country that this action is targeting.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this action is targeting.</returns>
        public Country GetTo(Game game)
        {
            return game.Countries[ToName];
        }

        /// <summary>
        /// Returns if the move is valid.
        /// </summary>
        /// <param name="game">The game the move belongs to.</param>
        /// <returns>If the move is valid.</returns>
        public bool IsValid(Game game)
        {
            Country to = GetTo(game);
            Country from = GetFrom(game);
            Player player = GetPlayer(game);

            if (from.OwnedByName != player.Name) return false;
            if (to.OwnedByName != player.Name) return false;
            if (player.UnitsToMove <= 0) return false;
            return true;
        }

        /// <summary>
        /// Executes the given move.  This should only be called on the server.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>Whether the move is valid and was executed.</returns>
        public bool Execute(Game game, Random random)
        {
            if (!IsValid(game)) return false;

            Country to = GetTo(game);
            Country from = GetFrom(game);
            Player player = GetPlayer(game);

            player.UnitsToMove--;
            from.Units--;
            to.Units++;

            return true;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;
            if (!(obj is Move)) return false;
            var other = obj as Move;
            if (PlayerName != other.PlayerName) return false;
            if (ToName != other.ToName) return false;
            if (FromName != other.FromName) return false;
            return true;
        }
    }
}
