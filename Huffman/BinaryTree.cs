using System;

namespace Huffman
{
    class BinaryTree : IComparable
    {
        public Node Root { get; set; }

        public BinaryTree(Node root)
        {
            Root = root;
        }

        public int CompareTo(object obj)
        {
            return Root.CompareTo((obj as BinaryTree).Root);
        }

        public override string ToString()
        {
            return Root.ToString();
        }
    }
}
