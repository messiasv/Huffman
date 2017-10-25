using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] testString = System.IO.File.ReadAllBytes(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\..\..\testString.txt");
            Console.WriteLine(testString.Length);
            FrequencyTable frequencyTable = new FrequencyTable();
            foreach(byte b in testString)
            {
                frequencyTable.Add(b);
            }
            Console.WriteLine(frequencyTable.ToString());

            Forest forest = new Forest();
            foreach (byte b in frequencyTable.Keys) forest.Add(new BinaryTree(new Node(b, frequencyTable[b])));

            BinaryTree binaryTree = forest.GetUniqueTree();
            Console.WriteLine(binaryTree.ToString());

            forest.Preorder(binaryTree.Root, new Code());
            Dictionary<Byte, Code> codeTable = forest.getCodeTable();
            Dictionary<Code, Byte> decodeTable = forest.getDecodeTable();
            foreach (Byte b in codeTable.Keys)
            {
                Console.WriteLine((char)b + " -> " + codeTable[b]);
            }
            foreach (Code c in decodeTable.Keys)
            {
                Console.WriteLine(c + " -> " + (char)decodeTable[c]);
            }

            int GetRequiredBytesNumber()
            {
                int size = 0;
                foreach(Byte b in frequencyTable.Keys) size += frequencyTable[b] * codeTable[b].Count;
                return size%8 == 0 ? size/8 : size/8 + 1;
            }

            int RequiredBytesNumber = GetRequiredBytesNumber();

            Byte[] compressedData = new Byte[RequiredBytesNumber];
            Console.WriteLine("testStringSize: " + testString.Length); // original size
            Console.WriteLine("compressedStringSize: " + RequiredBytesNumber); // seeked size

            List<bool> compressedDataBoolean = new List<bool>();
            foreach(Byte b in testString)
            {
                compressedDataBoolean.AddRange(codeTable[b]);
            }

            Byte[] BooleanListToByteArray(List<bool> boolList)
            {
                Byte[] _compressedData = new Byte[RequiredBytesNumber];
                Byte curr = 0;
                int i = 0, j = 0;
                Boolean b;
                while(b = boolList.Last()) { // reversed : (
                    boolList.Remove(b);
                    if (i == 0) {
                        curr = new byte();
                        curr = 0x0;
                    }
                    if(b)
                    {
                        curr = (Byte)((1 << i) | curr);
                    } else
                    {
                        curr = (Byte)((0 << i) | curr);
                    }
                    if (i == 7)
                    {
                        _compressedData[RequiredBytesNumber - 1 - j] = curr;
                        j++;
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
                return _compressedData;
            }

            compressedData = BooleanListToByteArray(compressedDataBoolean);
        }
    }
}
