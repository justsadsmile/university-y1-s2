namespace Lab2_DoubleCycleList.Logic
{
    public static class InputValidator
    {
        public static (bool IsValid, string ErrorMessage, int Value) ValidateInteger(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, "Input field is empty!", 0);

            if (!int.TryParse(input, out int result))
                return (false, "Please enter a valid integer number!", 0);

            return (true, "", result);
        }

        public static (bool IsValid, string ErrorMessage, int Value) ValidateIndex(string input, int maxIndex)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, "Index field is empty!", 0);

            if (!int.TryParse(input, out int result))
                return (false, "Please enter a valid integer index!", 0);

            if (result < 0 || result > maxIndex)
                return (false, $"Index must be between 0 and {maxIndex}!", 0);

            return (true, "", result);
        }
    }
}
