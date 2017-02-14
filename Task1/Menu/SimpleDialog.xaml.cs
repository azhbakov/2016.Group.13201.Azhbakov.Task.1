using System.Windows;

namespace Task1.Menu {
    internal sealed partial class SimpleDialog {
        internal SimpleDialog (string question, string defaultAnswer = "") {
            InitializeComponent ();
            LabelQuestion.Content = question;
            TextAnswer.Text = defaultAnswer;
        }

        private void ButtonDialogOk_Click (object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        internal string Answer => TextAnswer.Text;
    }
}
