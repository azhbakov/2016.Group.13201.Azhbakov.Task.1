using System.Windows.Media;
using System.Windows.Shapes;
using Task1.UnityGame;

namespace Task1.UnityRenderers {
    internal sealed class PlayerRenderer : ElementRenderer {
        private const float SizeMul = 0.9f;

        internal PlayerRenderer (GameObject gameObject) 
            : base (gameObject, new Ellipse {
                Fill = new SolidColorBrush (Colors.Yellow)
            }) {
        }

        protected override void UpdateSize () {
            Element.Width = CellSize * SizeMul;
            Element.Height = CellSize * SizeMul;
        }
    }
}
