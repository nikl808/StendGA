using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace stend
{
    class Sensor
    {
    }
    /*
    public enum LinearCommands { currCoord, ZeroLinear, currSpeed };

    public abstract class LocalSensor : IDisposable
    {
        protected IntPtr cPort;
        protected int cSlot;
        protected int cNumCh;
        protected double[] SignalRanges = new double[2];
        protected double[] MeasureRanges = new double[2];
        protected ManualResetEvent doneThread;
        private bool disposed = false;

        public LocalSensor(IntPtr port, int slot, int numChannels, double minSignal, double maxSignal, double minMeasure, double maxMeasure, ManualResetEvent Thread)
        {
            cPort = port;
            cSlot = slot;
            cNumCh = numChannels;
            SignalRanges[0] = minSignal;
            SignalRanges[1] = maxSignal;
            MeasureRanges[0] = minMeasure;
            MeasureRanges[1] = maxMeasure;
            doneThread = Thread;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    GC.Collect();
                }
            }
            disposed = true;
            Debug.WriteLine("sensor dispose");
        }

        ~LocalSensor()
        {
            Dispose(false);
        }

        //public abstract void ReadChannel(object sender, EventArgs e);
        public abstract void ReadChannel(object obj);
        public abstract double GetAiValue { get; }
    }

    public abstract class RemoteSensor : IDisposable
    {
        protected IntPtr hOpen;
        protected string extProtocol;
        protected double coordUnit;
        protected double speedUnit;
        private bool disposed = false;

        public RemoteSensor(string port, string baud, string protocol, string unitCoord, string unitSpeed)
        {
            extProtocol = protocol;

            if (unitCoord == "mm") coordUnit = 0.008;//lir-da9 actual discreteness in mm
            else if (unitCoord == "m") coordUnit = 0.0000008;

            if (unitSpeed == "m/s") speedUnit = 0;
            else if (unitSpeed == "mm/s") speedUnit = 0;

            //open com
            hOpen = PACNET.UART.Open(port + "," + baud + ",N,8,1");

            if (hOpen == (IntPtr)(-1)) throw new Exception("bad external interface");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    GC.Collect();
                }
            }
            disposed = true;
            PACNET.UART.Close(hOpen);
            hOpen = (IntPtr)(-1);
        }

        ~RemoteSensor()
        {
            Dispose(false);
            PACNET.UART.Close(hOpen);
        }

        public abstract double PoolSensorCoord(LinearCommands command, bool zero);
    }

    public class AISensors : LocalSensor
    {
        private double AiValue = 0.0;
        private double range = 0.0;
        private int numReadCh;

        public AISensors(IntPtr port, int slot, int numChannels, int numReadChannel, double minSignal, double maxSignal, double minMeasure, double maxMeasure, ManualResetEvent Thread)
            : base(port, slot, numChannels, minSignal, maxSignal, minMeasure, maxMeasure, Thread)
        {
            numReadCh = numReadChannel;
            range = (MeasureRanges[1] - MeasureRanges[0]) / (SignalRanges[1] - SignalRanges[0]);
        }

        public override void ReadChannel(object obj)
        {
            //modify
            //int threadIndex = (int)obj;
            //Debug.WriteLine("Thread: " + threadIndex + " started...");
            int AIVal = 0;
            PACNET.PAC_IO.ReadAIHex(cPort, cSlot, numReadCh, cNumCh, ref AIVal);
            AiValue = Convert.ToDouble((AIVal - SignalRanges[0]) * range);
            AiValue = Math.Round(AiValue, 4);
            //Debug.WriteLine("Thread: " + threadIndex + " result calc...");
            //doneThread.Set();
            //...
        }

        public override double GetAiValue
        {
            get
            {
                return AiValue;
            }
        }
    }

    public class SpeedLinearSensor : RemoteSensor
    {
        private byte[] ASCIISend;
        private byte[] BCDSend;
        private string receiveCom;
        private double Coord;
        private double Speed;
        private double ZeroPoint;

        public SpeedLinearSensor(string port, string addr, string baud, string protocol, string unitCoord, string unitSpeed)
            : base(port, baud, protocol, unitCoord, unitSpeed)
        {
            Coord = 0.0;
            Speed = 0.0;
            ZeroPoint = 0.0;
            ASCIISend = new byte[3] { 35, Convert.ToByte(addr, 16), 97 };
            BCDSend = new byte[2] { 52, Convert.ToByte(addr, 16) };
            receiveCom = "";
        }

        public override double PoolSensorCoord(LinearCommands command, bool zero)
        {
            if (hOpen != (IntPtr)(-1))
            {
                byte[] receive = null;

                if (extProtocol == "ascii")
                    receive = new byte[8];

                else if (extProtocol == "bcd")
                    receive = new byte[6];

                //send message
                if (extProtocol == "ascii") PACNET.UART.BinSend(hOpen, ASCIISend, 3);
                else if (extProtocol == "bcd") PACNET.UART.BinSend(hOpen, BCDSend, 2);

                //recieve com
                Thread.Sleep(5);//delete

                bool ret = false;
                if (extProtocol == "ascii") ret = PACNET.UART.Recv(hOpen, receive);
                else if (extProtocol == "bcd") ret = PACNET.UART.BinRecv(hOpen, receive, 6);

                //Convert byte to actual coordinate
                if (ret && extProtocol == "ascii") ASCIItoValue(receive);
                else if (ret && extProtocol == "bcd") BCDtoValue(receive);
                /*if ret == false=> send diagnostic message*/

               /* if (command == LinearCommands.currCoord && !zero) return Coord;
                else if (command == LinearCommands.ZeroLinear && zero)
                {
                    ZeroPoint = Coord;
                    return Coord = Coord - ZeroPoint;
                }
                else if (command == LinearCommands.currCoord && zero) return Coord = Coord - ZeroPoint;
                else if (command == LinearCommands.currSpeed) return Speed;
            }
            return 0.0;
        }

        //Service functions
        private void ASCIItoValue(byte[] bte)
        {
            receiveCom = Encoding.ASCII.GetString(bte, 0, 8);
            receiveCom = Regex.Replace(receiveCom, @"[>\r\n\t\0]", "");
            Coord = double.Parse(receiveCom);
            Coord = Coord * coordUnit;
        }

        private void BCDtoValue(byte[] bte)
        {
            receiveCom = BitConverter.ToString(bte);
            string[] arr = receiveCom.Split('-');
            Array.Copy(arr, 1, arr, 0, arr.Length - 1);
            StringBuilder reverse = new StringBuilder();
            for (int i = arr.Length - 1; i >= 0; i--)
                if (!arr[i].Contains("0B")) reverse.Append(arr[i]);
            Coord = double.Parse(reverse.ToString());
            Coord = Coord * coordUnit;
        }
    }
    */
}
