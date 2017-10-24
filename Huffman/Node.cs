using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Node : IComparable
    {
        public byte Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(byte symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public Node(int frequency)
        {
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

        public override string ToString()
        {
            string _str = "";
            if(this != null)
            {
                if(Left != null)
                {
                    _str += Left.ToString();
                }
                _str += "(" + (char)Symbol + ": " + Frequency + ")";
                if(Right != null)
                {
                    _str += Right.ToString();
                }
            }
            return _str;
        }
    }
}
