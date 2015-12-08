using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CS3450.TooRisky.Model;

namespace CS3450.TooRisky.Views
{
    public sealed partial class PlayerView
    {
        //Contructors - initializes the view
        public  PlayerView()
        {
           this.InitializeComponent();
        }

        /// <summary>
        /// Sets who this player view belongs to
        /// </summary>
        /// <param name="player"></param>
        public void SetPlayer(Player player)
        {
            PlayerNumberLabel.Text = player.PlayerNumber.ToString();
            PlayerCircle.Fill = player.PlayerNumber.Color();
            PlayerName.Text = player.Name;
            PlayerUnits.Text = $"Units: {player.TotalUnits}";
            TurnIndicator.Fill = player.PlayerNumber.Color();
        }

        /// <summary>
        /// Changes the background shading to indicate it is this player's turn. Also changes the visibility of the indicator bar.
        /// </summary>
        /// <param name="thisPlayersTurn"></param>
        public void SetTurn(bool thisPlayersTurn)
        {
            var colour = thisPlayersTurn ? "#FF959595" : "#00E6E6E6";
            Root.Background = new SolidColorBrush(Color.FromArgb(
                Convert.ToByte(colour.Substring(1, 2), 16),
                Convert.ToByte(colour.Substring(3, 2), 16),
                Convert.ToByte(colour.Substring(5, 2), 16),
                Convert.ToByte(colour.Substring(7, 2), 16)));
            TurnIndicator.Visibility = thisPlayersTurn ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Updates the playerview unit count and player status (playing/lost)
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="unitsCt"></param>
        public void UpdateStatus(bool isActive, int unitsCt)
        {
            if (!isActive)
            {
                PlayerUnits.Text = "#Rekt";
                PlayerIcon.Symbol = Symbol.Delete;

            }
            else
            {
                PlayerIcon.Symbol = Symbol.Contact;
                PlayerUnits.Text = $"Units: {unitsCt}";
            }
        }
    }
}
