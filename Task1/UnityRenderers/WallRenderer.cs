using System.Windows.Media;
using System.Windows.Shapes;
using Task1.UnityGame;

namespace Task1.UnityRenderers {
    internal sealed class WallRenderer : ElementRenderer {

        internal WallRenderer (GameObject gameObject) 
            : base (gameObject, new Rectangle {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2,
                Fill = Brushes.LightBlue
            }) {
        }

    }
}
