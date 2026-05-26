namespace Lab2_DoubleCycleList.Models
{
    public class DoubleNode<T> 
    {
        public T Info { get; set; } = default!;
        public DoubleNode<T>? Next { get; set; }
        public DoubleNode<T>? Prev { get; set; }

        public DoubleNode() { }

        public DoubleNode(T info)
        {
            Info = info;
        }

        public DoubleNode(T info, DoubleNode<T> next, DoubleNode<T> prev)
        {
            Info = info;
            Next = next;
            Prev = prev;
        }
    }
}