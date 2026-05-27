using System.Text;

namespace Lab3_Tree.Models
{
    public class BinarySearchTree
    {
        private TreeNode? _root;

        public TreeNode Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public BinarySearchTree()
        {
            _root = null;
        }

        public string Insert(int key, char info)
        {
            string message = "ok";
            _root = InsertRec(_root, key, info, ref message);
            return message;
        }

        private TreeNode? InsertRec(TreeNode? node, int key, char info, ref string message)
        {
            TreeNode? result = node;

            if (node == null)
            {
                message = "ok";
                result = new TreeNode(info, key);
            }
            else if (key == node.Key)
            {
                message = $"Duplicate key: {key}";
            }
            else if (key < node.Key)
            {
                node.Left = InsertRec(node.Left, key, info, ref message);
            }
            else
            {
                node.Right = InsertRec(node.Right, key, info, ref message);
            }

            return result;
        }

        public void Clear()
        {
            _root = null;
        }

        public void LKP(TreeNode root)
        {
            if (root != null)
            {
                LKP(root.Left);
                Console.WriteLine(root.Info);
                LKP(root.Right);
            }
        }

        public void LKP(TreeNode root, StringBuilder sb)
        {
            if (root != null)
            {
                LKP(root.Left, sb);
                sb.Append($"({root.Key}){root.Info} ");
                LKP(root.Right, sb);
            }
        }

        public void CopyByKeys(SingleLinkedList list, int minKey, int maxKey)
        {
            CopyByKeysRec(_root, list, minKey, maxKey);
        }

        private void CopyByKeysRec(TreeNode? node, SingleLinkedList list, int minKey, int maxKey)
        {
            if (node != null)
            {
                CopyByKeysRec(node.Left, list, minKey, maxKey);

                if (node.Key >= minKey && node.Key <= maxKey)
                {
                    list.Add(node.Key);
                }

                CopyByKeysRec(node.Right, list, minKey, maxKey);
            }
        }
    }
}