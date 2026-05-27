using System.Text;

namespace Lab3_Tree.Models
{
    public class Node
    {
        private int _info;
        private Node link;

        public int Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public Node Link
        {
            get { return link; }
            set { link = value;}
        }


        public Node() { } 

        public Node(int info)
        {
            Info = info;
            Link = null;
        }

        public Node(int info, Node link)
        {
            Info = info;
            Link = link;
        }

    }

    public class SingleLinkedList
    {
        private Node first;

        public SingleLinkedList()
        {
            first = null;
        }

        public void Clear()
        {
            first = null;
        }

        public void Add(int info)
        {
            if (first != null)
            {
                Node current = first;

                while (current.Link != null)
                {
                    current = current.Link;
                }

                current.Link = new Node(info);
            }
            else
            {
                first = new Node(info);
            }
        }

        public override string ToString()
        {
            string result = "(empty)";

            if (first != null)
            {

                var sb = new StringBuilder();
                Node current = first;

                while (current != null)
                {
                    sb.Append(current.Info);

                    if (current.Link != null)
                    {
                        sb.Append(" -> ");
                    }

                    current = current.Link;
                }

                result = sb.ToString();
            }

            return result;
        }
    }
}