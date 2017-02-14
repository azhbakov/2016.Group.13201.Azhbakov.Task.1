using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace RandomGhost {
    [UsedImplicitly]
    public sealed class RandomGhost : IComponent {
        private GameObject GameObject { get; }

        private FieldBody _fieldBody;

        private readonly List<DirectionUtils.Directions> _options = new List<DirectionUtils.Directions> ();
        private int _lastGridX, _lastGridY;
        private readonly Random _random = new Random();

        public RandomGhost (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            _fieldBody = GameObject.GetComponent<FieldBody> ();
            if (GameObject.GetComponent<FieldBody> () == null) {
                throw new InvalidOperationException ("No FieldBody component in Ghost");
            }
        }
        public void Update () {
            var gridPos = _fieldBody.Field.WorldToGrid (_fieldBody.Transform.X, _fieldBody.Transform.Y);

            if (gridPos.X == _lastGridX && gridPos.Y == _lastGridY) { return; }

            if (_fieldBody.Direction == DirectionUtils.Directions.No) { _fieldBody.MoveUp();}

            _options.Clear();
            if (_fieldBody.ClearAtRight ()) {
                _options.Add (_fieldBody.Direction.AtRight());
            }
            if (_fieldBody.ClearAtLeft ()) {
                _options.Add (_fieldBody.Direction.AtLeft());
            }
            if (_fieldBody.ClearAtFront ()) {
                _options.Add (_fieldBody.Direction);
            }
            if (_options.Count == 0) {
                _options.Add (_fieldBody.Direction.AtLeft().AtLeft());
            }
            if (_options.Count != 0) {
                _fieldBody.Move (_options[_random.Next (_options.Count)]);
            }
            _lastGridX = gridPos.X;
            _lastGridY = gridPos.Y;
        }
        public void Destroy () { }

    }
}
