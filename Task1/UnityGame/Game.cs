using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Task1.UnityGame {
    internal sealed class Game : IGame {
        private readonly Input _input = new Input();
        private readonly List<GameObject> _gameObjects = new List<GameObject> ();
        private readonly List<GameObject> _toBeAdded = new List<GameObject> ();
        private readonly List<GameObject> _toBeDestroyed = new List<GameObject> ();
        private bool _started;

        public Input Input () {
            return _input;
        }

        public GameObject Instantiate (float x = 0f, float y = 0f) {
            var g = new GameObject (this, x, y);
            _toBeAdded.Add (g);
            return g;
        }

        internal void Reset () {
            _toBeAdded.Clear();
            _toBeDestroyed.Clear();
            foreach (var g in _gameObjects) {
                Destroy(g);
            }
           _gameObjects.Clear();
            _started = false;
        }

        internal void Destroy (GameObject g) {
            if (g == null) {
                throw new ArgumentNullException();
            }
            if (!_gameObjects.Contains (g)) {
                throw new ArgumentException("Trying to destroy unknown gameobject");
            }

            _toBeDestroyed.Add (g);
        }

        internal void Update () {
            foreach (var g in _toBeAdded) {
                _gameObjects.Add (g);
            }
            _toBeAdded.Clear ();

            if (!_started) {
                foreach (var g in _gameObjects) {
                    g.Start ();
                }
                _started = true;
            }

            foreach (var g in _gameObjects) {
                g.Update ();
                if (!_started) { break; }
            }

            foreach (var g in _toBeDestroyed) {
                g.Destroy();
                _gameObjects.Remove (g);
            }
            _toBeDestroyed.Clear ();

            _input.Reset ();
        }

        public GameObject FindObjectWithTag (string tag) {
            foreach (var g in _gameObjects) {
                if (g.Tag != null && g.Tag.Equals (tag)) {
                    return g;
                }
            }
            return null;
        }

        public List<GameObject> FindObjectsWithTag (string tag) {
            var list = new List<GameObject> ();
            foreach (var g in _gameObjects) {
                if (g.Tag != null && g.Tag.Equals (tag)) {
                    list.Add(g);
                }
            }
            return list.Count == 0 ? null : list;
        }

        public TComponentType FindComponentByTag<TComponentType> (string tag) where TComponentType : class, IComponent {
            var g = FindObjectWithTag (tag);
            if (g == null) {
                throw new InvalidOperationException ("Object not found on the scene by tag");
            }
            var component = g.GetComponent<TComponentType> ();
            if (component == null) {
                throw new InvalidOperationException ("GameObject doesn't have specified component!");
            }
            return component;
        }

        internal void ButtonPressed (Key key) {
            _input.PressKey (key);
        }
    }
}
