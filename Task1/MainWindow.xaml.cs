using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Task1.UnityGame;
using Task1.Menu;
using Application = System.Windows.Application;
using KeyBinding = Task1.UnityGame.KeyBinding;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using Panel = System.Windows.Controls.Panel;

namespace Task1 {
	internal sealed partial class MainWindow {
		private readonly GameManager _gameManager;

	    private readonly DispatcherTimer _timer;
	    private const float GameSpeed = 100000;

        private readonly Panel _menu;
        private readonly Canvas _gameCanvas;

        public MainWindow () {
            InitializeComponent ();

            _menu = Content as Panel;
            _gameCanvas = new Canvas();

            try {
                _gameManager = new GameManager (_gameCanvas);
                _gameManager.ScoreSystem.SubscribeToNameRequest(NameRequest);
                _gameManager.SubscribeToEndGame(EndGame);
            } catch (Exception ex) {
                MessageBox.Show ("Game initialization error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown ();
            }

            KeyDown += ProcessMenuInput;

            _timer = new DispatcherTimer { Interval = new TimeSpan ((long) GameSpeed) };
            _timer.Tick += delegate {
                try {
                    _gameManager.Game.Update ();
                } catch (Exception ex) {
                    MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    SwitchToMenu();
                }
            };
        }

	    private void ContinueClick (object sender, RoutedEventArgs e) {
            SwitchToGame();
	    }

	    private void NewGameClick (object sender, RoutedEventArgs e) {
	        try { 
	            _gameManager.NewGame ();
	        }  catch (Exception ex) {
                EndGame();
                MessageBox.Show (ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ContinueButton.IsEnabled = true;
            SwitchToGame ();
            _timer.Start ();
        }

	    private void SettingsClick (object sender, RoutedEventArgs e) {
	        SwitchToSettings ();
	    }

		private void ScoresClick (object sender, RoutedEventArgs e) {
            SwitchToScores ();
		}

		private void ExitClick (object sender, RoutedEventArgs e) {
            Application.Current.Shutdown ();
        }

	    private void ProcessGameInput (object sender, KeyEventArgs e) {
            _gameManager.Game.ButtonPressed (e.Key);
        }

	    private void ProcessMenuInput (object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                SwitchToMenu ();
            }
        }

        private void EndGame () {
	        SwitchToMenu();
	        ContinueButton.IsEnabled = false;
	    }

	    private void SwitchToMenu () {
            _timer.Stop ();
            Content = _menu;
	        KeyDown -= ProcessGameInput;
        }

	    private void SwitchToGame () {
	        Content = _gameCanvas;
            KeyDown += ProcessGameInput;
            _timer.Start ();
        }


	    private static string NameRequest () {
            var inputDialog = new SimpleDialog ("Please enter your name:", "Player");
	        return inputDialog.ShowDialog () == true ? inputDialog.Answer : "Player";
	    }

	    private UIElement GetSettings () {
	        return new ControlsSettings(_gameManager.Game.Input().Binds, SwitchToMenu, SaveControlsSettings);
	    }

	    private static void SaveControlsSettings (List<KeyBinding> binds) {
	        if (binds == null) {
	            throw new ArgumentNullException("Binds cannot be"+" null on saving controls");
	        }
	        var list = binds.Select (b => new KeyValuePair<string, string> (b.Name, b.Key.ToString ())).ToList ();
	        ControlsIO.ControlsLoader.WriteControls(list);
	    }

	    private void SwitchToSettings () {
	        Content = GetSettings ();
	    }

        private UIElement GetScoreboard () {
            return new ScoreBoard(this);
	    }

	    private void SwitchToScores () {
	        Content = GetScoreboard ();
	    }
    }
}
