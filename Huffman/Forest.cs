using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Forest : List<BinaryTree>
    {
        private Dictionary<Byte, Code> codeTable;
        private Dictionary<Code, Byte> decodeTable;
        private Byte[] byteArray;
        int byteArrayElements;

        public Forest()
        {
            codeTable = new Dictionary<byte, Code>();
            decodeTable = new Dictionary<Code, byte>();
        }

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

        public void Preorder(Node node, Code huffCode)
        {
            if(node != null)
            {
                if(node.IsLeaf())
                {
                    codeTable.Add(node.Symbol, huffCode);
                    decodeTable.Add(huffCode, node.Symbol);
                    return;
                }
                Code _huffCodeL = new Code(); 
                _huffCodeL.AddRange(huffCode);
                Code _huffCodeR = new Code();
                _huffCodeR.AddRange(huffCode);
                _huffCodeL.Add(false);
                Preorder(node.Left, _huffCodeL);
                _huffCodeR.Add(true);
                Preorder(node.Right, _huffCodeR);
            }
        }
        
        public void ProduceByteArray(BitArray bitArray,Node root, Node node, int size, Boolean firstExec, int start)
        {
            if (firstExec)
            {
                byteArray = new Byte[size];
                byteArrayElements = 0;
            }

            if (node != null && start <= bitArray.Count)
            {
                if (node.IsLeaf())
                {
                    byteArray[byteArrayElements] = node.Symbol;
                    byteArrayElements++;
                    if(start < bitArray.Count) ProduceByteArray(bitArray, root, root, size, false, start);
                }
                else
                {
                    if (!bitArray[bitArray.Count - start - 1]) ProduceByteArray(bitArray, root, node.Left, size, false, start + 1);
                    else ProduceByteArray(bitArray, root, node.Right, size, false, start + 1);
                }
            }
        }

        public Dictionary<Byte, Code> getCodeTable()
        {
            return codeTable;
        }

        public Dictionary<Code, Byte> getDecodeTable()
        {
            return decodeTable;
        }

        public Byte[] GetByteArray()
        {
            return byteArray;
        }
    }
}
