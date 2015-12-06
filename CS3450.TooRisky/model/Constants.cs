using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CS3450.TooRisky.Model
{
    static class Constants
    {
        public static string CurrentHintText = "Welcome to 2Risky. Hints appear here";
        public static readonly Random RandomGen = new Random();
        public static readonly int InitialNumOfUnits = 3;
        public static readonly Brush AttackColor = new SolidColorBrush(Colors.DarkRed);
        public static readonly Brush MoveColor = new SolidColorBrush(Colors.DarkBlue);
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
                    c = Colors.Yellow;
                    break;
                case PlayerNumber.P5:
                    c = Colors.Orange;
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
