using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Huffman
{
    public struct HuffmanData
    {
        public byte[] compressedData;
        public byte[] uncompressedData;
        public int sizeOfUncompressedData;
        public List<KeyValuePair<byte, int>> frequency;
    }
    class Program
    {
        static void Main(string[] args)
        {

            bool Compress(ref HuffmanData data)
            {
                FrequencyTable frequencyTable = new FrequencyTable();
                foreach (byte b in data.uncompressedData)
                {
                    frequencyTable.Add(b);
                }

                Forest forest = new Forest();
                foreach (byte b in frequencyTable.Keys) forest.Add(new BinaryTree(new Node(b, frequencyTable[b])));

                BinaryTree binaryTree = forest.GetUniqueTree();

                forest.Preorder(binaryTree.Root, new Code());
                Dictionary<Byte, Code> codeTable = forest.getCodeTable();
                Dictionary<Code, Byte> decodeTable = forest.getDecodeTable();

                int GetRequiredBytesNumber()
                {
                    int size = 0;
                    foreach (Byte b in frequencyTable.Keys) size += frequencyTable[b] * codeTable[b].Count;
                    return size % 8 == 0 ? size / 8 : size / 8 + 1;
                }

                int RequiredBytesNumber = GetRequiredBytesNumber();

                Byte[] compressedData = new Byte[RequiredBytesNumber];

                List<bool> compressedDataBoolean = new List<bool>();
                foreach (Byte b in data.uncompressedData)
                {
                    compressedDataBoolean.AddRange(codeTable[b]);
                }
                int sizeDataUncompressed = compressedDataBoolean.Count;

                Byte[] BooleanListToByteArray(List<bool> boolList)
                {
                    Byte[] _compressedData = new Byte[RequiredBytesNumber];
                    Byte curr = 0;
                    int i = 0, j = 0;
                    Boolean b;
                    while (boolList.Count > 0)
                    {
                        b = boolList.Last();
                        boolList.RemoveAt(boolList.Count - 1);
                        if (i == 0)
                        {
                            curr = new byte();
                            curr = 0x0;
                        }
                        if (b)
                        {
                            curr = (Byte)((1 << i) | curr);
                        }
                        else
                        {
                            curr = (Byte)((0 << i) | curr);
                        }
                        if (i == 7)
                        {
                            _compressedData[j] = curr;
                            j++;
                            i = 0;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    if (i != 0) _compressedData[j] = curr;
                    return _compressedData;
                }

                compressedData = BooleanListToByteArray(compressedDataBoolean);
                List<KeyValuePair<Byte, int>> frequency = frequencyTable.ToList();

                data.compressedData = compressedData;
                data.frequency = frequency;
                data.sizeOfUncompressedData = data.uncompressedData.Length;
                return true;
            }

            bool Decompress(ref HuffmanData data)
            {
                Forest forest2 = new Forest();
                int necessarybytes = 0;
                foreach (KeyValuePair<Byte, int> kvp in data.frequency)
                {
                    forest2.Add(new BinaryTree(new Node(kvp.Key, kvp.Value)));
                    necessarybytes += kvp.Value;
                }

                BinaryTree binaryTree2 = forest2.GetUniqueTree();

                forest2.Preorder(binaryTree2.Root, new Code());
                BitArray bitArray = new BitArray(data.compressedData);
                int sizeDataUncompressed = 0;
                foreach (KeyValuePair<Byte, int> kvp in data.frequency)
                    sizeDataUncompressed += kvp.Value * forest2.getCodeTable()[kvp.Key].Count;
                int start = sizeDataUncompressed % 8 == 0 ? 0 : 8 - sizeDataUncompressed % 8;
                forest2.ProduceByteArrayNew(bitArray, binaryTree2.Root, necessarybytes, start);
                Byte[] finalArray = forest2.GetByteArray();
                data.uncompressedData = finalArray;
                return true;
            }

            // actual Main instructions
            byte[] testString = System.IO.File.ReadAllBytes(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\..\..\testString.txt");
            HuffmanData mainData = new HuffmanData();
            mainData.uncompressedData = testString;
            Compress(ref mainData);
            Decompress(ref mainData);
        }
    }
}
