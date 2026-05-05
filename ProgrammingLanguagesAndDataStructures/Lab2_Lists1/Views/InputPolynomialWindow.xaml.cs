using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Lab2_Lists1.Logic;

namespace Lab2_Lists1.Views
{
    public partial class InputPolynomialWindow : Window
    {
        public int[]? Coefficients { get; private set; }
        public int MaxPower { get; private set; }

        private List<TextBox> coeffBoxes = new List<TextBox>();

        public InputPolynomialWindow(int maxPower, string polynomialName)
        {
            InitializeComponent();
            MaxPower = maxPower;
            lblPolynomialName.Content = $"Polynomial {polynomialName} (max power = {maxPower})";

            for (int i = maxPower; i >= 0; i--)
            {
                var rowPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 3, 0, 3) };
                string powerText = i == 0 ? "a₀ = " : i == 1 ? "a₁ = " : $"a{i} = ";
                var label = new TextBlock { Text = powerText, Width = 60, VerticalAlignment = VerticalAlignment.Center, FontFamily = new System.Windows.Media.FontFamily("Consolas"), FontSize = 13 };
                var textBox = new TextBox { Width = 80, Padding = new Thickness(4), Text = "0", Tag = i };

                rowPanel.Children.Add(label);
                rowPanel.Children.Add(textBox);
                pnlCoefficients.Children.Add(rowPanel);
                coeffBoxes.Add(textBox);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            bool allValid = true;
            Coefficients = new int[MaxPower + 1];

            for (int i = 0; i < coeffBoxes.Count && allValid; i++)
            {
                int power = MaxPower - i;
                var validation = InputValidator.ValidateInteger(coeffBoxes[i].Text);
                if (!validation.IsValid)
                {
                    MessageBox.Show($"Invalid coefficient for x^{power}: {validation.ErrorMessage}", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    allValid = false;
                }
                else
                {
                    Coefficients[power] = validation.Value;
                }
            }

            DialogResult = allValid;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Coefficients = null;
            DialogResult = false;
            Close();
        }
    }
}
