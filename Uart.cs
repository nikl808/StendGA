using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PACNET;

namespace stend
{
    abstract class Uart: IDisposable
    {
        public IntPtr hPort { get; internal set; }
        public string ComPort { get; set; }
        public string Baudrate { get; set; }
        public string Parity { get; set; }
        public uint ReceiveMsgLenght { get; set; }

        public bool OpenCom()
        {
            hPort = UART.Open(ComPort + "," + Baudrate + "," + Parity);
            if (hPort == (IntPtr)(-1))
            {
                Error.instance.HandleErrorLog("ERROR::UART::" + ComPort + ":", PACNET.ErrHandling.GetErrorMessage(PACNET.ErrHandling.GetLastError()));
                Dispose();
                return false;
            }
            return true; 
        }
         
        public byte[] SendToCom(byte[] data) { return Send(data); }
        public byte[] SendToCom(string Command) { return Send(Command); }
        public void ReadSlot(int slot,int channel,int totalch, ref int val) { Read(slot,channel,totalch,ref val); }
        public void WriteSlot(int slot, int totalch, uint val){ Write(slot, totalch, val); }

        //Send data array to com
        protected virtual byte[] Send(byte[] data)
        {
            Error.instance.HandleErrorMessage("UART: wrong operation");
            return new byte[0];
        }

        //Read value from controller module
        protected virtual void Read(int slot,int channel,int totalch, ref int val) 
        { 
            Error.instance.HandleErrorMessage("UART: wrong operation"); 
            val = 0;
        }

        protected virtual void Write(int slot, int totalch, uint val)
        {
            Error.instance.HandleErrorMessage("UART: wrong operation");
        }

        //Send command to comprt
        protected virtual byte[] Send(string Command)
        {
            byte[] cmd = new byte[64];
            byte[] receive = new byte[ReceiveMsgLenght];
            cmd = MISC.AnsiString(Command);
            bool ret = UART.SendCmdExt(hPort, cmd, 64, receive, (uint)receive.Length);
            if (!ret)
            {
                Error.instance.HandleErrorLog("ERROR::UART:: " + ComPort + ":", "fail send command/receive data from");
                return new byte[0];
            }
            return receive;
        }
        public virtual void Dispose() { UART.Close(hPort); }
    }

    class XPacComportGeneric : Uart
    {     
        protected override byte[] Send(byte[] data)
        {
            byte[] receive = new byte[ReceiveMsgLenght];

            bool ret = UART.BinSend(hPort, data, (uint)data.Length);
            if (!ret) 
            {
                Error.instance.HandleErrorLog("ERROR::UART:: " + ComPort + ":", "fail send data");
                return new byte[0];
            }
            ret = UART.BinRecv(hPort, receive,ReceiveMsgLenght);
            if (!ret) 
            {
                Error.instance.HandleErrorLog("ERROR::UART:: " + ComPort + ":", "fail receive data");
                return new byte[0];
            }
            return receive;
        }
    }

    class XPacComportSpec : Uart
    {
        protected override byte[] Send(byte[] data)
        {
            byte[] receive = new byte[ReceiveMsgLenght];

            bool ret = UART.BinSend(hPort, data, (uint)data.Length);
            if (!ret)
            {
                Error.instance.HandleErrorLog("ERROR::UART:: " + ComPort + ":", "fail send data");
                return new byte[0];
            }
            ret = UART.Recv(hPort, receive);
            if (!ret)
            {
                Error.instance.HandleErrorLog("ERROR::UART:: " + ComPort + ":", "fail receive data");
                return new byte[0];
            }
            return receive;
        }
    }

    class XPacBackplane : Uart
    {
        protected override void Read(int slot,int channel,int totalch, ref int val)
        {
            bool ret = PACNET.PAC_IO.ReadAIHex(hPort, slot, channel, totalch, ref val);
            if (!ret)
            {
                Error.instance.HandleErrorLog("ERROR::BACKPLANE:: ", "Can't read slot " + slot.ToString());
                val = 0;
            }
        }

        protected override void Write(int slot,int totalch, uint val)
        {
            bool ret = PACNET.PAC_IO.WriteDO(hPort, slot, totalch, val);
            if (!ret) Error.instance.HandleErrorLog("ERROR::BACKPLANE:: ", "Can't write value: " + "to slot " + slot.ToString());
        }
    }
}