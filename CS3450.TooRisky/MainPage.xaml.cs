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
using CS3450.TooRisky.Views;
using System.Linq;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CS3450.TooRisky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<CountryController> _countryControllers;

        public MainPage()
        {
            this.InitializeComponent();
            Map.Width = this.Width;
            Map.InvalidateArrange();
            this.SizeChanged += MainPage_SizeChanged;
            _countryControllers = new List<CountryController>();
            NewGameButton_Click(this, null);
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


        private async void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var content = new ContentDialog()
            {
                Title = "New Game",
                Content = "What kind of game would you like to play?",
                PrimaryButtonText = "Local",
                SecondaryButtonText = "Network",
                IsSecondaryButtonEnabled = false
            };

            //Local game nested ass lambdas
            content.PrimaryButtonClick += async (dialog, args) =>
            {
                content.Hide();
                var a = new LocalGameDialog();
                a.PrimaryButtonClick += async (contentDialog, eventArgs) =>
                {
                    var c = contentDialog as LocalGameDialog;
                    if (c.PlayerNames.Count(p => p.Text == string.Empty) > 4)
                    {
                        a.Hide();
                        await new MessageDialog("At least 2 players must be added").ShowAsync();
                        NewGameButton_Click(this, null);
                    }
                    else
                    {
                        var players = new List<Player>();
                        foreach (var p in c.PlayerNames.Where(p => p.Text != string.Empty))
                        {
                            System.Diagnostics.Debug.WriteLine("Adding player: " + p.Text);
                            players.Add(new Player() {Name = p.Text});
                        }
                        InitGame(players);
                    }
                };
                a.SecondaryButtonClick += (sender1, clickEventArgs) =>
                {
                    a.Hide();
                    NewGameButton_Click(this, null);
                };
                await a.ShowAsync();
            };
            await content.ShowAsync();
            /*GameLobbyDialog gameLobbyDialog = new GameLobbyDialog();
            await gameLobbyDialog.ShowAsync();*/
        }

        public void InitGame(List<Player> players)
        {

            Game.Instance = new XML().Read();
            foreach (var p in players)
            {
                Game.Instance.AddPlayer(p);
            }
            Game.Instance.RandomlyAssignCountries();
            foreach (var c in Game.Instance.Countries)
            {
                _countryControllers.Add(new CountryController(c.Key));
                MainGrid.Children.Add(_countryControllers.Last().Button);
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

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            string content = "Lorem";
            ContentDialog cd = new ContentDialog()
            {
                Title = "About",
                Width = 500,
                Height = 500,
                Content = content,
                PrimaryButtonText = "OK"

            };

            await cd.ShowAsync();
        }

        private void MainGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {

            Coord.Text = e.GetPosition(MainGrid).X.ToString() + "," + e.GetPosition(MainGrid).Y.ToString();
        }
    }
}
