using System.Windows;

namespace Lab3_Tree.Views
{
    public partial class CreateTreeWindow : Window
    {
        public int[]? Keys { get; private set; }
        public char[]? Data { get; private set; }

        public CreateTreeWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter at least one node (format: key,data).", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string[] lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var keysList = new System.Collections.Generic.List<int>();
            var dataList = new System.Collections.Generic.List<char>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                string[] parts = line.Split(',');
                if (parts.Length < 2)
                {
                    MessageBox.Show($"Line {i + 1}: Expected format 'key,info'.", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(parts[0].Trim(), out int key))
                {
                    MessageBox.Show($"Line {i + 1}: Key must be an integer.", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string data = parts[1].Trim();
                if (string.IsNullOrEmpty(data) || data.Length != 1)
                {
                    MessageBox.Show($"Line {i + 1}: Info must be a single character.", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                keysList.Add(key);
                dataList.Add(data[0]);
            }

            if (keysList.Count == 0)
            {
                MessageBox.Show("Please enter at least one valid node.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Keys = keysList.ToArray();
            Data = dataList.ToArray();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}