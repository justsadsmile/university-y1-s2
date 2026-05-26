using System.Text;

namespace Lab2_DoubleCycleList.Models
{
    public class CycleDoubleLinkedList<T>
    {
        private DoubleNode<T>? _head;
        public DoubleNode<T>? Head => _head;

        public CycleDoubleLinkedList()
        {
            _head = new DoubleNode<T>(default(T)!);
            _head.Next = _head;
            _head.Prev = _head;
        }

        private DoubleNode<T>? GetNodeAt(int position)
        {
            DoubleNode<T>? result = null;

            if (position >= 1 && _head != null)
            {
                DoubleNode<T> current = _head.Next!;
                int i = 1;

                while (current != _head && i < position)
                {
                    current = current.Next!;
                    i++;
                }

                if (current != _head)
                {
                    result = current;
                }
            }

            return result;
        }

        public void AddToBeginning(T data)
        {
            if (_head != null)
            {
                var newNode = new DoubleNode<T>(data);

                newNode.Next = _head!.Next;
                newNode.Prev = _head;

                _head.Next!.Prev = newNode;
                _head.Next = newNode;
            }
        }

        public void AddToEnd(T data)
        {
            if (_head != null)
            {
                var newNode = new DoubleNode<T>(data);

                newNode.Next = _head;
                newNode.Prev = _head.Prev;

                _head.Prev!.Next = newNode;
                _head.Prev = newNode;
            }
        }

        public bool InsertAt(int position, T data)
        {
            bool success = false;

            if (position >= 1)
            {
                if (position == 1 && _head != null)
                {
                    AddToBeginning(data);
                    success = true;
                }
                else
                {
                    DoubleNode<T>? target = GetNodeAt(position);

                    if (target != null)
                    {
                        var newNode = new DoubleNode<T>(data);

                        newNode.Next = target;
                        newNode.Prev = target.Prev;

                        target.Prev!.Next = newNode;
                        target.Prev = newNode;

                        success = true;
                    }
                    else
                    {
                        DoubleNode<T>? last = GetNodeAt(position - 1);

                        if (last != null && last.Next == _head)
                        {
                            AddToEnd(data);
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public bool RemoveAt(int position)
        {
            bool isRemoved = false;

            if (position >= 1 && _head != null && _head.Next != _head)
            {
                if (position == 1)
                {
                    isRemoved = RemoveFromBeginning();
                }
                else
                {
                    DoubleNode<T>? target = GetNodeAt(position);

                    if (target != null)
                    {
                        target.Prev!.Next = target.Next;
                        target.Next!.Prev = target.Prev;

                        target.Next = null!;
                        target.Prev = null!;

                        isRemoved = true;
                    }
                }
            }
            return isRemoved;
        }

        public bool RemoveFromBeginning()
        {
            bool isRemoved = false;

            if (_head != null && _head.Next != _head)
            {
                DoubleNode<T> firstNode = _head.Next!;

                _head.Next = firstNode.Next;
                firstNode.Next!.Prev = _head;

                firstNode.Next = null!;
                firstNode.Prev = null!;

                isRemoved = true;
            }

            return isRemoved;
        }

        public bool RemoveFromEnd()
        {
            bool isRemoved = false;

            if (_head != null && _head.Prev != _head)
            {
                DoubleNode<T> lastNode = _head.Prev!;

                _head.Prev = lastNode.Prev;
                lastNode.Prev!.Next = _head;

                lastNode.Next = null!;
                lastNode.Prev = null!;

                isRemoved = true;
            }

            return isRemoved;
        }

        public T? GetAt(int position)
        {
            DoubleNode<T>? node = GetNodeAt(position);
            return node != null ? node.Info : default;
        }

        public void Clear()
        {
            if (_head != null) 
            {
                DoubleNode<T> current = _head.Next!;

                while (current != _head)
                {
                    DoubleNode<T> temp = current;
                    current = current.Next!;

                    temp.Next = null!;
                    temp.Prev = null!;
                }

                _head.Next = _head;
                _head.Prev = _head;
            }
        }

        public override string ToString()
        {
            if (_head == null || _head.Next == _head) return "[ empty list ]";

            var sb = new StringBuilder();
            DoubleNode<T> current = _head.Next!;
            int pos = 1;

            while (current != _head)
            {
                sb.Append($"({pos}){current.Info} <=> ");
                current = current.Next!;
                pos++;
            }

            sb.Append("(Head)");
            return sb.ToString();
        }
    }
}