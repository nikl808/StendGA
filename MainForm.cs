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
        #region class field
        private Timer timer;
        private XPacBackplane backplane;
        private Uart lirCom;
        private ModbusTcpSlave slave;
        private AnalogSensor StrainLoad;
        private AnalogSensor Pressure;
        private AnalogSensor Stretch;
        private LirMoving moving;
        private LirSpeed speed;
        private Outputs Output;
        private bool[] prevOutputs;
        // private BindingList<string> errors = new BindingList<string>();
        #endregion

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

            //Create outputs
            Output = new Outputs(backplane, 2, 16);

            //Create prevOutputs
            prevOutputs = new bool[6];

            //open backplane
            statusBar.Text = "";
            if (!backplane.OpenCom()) statusBar.Text += "Backplane: FAIL";
            else statusBar.Text += "Backplane: OK";

            //open com
            if (!lirCom.OpenCom()) statusBar.Text += " | Comport: " + lirCom.ComPort + " FAIL";
            else statusBar.Text += " | Comport: " + lirCom.ComPort + " OK";

            //start modbus slave
            slave.StartSlave();
            
            timer.Enabled = true;
        }
        #endregion

        #region modbus converter
        private ushort[] DoubleToUshort(double val)
        {
            long[] lVal = new long[1]{(long)(val * 1000)};
            ushort[] ret = new ushort[2]{0,0};

            Buffer.BlockCopy(lVal, 0, ret, 0, 4);
            return ret;
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
            ushort[] ret = new ushort[2];
            
            //Read strain load and store to AI[1-2]
            strLoad = StrainLoad.Read();
            ret = DoubleToUshort(strLoad);
            slave.slave.DataStore.InputRegisters[1] = ret[0];
            slave.slave.DataStore.InputRegisters[2] = ret[1];

            //Read pressure and store to AI[3-4]
            press = Pressure.Read();
            ret = DoubleToUshort(press);
            slave.slave.DataStore.InputRegisters[3] = ret[0];
            slave.slave.DataStore.InputRegisters[4] = ret[1];

            //Read stretch and store to AI[5-6]
            stretch = Stretch.Read();
            ret = DoubleToUshort(stretch);
            slave.slave.DataStore.InputRegisters[5] = ret[0];
            slave.slave.DataStore.InputRegisters[6] = ret[1];

            //Read moving and store to AI[7-8]
            mov = moving.Read();
            ret = DoubleToUshort(mov);
            slave.slave.DataStore.InputRegisters[7] = ret[0];
            slave.slave.DataStore.InputRegisters[8] = ret[1];

            //Calc speed and store to AI[9-10]
            spd = speed.Read();
            ret = DoubleToUshort(spd);
            slave.slave.DataStore.InputRegisters[9] = ret[0];
            slave.slave.DataStore.InputRegisters[10] = ret[1];

            //Display readable values
            if (ConEnCheck.Checked)
            {
                StrainLoadText.Text = strLoad.ToString();
                ComprText.Text = press.ToString();
                StrText.Text = stretch.ToString();
                MovingText.Text = mov.ToString();
                SpdText.Text = spd.ToString();
            }
            
            //Set outputs from modbus store
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    if (slave.slave.DataStore.CoilDiscretes[i] != prevOutputs[i])
                    {
                        Output.SetChannel(i);
                        prevOutputs[i] = slave.slave.DataStore.CoilDiscretes[i];
                    }
                }
            }
            
        }
        #endregion

        #region moving buttons click
        private void MovBtn_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "MovFwdBtn") Output.SetChannel(3);
            else if (btn.Name == "MovBkwdBtn") Output.SetChannel(4);
            else if (btn.Name == "MovStpBtn") Output.SetChannel(5);
        }
        #endregion

        #region pump buttons click
        private void PmpBtn_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "OilOnOffBtn") Output.SetChannel(0);
            else if (btn.Name == "LoPressBtn") Output.SetChannel(1);
            else if (btn.Name == "HiPressBtn") Output.SetChannel(2);
        }
        #endregion

        #region zero/actual click
        private void ZABtn_Click(object sender, EventArgs e)
        {
            if (moving.SetActual == true)
            {
                moving.SetZero = true;
                moving.SetActual = false;
            }
            else if (moving.SetActual == false) moving.SetActual = true;
        }
        #endregion

        #region enable controll check
        private void ConEnCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (!ConEnCheck.Checked)
            {
                StrainLoadText.Text = "";
                ComprText.Text = "";
                StrText.Text = "";
                MovingText.Text = "";
                SpdText.Text = "";
                OilOnOffBtn.Enabled = false;
                LoPressBtn.Enabled = false;
                HiPressBtn.Enabled = false;
                MovFwdBtn.Enabled = false;
                MovBkwdBtn.Enabled = false;
                MovStpBtn.Enabled = false;
                ZABtn.Enabled = false;
            }
            else
            {
                OilOnOffBtn.Enabled = true;
                LoPressBtn.Enabled = true;
                HiPressBtn.Enabled = true;
                MovFwdBtn.Enabled = true;
                MovBkwdBtn.Enabled = true;
                MovStpBtn.Enabled = true;
                ZABtn.Enabled = true;
                Output.ResetAll();
            }
        }
        #endregion

        #region Form closing
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            //Reset outputs
            Output.ResetAll();
        }
        #endregion
    }
}