using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Node : IComparable
    {
        private byte Symbol { get; set; }
        private int Frequency { get; set; }
        private Node Left { get; set; }
        private Node Right { get; set; }

        public Node(byte symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }

        public int CompareTo(object obj)
        {
            return Frequency <= (obj as Node).Frequency ? -1 : 1;
        }
    }
}
