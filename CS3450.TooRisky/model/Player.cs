using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.model
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

    }
}
