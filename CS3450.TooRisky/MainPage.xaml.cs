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

        private string _attacksForShown = null;

        public MainPage()
        {
            this.InitializeComponent();
            Map.Width = this.Width;
            Map.InvalidateArrange();
            //this.SizeChanged += MainPage_SizeChanged;
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
            //update country buttons
            foreach (var p in Game.Instance.Players)
            {
                foreach (var co in p.Value.CountriesOwned)
                {
                    var cc = _countryControllers.First(a => a.CountryName == co.Name);
                    cc.UpdateOwnerPlayer(p.Key);
                    cc.UpdateUnitsCt(Game.Instance.Countries[co.Name].Units);

                }
            }

            //Update Player Views Turn indicator
            PlayerView1.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P1);
            PlayerView2.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P2);
            PlayerView3.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P3);
            PlayerView4.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P4);
            PlayerView5.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P5);
            PlayerView6.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P6);

            //Update Player Views Unit Ct
            PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P1].TotalUnits);
            PlayerView2.SetUnitsCt(Game.Instance.Players[PlayerNumber.P1].TotalUnits);
            if(Game.Instance.Players.ContainsKey(PlayerNumber.P3))
                PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P3].TotalUnits);
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P4))
                PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P4].TotalUnits);
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P5))
                PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P5].TotalUnits);
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P6))
                PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P6].TotalUnits);


        }

        private async void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var content = new ContentDialog()
            {
                Title = "New Game",
                Content = "What kind of game would you like to play?\r\n(Sorry, you actually have no choice as we didn't get to implement networked games)",
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
                var contr = new CountryController(c.Key);
                contr.Button.Click += (sender, args) =>
                {
                    System.Diagnostics.Debug.WriteLine("Second controller event");
                    if (_attacksForShown != null)
                    {
                        var aToRemove = _countryControllers.First(a => a.CountryName == _attacksForShown).Attacks;
                        var mToRemove = _countryControllers.First(a => a.CountryName == _attacksForShown).Moves;
                        foreach (var a in aToRemove)
                        {
                            MainGrid.Children.Remove(a.Key);
                        }
                        foreach (var m in mToRemove)
                        {
                            MainGrid.Children.Remove(m.Key);
                        }
                        _attacksForShown = null;
                    }
                    if (!contr.ArrowsShown)
                    {
                        System.Diagnostics.Debug.WriteLine("Removing arrows");

                        foreach (var a in contr.Attacks)
                        {
                            MainGrid.Children.Remove(a.Key);
                        }
                        foreach (var m in contr.Moves)
                        {
                            MainGrid.Children.Remove(m.Key);
                        }
                    }
                    else
                    {
                        _attacksForShown = contr.CountryName;
                        System.Diagnostics.Debug.WriteLine("Adding arrows");
                        foreach (var a in contr.Attacks)
                        {
                            #region Attack Click Handler
                            a.Key.Tapped += (o, eventArgs) =>
                            {
                                a.Value.Execute(Constants.RandomGen);
                                UpdateUi();
                            };
                            #endregion
                            MainGrid.Children.Add(a.Key);

                        }
                        foreach (var m in contr.Moves)
                        {
                            MainGrid.Children.Add(m.Key);
                        }
                    }
                };
                _countryControllers.Add(contr);
                MainGrid.Children.Add(_countryControllers.Last().Button);
            }
       
            //Set Player Views
            PlayerView1.SetPlayer(Game.Instance.Players[PlayerNumber.P1]);
            PlayerView2.SetPlayer(Game.Instance.Players[PlayerNumber.P2]);

            if(Game.Instance.Players.ContainsKey(PlayerNumber.P3))
                PlayerView3.SetPlayer(Game.Instance.Players[PlayerNumber.P3]);
            else
                PlayerView3.Visibility = Visibility.Collapsed;

            if (Game.Instance.Players.ContainsKey(PlayerNumber.P4))
                PlayerView4.SetPlayer(Game.Instance.Players[PlayerNumber.P4]);
            else
                PlayerView4.Visibility = Visibility.Collapsed;

            if (Game.Instance.Players.ContainsKey(PlayerNumber.P5))
                PlayerView5.SetPlayer(Game.Instance.Players[PlayerNumber.P5]);
            else
                PlayerView5.Visibility = Visibility.Collapsed;

            if (Game.Instance.Players.ContainsKey(PlayerNumber.P6))
                PlayerView6.SetPlayer(Game.Instance.Players[PlayerNumber.P6]);
            else
                PlayerView6.Visibility = Visibility.Collapsed;

            //Update everything else we'd normally update after every action
            UpdateUi();

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

            YourName.Text = e.GetPosition(MainGrid).X.ToString() + "," + e.GetPosition(MainGrid).Y.ToString();
        }

        private void GameLogButton_Click(object sender, RoutedEventArgs e)
        {
            //Show Game log
        }

        private void ForfeitButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Instance.ForfeitCurrentPlayer();
        }

        private void EndPhase_Click(object sender, RoutedEventArgs e)
        {
            Game.Instance.EndCurrentPhase();
        }
    }
}
