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
        public  PlayerView()
        {
           this.InitializeComponent();
        }

        public void SetPlayer(Player player)
        {
            PlayerNumberLabel.Text = player.PlayerNumber.ToString();
            PlayerCircle.Fill = player.PlayerNumber.Color();
            PlayerName.Text = player.Name;
            PlayerUnits.Text = $"Units: {player.TotalUnits}";
        }

        public void SetTurn(bool thisPlayersTurn)
        {
            var colour = thisPlayersTurn ? "#FF787878" : "#00E6E6E6";
            Root.Background = new SolidColorBrush(Color.FromArgb(
                Convert.ToByte(colour.Substring(1, 2), 16),
                Convert.ToByte(colour.Substring(3, 2), 16),
                Convert.ToByte(colour.Substring(5, 2), 16),
                Convert.ToByte(colour.Substring(7, 2), 16)));
        }

        public void SetUnitsCt(int unitsCt)
        {
            PlayerUnits.Text = $"Units: {unitsCt}";
        }

        public void SetForfeited(bool forfeited)
        {
            if (forfeited)
            {
                PlayerUnits.Text = "#Rekt";
                PlayerIcon.Symbol = Symbol.Delete;

            }

        }

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
