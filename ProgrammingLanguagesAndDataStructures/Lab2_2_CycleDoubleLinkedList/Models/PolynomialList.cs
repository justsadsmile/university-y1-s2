using System;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Lab2_DoubleCycleList.Models
{
    public class PolynomialList
    {
        private CycleDoubleLinkedList<Term> _terms;
        private int _maxPower;

        public int MaxPower { get { return _maxPower; } }

        public DoubleNode<Term>? Head => _terms.Head;

        public PolynomialList(int maxPower)
        {
            _maxPower = maxPower;
            _terms = new CycleDoubleLinkedList<Term>();
        }

        public void AddTerm(Term term)
        {
            if (term.Coefficient != 0 && term.Power <= _maxPower && term.Power >= 0)
            {
                var newNode = new DoubleNode<Term>(term);
                DoubleNode<Term>? head = _terms.Head;
                bool done = false;

                if (head != null && (head.Next == head || term.Power > head.Next!.Info.Power))
                {
                    _terms.AddToBeginning(term);
                    done = true;
                }

                if (!done && head != null)
                {
                    DoubleNode<Term> current = head.Next!;
                    DoubleNode<Term> prev = head;
                    bool inserted = false;

                    do
                    {
                        if (current.Info.Power == term.Power)
                        {
                            current.Info.Coefficient += term.Coefficient;

                            if (current.Info.Coefficient == 0)
                            {
                                prev.Next = current.Next;
                                current.Next!.Prev = prev;

                                current = prev;
                            }

                            inserted = true;
                        }
                        else if (current.Info.Power < term.Power)
                        {
                            newNode.Prev = prev;
                            newNode.Next = current;
                            prev.Next = newNode;
                            current.Prev = newNode;
                            inserted = true;
                        }
                        else
                        {
                            prev = current;
                            current = current.Next!;
                        }
                    } while (current != head && !inserted);

                    if (!inserted && prev != head)
                    {
                        _terms.AddToEnd(term);
                    }
                }
            }
        }

        public bool EditTermByPower(int power, int newCoefficient)
        {
            bool result = false;
            DoubleNode<Term> head = _terms.Head;

            if (head?.Next != head)
            {
                DoubleNode<Term> current = head.Next!;
                DoubleNode<Term> prev = head;

                while (current != head && !result)
                {
                    if (current.Info.Power == power)
                    {
                        if (newCoefficient != 0)
                        {
                            current.Info.Coefficient = newCoefficient;
                        }
                        else
                        {
                            prev.Next = current.Next;
                            current.Next!.Prev = prev;
                            current.Next = null;
                            current.Prev = null;
                        }

                        result = true;
                    }
                    else
                    {
                        prev = current;
                        current = current.Next!;
                    }
                }
            }

            return result;
        }

        public PolynomialList Add(PolynomialList second)
        {
            int newMaxPower = Math.Max(_maxPower, second._maxPower);
            var result = new PolynomialList(newMaxPower);

            DoubleNode<Term>? head = _terms.Head;
            if (head != null && head.Next != head)
            {
                DoubleNode<Term> current = head.Next!;
                do
                {
                    result.AddTerm(new Term(current.Info.Coefficient, current.Info.Power));
                    current = current.Next!;
                } while (current != head);
            }

            head = second._terms.Head;
            if (head != null && head.Next != head)
            {
                DoubleNode<Term> current = head.Next!;
                do
                {
                    result.AddTerm(new Term(current.Info.Coefficient, current.Info.Power));
                    current = current.Next!;
                } while (current != head);
            }

            return result;
        }

        public bool RemoveByPower(int power)
        {
            bool found = false;

            if (_terms.Head != null)
            {
                DoubleNode<Term> head = _terms.Head;
                DoubleNode<Term> current = head.Next!;
                DoubleNode<Term> prev = head;         

                if (head.Next != head)
                {
                    do
                    {
                        if (current.Info.Power == power)
                        {
                            prev.Next = current.Next;
                            current.Next!.Prev = prev;
                            found = true;

                            current.Next = null;
                            current.Prev = null;
                        }
                        else
                        {
                            prev = current;
                            current = current.Next!;
                        }
                    } while (current != head && !found);
                }
            }

            return found;
        }

        public void Clear()
        {
            _terms.Clear();
        }

        public string ToPolynomialString()
        {
            string result;

            if (_terms.Head == null || _terms.Head.Next == _terms.Head)
            {
                result = "0";
            }
            else
            {
                var sb = new StringBuilder();
                DoubleNode<Term> head = _terms.Head;
                DoubleNode<Term> current = head.Next!;
                bool isFirst = true;

                do
                {
                    if (!isFirst && current.Info.Coefficient > 0)
                        sb.Append(" + ");
                    else if (!isFirst && current.Info.Coefficient < 0)
                        sb.Append(" ");

                    sb.Append(FormatTerm(current.Info));

                    current = current.Next!;
                    isFirst = false;
                } while (current != head);

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