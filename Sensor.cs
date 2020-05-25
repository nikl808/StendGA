using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace stend
{
    abstract class Sensor
     {
         protected Uart currCom;
         
         public Sensor(Uart port) { currCom = port; }

         public float Read() { return ReadSensor(); }

         //default implementation
         protected virtual float ReadSensor() 
         {
             Error.instance.HandleErrorMessage("Sensor: wrong operation");
             return 0.0f;
         }
     }

     //Read analog input signals from module
     class AnalogSensor : Sensor
     {
         private int startHw;
         private int hwRange;
         private int slot;
         private int channel;
         private int numChan;
         private float startSens;
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
                     startHw = cfg.ModRangeUnitMin;
                     if (hwName == "Slot1") slot = 1;
                     else if (hwName == "Slot2") slot = 2;
                     else if (hwName == "Slot3") slot = 3;
                 }
                 else if (cfg.Name == SensorName)
                 {
                     sensRange = cfg.SenRangeUnitMax - cfg.SenRangeUnitMin;
                     startSens = cfg.SenRangeUnitMin;
                 }
             }
         }

         protected override float ReadSensor()
         {
             int currVal = 0;
             currCom.ReadSlot(slot, channel, numChan, ref currVal);

             float AiValue = (float)((currVal - startHw) / hwRange) * sensRange + startSens;
             return (float)Math.Round(AiValue, 3);
         }
     }

     //Read lir-da coordinates
     class LirMoving : Sensor
     {
         public bool zero { get; set; }
         private float zeroCoord = 0.0f;
         private float unit;
         private string addr;
         private string protocol;
         
         public LirMoving(Uart port, Hardware hw, string hwName, string SensorName) : base(port) 
         {
             foreach (Config cfg in hw)
             {
                 if (cfg.Name == currCom.ComPort)
                 {
                     protocol = cfg.uartProtocol;
                     addr = cfg.uartAddr;
                     break;
                 }

                 else if (cfg.Name == SensorName)
                 {
                     unit = cfg.SenRangeUnitMin;
                     break;
                 }
             } 
         }

         protected override float ReadSensor()
         {
             float currCoord = 0;
             byte[] receive;

             if (protocol == "Lir_ASCII")
             {
                 receive = currCom.SendToCom(new byte[3] { 35, Convert.ToByte(addr, 16), 97 });
                 if (receive != null) ASCIItoValue(receive);
             }
             else if (protocol == "Lir_BCD")
             {
                 receive = currCom.SendToCom(new byte[2] { 52, Convert.ToByte(addr, 16) });
                 if (receive != null) BCDtoValue(receive);
             }

             if (zero) zeroCoord = currCoord;
           
             return currCoord = (currCoord - zeroCoord) * unit;
         }

         //converters
         private float ASCIItoValue(byte[] bte)
         {
             string receiveCom = Encoding.ASCII.GetString(bte, 0, 8);
             receiveCom = Regex.Replace(receiveCom, @"[>\r\n\t\0]", "");
             float currCoord = float.Parse(receiveCom);
             return currCoord;
         }

         private float BCDtoValue(byte[] bte)
         {
             string receiveCom = BitConverter.ToString(bte);
             string[] arr = receiveCom.Split('-');
             Array.Copy(arr, 1, arr, 0, arr.Length - 1);
             StringBuilder reverse = new StringBuilder();
             for (int i = arr.Length - 1; i >= 0; i--)
                 if (!arr[i].Contains("0B")) reverse.Append(arr[i]);
             float currCoord = float.Parse(reverse.ToString());
             return currCoord;
         }
     }

     //modify to callback
     class LirSpeed : Sensor
     {
         private float prevCoord = 0.0f;
         private float currTime = 0.0f;
         private float prevTime = 0.0f;
         private float currValue = 0.0f;
         private string unit;

         public LirSpeed(Uart port, Hardware hw, string hwName, string SensorName) : base(port)
         {
             foreach (Config cfg in hw)
             {
                 if (cfg.Name == hwName) unit = cfg.SensorUnit;
                 break;
             }
         }

         protected override float ReadSensor()
         {
             float speed = 0.0f;
             if (unit == "mm/s") speed = (currValue - prevCoord) * (1000 / currTime - prevTime);
             else if (unit == "m/s") speed = (currValue - prevCoord) * (currTime - prevTime);
             
             prevCoord = currValue;
             prevTime = currTime;
             return speed;
         }
     }
}