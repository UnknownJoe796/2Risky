using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CS3450.TooRisky.Model
{
    /// <summary>
    /// Represents a country in the game.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// The name of this country.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// A list of the names of the adjacent countries.
        /// </summary>
        public List<string> AdjacentCountryNames = new List<string>();

        /// <summary>
        /// The number of the units on this country.
        /// </summary>
        public int Units = 1;

        /// <summary>
        /// The name of the player that owns this country.
        /// </summary>
        public PlayerNumber OwnedBy;

        /// <summary>
        /// The graphical X coordinate of the center of this country.
        /// </summary>
        public int X = 0;

        /// <summary>
        /// The graphical Y coordinate of the center of this country.
        /// </summary>
        public int Y = 0;

        /// <summary>
        /// Flag indicating if the country was taken over, in which case the player can move as many units as they want into this country during this turn.
        /// This flag should be reset to false at the end of every turn.
        /// </summary>
        public bool JustTakenOver = false;

        public Button Button { get; set; }

        public Country()
        {
            //CreateButton();
        }

        private async void CreateButton()
        {
            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Button = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Content = Units.ToString(),
                    Margin = new Thickness(Convert.ToDouble(X), Convert.ToDouble(Y), 0.0, 0.0)
                };
            });
        }

        public void UpdateButtonProps(Brush bgColor)
        {
            Button.Content = Units.ToString();
            Button.Background = bgColor;
        }

    /// <summary>
        /// Retrieves a list of countries adjacent to this one.
        /// </summary>
        /// <param name="game">The game that this country is a part of.</param>
        /// <returns>A list of countries adjacent to this one.</returns>
        public List<Country> AdjacentCountries()
        {
            List<Country> countries = new List<Country>();
            foreach(string name in AdjacentCountryNames)
            {
                countries.Add(Game.Instance.Countries[name]);
            }
            return countries;
        }

        /// <summary>
        /// Returns all of the valid attacks that can be made from this country.
        /// </summary>
        /// <param name="game">The game that this country belongs to.</param>
        /// <returns>A list of the legal attacks that can be made from this country.</returns>
        public List<Attack> Attacks()
        {
            var actions = new List<Attack>();

            foreach(var country in AdjacentCountries())
            {
                if(country.OwnedBy != OwnedBy)
                {
                    var attack = new Attack()
                    {
                        PlayerNumber = OwnedBy,
                        FromName = Name,
                        ToName = country.Name
                    };
                    if (attack.IsValid())
                    {
                        actions.Add(attack);
                    }
                }
            }

            return actions;
        }

        /// <summary>
        /// Returns all of the valid moves that can be made from this country.
        /// </summary>
        /// <param name="game">The game that this country belongs to.</param>
        /// <returns>A list of the legal moves that can be made from this country.</returns>
        public List<Move> Moves()
        {
            var actions = new List<Move>();

            foreach (var country in AdjacentCountries())
            {
                if (country.OwnedBy == OwnedBy)
                {
                    var move = new Move()
                    {
                        PlayerNumber = OwnedBy,
                        FromName = Name,
                        ToName = country.Name
                    };
                    if (move.IsValid())
                    {
                        actions.Add(move);
                    }
                }
            }

            return actions;
        }
    }
}
