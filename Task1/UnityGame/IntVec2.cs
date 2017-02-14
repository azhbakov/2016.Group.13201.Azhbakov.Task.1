
using JetBrains.Annotations;

namespace Task1.UnityGame {
    public sealed class IntVec2 {
        [UsedImplicitly]
        public int X { get; set; }
        [UsedImplicitly]
        public int Y { get; set; }

        public IntVec2 (int x, int y) {
            X = x;
            Y = y;
        }

    }
}
