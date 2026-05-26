using System.Windows;
using System.Windows.Input;

namespace Lab2_DoubleCycleList.Views
{
    public partial class InputDialog : Window
    {
        private string _result = string.Empty;

        public string Result
        {
            get { return _result; }
            private set { _result = value; }
        }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            txtPrompt.Text = prompt;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = txtInput.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OK_Click(sender, e);
            }
        }
    }
}
