using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Task1.UnityGame;

namespace Task1.UnityRenderers {
    internal sealed class FruitRenderer : ElementRenderer {

        internal FruitRenderer (GameObject gameObject)
            : base (gameObject, new Image () {
                Source = GetImageSource (System.IO.Path.Combine (Environment.CurrentDirectory, @".\Data\Cherry-Bonus-icon.png"))
            }) {

        }

        private static BitmapImage GetImageSource (string path) {
            BitmapImage logo = new BitmapImage ();
            logo.BeginInit ();
            logo.UriSource = new Uri (path);
            logo.EndInit ();
            return logo;
        }

    }
}
