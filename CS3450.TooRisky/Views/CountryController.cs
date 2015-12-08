using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using CS3450.TooRisky.Model;

namespace CS3450.TooRisky.Views
{
    /// <summary>
    /// This takes care of the country buttons on the map.
    /// </summary>
    class CountryController
    {
        public string CountryName { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Button Button { get; private set; }
        public Dictionary<SymbolIcon, Attack> Attacks { get; private set; }
        
        public Dictionary<SymbolIcon, Move> Moves { get; private set; }

        public bool ArrowsShown { get; private set; }

        public bool ThisPlayersTurn => Game.Instance.CurrentPlayerNumber == Game.Instance.Countries[CountryName].OwnedBy;
        
        /// <summary>
        /// Initializes the controller, it's corresponding country and coordinates
        /// </summary>
        /// <param name="country"></param>
        public CountryController (string country)
        {
            this.CountryName = country;
            // -15 To center button on coordinate
            this.X = Game.Instance.Countries[country].X-15;
            this.Y = Game.Instance.Countries[country].Y-15;
            ArrowsShown = false;
            Attacks = new Dictionary<SymbolIcon, Attack>();
            Moves = new Dictionary<SymbolIcon, Move>();
            CreateButton();
        }

        /// <summary>
        /// Creates the button for the country
        /// </summary>
        private async void CreateButton()
        {
            Button = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = Game.Instance.Countries[CountryName].Units.ToString(),
                Margin = new Thickness(Convert.ToDouble(X), Convert.ToDouble(Y), 0.0, 0.0),
            };
            Button.Click += Button_Click;
        }

        /// <summary>
        /// Updates unit count on the button
        /// </summary>
        /// <param name="units"></param>
        public void UpdateUnitsCt(int units)
        {
            Button.Content = "";
            Button.Content = units.ToString();
        }

        /// <summary>
        /// Updates the background color (=owner player)
        /// </summary>
        /// <param name="player"></param>
        public void UpdateOwnerPlayer(PlayerNumber player)
        {
            Button.Background = player.Color();
        }


        /// <summary>
        /// Click listener. Calculates all the possible attacks and moves. Everything else is handled in MainPage.xaml.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ThisPlayersTurn) return;
            System.Diagnostics.Debug.WriteLine("First controller event");
            if (ArrowsShown)
            {
               ArrowsShown = false;
            }
            else
            {
                Attacks.Clear();
                Moves.Clear();
                ArrowsShown = true;
                foreach (var a in Game.Instance.Countries[CountryName].Attacks())
                {
                    var to = Game.Instance.Countries[a.ToName];
                    var tup = GetArrowPosAndRotation(X, Y, to.X, to.Y);
                    //custom arrow position for Kamchatka/Alaska
                    if (to.Name == "Alaska" && CountryName == "Kamchatka")
                        tup = Tuple.Create(new Thickness(1230, 130, 0, 0), 90.0);
                    if(to.Name == "Kamchatka" && CountryName == "Alaska")
                        tup = Tuple.Create(new Thickness(40, 120, 0, 0), 270.0);

                    var s = new SymbolIcon
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Foreground = Constants.AttackColor,
                        Symbol = Symbol.Up,
                        Margin = tup.Item1
                    };

                    var r = new RotateTransform()
                    {
                        Angle = tup.Item2
                    };
                    s.RenderTransform = r;
                    Attacks.Add(s, a);
                }

                foreach (var a in Game.Instance.Countries[CountryName].Moves())
                {
                    var to = Game.Instance.Countries[a.ToName];
                    var tup = GetArrowPosAndRotation(X, Y, to.X, to.Y);
                    //custom arrow position for Kamchatka/Alaska
                    if (to.Name == "Alaska" && CountryName == "Kamchatka")
                        tup = Tuple.Create(new Thickness(1230, 130, 0, 0), 90.0);
                    if (to.Name == "Kamchatka" && CountryName == "Alaska")
                        tup = Tuple.Create(new Thickness(40, 120, 0, 0), 270.0);

                    var s = new SymbolIcon
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Foreground = Constants.MoveColor,
                        Symbol = Symbol.Up,
                        Margin = tup.Item1
                    };

                    var r = new RotateTransform()
                    {
                        Angle = tup.Item2
                    };
                    s.RenderTransform = r;
                    Moves.Add(s, a);
                }
            }
            System.Diagnostics.Debug.WriteLine("First controller event done");

        }

        /// <summary>
        /// Calculates the position and angle at which we need to show the arrows for attacks/moves.
        /// </summary>
        /// <param name="xFrom"></param>
        /// <param name="yFrom"></param>
        /// <param name="xTo"></param>
        /// <param name="yTo"></param>
        /// <returns></returns>
        public Tuple<Thickness, double> GetArrowPosAndRotation(int xFrom, int yFrom, int xTo, int yTo)
        {
            var x = ((double)(xFrom + xTo)) / 2.0;
            var y = ((double)(yFrom + yTo)) / 2.0;
            var xLength = (double)(xFrom - xTo);
            var yLength = yFrom - yTo == 0 ? 0.00001 : (double)(yFrom - yTo);   //avoid dividing by 0
            var angle = Math.Acos(-yLength/Math.Sqrt(Math.Pow(xLength,2)+Math.Pow(yLength,2))) * (180.0 / Math.PI) + 180;
            if(xLength < 0) angle *= -1;
            var margin = new Thickness(x, y, 0, 0);
            return Tuple.Create(margin, angle);
        }
    }
}
