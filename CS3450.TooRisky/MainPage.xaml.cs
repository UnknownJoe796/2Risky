using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CS3450.TooRisky.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CS3450.TooRisky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Game game;
        public PlayerViewModel ViewModel;
        
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new PlayerViewModel();
        }

        public void AddPlayers()
        {
           
        }

        public void UpdateUi()
        {

        }

        public class PlayerViewModel
        {
            public ObservableCollection<Player> Players = new ObservableCollection<Player>();

            public PlayerViewModel()
            {
                this.Players.Add(new Player() { Name = "Test 1", TotalUnits = 69 });
                this.Players.Add(new Player() { Name = "Test 2", TotalUnits = 50 });
                //todo
            }

            //public event PropertyChangedEventHandler PropertyChanged;   //TODO!!!
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO 
        }

        private async void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string content = "Lorem";
            ContentDialog cd = new ContentDialog()
            {
                Title = "Help",
                Width = 500,
                Height = 500,
                Content = content,
                PrimaryButtonText = "OK"

            };

            await cd.ShowAsync();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
