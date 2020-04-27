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
        public int ReceiveMsgLenght { get; set; }
         
        public byte[] SendToCom(byte[] data) { return Send(data); }
        public byte[] SendToCom(string Command) { return Send(Command); }
             
        protected abstract byte[] Send(byte[] data);
        protected abstract byte[] Send(string Command);
        public virtual void Dispose() { }
    }

    class XPacComport : Uart
    {     
        public void OpenCom()
        {
            hPort = UART.Open(ComPort + "," + Baudrate + "," + Parity);
            if (hPort == (IntPtr)(-1))
            {
                Error.instance.HandleErrorLog("ERROR::UART::" + ComPort + ":",PACNET.ErrHandling.GetErrorMessage(PACNET.ErrHandling.GetLastError()));
                Dispose();
            }
        }

        public override void Dispose() { UART.Close(hPort); }

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

        protected override byte[] Send(string Command)
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
    }
}