using System;
using System.Collections.Generic;

namespace Task1.UnityGame.Components {
    public sealed class FieldBody : IComponent {
        private GameObject GameObject { get; }

        public Field Field { get; private set; }

        public Transform Transform { get; private set; }

        public DirectionUtils.Directions Direction { get; private set; } = DirectionUtils.Directions.No;
        private DirectionUtils.Directions _desiredDirection = DirectionUtils.Directions.No;

        private const float Eps = 0.001f;

        private bool NeedTurn => Direction != _desiredDirection;

        internal float MaxSpeed { private get; set; }
        internal bool IsObstacle { private get; set; }

        internal FieldBody (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException("GameObject cannot be null on"+" component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            var g = GameObject.Game.FindObjectWithTag (Tags.FieldTag);
            if (g == null) {
                throw new InvalidOperationException("Field GameObject not found by tag");
            }
            Field = g.GetComponent<Field> ();
            if (Field == null) {
                throw new InvalidOperationException("Field component not found in Field GameObject");
            }
            Transform = GameObject.Transform;

            if (IsObstacle) {
                Field.AddObstacle (Transform);
            } else {
                Field.AddMovingTransform (Transform);
            }
        }

        public void Update () {
            Move ();
        }

        public void Destroy () {
            if (IsObstacle) {
                Field.RemoveObstacle (Transform);
            } else {
                Field.RemoveMovingTransform (Transform);
            }
        }

        private void Move () {
            if (Math.Abs (MaxSpeed) < Eps) { return; }
            var amountToMove = MaxSpeed;

            while (amountToMove > 0)
            {
                Step(ref amountToMove);
            }
        }

        private void Step (ref float amountToMove) 
        {
            var toObstacle = ClampByObstacle(amountToMove);
            // If dont need to turn - move forward while you can
            if (!NeedTurn)
            {
                Transform.Translate(toObstacle * Direction.X(), toObstacle * Direction.Y());
                amountToMove = 0; // we dont continue moving when hit obstacle
                return;
            }

            // If we need turn
            // If we can make turn immediately - do it
            if (Direction == DirectionUtils.Directions.No || Direction.IsOpposite(_desiredDirection))
            {
                Direction = _desiredDirection;
                return;
            }

            // If we need to travel some distance before making turn
            var toTurn = ClampByTurn(amountToMove);
            if (toObstacle < toTurn)
            {
                Transform.Translate(toObstacle * Direction.X(), toObstacle * Direction.Y());
                amountToMove = 0; // we dont continue moving when hit obstacle
                return;
            }

            Transform.Translate(toTurn * Direction.X(), toTurn * Direction.Y());
            amountToMove -= toTurn;
            if (toTurn < amountToMove) // dont turn if it is not reached yet
            {
                Direction = _desiredDirection;
            }
        }

        private float ClampByTurn (float amountToMove) {
            var currentCell = Field.WorldToGrid(Transform.X, Transform.Y);
            var transform = Field.ObstacleAtCell(currentCell.X + _desiredDirection.X(), currentCell.Y + _desiredDirection.Y());
            if (transform != null)
            {
                return amountToMove;
            }

            var pos = Direction.MovingX() ? Transform.X : Transform.Y;

            float before, after;
            var t = (int)(pos / (Field.CellSize / 2));
            if (t % 2 == 1) { // we are in right part of cell
                before = t * Field.CellSize / 2;
                after = before + Field.CellSize;
            } else { // we are in left part of cell
                after = t * Field.CellSize/2 + Field.CellSize/2;
                before = after - Field.CellSize;
            }

            float dist;
            if (Direction.MovingPositive()) {
                dist = after - pos;
            } else {
                dist = pos - before;
            }
            if (Math.Abs (pos - before) < Eps || Math.Abs (pos - after) < Eps) {
                dist = 0;
            }
            return Math.Min(dist, amountToMove);
        }

        private float ClampByObstacle (float amountToMove) {
            var nextCell = Field.WorldToGrid(Transform.X + Direction.X() * Field.CellSize / 2 + Direction.X() * amountToMove,
                                                Transform.Y + Direction.Y() * Field.CellSize / 2 + Direction.Y() * amountToMove);

            // If not colliding
            var transform = Field.ObstacleAtCell(nextCell.X, nextCell.Y);
            if (transform == null) {
                return amountToMove;
            }

            // If colliding
            var cellBorder = (nextCell.X*Math.Abs(Direction.X ()) + nextCell.Y* Math.Abs (Direction.Y ()))*Field.CellSize;
            if (!Direction.MovingPositive()) { cellBorder += Field.CellSize;}

            var playersBorder = (Transform.X + (Field.CellSize/2)*Direction.X ())* Math.Abs (Direction.X ()) +
                            +(Transform.Y + (Field.CellSize/2)*Direction.Y ())* Math.Abs (Direction.Y ());

            amountToMove = playersBorder - cellBorder;
            return Math.Min(Math.Abs(amountToMove), MaxSpeed);
        }

        internal List<Transform> GetTransformsInCurrentCell ()
        {
            var currentCell = Field.WorldToGrid(Transform.X, Transform.Y);
            var list = Field.MovingTransformAtCell (currentCell.X, currentCell.Y);
            if (!list.Remove (Transform)) {
                throw new InvalidOperationException("FieldBody is not presented in it's own cell!");
            }
            return list;
        }


        internal void MoveLeft () {
            _desiredDirection = DirectionUtils.Directions.Left;
        }
        internal void MoveRight () {
            _desiredDirection = DirectionUtils.Directions.Right;
        }
        internal void MoveDown () {
            _desiredDirection = DirectionUtils.Directions.Down;
        }

        public void MoveUp () {
            _desiredDirection = DirectionUtils.Directions.Up;
        }
        public void Move (DirectionUtils.Directions direction) {
            _desiredDirection = direction;
        }

        public bool ClearAtRight () {
            return ClearAtDirection (Direction.AtRight());
        }

        public bool ClearAtLeft () {
            return ClearAtDirection (Direction.AtLeft());
        }

        public bool ClearAtFront () {
            return ClearAtDirection (Direction != DirectionUtils.Directions.No ? Direction : DirectionUtils.Directions.Up);
        }

        private bool ClearAtDirection (DirectionUtils.Directions direction) {
            var gridPos = Field.WorldToGrid (Transform.X, Transform.Y);
            return Field.ObstacleAtCell (gridPos.X + direction.X(),
                       gridPos.Y + direction.Y()) == null;
        }
    }
}
