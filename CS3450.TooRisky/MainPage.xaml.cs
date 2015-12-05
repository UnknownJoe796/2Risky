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
using Windows.UI;

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
            Map.Width = this.Width;
            Map.InvalidateArrange();
            this.ViewModel = new PlayerViewModel();
            this.SizeChanged += MainPage_SizeChanged;

            var b = new Button
            {
                Content = "11",
                Margin = new Thickness(50, 50, 0, 0),
                Background = new SolidColorBrush(Colors.Green)
            };
            MainGrid.Children.Add(b);
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = 1280;
            this.Height = 720;
            this.InitializeComponent();
        }

        public void UpdateUi()
        {

        }

        public class PlayerViewModel
        {
            public ObservableCollection<Player> Players = new ObservableCollection<Player>();

            public PlayerViewModel()
            {
                this.Players.Add(new Player() { Name = "Test 1" });
                this.Players.Add(new Player() { Name = "Test 2" });
                //todo
            }

            //public event PropertyChangedEventHandler PropertyChanged;   //TODO!!!
        }

        private async void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO 
            GameLobbyDialog gameLobbyDialog = new GameLobbyDialog();
            await gameLobbyDialog.ShowAsync();
        }

        public void InitGame()
        {


            //Adds country buttons to the view
            foreach (var c in Game.Instance.Countries)
            {
                MainGrid.Children.Add(c.Value.Button);
            }
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
