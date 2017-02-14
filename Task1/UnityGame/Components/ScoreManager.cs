using System;

namespace Task1.UnityGame.Components {
    internal sealed class ScoreManager : IComponent {
        internal ScoreManager (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
        }

        internal int Score { get; set; }

        public void Start () { }
        public void Update () { }
        public void Destroy () { }

    }
}
