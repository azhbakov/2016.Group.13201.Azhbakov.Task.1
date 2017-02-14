using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Task1.Menu {
    internal sealed partial class ScoreBoard {
        internal ScoreBoard (FrameworkElement window) {
            InitializeComponent ();

            var scrollViewer = new ScrollViewer () {
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };
            var verticalStackPanel = new StackPanel () {
                Background = Brushes.CadetBlue,
                Orientation = Orientation.Vertical,
                CanVerticallyScroll = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = window.ActualWidth - 2 * window.ActualWidth / 5,
            };
            scrollViewer.Content = verticalStackPanel;
            verticalStackPanel.Children.Add (new TextBlock () {
                Foreground = Brushes.Navy,
                FontSize = 35,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "High Scores"
            });

            var scores = ScoreSystem.ScoreSystem.ReadHighscores ();
            foreach (var s in scores) {
                var horizontalPanel = new DockPanel () {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = window.ActualWidth - 2 * window.ActualWidth / 4
                };

                var nameTextBlock = new TextBlock () {
                    Foreground = Brushes.Navy,
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = s.Key
                };
                DockPanel.SetDock (nameTextBlock, Dock.Left);

                var valueTextBlock = new TextBlock () {
                    Foreground = Brushes.Navy,
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Text = s.Value.ToString ()
                };
                DockPanel.SetDock (valueTextBlock, Dock.Right);

                horizontalPanel.Children.Add (nameTextBlock);
                horizontalPanel.Children.Add (valueTextBlock);
                verticalStackPanel.Children.Add (horizontalPanel);
            }
            Content = scrollViewer;
        }
    }
}
