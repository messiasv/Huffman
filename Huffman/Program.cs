﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    foreach (Byte b in frequencyTable.Keys) size += frequencyTable[b] * codeTable[b].Count;
                    return size % 8 == 0 ? size / 8 : size / 8 + 1;
                }

                int RequiredBytesNumber = GetRequiredBytesNumber();

                Byte[] compressedData = new Byte[RequiredBytesNumber];
                Console.WriteLine("testStringSize: " + data.uncompressedData.Length); // original size
                Console.WriteLine("compressedStringSize: " + RequiredBytesNumber); // seeked size

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
                    { // reversed : ( --> not anymore
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
                Console.WriteLine(data.uncompressedData);
                Console.WriteLine(necessarybytes);

                BinaryTree binaryTree2 = forest2.GetUniqueTree();
                Console.WriteLine(binaryTree2.ToString());

                forest2.Preorder(binaryTree2.Root, new Code());
                BitArray bitArray = new BitArray(data.compressedData);
                int sizeDataUncompressed = 0;
                foreach (KeyValuePair<Byte, int> kvp in data.frequency) sizeDataUncompressed += kvp.Value * forest2.getCodeTable()[kvp.Key].Count;

                Console.WriteLine(sizeDataUncompressed);
                Console.WriteLine(bitArray.Count);
                int start = 0;
                int blop = sizeDataUncompressed % 8;
                Console.WriteLine(blop);
                if (!(blop == 0))
                {
                    start = 8 - blop; // values between 0 - 7
                }
                Console.WriteLine(start);
                //int start = sizeDataUncompressed % 8 == 0 ? 0 : 8 - sizeDataUncompressed % 8;
                //forest2.ProduceByteArray(bitArray, binaryTree2.Root, binaryTree2.Root, necessarybytes, true, start);
                Console.WriteLine("ha");
                forest2.ProduceByteArrayNew(bitArray, binaryTree2.Root, necessarybytes, start);
                Console.WriteLine("ho");
                Byte[] finalArray = forest2.GetByteArray();
                //for (int i = 0; i < finalArray.Count(); i++)
                //{
                //    if (finalArray[i] != testString[i]) Console.Write("not working");
                //}
                data.uncompressedData = finalArray;
                return true;
            }


            byte[] testString = System.IO.File.ReadAllBytes(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\..\..\testString.txt");
            HuffmanData mainData = new HuffmanData();
            mainData.uncompressedData = testString;
            Compress(ref mainData);
            Decompress(ref mainData);

            //byte[] testString = { 65, 66, 67, 154, 42, 68, 65, 26, 24, 16, 94, 46 };
            Console.WriteLine(testString.Length);
            Console.WriteLine(testString[mainData.sizeOfUncompressedData - 1]);
            Console.WriteLine(mainData.uncompressedData[mainData.sizeOfUncompressedData-1]);


            // decompress
            
        }
    }
}
