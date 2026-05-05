using System;
using System.Windows;
using DotNetEnv;
using Lab1_RecursiveAlgorithms.Logic;

namespace Lab1_RecursiveAlgorithms
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            DotNetEnv.Env.Load();

            string devName = Env.GetString("DEV_NAME", "Unknown Developer");
            string devGroup = Env.GetString("DEV_GROUP", "Unknown Group");
            string task = Env.GetString("TASK_DESC", "No description provided.");

            txtTaskDescription.Text = $"Task: {task}";
            txtStudentInfo.Text = $"Developer: {devName}\nGroup: {devGroup}";

            TaskPanel.Visibility = Visibility.Collapsed;
            AboutPanel.Visibility = Visibility.Visible;
        }

        private void ShowTask_Click(object sender, RoutedEventArgs e)
        {
            AboutPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Visible;
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            listResults.Items.Clear();

            var validation = InputValidator.ValidateNumber(txtInput.Text);

            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage, "Input Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                txtInput.Focus();
                return;
            }

            try
            {
                int n = validation.Value;
                var partitions = PartitionCalculator.GetPartitions(n);

                if (partitions.Count == 0)
                {
                    listResults.Items.Add("No partitions found.");
                }
                else
                {
                    foreach (var item in partitions)
                    {
                        listResults.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Exit application?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}