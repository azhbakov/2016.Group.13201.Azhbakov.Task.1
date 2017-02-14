using System.Windows.Media;
using System.Windows.Shapes;
using Task1.UnityGame;

namespace Task1.UnityRenderers {
    internal sealed class SuperPillRenderer : ElementRenderer {
        private const float SizeMul = 0.33f;

        internal SuperPillRenderer (GameObject gameObject)
            : base (gameObject, new Ellipse {
                Fill = new SolidColorBrush (Colors.Coral)
            }) {
        }

        protected override void UpdateSize () {
            Element.Width = CellSize * SizeMul;
            Element.Height = CellSize * SizeMul;
        }
    }
}
