using System.Windows;

namespace Task1.UnityGame {
    internal interface IRenderer : IComponent {
        FrameworkElement GetElement ();
        void UpdateGraphics (double canvasWidth, double canvasHeight);
        double GetPositionX ();
        double GetPositionY ();
    }
}
