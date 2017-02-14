using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Task1.UnityGame.Components {
    internal sealed class Camera : IComponent {
        private readonly List<IRenderer> _renderers = new List<IRenderer> ();
        private readonly Canvas _canvas;

        internal Camera (GameObject gameObject, Canvas canvas) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }

            if (canvas == null) {
                throw new ArgumentNullException ("Canvas cannot be null on" + " camera initialization");
            }
            _canvas = canvas;
            _canvas.Background = new SolidColorBrush (Colors.Gray);
            _canvas.UpdateLayout();
        }

        internal void AddRenderer (IRenderer renderer) {
            if (renderer == null) {
                throw new ArgumentNullException("Renderer cannot be "+" null");
            }
            _renderers.Add(renderer);
            _canvas.Children.Add (renderer.GetElement());
        }

        internal void RemoveRenderer (IRenderer renderer) {
            if (renderer == null) {
                throw new ArgumentNullException ("Renderer cannot be " + " null");
            }
            if (!_renderers.Contains (renderer)) {
                throw new ArgumentException("No such renderer "+"found!");
            }
            if (!_canvas.Children.Contains (renderer.GetElement ())) {
                throw new ArgumentException ("No such element " + "found in canvas!");
            }
            _renderers.Remove (renderer);
            _canvas.Children.Remove(renderer.GetElement());
        }

        public void Start () {
            
        }
        public void Update () {
            Repaint ();
        }
        public void Destroy () {

        }

        private void Repaint () {
            foreach (var renderer in _renderers) {
                renderer.UpdateGraphics(_canvas.ActualWidth, _canvas.ActualHeight);
                UpdatePosition (renderer);
            }
        }


        private void UpdatePosition (IRenderer renderer) {
            if (renderer == null) {
                throw new ArgumentNullException ("Renderer cannot be " + " null");
            }
            var element = renderer.GetElement ();
            if (!_canvas.Children.Contains (element)) {
                throw new ArgumentException("Canvas doesn't contain such "+"element");
            }

            Canvas.SetLeft(element, Math.Ceiling (renderer.GetPositionX ()));
            Canvas.SetTop (element, Math.Ceiling (renderer.GetPositionY ()));
        }
    }
}
