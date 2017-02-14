using System;
using JetBrains.Annotations;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace SimpleGhost {
    [UsedImplicitly]
    public sealed class RightHandGhost : IComponent {
        private GameObject GameObject { get; }

        private FieldBody _fieldBody;
        private int _lastGridX, _lastGridY;

        public RightHandGhost (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            _fieldBody = GameObject.GetComponent<FieldBody> ();
            if (_fieldBody == null) {
                throw new InvalidOperationException ("Ghost GameObject doesn't have FieldBody component");
            }
            _fieldBody.MoveUp ();
        }
        public void Update () {
            var gridPos = _fieldBody.Field.WorldToGrid (_fieldBody.Transform.X, _fieldBody.Transform.Y);

            if (gridPos.X == _lastGridX && gridPos.Y == _lastGridY) { return;}
            _lastGridX = gridPos.X;
            _lastGridY = gridPos.Y;

            if (!_fieldBody.ClearAtFront ()) {
                _fieldBody.Move (_fieldBody.ClearAtLeft () ? _fieldBody.Direction.AtLeft() : _fieldBody.Direction.AtLeft().AtLeft());
            }
            if (_fieldBody.ClearAtRight ()) {
                _fieldBody.Move (_fieldBody.Direction.AtRight());
            }
        }
        public void Destroy () { }

    }
}
