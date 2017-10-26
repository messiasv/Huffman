using System;
using System.Collections.Generic;

namespace Huffman
{
    class Code : List<bool>
    {
        public override string ToString()
        {
            string _str = "";
            foreach(bool b in this)
            {
                if (b) _str += "1";
                else _str += "0";
            }
            return _str;
        }
    }
}
