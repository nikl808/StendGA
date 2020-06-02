using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace stend
{
    class Utils
    {
        public void SetBit(out uint val, int bitnum)
        {
            val = 0;
            val |= (uint)1 << bitnum;
        }
        public void InvBit(ref uint val, int bitnum) { val ^= (uint)1 << bitnum; }
        public bool IsBit(uint val, int bitnum)
        {
            if ((val & (1 << bitnum)) != 0) return true;
            else return false;
        }
    }
}