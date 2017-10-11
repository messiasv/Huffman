using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Forest : List<BinaryTree>
    {
        public BinaryTree GetUniqueTree()
        {
            while (Count > 1) MergeTrees();
            return this.First();
        }

        public void MergeTrees()
        {
            if(Count > 1)
            {
                Sort();
                BinaryTree bt1, bt2, newbt;
                bt1 = this.First();
                RemoveAt(0);
                bt2 = this.First();
                RemoveAt(0);
                Node newNode = new Node(bt1.Root.Frequency + bt2.Root.Frequency)
                {
                    Left = bt1.Root,
                    Right = bt2.Root
                };
                newbt = new BinaryTree(newNode);
                Add(newbt);
            }
        }
    }
}
