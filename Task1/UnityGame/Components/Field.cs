using System;
using System.Collections.Generic;

namespace Task1.UnityGame.Components {
    public sealed class Field : IComponent {
        private readonly List<Transform> _movingTransforms = new List<Transform> ();
        private readonly Transform[,] _obstacles;

        internal const float CellSize = 1;
        public int Width { get; }
        public int Height { get; }


        internal Field (GameObject gameObject, int width, int height) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }

            if (width <= 0 || height <= 0) {
                throw new ArgumentOutOfRangeException ("Field width and" + " height must be positive");
            }
            Width = width;
            Height = height;
            _obstacles = new Transform[Width, Height];
        }


        public void Start () {
        }
        public void Update () { }
        public void Destroy () { }

        public IntVec2 WorldToGrid (float worldX, float worldY) {
            if (worldX < 0 || worldX >= Width*CellSize ||
                worldY < 0 || worldY >= Height*CellSize) {
                throw new ArgumentOutOfRangeException ("Point doesn't "+"belong to field");
            }
            var x = (int) (worldX/CellSize);
            var y = (int) (worldY/CellSize);
            return new IntVec2(x, y);
        }

        public Transform ObstacleAtCell (int gridX, int gridY) {
            if (gridX < 0 || gridX >= Width ||
                gridY < 0 || gridY >= Height) {
                throw new ArgumentOutOfRangeException ("Point doesn't " + "belong to field");
            }
            if (_obstacles[gridX, gridY] != null) {
                return _obstacles[gridX, gridY];
            }
            return null;
        }

        internal List<Transform> MovingTransformAtCell(int gridX, int gridY)
        {
            if (gridX < 0 || gridX >= Width ||
                gridY < 0 || gridY >= Height)
            {
                throw new ArgumentOutOfRangeException("Point doesn't " + "belong to field");
            }
            var list = new List<Transform>();
            foreach (var t in _movingTransforms) {
                IntVec2 gridPos = WorldToGrid (t.X, t.Y);
                if (gridPos.X == gridX && gridPos.Y == gridY) {
                    list.Add(t);
                }
            }
            return list;
        }

        internal void AddObstacle (Transform obstacle) {
            if (obstacle == null) {
                throw new ArgumentNullException("Obstacle cannot be"+" null");
            }
            IntVec2 gridPos = WorldToGrid (obstacle.X, obstacle.Y);
            if (_obstacles[gridPos.X, gridPos.Y] != null) {
                throw new ArgumentException("Cell " + gridPos.X + ":" + gridPos.Y + " is already occupied!");
            }
            _obstacles[gridPos.X, gridPos.Y] = obstacle;
        }

        internal void AddMovingTransform (Transform transform) {
            if (transform == null) {
                throw new ArgumentNullException ("Transform cannot be" + " null");
            }
            _movingTransforms.Add(transform);
        }

        internal void RemoveObstacle (Transform transform) {
            if (transform == null) {
                throw new ArgumentNullException ("Transform cannot be" + " null");
            }
            var gridPos = WorldToGrid (transform.X, transform.Y);
            if (_obstacles[gridPos.X, gridPos.Y] != transform) {
                throw new InvalidOperationException ("Transform not " + "found in field");
            }
            _obstacles[gridPos.X, gridPos.Y] = null;
        }

        internal void RemoveMovingTransform (Transform transform) {
            if (transform == null) {
                throw new ArgumentNullException ("Transform cannot be" + " null");
            }
            if (!_movingTransforms.Contains(transform)) {
                throw new InvalidOperationException ("Transform not " + "found in field");
            }
            _movingTransforms.Remove (transform);
        }
    }
}
