using System.Text;
using System;

namespace Lab2_Lists1.Models
{
    public class PolynomialList
    {
        private Node<Term>? _first;
        private int _maxPower;

        public Node<Term>? First { get { return _first; } }

        public int MaxPower { get { return _maxPower; } }

        public PolynomialList(int maxPower)
        {
            this._maxPower = maxPower;
        }

        public void AddTerm(Term term)
        {
            if (term.Coefficient != 0 && term.Power <= _maxPower && term.Power >= 0)
            {
                var newNode = new Node<Term>(term);

                if (_first == null || term.Power > _first.Data.Power)
                {
                    newNode.Next = _first;
                    _first = newNode;
                }
                else
                {
                    Node<Term>? current = _first;
                    Node<Term>? prev = null;
                    while (current != null && current.Data.Power > term.Power)
                    {
                        prev = current;
                        current = current.Next;
                    }

                    if (current != null && current.Data.Power == term.Power)
                    {
                        current.Data.Coefficient += term.Coefficient;
                        if (current.Data.Coefficient == 0)
                        {
                            if (prev == null)
                                _first = current.Next;
                            else
                                prev.Next = current.Next;
                        }
                    }
                    else
                    {
                        newNode.Next = current;
                        prev!.Next = newNode;
                    }
                }
            }
        }

        public bool EditTermByPower(int power, int newCoefficient)
        {
            Node<Term>? current = _first;
            bool result = false;
            if (newCoefficient != 0)
            {
                while (current != null)
                {
                    if (current.Data.Power == power)
                    {
                        current.Data.Coefficient = newCoefficient;
                        result = true;
                    }
                    current = current.Next;
                }
            }
            else
            {
                result = RemoveByPower(power);
            }

            return result;
        }

        public PolynomialList Add(PolynomialList other)
        {
            int newMaxPower = Math.Max(this._maxPower, other._maxPower);
            var result = new PolynomialList(newMaxPower);

            Node<Term>? current = this._first;

            while (current != null)
            {
                result.AddTerm(new Term(current.Data.Coefficient, current.Data.Power));
                current = current.Next;
            }

            current = other._first;

            while (current != null)
            {
                result.AddTerm(new Term(current.Data.Coefficient, current.Data.Power));
                current = current.Next;
            }

            return result;
        }

        public bool RemoveByPower(int power)
        {
            bool found = false;
            Node<Term>? current = _first;
            Node<Term>? prev = null;

            while (current != null && !found)
            {
                if (current.Data.Power == power)
                {
                    if (prev == null)
                        _first = current.Next;
                    else
                        prev.Next = current.Next;
                    found = true;
                }
                else
                {
                    prev = current;
                    current = current.Next;
                }
            }
            return found;
        }

        public void Clear()
        {
            _first = null;
        }

        public string ToPolynomialString()
        {
            string result;

            if (_first == null)
            {
                result = "0";
            }
            else
            {
                var sb = new StringBuilder();
                Node<Term>? current = this._first;
                bool first = true;

                while (current != null)
                {
                    if (!first && current.Data.Coefficient > 0)
                        sb.Append(" + ");
                    else if (!first && current.Data.Coefficient < 0)
                        sb.Append(" ");

                    sb.Append(FormatTerm(current.Data));

                    current = current.Next;
                    first = false;
                }
                result = sb.ToString();
            }

            return result;
        }

        private string FormatTerm(Term term)
        {
            string result;
            if (term.Power == 0)
            {
                result = $"{term.Coefficient}";
            }
            else
            {
                string coeffStr = term.Coefficient == 1 ? "" :
                                  term.Coefficient == -1 ? "-" :
                                  $"{term.Coefficient}";

                if (term.Power == 1)
                    result = $"{coeffStr}x";
                else
                    result = $"{coeffStr}x^{term.Power}";
            }
            return result;
        }
    }
}