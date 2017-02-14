using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace Task1.UnityRenderers {
    internal sealed class LivesRenderer : IRenderer {
        private GameObject GameObject { get; }
        private Camera _camera;

        private readonly TextBlock _element = new TextBlock {
            Foreground = Brushes.Firebrick,
            FontSize = 20,
            FontWeight = FontWeights.Bold,
            TextAlignment = TextAlignment.Left,
            Width = 70,
            Height = 30
        };
        private Point _position;

        private const int ScoreTextXOffset = 20;
        private const int ScoreTextYOffset = 30;

        private Player _player;

        internal LivesRenderer (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public FrameworkElement GetElement () {
            return _element;
        }

        public void UpdateGraphics (double canvasWidth, double canvasHeight) {
            _element.Text = "Lives: " + _player.Lives;
            _position.X = canvasWidth - ScoreTextXOffset - _element.Width;
            _position.Y = canvasHeight - ScoreTextYOffset;
        }

        public double GetPositionX () {
            return _position.X;
        }

        public double GetPositionY () {
            return _position.Y;
        }

        public void Start () {
            _camera = GameObject.Game.FindComponentByTag<Camera> (Tags.CameraTag);
            _camera.AddRenderer (this);

            _player = GameObject.Game.FindComponentByTag<Player> (Tags.PlayerTag);
        }
        public void Update () { }
        public void Destroy () {
            _camera.RemoveRenderer (this);
        }
    }
}
