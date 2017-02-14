using System.Windows.Media;
using System.Windows.Shapes;
using Task1.UnityGame;
using Task1.UnityGame.Components;

namespace Task1.UnityRenderers {
    internal sealed class GhostRenderer : ElementRenderer {

        private Player _player;
        private const float SizeMul = 0.9f;

        internal GhostRenderer (GameObject gameObject) 
            : base (gameObject, new Ellipse {
                Fill = new SolidColorBrush (Colors.DarkKhaki)
            }) {
        }

        public override void Start () {
            base.Start();
            _player = GameObject.Game.FindComponentByTag<Player> (Tags.PlayerTag);
        }

        public override void UpdateGraphics (double canvasWidth, double canvasHeight) {
            base.UpdateGraphics (canvasWidth, canvasHeight);
            var ellipse = (Ellipse)Element;
            ellipse.Fill = _player.PoweredUp ? Brushes.Coral : Brushes.DarkKhaki;
        }

        protected override void UpdateSize () {
            Element.Width = CellSize * SizeMul;
            Element.Height = CellSize * SizeMul;
        }
    }
}
