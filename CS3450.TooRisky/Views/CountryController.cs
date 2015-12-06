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

        public void UpdateUnitsCt(int units)
        {
            Button.Content = units.ToString();
        }

        public void UpdateOwnerPlayer(PlayerNumber player)
        {
            Button.Background = player.Color();
        }

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
