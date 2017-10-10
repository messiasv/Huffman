using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class FrequencyTable : SortedDictionary<byte,int>
    {
        public void Add(byte b)
        {
            if(ContainsKey(b))
            {
                this[b]++;
            }
            else
            {
                Add(b, 1);
            }
        }

        public override string ToString()
        {
            string _ = "";
            foreach(byte b in this.Keys)
            {
                _ += b + " :" + this[b] + "\n";
            }
            return _;
        }
    }
}
