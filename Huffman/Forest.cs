using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Huffman
{
    class Forest : List<BinaryTree>
    {
        private Dictionary<Byte, Code> codeTable;
        private Dictionary<Code, Byte> decodeTable;
        private Byte[] byteArray;
        int byteArrayElement;

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
                Sort((_bt1, _bt2) => _bt1.CompareTo(_bt2));
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

        public void ProduceByteArrayNew(BitArray bitArray, Node root, int size, int start)
        {
            byteArray = new Byte[size];
            byteArrayElement = 0;
            Node node = root;
                
            while(byteArrayElement < size) {
                if (node != null)
                {
                    if (node.IsLeaf())
                    {
                        byteArray[byteArrayElement] = node.Symbol;
                        byteArrayElement++;
                        if (start < bitArray.Count) node = root;
                        else return;
                    }
                    else if(start < bitArray.Count)
                    {
                        if (!bitArray[bitArray.Count - start - 1])
                        {
                            node = node.Left;
                            start++;
                        }
                        else
                        {
                            node = node.Right;
                            start++;
                        }
                    }
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
