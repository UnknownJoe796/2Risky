using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CS3450.TooRisky.Model
{
    //This contains game constants and other globals
    static class Constants
    {
        /// <summary>
        /// This is the hint text currently displayed. This is not really a constant
        /// </summary>
        public static string CurrentHintText = "Welcome to 2Risky. Hints appear here";
        /// <summary>
        /// This hint is to appear during the placement phase
        /// </summary>
        public static readonly string PlacementHint =
            "Placement: click territories where you'd like to place units. Click End Phase when done";
        /// <summary>
        /// This hint is to appear during the attack phase
        /// </summary>
        public static readonly string AttackHint =
            "Attack: click territories from which you want to attack. Click End Phase when done";
        /// <summary>
        /// This hint is to appear during the move phase
        /// </summary>
        public static readonly string MoveHint =
            "Move: click territories from which you want to move units. Click End Turn when done";

        /// <summary>
        /// Global random number generator.
        /// </summary>
        public static readonly Random RandomGen = new Random();

        /// <summary>
        /// This is the number of units that will be assigned to each territory at the beginning
        /// </summary>
        public static readonly int InitialNumOfUnits = 3;

        /// <summary>
        /// This is the number of units a player can move at the end of each turn
        /// </summary>
        public static readonly int UnitsToMove = 5;
        
        /// <summary>
        /// Represents the color for attack arrows
        /// </summary>
        public static readonly Brush AttackColor = new SolidColorBrush(Colors.DarkRed);
        
        /// <summary>
        /// Represents the color for move arrows
        /// </summary>
        public static readonly Brush MoveColor = new SolidColorBrush(Colors.DarkBlue);
       
        /// <summary>
        /// Extension method for PlayerNumber enum to get the color for that player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Brush Color(this PlayerNumber player)
        {
            Color c;
            switch (player)
            {
                case PlayerNumber.None:
                    c = Colors.Gray;
                    break;
                case PlayerNumber.P1:
                    c = Colors.Green;
                    break;
                case PlayerNumber.P2:
                    c = Colors.DodgerBlue;
                    break;
                case PlayerNumber.P3:
                    c = Colors.Red;
                    break;
                case PlayerNumber.P4:
                    c = Colors.Orange;
                    break;
                case PlayerNumber.P5:
                    c = Colors.SaddleBrown;
                    break;
                case PlayerNumber.P6:
                    c = Colors.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(player), player, null);
            }
            return new SolidColorBrush(c);
        }
    }
}
