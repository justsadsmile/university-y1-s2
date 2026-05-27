using System;
using System.Windows;
using DotNetEnv;
using Lab3_Tree.Models;
using Lab3_Tree.Views;

namespace Lab3_Tree
{
    public partial class MainWindow : Window
    {
        private BinarySearchTree? _tree;
        private SingleLinkedList? _list;

        public MainWindow()
        {
            InitializeComponent();
            _tree = null;
            _list = null;
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string envPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
                if (System.IO.File.Exists(envPath))
                    Env.Load(envPath);
                else
                    Env.Load();

                string devName = Env.GetString("DEV_NAME", "Unknown Developer");
                string devGroup = Env.GetString("DEV_GROUP", "Unknown Group");
                string task = Env.GetString("TASK_DESC", "No description provided.");

                txtTaskDescription.Text = $"Task: {task}";
                txtStudentInfo.Text = $"Developer: {devName}\nGroup: {devGroup}";

                ShowAboutView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading About page: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTree_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateTreeWindow { Owner = this };
            bool ok = createWindow.ShowDialog() == true;

            if (ok && createWindow.Keys != null && createWindow.Data != null)
            {
                _tree = new BinarySearchTree();
                _list = new SingleLinkedList();
                string messages = "";

                for (int i = 0; i < createWindow.Keys.Length; i++)
                {
                    int key = createWindow.Keys[i];
                    char info = createWindow.Data[i];
                    string result = _tree.Insert(key, info);
                    if (result != "ok")
                        messages += result + "\n";
                }

                if (!string.IsNullOrEmpty(messages))
                {
                    MessageBox.Show(messages, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                ShowTaskView();
                UpdateDisplay();
            }
            else
            {
                EmptyPanel.Visibility = Visibility.Visible;
            }
        }

        private void ProcessTree_Click(object sender, RoutedEventArgs e)
        {
            if (_tree == null || (_tree.Root == null))
            {
                MessageBox.Show("Tree not created. Use 'Create Tree' first!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_list == null)
            {
                _list = new SingleLinkedList();
            }

            ShowTaskView();
            var processWindow = new ProcessTreeWindow(_tree, _list) { Owner = this };
            processWindow.ShowDialog();
        }

        private void DestroyTree_Click(object sender, RoutedEventArgs e)
        {
            if (_tree == null || (_tree.Root == null))
            {
                MessageBox.Show("Tree is already empty!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Destroy the tree?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _tree.Clear();
                _list?.Clear();
                ShowEmptyView();
                UpdateDisplay();
            }
        }

        private void ShowEmptyView()
        {
            EmptyPanel.Visibility = Visibility.Visible;
            AboutPanel.Visibility = Visibility.Collapsed;
            MainWorkArea.Visibility = Visibility.Collapsed;
        }

        private void ShowTaskView()
        {
            EmptyPanel.Visibility = Visibility.Collapsed;
            AboutPanel.Visibility = Visibility.Collapsed;
            MainWorkArea.Visibility = Visibility.Visible;
        }

        private void ShowAboutView()
        {
            EmptyPanel.Visibility = Visibility.Collapsed;
            MainWorkArea.Visibility = Visibility.Collapsed;
            AboutPanel.Visibility = Visibility.Visible;
        }

        private void UpdateDisplay()
        {
            treeView.Items.Clear();
            if (_tree != null && _tree.Root != null)
            {
                var rootItem = new System.Windows.Controls.TreeViewItem
                {
                    Header = $"({_tree.Root.Key}){_tree.Root.Info}"
                };
                PopulateTreeView(_tree.Root, rootItem);
                treeView.Items.Add(rootItem);
            }
        }

        private void PopulateTreeView(TreeNode node, System.Windows.Controls.TreeViewItem parentItem)
        {
            if (node.Left != null)
            {
                var leftItem = new System.Windows.Controls.TreeViewItem
                {
                    Header = $"({node.Left.Key}){node.Left.Info}"
                };
                parentItem.Items.Add(leftItem);
                PopulateTreeView(node.Left, leftItem);
            }

            if (node.Right != null)
            {
                var rightItem = new System.Windows.Controls.TreeViewItem
                {
                    Header = $"({node.Right.Key}){node.Right.Info}"
                };
                parentItem.Items.Add(rightItem);
                PopulateTreeView(node.Right, rightItem);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Exit application?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}