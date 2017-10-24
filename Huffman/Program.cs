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
            foreach (Byte b in forest.getCodeTable().Keys)
            {
                Console.WriteLine((char)b + " -> " + forest.getCodeTable()[b]);
            }
            foreach (Code c in forest.getDecodeTable().Keys)
            {
                Console.WriteLine(c + " -> " + (char)forest.getDecodeTable()[c]);
            }
        }
    }
}
