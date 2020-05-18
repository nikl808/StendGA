using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stend
{
    public partial class LirProgForm : Form
    {
        #region class field
        string protocol;
        string baudrate;
        Dictionary<string, string> progRequest = new Dictionary<string,string>();
        #endregion

        #region constructor
        public LirProgForm(string protoc,string baud)
        {
            InitializeComponent();
            if (protoc == "Lir_ASCII") protocol = "00";
            else if (protoc == "Lir_BCD") protocol = "01";
            switch (baud)
            {
                case "19200":
                    baudrate = "00";
                    break;
                case "28800":
                    baudrate = "01";
                    break;
                case "38400":
                    baudrate = "02";
                    break;
                case "57600":
                    baudrate = "03";
                    break;
                case "76800":
                    baudrate = "04";
                    break;
                case "115200":
                    baudrate = "05";
                    break;
            }
            LirProgForm_Init();
        }
        #endregion

        #region form initialization
        private void LirProgForm_Init()
        {            
            byte[] first = Encoding.ASCII.GetBytes("#p#");

            progRequest.Add("const" ,(BitConverter.ToString(first).Replace("-","")));
            progRequest.Add("addr", "01");
            progRequest.Add("protocol", protocol);
            progRequest.Add("speed", baudrate);
            progRequest.Add("bit", "00");
        }
        #endregion

        #region domain updown event
        private void BitUpDown_ValueChanged(object sender, EventArgs e) { progRequest["bit"] = (Convert.ToInt32(BitUpDown.Value)).ToString("x2"); }

        private void NetUpDown_ValueChanged(object sender, EventArgs e) { progRequest["addr"] = (Convert.ToInt32(NetUpDown.Value)).ToString("x2"); }
        #endregion

        #region Write button click
        private void ProgButton_Click(object sender, EventArgs e)
        {
            // build message
            string raw = progRequest["const"] +
                         progRequest["addr"] +
                         progRequest["protocol"] +
                         progRequest["speed"] +
                         progRequest["bit"] + "0D";
            byte[] send = StringToByteArray(raw);
            byte[] receive = new byte[6];

            //open com and send data
            using (XPacComport comport = new XPacComport())
            {
                comport.ComPort = "COM3";
                comport.Baudrate = "19200";
                comport.Parity = "N,8,1";
                comport.ReceiveMsgLenght = 6;
                comport.OpenCom();
                comport.SendToCom(send);
            }

            //compare
            byte[] snd = new byte[4];
            byte[] rcv = new byte[4];
            for (int i = 0; i < snd.Length; i++) snd[i] = send[i + 3];
            for (int i = 0; i < rcv.Length; i++) rcv[i] = receive[i + 1];

            var res = snd.SequenceEqual(rcv);
            if (!res) Error.instance.HandleErrorMessage ("Lir-916 is not programmed");
                
            else Error.instance.HandleWarningMessage("OK");
        }
        #endregion

        #region String-to-array converter
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        #endregion
    }
}