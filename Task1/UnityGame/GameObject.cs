using System;
using System.Collections.Generic;
using Task1.UnityGame.Components;

namespace Task1.UnityGame {
    public sealed class GameObject {
        public IGame Game { get; }
        public Transform Transform { get; }
        internal string Tag { get; set; }

        private bool _started;

        private readonly List<IComponent> _components = new List<IComponent> ();

        internal GameObject (IGame game, float x, float y) {
            Game = game;
            Transform = new Transform(this, x, y);
        }

        internal IComponent AddComponent (IComponent component) {
            if (component == null) {
                throw new ArgumentNullException("Component cannot"+" be null");
            }
            _components.Add(component);
            return component;
        }

        public TComponentType GetComponent<TComponentType> () where TComponentType : class {
            foreach (var c in _components) {
                if (c is TComponentType) {
                    return c as TComponentType;
                }
            }
            return null;
        }

        internal void Start () {
            foreach (var component in _components) {
                component.Start();
            }
            _started = true;
        }

        internal void Update () {
            if (!_started) {
                Start();
            }
            foreach (var component in _components) {
                component.Update ();
            }
        }

        internal void Destroy () {
            foreach (var component in _components) {
                component.Destroy();
            }
        }
    }
}
