using System;

namespace Task1.UnityGame.Components {
    internal sealed class PlayerController : IComponent {
        private GameObject GameObject { get; }

        private FieldBody _fieldBody;

        internal PlayerController (GameObject gameObject) {
            if (gameObject == null) {
                throw new ArgumentNullException ("GameObject cannot be null on" + " component initialization");
            }
            GameObject = gameObject;
        }

        public void Start () {
            _fieldBody = GameObject.GetComponent<FieldBody> ();
            if (_fieldBody == null) {
                throw new InvalidOperationException ("Player GameObject doesn't have FieldBody component");
            }
        }

        public void Update () {
            if (GameObject.Game.Input().GetKey (Actions.Up)) {
                _fieldBody.MoveUp();
            }
            if (GameObject.Game.Input().GetKey (Actions.Left)) {
                _fieldBody.MoveLeft ();
            }
            if (GameObject.Game.Input().GetKey (Actions.Right)) {
                _fieldBody.MoveRight();
            }
            if (GameObject.Game.Input().GetKey (Actions.Down)) {
                _fieldBody.MoveDown ();
            }
        }

        public void Destroy () {
            
        }
    }
}
