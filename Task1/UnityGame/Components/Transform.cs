namespace Task1.UnityGame.Components {
    public sealed class Transform {
        internal GameObject GameObject { get; }
        public float X { get; private set; }
        public float Y { get; private set; }

        internal Transform (GameObject gameObject, float x, float y) {
            GameObject = gameObject;
            X = x;
            Y = y;
        }

        internal void Translate (float dx, float dy) {
            X += dx;
            Y += dy;
        }

        internal void Jump (float x, float y) {
            X = x;
            Y = y;
        }
    }
}
