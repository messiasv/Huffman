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
            foreach(KeyValuePair<byte,int> kvp in frequencyTable) forest.Add(new BinaryTree(new Node(kvp.Key, kvp.Value)));
        }
    }
}
