using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace Task1.UnityRenderers {

    internal sealed class FinalTextRenderer : IRenderer {
        private GameObject GameObject { get; }
        private Camera _camera;

        private readonly TextBlock _element;
        internal string Text { private get; set; }

        private Point _position;

        internal FinalTextRenderer (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;

            _element = new TextBlock () {
                Foreground = Brushes.Black,
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 100,
            };
        }

        public FrameworkElement GetElement () {
            return _element;
        }

        public void UpdateGraphics (double canvasWidth, double canvasHeight) {
            _element.Text = Text;
            _element.Width = canvasWidth;
            _position.X = 0;
            _position.Y = canvasHeight/2 - _element.Height/2;
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
        }
        public void Update () { }
        public void Destroy () {
            _camera.RemoveRenderer (this);
        }
    }
}
