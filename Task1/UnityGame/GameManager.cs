using System;
using System.Windows.Controls;

namespace Task1.UnityGame {
    internal sealed class GameManager {
        internal Game Game { get; }
        internal ScoreSystem.ScoreSystem ScoreSystem { get; }
        private readonly LevelLoader _levelLoader;
        private readonly Canvas _canvas;
        private event Action EndGame;

        internal GameManager (Canvas canvas) {
            if (canvas == null) {
                throw new ArgumentNullException ("Canvas cannot " + "be null on game initialization");
            }
            _canvas = canvas;

            Game = new Game();

            ScoreSystem = new ScoreSystem.ScoreSystem();

            _levelLoader = new LevelLoader();

            foreach (var pair in ControlsIO.ControlsLoader.ReadControls()) {
                Game.Input().RegisterAction(pair.Key, pair.Value);
            }
        }

        internal void NewGame () {
            _levelLoader.Reset();
            PlayNextLevel (0);
        }

        private void WinLevel (int score) {
            if (_levelLoader.AllRead) {
                WinGame (score);
            } else {
                PlayNextLevel(score);
            }
        }

        private void WinGame (int score) {
            ScoreSystem.UpdateHighScores(score);
            EndGame?.Invoke();
        }

        private void PlayNextLevel (int score) {
            Game.Reset();
            GameInitializer.InitializeGame (Game, _canvas, 
                _levelLoader.ReadNextLevel (), GhostLoader.GetGhostTypes (),
                WinLevel, Lose, score);
        }

        private void Lose (int score) {
            ScoreSystem.UpdateHighScores (score);
            EndGame?.Invoke();
        }

        internal void SubscribeToEndGame (Action reaction) {
            EndGame += reaction;
        } 
    }
}
