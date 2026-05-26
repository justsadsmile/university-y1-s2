namespace Lab2_Lists1.Models
{
    public class Term
    {
        public int Coefficient { get; set; }
        public int Power { get; set; }

        public Term(int coefficient, int power)
        {
            Coefficient = coefficient;
            Power = power;
        }

        public override string ToString()
        {
            string formattedTerm = $"{Coefficient}x^{Power}";
            if (Power == 0)
                formattedTerm = $"{Coefficient}";
            if (Power == 1)
                formattedTerm = $"{Coefficient}x";

            return formattedTerm ;
        }

        public string ToListBoxFormat(int index)
        {
            return $"[{index}] = {this}";
        }
    }
}
