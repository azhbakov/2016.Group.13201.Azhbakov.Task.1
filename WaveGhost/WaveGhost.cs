using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace WaveGhost {
    [UsedImplicitly]
    public sealed class WaveGhost : IComponent {
        private GameObject GameObject { get; }

        private FieldBody _fieldBody;
        private Player _player;
        private float _spawnX, _spawnY;

        private int[,] _map;
        private const int ObstacleMarker = -1;
        private const int EmptyMarker = -2;

        private List<IntVec2> _cellsToProcess = new List<IntVec2> ();

        public WaveGhost (GameObject gameObject) {
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

            _player = GameObject.Game.FindComponentByTag<Player> (Tags.PlayerTag);

            _spawnX = _fieldBody.Transform.X;
            _spawnY = _fieldBody.Transform.Y;
        }

        public void Update () {
            _fieldBody.Move(GetDirByWave ());
        }

        public void Destroy () { }

        private void InitMap () {
            _map = new int[_fieldBody.Field.Width, _fieldBody.Field.Height];

            // Init obstacles
            for (var x = 0; x < _fieldBody.Field.Width; x++) {
                for (var y = 0; y < _fieldBody.Field.Height; y++) {
                    if (_fieldBody.Field.ObstacleAtCell (x, y) != null) {
                        _map[x, y] = ObstacleMarker;
                    }
                }
            }
        }

        private void ResetMap () {
            _cellsToProcess.Clear ();
            for (var x = 0; x < _fieldBody.Field.Width; x++) {
                for (var y = 0; y < _fieldBody.Field.Height; y++) {
                    if (_map[x, y] != ObstacleMarker) {
                        _map[x, y] = EmptyMarker;
                    }
                }
            }
        }

        private void WaveStep () {
            var newCells = new List<IntVec2> ();
            foreach (var p in _cellsToProcess) {
                var d = _map[p.X, p.Y];

                for (var i = -1; i <= 1; i++) {
                    for (var j = -1; j <= 1; j++) {
                        if ((i == 0 || j != 0) && (i != 0 || j == 0)) { continue;}
                        var x = p.X + i;
                        var y = p.Y + j;
                        if (_map[x, y] != EmptyMarker) { continue;}
                        _map[x, y] = d + 1;
                        newCells.Add (new IntVec2 (x, y));
                    }
                }
            }
            _cellsToProcess = newCells;
        }

        private DirectionUtils.Directions GetDirByWave () {
            if (_map == null) {
                InitMap ();
            }
            ResetMap ();

            var start = _fieldBody.Field.WorldToGrid (GameObject.Transform.X, GameObject.Transform.Y);
            if (_map == null) {
                throw new InvalidOperationException("Map must be initialized in WaveGhost");
            }
            _map[start.X, start.Y] = 0;
            _cellsToProcess.Add (start);

            var finish = _player.PoweredUp ? _fieldBody.Field.WorldToGrid (_spawnX, _spawnY) : _fieldBody.Field.WorldToGrid (_player.GameObject.Transform.X, _player.GameObject.Transform.Y);

            while (_map[finish.X, finish.Y] == EmptyMarker || _cellsToProcess.Count != 0) {
                WaveStep ();
            }

            var nextPoint = GetNextPoint (finish);
            if (nextPoint?.X < start.X) { return DirectionUtils.Directions.Left; }
            if (nextPoint?.X > start.X) { return DirectionUtils.Directions.Right; }
            if (nextPoint?.Y < start.Y) { return DirectionUtils.Directions.Down; }
            if (nextPoint?.Y > start.Y) { return DirectionUtils.Directions.Up; }
            return DirectionUtils.Directions.No;
        }

        private IntVec2 GetNextPoint (IntVec2 finish) {
            if (_map[finish.X, finish.Y] == EmptyMarker) {
                return null;
            }
            if (_map[finish.X, finish.Y] == 0) {
                return finish;
            }

            var currentX = finish.X;
            var currentY = finish.Y;

            while (_map[currentX, currentY] != 1) {
                var d = _map[currentX, currentY];

                var found = false;
                for (var i = -1; i <= 1; i++) {
                    for (var j = -1; j <= 1; j++) {
                        if ((i == 0 || j != 0) && (i != 0 || j == 0)) { continue;}
                        var nextX = currentX + i;
                        var nextY = currentY + j;
                        if (_map[nextX, nextY] != d - 1) { continue;}
                        currentX = nextX;
                        currentY = nextY;
                        found = true;
                        break;
                    }
                    if (found) { break; }
                }
            }
            return new IntVec2 (currentX, currentY);
        }

    }
}
