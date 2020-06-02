using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace stend
{
    class Outputs
    {
        private XPacBackplane backp;
        private Utils bits;
        private int currSlot;
        private int numCh;
        private uint isOn;
        private uint pumpState;
        private uint outputsState;
        
        public Outputs(XPacBackplane bp, int slot, int totalch) 
        { 
            backp = bp;
            bits = new Utils();
            currSlot = slot;
            numCh = totalch;
        }

        public void SetChannel(int channel) 
        {
            uint currState = 0;

            if (channel == 0)
            {
                if (bits.IsBit(isOn, 0)) bits.InvBit(ref isOn, 0);
                else bits.SetBit(out isOn, channel);
                backp.WriteSlot(currSlot, numCh, isOn);
                outputsState = isOn;
            }

            //if pump is on
            if (bits.IsBit(isOn, 0))
            {
                if (channel == 1 | channel == 2) bits.SetBit(out pumpState, channel);

                //if pump pressure is selected
                if (pumpState != 0)
                {
                    bits.SetBit(out currState, channel);
                    currState |= isOn | pumpState;
                    backp.WriteSlot(currSlot, numCh, currState);
                    outputsState = currState;
                }
            }
            else ResetAll();
        }

        public void ResetAll() 
        { 
            backp.WriteSlot(currSlot, numCh, 0x0);
            pumpState = 0;
        }
    }
}