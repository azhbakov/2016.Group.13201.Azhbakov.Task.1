using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Task1.UnityGame.Components;
using Task1.UnityRenderers;

namespace Task1.UnityGame {
    internal static class GameInitializer {

        private const int StartLives = 3;
        private const int PillReward = 100;
        private const int SuperPillReward = 200;
        private const int FruitReward = 300;
        private const int GhostReward = 400;
        private const int PowerUpTimeMilliseconds = 5000;
        private const float PlayerSpeed = 0.1f;
        private const float GhostSpeed = 0.04f;
        private const int DeathCooldownMilliseconds = 1000;

        private static Game _game;
        private static Canvas _canvas;
        private static LevelData _levelData;
        private static List<Type> _ghostTypes;
        private static int _ghostCounter;
        private static Action<int> _victoryAction, _defeatAction;
        private static int _score;

        internal static void InitializeGame (Game game, Canvas canvas,
                                LevelData levelData, List<Type> ghostTypes,
                                Action<int> victoryAction, Action<int> defeatAction, int score) {
            if (game == null) {
                throw new ArgumentNullException ("Game cannot " + "be null on game initialization");
            }
            _game = game;
            if (canvas == null) {
                throw new ArgumentNullException ("Canvas cannot " + "be null on game initialization");
            }
            _canvas = canvas;
            if (levelData == null) {
                throw new ArgumentNullException ("LevelData cannot " + "be null on game initialization");
            }
            _levelData = levelData;
            if (ghostTypes == null) {
                throw new ArgumentNullException ("Ghost types cannot " + "be null on game initialization");
            }
            _ghostTypes = ghostTypes;
            if (victoryAction == null) {
                throw new ArgumentNullException ("Victory action cannot " + "be null on game initialization");
            }
            _victoryAction = victoryAction;
            if (defeatAction == null) {
                throw new ArgumentNullException ("Defeat action cannot " + "be null on game initialization");
            }
            _defeatAction = defeatAction;
            _score = score;

            _ghostCounter = 0;
            LoadScene ();
        }

        private static void LoadScene () {
            CreateCamera ();
            CreateField ();
            CreateFieldObjects ();
            CreateScoreManager ();
            CreateWinLoseChecker ();
            CreateTextRenderers ();
        }

        private static void CreateCamera () {
            var g = _game.Instantiate ();
            g.Tag = Tags.CameraTag;
            g.AddComponent (new Camera (g, _canvas));
        }

        private static void CreateField () {
            var g = _game.Instantiate ();
            g.Tag = Tags.FieldTag;
            g.AddComponent (new Field(g, _levelData.FieldWidth, _levelData.FieldHeight));
        }

        private static void CreateFieldObjects () {
            foreach (var e in _levelData.Entities) {
                if (e.GridX < 0 || e.GridX >= _levelData.FieldWidth ||
                    e.GridY < 0 || e.GridY >= _levelData.FieldHeight) {
                    throw new ArgumentOutOfRangeException ("Grid coordinates (" + e.GridX + ":" + e.GridY + ")" +
                                                           " not matching to field parameters Grid width=" +
                                                           _levelData.FieldWidth +
                                                           ", Grid height=" + _levelData.FieldHeight);
                }

                const float cellSize = 1;
                var x = e.GridX + cellSize/2;
                var y = e.GridY + cellSize/2;

                switch (e.Type) {
                    case EntityTypes.Pill:
                        CreatePill (x, y);
                        break;

                    case EntityTypes.SuperPill:
                        CreateSuperPill (x, y);
                        break;

                    case EntityTypes.Wall:
                        CreateWall (x, y);
                        break;

                    case EntityTypes.Fruit:
                        CreateFruit (x, y);
                        break;

                    case EntityTypes.Player:
                        CreatePlayer (x, y);
                        break;

                    case EntityTypes.Ghost:
                        CreateGhost (x, y);
                        break;

                    default:
                        throw new ArgumentException ("Unknown level entity");
                }
            }
        }

        private static void CreateScoreManager () {
            var g = _game.Instantiate ();
            g.Tag = Tags.ScoreManagerTag;
            var s = g.AddComponent (new ScoreManager (g)) as ScoreManager;
            if (s == null) { throw new InvalidOperationException (); }
            s.Score = _score;
        }

        private static void CreateWinLoseChecker () {
            var g = _game.Instantiate ();
            var w = g.AddComponent (new WinLoseChecker (g)) as WinLoseChecker;
            if (w == null) { throw new InvalidOperationException ();}
            w.VictoryAction = _victoryAction;
            w.DefeatAction = _defeatAction;
        }

        private static void CreateTextRenderers () {
            var g = _game.Instantiate ();
            g.AddComponent (new ScoreRenderer (g));
            g = _game.Instantiate ();
            g.AddComponent (new LivesRenderer (g));
        }


        private static void CreatePlayer (float x, float y) {
            var g = _game.Instantiate (x, y);
            g.Tag = Tags.PlayerTag;
            var f = g.AddComponent (new FieldBody (g)) as FieldBody;
            if (f == null) { throw new InvalidOperationException ();}
            f.MaxSpeed = PlayerSpeed;

            var p = g.AddComponent (new Player (g)) as Player;
            if (p == null) { throw new InvalidOperationException ();}
            p.Lives = StartLives;

            g.AddComponent (new PlayerController (g));

            g.AddComponent (new PlayerRenderer (g));

            var c = g.AddComponent(new FieldCollider(g)) as FieldCollider;
            if (c == null) { throw new InvalidOperationException ();}
            c.Reaction = delegate (GameObject gameObject) {
                if (gameObject.Tag != Tags.GhostTag || p.PoweredUp) {
                    return;
                }
                c.Cooldown = DeathCooldownMilliseconds;
                if (p.Lives != 0) {  p.Lives--; }
                if (p.Lives == 0) {
                    _game.Destroy (g);
                    return;
                }
                g.Transform.Jump (x, y);
            };
        }

        private static void CreateWall (float x, float y) {
            var g = _game.Instantiate (x, y);
            var f = g.AddComponent(new FieldBody(g)) as FieldBody;
            if (f == null) { throw new InvalidOperationException ();}
            f.IsObstacle = true;
            g.AddComponent (new WallRenderer(g));
        }

        private static void CreatePill (float x, float y) {
            var g = _game.Instantiate (x, y);
            g.Tag = Tags.PillTag;
            g.AddComponent(new FieldBody(g));

            g.AddComponent (new PillRenderer (g));

            var c = g.AddComponent (new FieldCollider (g)) as FieldCollider;
            if (c == null) { throw new InvalidOperationException ();}
            c.Reaction = delegate (GameObject gameObject) {
                if (gameObject.Tag != Tags.PlayerTag) {
                    return;
                }
                AddPoints (PillReward);
                _game.Destroy (g);
            };
        }

        private static void CreateSuperPill (float x, float y) {
            var g = _game.Instantiate (x, y);
            g.Tag = Tags.SuperPillTag;

            g.AddComponent (new FieldBody (g));

            g.AddComponent (new SuperPillRenderer (g));

            var c = g.AddComponent (new FieldCollider (g)) as FieldCollider;
            if (c == null) { throw new InvalidOperationException ();}
            c.Reaction = delegate (GameObject gameObject) {
                if (gameObject.Tag != Tags.PlayerTag) {
                    return;
                }

                var p = gameObject.GetComponent<Player> ();
                if (gameObject.GetComponent<Player> () == null) { throw new InvalidOperationException ("Player doesn't have Player component!"); }
                p.PowerUp (PowerUpTimeMilliseconds);

                AddPoints (SuperPillReward);
                _game.Destroy (g);
            };
        }

        private static void CreateFruit (float x, float y) {
            var g = _game.Instantiate (x, y);
            g.Tag = Tags.FruitTag;

            g.AddComponent(new FieldBody(g));

            g.AddComponent (new FruitRenderer (g));

            var c = g.AddComponent (new FieldCollider (g)) as FieldCollider;
            if (c == null) { throw new InvalidOperationException ();}
            c.Reaction = delegate (GameObject gameObject) {
                if (gameObject.Tag != Tags.PlayerTag) {
                    return;
                }
                AddPoints (FruitReward);
                _game.Destroy (g);
            };
        }

        private static void CreateGhost (float x, float y) {
            var g = _game.Instantiate (x, y);
            g.Tag = Tags.GhostTag;
            var f = g.AddComponent(new FieldBody(g)) as FieldBody;
            if (f == null) { throw new InvalidOperationException ();}
            f.MaxSpeed = GhostSpeed;

            g.AddComponent (Activator.CreateInstance (_ghostTypes[_ghostCounter], g) as IComponent);
            _ghostCounter++;
            if (_ghostCounter >= _ghostTypes.Count) {
                _ghostCounter = 0;
            }

            g.AddComponent (new GhostRenderer (g));

            var c = g.AddComponent (new FieldCollider (g)) as FieldCollider;
            if (c == null) { throw new InvalidOperationException ();}
            c.Reaction = delegate (GameObject gameObject) {
                if (gameObject.Tag != Tags.PlayerTag) {
                    return;
                }
                var p = gameObject.GetComponent<Player> ();
                if (p == null) { throw new InvalidOperationException ("No Player found in Player GameObject"); }
                if (!p.PoweredUp) { return; }

                c.Cooldown = DeathCooldownMilliseconds;
                AddPoints (GhostReward);
                g.Transform.Jump(x, y);
            };
        }

        private static void AddPoints (int reward) {
            var gsm = _game.FindObjectWithTag (Tags.ScoreManagerTag);
            if (gsm == null) {
                throw new InvalidOperationException ("ScoreManager not found on the scene by tag");
            }
            var scoreManager = gsm.GetComponent<ScoreManager> ();
            if (scoreManager == null) {
                throw new InvalidOperationException ("ScoreManager doesn't have ScoreManager component!");
            }
            scoreManager.Score += reward;
        }

    }
}
