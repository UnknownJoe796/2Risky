using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// An action the player can take.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// The player that could make this action.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The player that could make this move.</returns>
        Player GetPlayer();

        /// <summary>
        /// The country that this action originates from.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this attack originates from.</returns>
        Country GetFrom();

        /// <summary>
        /// The country that this action is targeting.
        /// </summary>
        /// <param name="game">The game that this action is a part of.</param>
        /// <returns>The country that this action is targeting.</returns>
        Country GetTo();

        /// <summary>
        /// Returns if the move is valid.
        /// </summary>
        /// <param name="game">The game the move belongs to.</param>
        /// <returns>If the move is valid.</returns>
        bool IsValid();

        /// <summary>
        /// Executes the given action.  This should only be called on the server.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="random">The random number generator to use.</param>
        /// <returns>Whether the action is valid and was executed.</returns>
        bool Execute(Random random);
    }
}
