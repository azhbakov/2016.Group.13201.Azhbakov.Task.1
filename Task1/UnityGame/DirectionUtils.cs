using System.Collections.Generic;

namespace Task1.UnityGame {
    public static class DirectionUtils {
        public enum Directions { Up, Right, Down, Left, No } // DO NOT CHANGE ORDER, DO NOT REMOVE
        private static readonly List<Direction> RegisteredDirections = new List<Direction> ();

        static DirectionUtils () { // DO NOT CHANGE ORDER, DO NOT REMOVE
            RegisteredDirections.Add(new Direction(Directions.Up, 0, 1));
            RegisteredDirections.Add (new Direction (Directions.Right, 1, 0));
            RegisteredDirections.Add (new Direction (Directions.Down, 0, -1));
            RegisteredDirections.Add (new Direction (Directions.Left, -1, 0));
            RegisteredDirections.Add (new Direction (Directions.No, 0, 0));
        }

        internal static int X (this Directions direction) {
            return RegisteredDirections[(int) direction].X;
        }

        internal static int Y (this Directions direction) {
            return RegisteredDirections[(int) direction].Y;
        }

        public static Directions AtRight (this Directions direction) {
            return RegisteredDirections[((int) direction + 1) % (RegisteredDirections.Count - 1)].Name;
        }

        public static Directions AtLeft (this Directions direction) {
            return RegisteredDirections[((int) direction+RegisteredDirections.Count-1 - 1) % (RegisteredDirections.Count - 1)].Name;
        }

        internal static bool IsOpposite (this Directions one, Directions another) {
            return one.X ()*-1 == another.X () && one.Y ()*-1 == another.Y ();
        }

        internal static bool MovingX (this Directions direction) {
            return direction.X () != 0;
        }
        internal static bool MovingPositive (this Directions direction) {
            return direction.X () > 0 || direction.Y () > 0;
        }

        private sealed class Direction {
            internal Directions Name { get; }
            internal int X { get; }
            internal int Y { get; }

            internal Direction (Directions name, int x, int y) {
                Name = name;
                X = x;
                Y = y;
            }
        }
    }
}
