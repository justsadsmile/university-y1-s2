namespace Lab3_Tree.Models
{
    public class TreeNode
    {
        public int Key { get; set; }
        public char Info { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public TreeNode() { }

        public TreeNode(char info, int key)
        {
            Info = info; Key = key;
        }
        public TreeNode(char info, int key, TreeNode left, TreeNode right)
        {
            Info = info; 
            Key = key; Left = left; 
            Right = right;
        }
    }
}

