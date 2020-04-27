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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //config test
            Error.instance._list += ListBoxEvent;
            Hardware test = new Hardware();
            //XMLFileWriter fw = new XMLWriter();
            ConfigFileWriter guiConf = new ConfigFileWriter();
            //fw.WriteFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\test.xml", test);
            guiConf.WriteFile<string>("System_Disk2\\StandGA\\gui\\conf.txt", "{Root} = System_Disk2\\StandGA\\;");
            guiConf.WriteFile<string>("System_Disk2\\StandGA\\gui\\conf.txt", "{GUI} = CrossingTheLine;");
            XMLFileReader fr = new XMLFileReader();
            test = fr.ReadFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\test.xml");
            
            //uart test
            XPacComport TestCom = new XPacComport();
            TestCom.ComPort = "COM3";
            TestCom.Baudrate = "19200";
            TestCom.Parity = "N,8,1";
            TestCom.ReceiveMsgLenght = 8;

            TestCom.OpenCom();
            byte[] ASCIISend = new byte[3] { 35, Convert.ToByte("01", 16), 97 };
            TestCom.SendToCom(ASCIISend);

            //modbus tcp master test
            //ModbusTcpMaster mMaster = new ModbusTcpMaster("192.168.0.5", 502, new WriteFuncFactory());
            //mMaster.WriteSingleRegister(1, 1, 1000);            

            ModbusTcpSlave mSlave = new ModbusTcpSlave(1);
            mSlave.StartSlave();
        }
        void ListBoxEvent(object sender, MsgListEvent e){ LogList.Items.Add(e.MsgLine); }   
    }
}