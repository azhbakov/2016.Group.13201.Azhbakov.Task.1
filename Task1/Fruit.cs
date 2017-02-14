using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan2 {
    class Fruit : Transform, IDestroyable {

        private event Action DestroyEvent;

        internal Fruit (float x, float y) : base (x, y) { }

        internal override bool CollideWith (Transform t) {
            Destroy ();
            return false;
        }

        public virtual void Destroy () {
            DestroyEvent?.Invoke ();
        }

        public void SubscribeToDestroy (Action reaction) {
            DestroyEvent += reaction;
        }
    }
}
