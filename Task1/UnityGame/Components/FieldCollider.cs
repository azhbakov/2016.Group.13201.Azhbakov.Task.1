using System;
using System.Diagnostics;

namespace Task1.UnityGame.Components
{
    internal sealed class FieldCollider : IComponent {
        private GameObject GameObject { get; }

        private int _cooldown;
        internal int Cooldown {
            set {
                _cooldown = value;
                _stopwatch.Reset ();
                _stopwatch.Start();
            }
        }
        internal Action<GameObject> Reaction { private get; set; }
        private FieldBody _fieldBody;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        internal FieldCollider (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            _fieldBody = GameObject.GetComponent<FieldBody> ();
            if (_fieldBody == null) {
                throw new InvalidOperationException ("FieldBody component not found in GameObject with FieldCollider");
            }
        }
        public void Update () {
            if (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds < _cooldown) {
                return;
            }
            _stopwatch.Reset();
            if (Reaction == null) { return; }

            var collidedObjects = _fieldBody.GetTransformsInCurrentCell();
            foreach (var t in collidedObjects)
            {
                Reaction(t.GameObject);
                break;
            }
        }
        public void Destroy () { }
    }
}
