using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace stend
{
    #region Sensor
    abstract class Sensor
     {
         protected Uart currCom;
         protected static TimeSpan ts;
         protected static double currCoord;
         
         public Sensor(Uart port) { currCom = port; }

         public double Read() { return ReadSensor(); }

         //default implementation
         protected virtual double ReadSensor() 
         {
             Error.instance.HandleErrorMessage("Sensor: wrong operation");
             return 0.0;
         }
     }
    #endregion

    #region Analog inputs
    class AnalogSensor : Sensor
     {
         private int slot;
         private int channel;
         private int nullHw;
         private int numChan;
         private float nullSens;
         private int hwRange;
         private float sensRange;
         
         public AnalogSensor(Uart port, Hardware hw, string hwName, string SensorName, int chan, int numCh) : base(port)
         {
             channel = chan;
             numChan = numCh;

             foreach (Config cfg in hw)
             {
                 if (cfg.Name == hwName)
                 {
                     hwRange = cfg.ModRangeUnitMax - cfg.ModRangeUnitMin;
                     nullHw = cfg.ModRangeUnitMin;
                     if (hwName == "Slot1") slot = 1;
                     else if (hwName == "Slot2") slot = 2;
                     else if (hwName == "Slot3") slot = 3;
                 }
                 else if (cfg.Name == SensorName)
                 {
                     sensRange = cfg.SenRangeUnitMax - cfg.SenRangeUnitMin;
                     nullSens = cfg.SenRangeUnitMin;
                 }
             }
         }

         protected override double ReadSensor()
         {
             int currVal = 0;
             //Read controller module
             currCom.ReadSlot(slot, channel, numChan, ref currVal);
             //calc linear scale
             var AiValue = (double)(currVal - nullHw) / hwRange * sensRange + nullSens;
             return Math.Round(AiValue, 3);
         }
     }
    #endregion

    #region lir moving
    class LirMoving : Sensor
     {
         public bool SetZero { get; set; }
         public bool SetActual { get; set; }
         private double zeroCoord = 0.0f;
         private float unit;
         private string addr;
         private string protocol;
         private Stopwatch timer;

         public LirMoving(Uart port, Hardware hw, string hwName, string SensorName) : base(port) 
         {
             timer = new Stopwatch();
             SetActual = true;

             foreach (Config cfg in hw)
             {
                 if (cfg.Name == currCom.ComPort)
                 {
                     protocol = cfg.uartProtocol;
                     addr = cfg.uartAddr;
                 }

                 else if (cfg.Name == SensorName) unit = cfg.SenRangeUnitMax;
             }
         }

         protected override double ReadSensor()
         {
             byte[] receive;

             if (protocol == "Lir_ASCII")
             {
                 receive = currCom.SendToCom(new byte[3] { 35, Convert.ToByte(addr, 16), 97 });
                 if (receive.Length != 0)
                 {
                     timer.Stop();
                     ts = timer.Elapsed;
                     currCoord = ASCIItoValue(receive);
                 }
             }
             else if (protocol == "Lir_BCD")
             {
                 receive = currCom.SendToCom(new byte[2] { 52, Convert.ToByte(addr, 16) });
                 if (receive.Length != 0)
                 {
                     timer.Stop();
                     ts = timer.Elapsed;
                     currCoord = BCDtoValue(receive);
                 }
             }

             if (SetZero)
             {
                 zeroCoord = currCoord;
                 SetZero = false;
             }
             else if (SetActual) zeroCoord = 0;
             
             currCoord = (currCoord - zeroCoord) * unit;

             timer.Reset();
             timer.Start();
             currCoord = Math.Round(currCoord, 3);
             return currCoord;
         }

         //converters
         private double ASCIItoValue(byte[] bte)
         {
             string receiveCom = Encoding.ASCII.GetString(bte, 0, 8);
             receiveCom = Regex.Replace(receiveCom, @"[>\r\n\t]", "");

             return double.Parse(receiveCom);
         }

         private double BCDtoValue(byte[] bte)
         {
             string receiveCom = BitConverter.ToString(bte);
             string[] arr = receiveCom.Split('-');
             Array.Copy(arr, 1, arr, 0, arr.Length - 1);
             StringBuilder reverse = new StringBuilder();
             for (int i = arr.Length - 1; i >= 0; i--) if (!arr[i].Contains("0B")) reverse.Append(arr[i]);
             return double.Parse(reverse.ToString());
         }
     }
    #endregion

    #region lir speed
    class LirSpeed : Sensor
     {
         private double prevCoord = 0.0f;
         private string movUnit;
         private string spdUnit;
         
         public LirSpeed(Uart port, Hardware hw, string SensorName) : base(port)
         {
             foreach (Config cfg in hw) 
             {
                 if (cfg.Name == SensorName) spdUnit = cfg.SensorUnit;
                 if (cfg.Name == "Moving") movUnit =  cfg.SensorUnit;
             }
         }

         protected override double ReadSensor()
         {
             double speed = 0.0f;
             int timespan = 0;
             if (ts.Milliseconds == 0) timespan = 1;
             else timespan = ts.Milliseconds;

             if (prevCoord != currCoord)
             {
                 if ((movUnit == "m" && spdUnit == "m/s") | (movUnit == "mm" && spdUnit == "mm/s")) speed = Math.Abs((currCoord - prevCoord) / (timespan * 0.001));
                 else speed = Math.Abs((currCoord - prevCoord) * 0.001 / (timespan * 0.001));
                 prevCoord = currCoord;
             }
             
             return Math.Round(speed,3);
         }
     }
    #endregion
}