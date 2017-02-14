using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Task1.UnityGame {
    public sealed class Input {
        private readonly List<Key> _pressedKeys = new List<Key> ();
        internal List<KeyBinding> Binds { get; }

        internal Input () {
            Binds = new List<KeyBinding> ();
        }

        internal void PressKey (Key key) {
            if (!_pressedKeys.Contains (key)) {
                _pressedKeys.Add(key);
            }
        }

        private bool GetKey (Key key) {
            return _pressedKeys.Contains (key);
        }
        internal bool GetKey (string name) {
            return GetKey (GetBind(name));
        }

        internal Key GetBind (string name) {
            Key? key = null;
            foreach (var b in Binds) {
                if (b.Name.Equals (name)) {
                    key = b.Key;
                }
            }
            if (key == null) {
                throw new ArgumentException ("No key action " + name + " registered!");
            }
            return key.Value;
        }

        internal void RegisterAction (string name, Key key) {
            foreach (var b in Binds) {
                if (!b.Name.Equals (name)) { continue;}
                b.Key = key;
                return;
            }
            Binds.Add(new KeyBinding (name, key));
        }

        internal void Reset () {
            _pressedKeys.Clear();
        }
    }

    internal sealed class KeyBinding {
        public string Name { get; }
        public Key Key { get; set; }

        internal KeyBinding (string name, Key key) {
            Name = name;
            Key = key;
        }
    }
}
