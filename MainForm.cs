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
        public Hardware cfg = new Hardware(12);
        public MainForm()
        {
            InitializeComponent();
            
            //config test
            Error.instance._list += ListBoxEvent;
            
            //XMLFileWriter fw = new XMLWriter();
            //ConfigFileWriter guiConf = new ConfigFileWriter();
            //fw.WriteFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\test.xml", test);
            //guiConf.WriteFile<string>("System_Disk2\\StandGA\\gui\\conf.txt", "{Root} = System_Disk2\\StandGA\\;");
            //guiConf.WriteFile<string>("System_Disk2\\StandGA\\gui\\conf.txt", "{GUI} = CrossingTheLine;");
            //XMLFileReader fr = new XMLFileReader();
            //test = fr.ReadFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\test.xml");
            
            //uart test
            //XPacComport TestCom = new XPacComport();
            //TestCom.ComPort = "COM3";
            //TestCom.Baudrate = "19200";
            //TestCom.Parity = "N,8,1";
            //TestCom.ReceiveMsgLenght = 8;

            //TestCom.OpenCom();
            //byte[] ASCIISend = new byte[3] { 35, Convert.ToByte("01", 16), 97 };
            //TestCom.SendToCom(ASCIISend);

            //modbus tcp master test
            //ModbusTcpMaster mMaster = new ModbusTcpMaster("192.168.0.5", 502, new WriteFuncFactory());
            //mMaster.WriteSingleRegister(1, 1, 1000);            

            //ModbusTcpSlave mSlave = new ModbusTcpSlave(1);
            //mSlave.StartSlave();

            cfg.config[0] = new uConfig("COM3", "19200", "Generic");
            cfg.config[1] = new uConfig("COM4", "19200", "Generic");
            cfg.config[2] = new uConfig("COM5", "19200", "Generic");
            //tcp
            cfg.config[3] = new tConfig("Eth", "Generic", 1, "0.0.0.0", false);
            //module
            cfg.config[4] = new mConfig("Slot1", "DI/DO", 0, 65535);
            cfg.config[5] = new mConfig("Slot2", "AI/AO", 0, 65535);
            cfg.config[6] = new mConfig("Slot3", "AI/AO", 0, 65535);
            //sensor
            cfg.config[7] = new sConfig("StrainLoad", "kgf", 0, 500);
            cfg.config[8] = new sConfig("Pressure", "kPa", 0, 500);
            cfg.config[9] = new sConfig("Pressure", "kPa", 0, 500);
            cfg.config[10] = new sConfig("Moving", "mm", 0, 0);
            cfg.config[11] = new sConfig("Speed", "mm/s", 0, 0);
            
            XMLFileWriter fw = new XMLFileWriter();
            fw.WriteFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\hardware.xml", cfg);
        }

        public void MainForm_Init(Hardware cfg)
        {
        }

        private void ListBoxEvent(object sender, MsgListEvent e){ LogList.Items.Add(e.MsgLine); }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            XMLFileReader fw = new XMLFileReader();
            cfg = fw.ReadFile<Hardware>("System_Disk2\\StandGA\\System\\hardware.xml");

            SettingsForm form = new SettingsForm(cfg);
            form.Show();
        }   
    }
}