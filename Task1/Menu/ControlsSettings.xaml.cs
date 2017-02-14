using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Button = System.Windows.Controls.Button;
using KeyBinding = Task1.UnityGame.KeyBinding;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;

namespace Task1.Menu {
    internal sealed partial class ControlsSettings {

        private bool _isEditing;
        private readonly List<KeyBinding> _binds;
        private readonly Action _backAction;
        private readonly Action<List<KeyBinding>> _saveAction;

        public ControlsSettings (List<KeyBinding> binds, Action backAction, Action<List<KeyBinding>> saveAction) {
            if (binds == null) {
                throw new ArgumentNullException("Control settings need non null "+"binds to display");
            }
            _binds = binds;
            if (backAction == null) {
                throw new ArgumentNullException("Settings window need \"back\" "+"delegate");
            }
            _backAction = backAction;
            if (saveAction == null) {
                throw new ArgumentNullException ("Settings window need \"save\" " + "delegate");
            }
            _saveAction = saveAction;

            InitializeComponent ();

            ItemsList.ItemsSource = binds;
        }

        private void ButtonClick (object o, RoutedEventArgs e) {
            if (_isEditing) { return; }

            var b = o as Button;
            if (b == null) {
                throw new ArgumentException("Button click should be called by button");
            }

            var previousValue = b.Content.ToString();
            b.Content = "...";
            _isEditing = true;

            KeyEventHandler readKey = null;
            readKey = delegate (object sender, KeyEventArgs ev) {
                foreach (var bind in _binds) {
                    if (bind.Key == ev.Key && !previousValue.Equals(ev.Key.ToString()) || ev.Key == Key.Escape) {
                        MessageBox.Show ("Button is already in use!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        ev.Handled = true;
                        return;
                    }
                }
                b.Content = ev.Key == Key.Back ? previousValue : ev.Key.ToString();
                _isEditing = false;
                PreviewKeyDown -= readKey;

                ev.Handled = true;
            };

            PreviewKeyDown += readKey;
        }

        private void BackButton_Click (object sender, RoutedEventArgs e) {
            _backAction ();
        }

        private void SaveButton_Click (object sender, RoutedEventArgs e) {
            _saveAction (_binds);
        }
    }
}
