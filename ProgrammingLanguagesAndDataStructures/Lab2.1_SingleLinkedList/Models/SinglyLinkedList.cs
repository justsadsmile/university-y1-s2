using System.Text;

namespace Lab2_Lists1.Models
{
    public class SinglyLinkedList<T>
    {
        private Node<T>? _first;

        public void AddToBeginning(T data)
        {
            var newNode = new Node<T>(data);
            newNode.Next = _first;
            _first = newNode;
        }

        public void AddToEnd(T data)
        {
            var newNode = new Node<T>(data);

            if (_first != null)
            {
                Node<T> current = _first;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            else
            {
                _first = newNode;
            }
        }

        public bool InsertAt(int position, T data)
        {
            bool result = true;

            if (position < 1)
            {
                result = false;
            }
            else if (position == 1)
            {
                AddToBeginning(data);
            }
            else
            {
                var newNode = new Node<T>(data);
                Node<T>? current = _first;
                int currentIndex = 1;
                while (current != null && currentIndex < position - 1)
                {
                    current = current.Next;
                    currentIndex++;
                }

                if (current == null)
                {
                    result = false;
                }
                else
                {
                    newNode.Next = current.Next;
                    current.Next = newNode;
                }
            }

            return result;
        }

        public bool RemoveAt(int position)
        {
            bool result = true;

            if (position < 1 || _first == null)
            {
                result = false;
            }
            else if (position == 1)
            {
                _first = _first.Next;
            }
            else
            {
                Node<T>? current = _first;
                int currentIndex = 1;
                while (current.Next != null && currentIndex < position - 1)
                {
                    current = current.Next;
                    currentIndex++;
                }

                if (current.Next == null)
                {
                    result = false;
                }
                else
                {
                    current.Next = current.Next.Next;
                }
            }

            return result;
        }

        public bool RemoveFromBeginning()
        {
            bool result = true;
            if (_first == null)
            {
                result = false;
            }
            else
            {
                _first = _first.Next;
            }
            return result;
        }

        public bool RemoveFromEnd()
        {
            bool result = true;

            if (_first == null)
            {
                result = false;
            }
            else if (_first.Next == null)
            {
                _first = null;
            }
            else
            {
                Node<T> current = _first;
                while (current.Next!.Next != null)
                {
                    current = current.Next;
                }
                current.Next = null;
            }

            return result;
        }

        public T? GetAt(int position)
        {
            T? result = default;

            if (position >= 1 && _first != null)
            {
                Node<T>? current = _first;
                int currentIndex = 1;

                while (current != null && currentIndex < position)
                {
                    current = current.Next;
                    currentIndex++;
                }

                if (current != null)
                {
                    result = current.Data;
                }
            }

            return result;
        }

        public void Clear()
        {
            _first = null;
        }

        public override string ToString()
        {
            string result;

            if (_first != null)
            {
                var sb = new StringBuilder();
                Node<T>? current = this._first;
                int pos = 1;
                bool first = true;

                while (current != null)
                {
                    if (!first)
                        sb.Append(" -> ");
                    sb.Append($"({pos}){current.Data}");
                    current = current.Next;
                    first = false;
                    pos++;
                }
                result = sb.ToString();
                
            }
            else
            {
                result = "[ empty list ]";
            }

            return result;
        }
    }
}
