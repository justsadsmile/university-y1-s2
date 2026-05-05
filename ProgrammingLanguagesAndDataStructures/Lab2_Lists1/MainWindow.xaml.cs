using System;
using System.Windows;
using System.Windows.Controls;
using DotNetEnv;
using Lab2_Lists1.Models;
using Lab2_Lists1.Views;
using Lab2_Lists1.Logic;

namespace Lab2_Lists1
{
    public partial class MainWindow : Window
    {
        private PolynomialList? polyP;
        private PolynomialList? polyQ;
        private int maxPower;

        public class TermDisplayItem
        {
            public int Position { get; set; }
            public int Coefficient { get; set; }
            public int Power { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            maxPower = 0;
        }


        private void UpdateDisplay()
        {
            dgListDisplay.Columns.Clear();
            dgListDisplay.Items.Clear();

            if (polyP == null && polyQ == null)
            {
                var col = new DataGridTextColumn
                {
                    Header = "Status",
                    Binding = new System.Windows.Data.Binding("Message")
                };
                dgListDisplay.Columns.Add(col);
                dgListDisplay.Items.Add(new { Message = "[ No polynomials created ]" });
            }
            else
            {
                dgListDisplay.Columns.Add(new DataGridTextColumn { Header = "Polynomial", Binding = new System.Windows.Data.Binding("Polynomial"), Width = 80 });
                dgListDisplay.Columns.Add(new DataGridTextColumn { Header = "Position", Binding = new System.Windows.Data.Binding("Position"), Width = 70 });
                dgListDisplay.Columns.Add(new DataGridTextColumn { Header = "Coefficient", Binding = new System.Windows.Data.Binding("Coefficient"), Width = 100 });
                dgListDisplay.Columns.Add(new DataGridTextColumn { Header = "Power", Binding = new System.Windows.Data.Binding("Power"), Width = 70 });

                if (polyP != null)
                {
                    if (polyP.First == null)
                    {
                        dgListDisplay.Items.Add(new { Polynomial = "P(x)", Position = "-", Coefficient = "0 (empty)", Power = "-" });
                    }
                    else
                    {
                        Node<Term>? current = polyP.First;
                        int pos = 1;
                        while (current != null)
                        {
                            dgListDisplay.Items.Add(new
                            {
                                Polynomial = "P(x)",
                                Position = pos,
                                Coefficient = current.Data.Coefficient,
                                Power = current.Data.Power
                            });
                            current = current.Next;
                            pos++;
                        }
                    }
                }

                if (polyQ != null)
                {
                    if (polyQ.First == null)
                    {
                        dgListDisplay.Items.Add(new { Polynomial = "Q(x)", Position = "-", Coefficient = "0 (empty)", Power = "-" });
                    }
                    else
                    {
                        Node<Term>? current = polyQ.First;
                        int pos = 1;
                        while (current != null)
                        {
                            dgListDisplay.Items.Add(new
                            {
                                Polynomial = "Q(x)",
                                Position = pos,
                                Coefficient = current.Data.Coefficient,
                                Power = current.Data.Power
                            });
                            current = current.Next;
                            pos++;
                        }
                    }
                }
            }

            if (polyP != null)
            {
                DisplayPolynomialInGrid(dgPoly1, txtPoly1Str, polyP, "P(x)");
            }

            if (polyQ != null)
            {
                DisplayPolynomialInGrid(dgPoly2, txtPoly2Str, polyQ, "Q(x)");
            }
        }
        private void DisplayPolynomialInGrid(DataGrid grid, TextBlock textBlock, PolynomialList poly, string name)
        {
            grid.Items.Clear();
            if (poly.First == null)
            {
                grid.Items.Add(new TermDisplayItem { Position = 0, Coefficient = 0, Power = 0 });
                textBlock.Text = $"{name} = 0 (empty)";
            }
            else
            {
                Node<Term>? current = poly.First;
                int pos = 1;
                while (current != null)
                {
                    grid.Items.Add(new TermDisplayItem
                    {
                        Position = pos,
                        Coefficient = current.Data.Coefficient,
                        Power = current.Data.Power
                    });
                    current = current.Next;
                    pos++;
                }
                textBlock.Text = $"{name} = {poly.ToPolynomialString()}";
            }
        }


        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
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


                EmptyPanel.Visibility = Visibility.Collapsed;

                MainWorkArea.Visibility = Visibility.Collapsed;

                AboutPanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show($"Error loading About page: {message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowTask_Click(object sender, RoutedEventArgs e)
        {
            EmptyPanel.Visibility = Visibility.Collapsed;
            AboutPanel.Visibility = Visibility.Collapsed;

            MainWorkArea.Visibility = Visibility.Visible;

            TaskPanel.Visibility = Visibility.Visible;
        }

        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);

            var powerDialog = new InputDialog("Enter maximum polynomial power (e.g. 5):");
            powerDialog.Owner = this;
            bool proceed = powerDialog.ShowDialog() == true;

            if (proceed)
            {
                var powerValidation = InputValidator.ValidateInteger(powerDialog.Result);
                if (!powerValidation.IsValid || powerValidation.Value < 0 || powerValidation.Value > 50)
                {
                    MessageBox.Show("Please enter a valid power (0-50).", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    proceed = false;
                }
                else
                {
                    maxPower = powerValidation.Value;
                }
            }

            if (proceed)
            {
                var inputP = new InputPolynomialWindow(maxPower, "P") { Owner = this };
                bool okP = inputP.ShowDialog() == true && inputP.Coefficients != null;
                proceed = okP;

                if (proceed)
                {
                    var inputQ = new InputPolynomialWindow(maxPower, "Q") { Owner = this };
                    bool okQ = inputQ.ShowDialog() == true && inputQ.Coefficients != null;
                    proceed = okQ;

                    if (proceed)
                    {
                        int[] coeffsP = inputP.Coefficients!;
                        polyP = new PolynomialList(maxPower);
                        for (int i = 0; i <= maxPower; i++)
                        {
                            if (coeffsP[i] != 0)
                                polyP.AddTerm(new Term(coeffsP[i], i));
                        }

                        int[] coeffsQ = inputQ.Coefficients!;
                        polyQ = new PolynomialList(maxPower);
                        for (int i = 0; i <= maxPower; i++)
                        {
                            if (coeffsQ[i] != 0)
                                polyQ.AddTerm(new Term(coeffsQ[i], i));
                        }

                        UpdateDisplay();
                    }
                }
            }
        }

        private void AddToBeginning_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            AddTermToList();
        }

        private void AddToEnd_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            AddTermToList();
        }

        private void AddAtIndex_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            AddTermToList();
        }

        private void DeleteFromBeginning_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            DeleteTermFromList();
        }

        private void DeleteFromEnd_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            DeleteTermFromList();
        }

        private void DeleteAtIndex_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            DeleteTermFromList();
        }

        private void AddTermToList()
        {
            bool proceed = polyP != null && polyQ != null;
            PolynomialList? targetList = null;
            string listName = "";

            if (!proceed)
            {
                MessageBox.Show("Polynomials not created. Use 'Create List' first!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (proceed)
            {
                var chooseDialog = new InputDialog("Edit P(x) or Q(x)?\nEnter 'P' or 'Q':");
                chooseDialog.Owner = this;
                bool chose = chooseDialog.ShowDialog() == true;

                if (chose)
                {
                    string choice = chooseDialog.Result.Trim().ToUpper();
                    if (choice == "P")
                    {
                        targetList = polyP;
                        listName = "P(x)";
                    }
                    else if (choice == "Q")
                    {
                        targetList = polyQ;
                        listName = "Q(x)";
                    }
                    else
                    {
                        MessageBox.Show("Invalid choice. Enter 'P' or 'Q'.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        proceed = false;
                    }
                }
                else
                {
                    proceed = false;
                }
            }

            if (proceed && targetList != null)
            {
                var coeffDialog = new InputDialog($"Enter coefficient for {listName}:");
                coeffDialog.Owner = this;
                bool coeffOk = coeffDialog.ShowDialog() == true;

                if (coeffOk)
                {
                    var coeffValidation = InputValidator.ValidateInteger(coeffDialog.Result);
                    if (!coeffValidation.IsValid)
                    {
                        MessageBox.Show(coeffValidation.ErrorMessage, "Input Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        proceed = false;
                    }
                    else if (coeffValidation.Value == 0)
                    {
                        MessageBox.Show("Coefficient cannot be 0.", "Info",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        proceed = false;
                    }
                    else
                    {
                        var powerDialog = new InputDialog($"Enter power (0-{maxPower}):");
                        powerDialog.Owner = this;
                        bool powerOk = powerDialog.ShowDialog() == true;

                        if (powerOk)
                        {
                            var powerValidation = InputValidator.ValidateInteger(powerDialog.Result);
                            if (!powerValidation.IsValid || powerValidation.Value < 0 || powerValidation.Value > maxPower)
                            {
                                MessageBox.Show($"Power must be between 0 and {maxPower}.", "Input Error",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                                proceed = false;
                            }
                            else
                            {
                                targetList.AddTerm(new Term(coeffValidation.Value, powerValidation.Value));
                                UpdateDisplay();
                            }
                        }
                    }
                }
            }
        }

        private void EditByPower_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);
            EditTermInList();
        }

        private void EditTermInList()
        {
            bool proceed = polyP != null && polyQ != null;
            PolynomialList? targetList = null;
            string listName = "";

            if (!proceed)
            {
                MessageBox.Show("Polynomials not created. Use 'Create List' first!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var chooseDialog = new InputDialog("Edit P(x) or Q(x)?\nEnter 'P' or 'Q':");
            chooseDialog.Owner = this;
            if (chooseDialog.ShowDialog() == true)
            {
                string choice = chooseDialog.Result.Trim().ToUpper();
                if (choice == "P")
                {
                    targetList = polyP;
                    listName = "P(x)";
                }
                else if (choice == "Q")
                {
                    targetList = polyQ;
                    listName = "Q(x)";
                }
                else
                {
                    MessageBox.Show("Invalid choice. Enter 'P' or 'Q'.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    proceed = false;
                }
            }
            else
            {
                proceed = false;
            }

            if (proceed && targetList != null)
            {
                if (targetList.First == null)
                {
                    MessageBox.Show($"{listName} is empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var powerDialog = new InputDialog($"Enter power of term to edit in {listName} (0-{maxPower}):");
                powerDialog.Owner = this;
                if (powerDialog.ShowDialog() == true)
                {
                    var powerValidation = InputValidator.ValidateInteger(powerDialog.Result);
                    if (!powerValidation.IsValid || powerValidation.Value < 0 || powerValidation.Value > maxPower)
                    {
                        MessageBox.Show($"Power must be between 0 and {maxPower}.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int targetPower = powerValidation.Value;

                    var coeffDialog = new InputDialog($"Enter NEW coefficient for x^{targetPower}:");
                    coeffDialog.Owner = this;
                    if (coeffDialog.ShowDialog() == true)
                    {
                        var coeffValidation = InputValidator.ValidateInteger(coeffDialog.Result);
                        if (!coeffValidation.IsValid)
                        {
                            MessageBox.Show(coeffValidation.ErrorMessage, "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        bool edited = targetList.EditTermByPower(targetPower, coeffValidation.Value);

                        if (edited)
                        {
                            UpdateDisplay();
                        }
                        else
                        {
                            MessageBox.Show($"No term with power {targetPower} found in {listName}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void DeleteTermFromList()
        {
            bool proceed = polyP != null && polyQ != null;
            PolynomialList? targetList = null;
            string listName = "";

            if (!proceed)
            {
                MessageBox.Show("Polynomials not created. Use 'Create List' first!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (proceed)
            {
                var chooseDialog = new InputDialog("Edit P(x) or Q(x)?\nEnter 'P' or 'Q':");
                chooseDialog.Owner = this;
                bool chose = chooseDialog.ShowDialog() == true;

                if (chose)
                {
                    string choice = chooseDialog.Result.Trim().ToUpper();
                    if (choice == "P")
                    {
                        targetList = polyP;
                        listName = "P(x)";
                    }
                    else if (choice == "Q")
                    {
                        targetList = polyQ;
                        listName = "Q(x)";
                    }
                    else
                    {
                        MessageBox.Show("Invalid choice. Enter 'P' or 'Q'.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        proceed = false;
                    }
                }
                else
                {
                    proceed = false;
                }
            }

            if (proceed && targetList != null)
            {
                if (targetList.First == null)
                {
                    MessageBox.Show($"{listName} is empty!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    proceed = false;
                }
                else
                {
                    var powerDialog = new InputDialog($"Enter power of term to delete from {listName} (0-{maxPower}):");
                    powerDialog.Owner = this;
                    bool powerOk = powerDialog.ShowDialog() == true;

                    if (powerOk)
                    {
                        var powerValidation = InputValidator.ValidateInteger(powerDialog.Result);
                        if (!powerValidation.IsValid || powerValidation.Value < 0 || powerValidation.Value > maxPower)
                        {
                            MessageBox.Show($"Power must be between 0 and {maxPower}.", "Input Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            proceed = false;
                        }
                        else
                        {
                            bool removed = targetList.RemoveByPower(powerValidation.Value);
                            if (removed)
                                UpdateDisplay();
                            else
                                MessageBox.Show($"No term with power {powerValidation.Value} found in {listName}.", "Info",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void ProcessList_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);

            if (polyP == null || polyQ == null)
            {
                MessageBox.Show("Polynomials not created. Use 'Create List' first!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                PolynomialList sum = polyP.Add(polyQ);
                var processWindow = new ProcessListWindow(polyP, polyQ, sum) { Owner = this };
                if (processWindow.ShowDialog() == true && processWindow.Success)
                    UpdateDisplay();
            }
        }

        private void DestroyList_Click(object sender, RoutedEventArgs e)
        {
            ShowTask_Click(sender, e);

            bool bothEmpty = (polyP == null || polyP.First == null) && (polyQ == null || polyQ.First == null);

            if (bothEmpty)
            {
                MessageBox.Show("Polynomials are already empty!", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var result = MessageBox.Show("Destroy both polynomials P(x) and Q(x)?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    polyP?.Clear();
                    polyQ?.Clear();
                    UpdateDisplay();
                }
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
