using System;
using System.Windows;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace Task1.UnityRenderers {
    internal class ElementRenderer : IRenderer {
        internal GameObject GameObject { get; }

        private readonly Transform _transform;
        protected readonly FrameworkElement Element;

        private Field _field;
        private Camera _camera;

        protected float CellSize;
        private double _elementX;
        private double _elementY;

        internal ElementRenderer (GameObject gameObject, FrameworkElement element) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;

            _transform = GameObject.Transform;

            Element = element;
        }

        public FrameworkElement GetElement () {
            return Element;
        }

        public virtual void Start () {
            _field = GameObject.Game.FindComponentByTag<Field> (Tags.FieldTag);

            _camera = GameObject.Game.FindComponentByTag<Camera> (Tags.CameraTag);
            _camera.AddRenderer(this);
        }
        public virtual void Update () { }
        public void Destroy () {
            _camera.RemoveRenderer(this);
        }

        public virtual void UpdateGraphics (double canvasWidth, double canvasHeight) {
            UpdateCellSize (canvasWidth, canvasHeight);
            UpdateSize ();
            var canvasPosition = WorldToCanvas (canvasWidth, canvasHeight);
            _elementX = canvasPosition.X - Math.Ceiling(Element.Width/2);
            _elementY = canvasHeight - canvasPosition.Y - Math.Floor (Element.Height/2);
        }

        public double GetPositionX () {
            return _elementX;
        }

        public double GetPositionY () {
            return _elementY;
        }

        protected virtual void UpdateSize () {
            Element.Width = Math.Ceiling(CellSize);
            Element.Height = Math.Ceiling (CellSize);
        }

        private void UpdateCellSize (double canvasWidth, double canvasHeight) {
            var hRatio = canvasWidth / _field.Width;
            var vRatio = canvasHeight / _field.Height;
            CellSize = (float)Math.Min (hRatio, vRatio);
        }

        private Point WorldToCanvas (double canvasWidth, double canvasHeight) {
            var worldX = _transform.X;
            var worldY = _transform.Y;

            var normX = worldX / _field.Width;
            var normY = worldY / _field.Height;

            var fieldWidth = CellSize * _field.Width;
            var fieldHeight = CellSize * _field.Height;

            var offsetX = (canvasWidth - fieldWidth) / 2;
            var offsetY = (canvasHeight - fieldHeight) / 2;

            var canvasX = offsetX + normX * fieldWidth;
            var canvasY = offsetY + normY * fieldHeight;

            return new Point (canvasX, canvasY);
        }
    }
}
