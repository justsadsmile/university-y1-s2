using System.Text;
using System.Windows;
using Lab3_Tree.Models;

namespace Lab3_Tree.Views
{
    public partial class ProcessTreeWindow : Window
    {
        private readonly BinarySearchTree _tree;
        private readonly SingleLinkedList _list;

        public bool Success { get; private set; }

        public ProcessTreeWindow(BinarySearchTree tree, SingleLinkedList list)
        {
            InitializeComponent();
            _tree = tree;
            _list = list;
            Success = false;

            var sb = new StringBuilder();
            if (_tree.Root != null)
                _tree.LKP(_tree.Root, sb);
            txtTree.Text = sb.ToString().TrimEnd();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtMinKey.Text, out int minKey))
            {
                MessageBox.Show("Please enter a valid minimum key.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtMaxKey.Text, out int maxKey))
            {
                MessageBox.Show("Please enter a valid maximum key.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (minKey > maxKey)
            {
                MessageBox.Show("Minimum key cannot be greater than maximum key.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _list.Clear();
            _tree.CopyByKeys(_list, minKey, maxKey);

            txtList.Text = _list.ToString();
            Success = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}