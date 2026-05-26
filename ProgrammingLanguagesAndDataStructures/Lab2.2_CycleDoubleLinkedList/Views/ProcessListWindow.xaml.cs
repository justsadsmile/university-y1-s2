using System.Windows;
using Lab2_DoubleCycleList.Models;

namespace Lab2_DoubleCycleList.Views
{
    public class TermDisplayItem
    {
        public int Position { get; set; }
        public int Coefficient { get; set; }
        public int Power { get; set; }
    }

    public partial class ProcessListWindow : Window
    {
        public bool Success { get; private set; }

        public ProcessListWindow(PolynomialList poly1, PolynomialList poly2, PolynomialList sum)
        {
            InitializeComponent();
            DisplayPolynomials(poly1, poly2, sum);
        }

        private void DisplayPolynomials(PolynomialList poly1, PolynomialList poly2, PolynomialList sum)
        {
            DisplayPolynomialInGrid(dgPoly1, txtPoly1Str, poly1, "P(x)");
            DisplayPolynomialInGrid(dgPoly2, txtPoly2Str, poly2, "Q(x)");
            DisplayPolynomialInGrid(dgResult, txtResultStr, sum, "S(x) = P(x) + Q(x)");
        }

        private void DisplayPolynomialInGrid(System.Windows.Controls.DataGrid grid, System.Windows.Controls.TextBlock textBlock, PolynomialList poly, string name)
        {
            grid.Items.Clear();

            if (poly.Head == null || poly.Head.Next == poly.Head)
            {
                grid.Items.Add(new TermDisplayItem { Position = 0, Coefficient = 0, Power = 0 });
                textBlock.Text = $"{name} = 0 (empty)";
            }
            else
            {
                DoubleNode<Term> current = poly.Head.Next!;
                int pos = 1;

                while (current != poly.Head)
                {
                    grid.Items.Add(new TermDisplayItem
                    {
                        Position = pos,
                        Coefficient = current.Info.Coefficient,
                        Power = current.Info.Power
                    });

                    current = current.Next!;
                    pos++;
                }
                textBlock.Text = $"{name} = {poly.ToPolynomialString()}";
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Success = true;
            DialogResult = true;
            Close();
        }
    }
}
