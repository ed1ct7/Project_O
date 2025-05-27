using System.Windows.Controls;
using System.Windows.Input;

namespace Project_O.UserControls
{
    /// <summary>
    /// Interaction logic for ucLesson.xaml
    /// </summary>
    public partial class ucPasswordBox : UserControl
    {
        public ucPasswordBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string _realText = string.Empty;

        private void MaskedTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Define allowed special symbols (modify as needed)
            string allowedSpecialSymbols = "!@#$%^&*()_+-=";

            // Check character types
            bool isLatin = e.Text.All(c => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
            bool isKirilic = e.Text.All(c => (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я'));
            bool isDigit = e.Text.All(char.IsDigit);
            bool isSpecialSymbol = e.Text.All(c => allowedSpecialSymbols.Contains(c));

            // If character is not allowed, block the input
            if (!(isLatin || isKirilic || isDigit || isSpecialSymbol))
            {
                e.Handled = true;
                return;
            }

            // Process allowed character
            TextBox textBox = (TextBox)sender;

            // Insert the bullet character instead of the actual character
            int caretPosition = textBox.CaretIndex;
            textBox.Text = textBox.Text.Insert(caretPosition, "•");
            textBox.CaretIndex = caretPosition + 1;

            // Store the actual character in your _realText variable
            _realText = _realText.Insert(caretPosition, e.Text);

            // Mark the event as handled to prevent default processing
            e.Handled = true;
        }
        private void MaskedTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (e.Key == Key.Back && textBox.CaretIndex > 0)
            {
                // Remove both the bullet and the real character
                int pos = textBox.CaretIndex - 1;
                textBox.Text = textBox.Text.Remove(pos, 1);
                _realText = _realText.Remove(pos, 1);
                textBox.CaretIndex = pos;
                e.Handled = true;
            }
            else if (e.Key == Key.Delete && textBox.CaretIndex < textBox.Text.Length)
            {
                int pos = textBox.CaretIndex;
                textBox.Text = textBox.Text.Remove(pos, 1);
                _realText = _realText.Remove(pos, 1);
                e.Handled = true;
            }
        }
    }
}
