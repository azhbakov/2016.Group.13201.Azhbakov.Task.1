using System;
using System.Diagnostics;

namespace Task1.UnityGame.Components {
    public sealed class Player : IComponent {
        public GameObject GameObject { get; private set; }

        private readonly Stopwatch _powerUpStopwatch = new Stopwatch();
        private int _powerUpMaxTimeMillis;

        internal Player (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        internal int Lives { get; set; }
        public bool PoweredUp => _powerUpStopwatch.IsRunning;

        internal void PowerUp (int timeInMillis) {
            if (timeInMillis < 0) { throw new ArgumentOutOfRangeException("Power-up "+"time must be positive");}
            _powerUpMaxTimeMillis = timeInMillis;
            _powerUpStopwatch.Start();
        }

        public void Start () { }
        public void Update () {
            if (_powerUpStopwatch.ElapsedMilliseconds >= _powerUpMaxTimeMillis) {
                _powerUpStopwatch.Reset();
            }
        }
        public void Destroy () { }
    }
}
