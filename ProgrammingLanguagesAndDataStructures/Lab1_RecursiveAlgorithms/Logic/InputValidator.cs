using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_RecursiveAlgorithms.Logic
{
    public static class InputValidator
    {
        public static (bool IsValid, string ErrorMessage, int Value) ValidateNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, "Input field is empty!", 0);

            if (!int.TryParse(input, out int result))
                return (false, "Please enter a valid integer number!", 0);

            if (result <= 1)
                return (false, "Number must be greater than 1 to have partitions!", 0);

            if (result > 50) // be careful with this
                return (false, "Number is too large for this algorithm!", 0);

            return (true, "", result);
        }
    }
}
