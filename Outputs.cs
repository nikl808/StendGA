using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace stend
{
    class Outputs
    {
        public IOutputState State { get; set; }
 
        public Outputs(IOutputState state) { State = state; }
 
        public void ChannelOn() { State.On(this); }
        
        public void ChannelOff() { State.Off(this); }
    }

    interface IOutputState
    {
        void On(Outputs channel);
        void Off(Outputs channel);
    }


    class DOState : IOutputState
    {
        public void On(Outputs channel) { channel.State = new DOState(); }
        public void Off(Outputs channel) { channel.State = new DOState(); }
    }


    /*
    static void Main(string[] args)
    {
        Water water = new Water(new LiquidWaterState());
        water.Heat();
        water.Frost();
        water.Frost();
 
        Console.Read();
    }
 */
}