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
using Windows.UI.Xaml.Media.Imaging;
using Placement = CS3450.TooRisky.Model.Placement;

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

        private ContentDialog _forfeitDialog;

        private ContentDialog _winDialog;

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
            this.Height = 720 + 48;
            this.InitializeComponent();
        }

        public async void UpdateUi()
        {
            //Check for game over
            if (Game.Instance.GameOver())
            {
                var winPl = Game.Instance.Players.First(a => a.Value.IsActive);
                var _winDialog = new ContentDialog()
                {
                    Title = "WIN!",
                    Content = "Congratulations, player " + winPl.Value.Name + " wins.",
                    PrimaryButtonText = "Exit"
                };
                _winDialog.PrimaryButtonClick += (sender, args) =>
                {
                    Application.Current.Exit();
                };
                if (_forfeitDialog != null && _forfeitDialog.Visibility == Visibility.Visible)
                {
                    _forfeitDialog.Hide();
                }
                await _winDialog.ShowAsync();
            }

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

            //Set Player Views
            SetPlayerViews();


            //Update Turn Phase Label
            EndTurnButton.Label = Game.Instance.CurrentPhase == TurnPhase.Move ? "End Turn" : "End Phase";

            //Update Hint text
            HintText.Text = Constants.CurrentHintText;



            //UpdatePlayerStats
            YourName.Text = Game.Instance.Players[Game.Instance.CurrentPlayerNumber].Name + "'s turn";
            CountriesOwnedLabel.Text =
                Game.Instance.Players[Game.Instance.CurrentPlayerNumber].CountriesOwned.Count.ToString();
            UnitsToPlaceLabel.Text = Game.Instance.Players[Game.Instance.CurrentPlayerNumber].UnitsToPlace.ToString();
            UnitsToMoveLabel.Text = Game.Instance.Players[Game.Instance.CurrentPlayerNumber].UnitsToMove.ToString();
            ZoneBonusLabel.Text =
                Game.Instance.Players[Game.Instance.CurrentPlayerNumber].ContinentsOwned.Sum(a => a.Worth).ToString();
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
        }

        public void InitGame(List<Player> players)
        {

            Game.Instance = new XML().Read();
            foreach (var p in players)
            {
                Game.Instance.AddPlayer(p);
            }
            Game.Instance.RandomlyAssignCountries();
            Game.Instance.CurrentPlayerNumber =
                (PlayerNumber) Constants.RandomGen.Next(1, Game.Instance.Players.Count + 1);
            Game.Instance.Players[Game.Instance.CurrentPlayerNumber].SetReinforcments();

            //wire up country controllers
            foreach (var c in Game.Instance.Countries)
            {
                var contr = new CountryController(c.Key);
                contr.Button.Click += (sender, args) =>
                {
                    if (!contr.ThisPlayersTurn) return;
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
                        switch (Game.Instance.CurrentPhase)
                        {
                            case TurnPhase.Placement:
                                Placement pl = new Placement()
                                {
                                    ToName = contr.CountryName,
                                    PlayerNumber = Game.Instance.CurrentPlayerNumber
                                };
                                if (pl.IsValid())
                                    pl.Execute(null);
                                break;
                            case TurnPhase.Attack:
                                foreach (var a in contr.Attacks)
                                {
                                    #region Attack Click Handler
                                    if (a.Value.IsValid())
                                    {
                                        a.Key.Tapped += (o, eventArgs) =>
                                        {
                                            if (a.Value.IsValid())
                                            {
                                                a.Value.Execute(Constants.RandomGen);
                                                if (!a.Value.IsValid())
                                                {
                                                    RemoveActionArrows();
                                                }
                                            }
                                            UpdateUi();
                                        };

                                        #endregion

                                        MainGrid.Children.Add(a.Key);
                                    }
                                }
                                break;
                            default:    //TurnPhase.Move
                                foreach (var a in contr.Moves)
                                {
                                    #region Moves Click Handler
                                    if (a.Value.IsValid())
                                    {
                                        a.Key.Tapped += (o, eventArgs) =>
                                        {
                                            if (a.Value.IsValid())
                                            {
                                                a.Value.Execute(null);
                                                if (!a.Value.IsValid())
                                                {
                                                    RemoveActionArrows();
                                                }
                                            }
                                            UpdateUi();
                                        };

                                        #endregion

                                        MainGrid.Children.Add(a.Key);
                                    }
                                }
                                break;
                        }
                    }
                    UpdateUi();
                };
                _countryControllers.Add(contr);
                MainGrid.Children.Add(_countryControllers.Last().Button);
            }
            //Update everything else we'd normally update after every action
            Constants.CurrentHintText = Constants.PlacementHint;

            UpdateUi();

        }

        public void SetPlayerViews()
        {
            //Set Player Views

            PlayerView1.SetPlayer(Game.Instance.Players[PlayerNumber.P1]);
            PlayerView2.SetPlayer(Game.Instance.Players[PlayerNumber.P2]);

            if (Game.Instance.Players.ContainsKey(PlayerNumber.P3))
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

            //Update Player Views Turn indicator
            PlayerView1.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P1);
            PlayerView2.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P2);
            PlayerView3.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P3);
            PlayerView4.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P4);
            PlayerView5.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P5);
            PlayerView6.SetTurn(Game.Instance.CurrentPlayerNumber == PlayerNumber.P6);

            //Update Player Views Unit Ct
            //P1
            if(Game.Instance.Players[PlayerNumber.P1].IsActive)
                PlayerView1.SetUnitsCt(Game.Instance.Players[PlayerNumber.P1].TotalUnits);
            else
                PlayerView1.SetForfeited();

            //P2
            if (Game.Instance.Players[PlayerNumber.P2].IsActive)
                PlayerView2.SetUnitsCt(Game.Instance.Players[PlayerNumber.P2].TotalUnits);
            else 
                PlayerView2.SetForfeited();
            //P3
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P3))
                if (Game.Instance.Players[PlayerNumber.P3].IsActive)
                    PlayerView3.SetUnitsCt(Game.Instance.Players[PlayerNumber.P3].TotalUnits);
                else
                    PlayerView3.SetForfeited();
            //P4
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P4))
                if (Game.Instance.Players[PlayerNumber.P4].IsActive)
                    PlayerView4.SetUnitsCt(Game.Instance.Players[PlayerNumber.P4].TotalUnits);
                else
                    PlayerView4.SetForfeited();
            //P5
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P5))
                if (Game.Instance.Players[PlayerNumber.P5].IsActive)
                    PlayerView5.SetUnitsCt(Game.Instance.Players[PlayerNumber.P5].TotalUnits);
                else
                    PlayerView5.SetForfeited();
            //P6
            if (Game.Instance.Players.ContainsKey(PlayerNumber.P6))
                if (Game.Instance.Players[PlayerNumber.P6].IsActive)
                    PlayerView6.SetUnitsCt(Game.Instance.Players[PlayerNumber.P6].TotalUnits);
                else
                    PlayerView6.SetForfeited();
        }

        private async void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string content = "A game of 2Risky is played by players taking turns.\n";
            content += "Each turn is seperated into 3 phases: unit placement\n";
            content += "attack, and movement.\n";
            content += "MOVE PHASE\n";
            content += "At the begining of each player's turn they are given a set of\nreinforcements";
            content += " this is the Unit placement phase.\nThe number of reinforcements is equal to";
            content += " the number of countries\nowned divided by 3 rounded down, or 3 whichever is larger";
            content += " plus the continental bonus(see below).\n";
            content += "ATTACK PHASE\n";
            content += "The next phase is the attack phase, this is where a player can choose\nto attack countries adjacent to the ones they own";
            content += "if they\nsuccessfully bring their opponents units on that country to 0, they\nclaim control of it and move 1 unit to";
            content += "that country\nThis can continue as long as a player wishes and is able.\n";
            content += "MOVE PHASE\n";
            content += "The third and final phase is the move phase.\nThis is where players get to move units around to their new\ncountries";
            content += " each player can move 5 total units to adjacent countries.\n";
            content += "WINNING\n";
            content += "This continues until one player controls the world, or everyone else\nforfeits.";
            ContentDialog cd = new ContentDialog()
            {
                Title = "Help",
                Content = content,
                PrimaryButtonText = "OK"

            };

            await cd.ShowAsync();
        }

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            string content = "This project was created in the Fall of 2015 ";
            content += "as the final project\n for CS3450.\n\nIt was created by:\n";
            content += "Derek Hunter\t\t\tGreg Vernon\nFabio Göttlicher\t\t\tJustin Young\nNate Ashby\t\t\tJoe Ivey";
            ContentDialog cd = new ContentDialog()
            {
                Title = "About",
                Width = 500,
                Height = 500,
                Content = content,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Github"

            };
            cd.SecondaryButtonClick += async (dialog, args) =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/UnknownJoe796/2Risky"));
            };

            await cd.ShowAsync();
        }

        private async void GameLogButton_Click(object sender, RoutedEventArgs e)
        {
            var gameLogDialog = new GameLogDialog();
            gameLogDialog.SetContent();
            await gameLogDialog.ShowAsync();
        }

        private async void ForfeitButton_Click(object sender, RoutedEventArgs e)
        {
            var img = new Image();
            XamlAnimatedGif.AnimationBehavior.SetSourceUri(img, new Uri("ms-appx:///Assets/Forfeit.gif"));
            _forfeitDialog = new ContentDialog()
            {
                Title = "You sure you wanna forfeit?",
                PrimaryButtonText = "Yes, I suck",
                SecondaryButtonText = "Keep Playing",
                Content = img,
            };
            _forfeitDialog.PrimaryButtonClick += (contentDialog, args) =>
            {
                Game.Instance.ForfeitCurrentPlayer();
                UpdateUi();
            };
            
            await _forfeitDialog.ShowAsync();
            
        }

        private void EndPhase_Click(object sender, RoutedEventArgs e)
        {
            Game.Instance.EndCurrentPhase();
            RemoveActionArrows();
            UpdateUi();
        }

        private void RemoveActionArrows()
        {
            foreach (SymbolIcon a in MainGrid.Children.OfType<SymbolIcon>())
            {
                MainGrid.Children.Remove(a);
            }
        }
    }
}
