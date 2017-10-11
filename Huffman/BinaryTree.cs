﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
