using System;
using Task1.UnityRenderers;

namespace Task1.UnityGame.Components {
    internal sealed class WinLoseChecker : IComponent {
        private GameObject GameObject { get; }

        private Player _player;
        private ScoreManager _scoreManager;

        private enum State { InProgress, Victory, Defeat };
        private State _state = State.InProgress;

        internal Action<int> VictoryAction { private get; set; }
        internal Action<int> DefeatAction { private get; set; }

        internal WinLoseChecker (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            if (VictoryAction == null) {
                throw new InvalidOperationException("Victory action must be set in WinLoseChecker");
            }
            if (DefeatAction == null) {
                throw new InvalidOperationException ("Defeat action must be set in WinLoseChecker");
            }

            _player = GameObject.Game.FindComponentByTag<Player> (Tags.PlayerTag);
            _scoreManager = GameObject.Game.FindComponentByTag<ScoreManager> (Tags.ScoreManagerTag);
        }

        public void Update () {
            switch (_state) {
                case State.InProgress:
                    if (_player.Lives == 0) {
                        _state = State.Defeat;
                        ShowText("lost");
                        return;
                    }
                    if (GameObject.Game.FindObjectsWithTag(Tags.PillTag) == null &&
                        GameObject.Game.FindObjectsWithTag (Tags.SuperPillTag) == null &&
                        GameObject.Game.FindObjectsWithTag (Tags.FruitTag) == null) {
                        _state = State.Victory;
                        ShowText("won");
                    }
                    break;
                case State.Defeat:
                    if (GameObject.Game.Input().GetKey (Actions.Continue)) {
                        DefeatAction (_scoreManager.Score);
                    }
                    break;
                case State.Victory:
                    if (GameObject.Game.Input().GetKey (Actions.Continue)) {
                        VictoryAction (_scoreManager.Score);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Unknown game state in WinLoseChecker");
            }
        }
        public void Destroy () { }

        private void ShowText (string word) {
            var g = GameObject.Game.Instantiate ();
            var r = g.AddComponent (new FinalTextRenderer (g)) as FinalTextRenderer;
            if (r == null) { throw new InvalidOperationException ();}
            r.Text = "You " + word + "! Your score: " + _scoreManager.Score + "\n press " +
                                 GameObject.Game.Input().GetBind (Actions.Continue) + " to continue";
        }
    }
}
