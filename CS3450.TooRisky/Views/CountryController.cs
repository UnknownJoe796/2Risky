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
        public Dictionary<SymbolIcon, string> Attacks { get; private set; }
        
        public Dictionary<SymbolIcon, string> Moves { get; private set; }

        public bool ArrowsShown { get; private set; }
        public CountryController (string country)
        {
            this.CountryName = country;
            this.X = Game.Instance.Countries[country].X;
            this.Y = Game.Instance.Countries[country].Y;
            ArrowsShown = false;
            Attacks = new Dictionary<SymbolIcon, string>();
            Moves = new Dictionary<SymbolIcon, string>();
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

        public void UpdateOwenerPlayer(PlayerNumber player)
        {
            Button.Background = player.Color();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ArrowsShown)
            {
               Attacks.Clear();
               Moves.Clear();
            }
            else
            {
                foreach (var a in Game.Instance.Countries[CountryName].Attacks())
                {
                    var to = Game.Instance.Countries[a.ToName];
                    var tup = GetArrowPosAndRotation(X, Y, to.X, to.Y);
                    var s = new SymbolIcon
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Foreground = Constants.AttackColor
                    };

                    var r = new RotateTransform()
                    {
                        Angle = tup.Item2
                    };
                    s.RenderTransform = r;
                    Attacks.Add(s, a.ToName);
                }

                foreach (var a in Game.Instance.Countries[CountryName].Moves())
                {
                    var to = Game.Instance.Countries[a.ToName];
                    var tup = GetArrowPosAndRotation(X, Y, to.X, to.Y);
                    var s = new SymbolIcon
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Foreground = Constants.MoveColor
                    };

                    var r = new RotateTransform()
                    {
                        Angle = tup.Item2
                    };
                    s.RenderTransform = r;
                    Moves.Add(s, a.ToName);
                }

                //TODO add to view here!!
            }
        }

        public Tuple<Thickness, double> GetArrowPosAndRotation(int xFrom, int yFrom, int xTo, int yTo)
        {
            var x = ((double)(xFrom + xTo)) / 2.0;
            var y = ((double)(yFrom + yTo)) / 2.0;
            var xLength = xTo - xFrom;
            var yLength = yTo - yFrom;
            var angle = Math.Tan(yLength / xLength) * (180.0 / Math.PI);
            var margin = new Thickness(x, y, 0, 0);
            return Tuple.Create(margin, angle);
        }
    }
}
