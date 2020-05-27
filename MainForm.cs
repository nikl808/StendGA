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
        private Timer timer;
        private XPacBackplane backplane;
        private Uart lirCom;
        private ModbusTcpSlave slave;
        private AnalogSensor StrainLoad;
        private AnalogSensor Pressure;
        private AnalogSensor Stretch;
        private LirMoving moving;
        private LirSpeed speed;
       // private BindingList<string> errors = new BindingList<string>();
       
        #region constructor
        public MainForm()
        {
            InitializeComponent();
            
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += MainHandler;
            
            MainForm_Init();
            //init error list event
            Error.instance._list += ErrorEvent;
            //LogList.DataSource = errors;
            
        }
        #endregion

        #region form initialization
        public void MainForm_Init()
        {
            //Init config
            Hardware cfg = new Hardware();
            using(XMLFileReader fw = new XMLFileReader()) cfg = fw.ReadFile<Hardware>("System_Disk2\\StandGA\\System\\hardware.xml");

            //setup backplane
            backplane = new XPacBackplane();
            backplane.ComPort = "COM1";
            backplane.Baudrate = "115200";
            backplane.Parity = "N,8,1";
            backplane.ReceiveMsgLenght = 0;

            foreach (Config itr in cfg)
            {
                //setup com for lir
                if (itr.Name == "COM3")
                {
                    if (itr.uartProtocol == "Lir_ASCII")
                    {
                        lirCom = new XPacComportSpec();
                        lirCom.ReceiveMsgLenght = 13;
                    }
                    else if (itr.uartProtocol == "Lir_BCD")
                    {
                        lirCom = new XPacComportGeneric(); 
                        lirCom.ReceiveMsgLenght = 6;
                    }
                    else if (itr.uartProtocol == "Generic")
                    {
                        lirCom = new XPacComportGeneric();
                        lirCom.ReceiveMsgLenght = 50;
                    }
                    lirCom.ComPort = itr.Name;
                    lirCom.Baudrate = itr.uartBaudrate;
                    lirCom.Parity = "N,8,1";
                }

                //Init Labels
                else if (itr.Name == "StrainLoad") StrainLoadLabel.Text = itr.SensorUnit;
                else if (itr.Name == "Pressure")
                {
                    ComprLabel.Text = itr.SensorUnit;
                    StrLabel.Text = itr.SensorUnit;
                }
                else if (itr.Name == "Moving") MovLabel.Text = itr.SensorUnit;
                else if (itr.Name == "Speed") SpdLabel.Text = itr.SensorUnit;

                //setup modbus slave
                else if (itr.Name == "Eth" && itr.asMaster == false) slave = new ModbusTcpSlave(itr.slaveID);
            }

            //Create sensors
            StrainLoad = new AnalogSensor(backplane, cfg, "Slot3", "StrainLoad",0,8);
            Pressure= new AnalogSensor(backplane, cfg, "Slot1", "Pressure",0,8);
            Stretch = new AnalogSensor(backplane, cfg, "Slot1", "Pressure",1,8);
            moving = new LirMoving(lirCom, cfg, lirCom.ComPort, "Moving");
            speed = new LirSpeed(lirCom, cfg, "Speed");

            //open backplane
            statusBar.Text = "";
            if (!backplane.OpenCom()) statusBar.Text += "Backplane: FAIL";
            else statusBar.Text += "Backplane: OK";

            //open com
            if (!lirCom.OpenCom()) statusBar.Text += " | Comport: " + lirCom.ComPort + " FAIL";
            else statusBar.Text += " | Comport: " + lirCom.ComPort + " OK";

            //start modbus slave
            //slave.StartSlave();
            
            timer.Enabled = true;
        }
        #endregion

        #region Error event
        private void ErrorEvent(object sender, MsgListEvent e){/*errors.Add(e.MsgLine);*/ }
        #endregion

        #region SettingForm event
        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            backplane.Dispose();
            lirCom.Dispose();
            timer.Enabled = false;
            SettingsForm form = new SettingsForm();
            form.Owner = this;
            form.Show();
        }
        #endregion

        #region main program handler
        public void MainHandler(object sender, EventArgs e)
        {
            double strLoad, press, stretch, mov, spd;

            strLoad = StrainLoad.Read();
            press = Pressure.Read();
            stretch = Stretch.Read();
            mov = moving.Read();
            spd = speed.Read();

            //Display readable values
            
                StrainLoadText.Text = strLoad.ToString();
                ComprText.Text = press.ToString();
                StrText.Text = stretch.ToString();
                MovingText.Text = mov.ToString();
                SpdText.Text = spd.ToString();
        }
        #endregion

        #region moving buttons click
        private void MovBtn_click(object sender, EventArgs e)
        {
        }
        #endregion

        #region pump buttons click
        private void PmpBtn_click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}