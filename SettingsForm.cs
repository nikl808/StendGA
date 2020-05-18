using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Net;
using System.Net.Sockets;

namespace stend
{
    public partial class SettingsForm : Form
    {
        #region class field
        Hardware currCfg = new Hardware();
        //dictionary of values of physical quantities
        Dictionary<string, string[]> Units = new Dictionary<string, string[]>();
        #endregion

        #region constructor
        public SettingsForm(Hardware config)
        {
            InitializeComponent();
            currCfg = ConfigClone.Clone(config);
            SettingsForm_Init();
        }
        #endregion

        #region Form initialization
        //initialize form components 
        private void SettingsForm_Init()
        {
            Dictionary<string, string[]> ParseItems = new Dictionary<string, string[]>();

            using (ConfigFileParser parser = new ConfigFileParser())
            {
                //Open settings file
                using (FileReader reader = new GenericFileReader()) { parser.Parse(reader.ReadFile("System_Disk2\\StandGA\\System\\settings.ini", 20)); }               //get gui items
                ParseItems = parser.GetSetting("HWSettings");
                //get units
                Units = parser.GetSetting("Units");
            }
            //Initialize comboboxes
            foreach (KeyValuePair<string, string[]> keyVal in ParseItems)
            {
                switch (keyVal.Key)
                {
                    case "ModuleType":
                        FillItems(SlotTypeCombo1, (string[])keyVal.Value.Clone());
                        FillItems(SlotTypeCombo2, (string[])keyVal.Value.Clone());
                        FillItems(SlotTypeCombo3, (string[])keyVal.Value.Clone());
                        break;
                    case "Baudrate":
                        FillItems(BaudCombo1, (string[])keyVal.Value.Clone());
                        FillItems(BaudCombo2, (string[])keyVal.Value.Clone());
                        FillItems(BaudCombo3, (string[])keyVal.Value.Clone());
                        break;
                    case "UartProtocol":
                        FillItems(UProtocCombo1, (string[])keyVal.Value.Clone());
                        FillItems(UProtocCombo2, (string[])keyVal.Value.Clone());
                        FillItems(UProtocCombo3, (string[])keyVal.Value.Clone());
                        break;
                    case "EthProtocol":
                        FillItems(EProtocCombo, (string[])keyVal.Value.Clone());
                        break;
                    case "LoadCellUnit":
                        FillItems(SLUnitCombo, (string[])keyVal.Value.Clone());
                        break;
                    case "PressureUnit":
                        FillItems(PressUnitCombo, (string[])keyVal.Value.Clone());
                        break;
                    case "MovingUnit":
                        FillItems(MovUnitCombo, (string[])keyVal.Value.Clone());
                        break;
                    case "SpeedUnit":
                        FillItems(SpdUnitCombo, (string[])keyVal.Value.Clone());
                        break;
                    default: 
                        Error.instance.HandleErrorMessage("Error setting string: "+keyVal.Key);
                        break;
                }
            }

            //fill textboxes and select items in comboboxes
            foreach (Config iter in currCfg)
            {
                switch (iter.Name)
                {
                    case "COM3":
                        BaudCombo1.SelectedItem = iter.uartBaudrate;
                        UProtocCombo1.SelectedItem = iter.uartProtocol;
                        break;
                    case "COM4":
                        BaudCombo2.SelectedItem = iter.uartBaudrate;
                        UProtocCombo2.SelectedItem = iter.uartProtocol;
                        break;
                    case "COM5":
                        BaudCombo3.SelectedItem = iter.uartBaudrate;
                        UProtocCombo3.SelectedItem = iter.uartProtocol;
                        break;
                    case "Eth":
                        EProtocCombo.SelectedItem = iter.ethProtocol;
                        SlvIDText.Text = iter.slaveID.ToString();
                        SlvIPText.Text = iter.slaveIP;
                        IPAddress addr = IPAddress.Parse("127.0.0.1");
                        foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                            {
                                addr = ip;
                                break;
                            }
                        }
                        EthIpText.Text = addr.ToString();
                        //set master checkbox
                        TcpMasterCheck.Checked = iter.asMaster;
                        if (iter.ethProtocol == "ModbusTCP" && TcpMasterCheck.Enabled == false)
                        {
                            TcpMasterCheck.Enabled = true;
                            if (TcpMasterCheck.Checked == true && (SlvIDText.ReadOnly == true | SlvIPText.ReadOnly == true))
                            {
                                SlvIDText.ReadOnly = false;
                                SlvIPText.ReadOnly = false;
                            }
                        }
                        break;
                    case "Slot1":
                        SlotTypeCombo1.SelectedItem = iter.Type;
                        SlotMinRanText1.Text = iter.ModRangeUnitMin.ToString();
                        SlotMaxRanText1.Text = iter.ModRangeUnitMax.ToString();
                        SlotNameText1.Text = PACNET.Sys.GetModuleName(1);
                        break;
                    case "Slot2":
                        SlotTypeCombo2.SelectedItem = iter.Type;
                        SlotMinRanText2.Text = iter.ModRangeUnitMin.ToString();
                        SlotMaxRanText2.Text = iter.ModRangeUnitMax.ToString();
                        SlotNameText2.Text = PACNET.Sys.GetModuleName(2);
                        break;
                    case "Slot3":
                        SlotTypeCombo3.SelectedItem = iter.Type;
                        SlotMinRanText3.Text = iter.ModRangeUnitMin.ToString();
                        SlotMaxRanText3.Text = iter.ModRangeUnitMax.ToString();
                        SlotNameText3.Text = PACNET.Sys.GetModuleName(3);
                        break;
                    case "StrainLoad":
                        SLUnitCombo.SelectedItem = iter.SensorUnit;
                        SLminMeasText.Text = iter.SenRangeUnitMin.ToString();
                        SLmaxMeasText.Text = iter.SenRangeUnitMax.ToString();
                        break;
                    case "Pressure":
                        PressUnitCombo.SelectedItem = iter.SensorUnit;
                        PressMinMeasText.Text = iter.SenRangeUnitMin.ToString();
                        PressMaxMeasText.Text = iter.SenRangeUnitMax.ToString();
                        break;
                    case "Moving":
                        MovUnitCombo.SelectedItem = iter.SensorUnit;
                        break;
                    case "Speed":
                        SpdUnitCombo.SelectedItem = iter.SensorUnit;
                        break;
                    default:
                        Error.instance.HandleErrorMessage("Error config string: " + iter.Name);
                        break;
                }
            }
        }
        #endregion

        #region Fill combobox with items
        private void FillItems(ComboBox ctrl, string[] items) { for (int i = 0; i < items.Length; i++) ctrl.Items.Add(items[i]); }
        #endregion

        #region Restore previous text
        private void RestorePrevText(TextBox ctrl, Config itr)
        {
            if ((ctrl.Name == "SlotMinRanText1" && itr.Name == "Slot1") |
                (ctrl.Name == "SlotMinRanText2" && itr.Name == "Slot2") |
                (ctrl.Name == "SlotMinRanText3" && itr.Name == "Slot3")) ctrl.Text = itr.ModRangeUnitMin.ToString();

            if ((ctrl.Name == "SlotMaxRanText1" && itr.Name == "Slot1") |
                (ctrl.Name == "SlotMaxRanText2" && itr.Name == "Slot2") |
                (ctrl.Name == "SlotMaxRanText3" && itr.Name == "Slot3")) ctrl.Text = itr.ModRangeUnitMax.ToString();

            if ((ctrl.Name == "SLminMeasText" && itr.Name == "StrainLoad") |
                (ctrl.Name == "PressMinMeasText" && itr.Name == "Pressure")) ctrl.Text = itr.SenRangeUnitMin.ToString();

            if ((ctrl.Name == "SLmaxMeasText" && itr.Name == "StrainLoad") |
                (ctrl.Name == "PressMaxMeasText" && itr.Name == "Pressure")) ctrl.Text = itr.SenRangeUnitMax.ToString();

            if (ctrl.Name == "SlvIDText" && itr.Name == "Eth") ctrl.Text = itr.slaveID.ToString();
            if (ctrl.Name == "SlvIPText" && itr.Name == "Eth") ctrl.Text = itr.slaveIP;
        }
        #endregion

        #region Combobox event
        private void ComboBoxSelected(object sender, EventArgs e)
        {
           ComboBox ctrl = sender as ComboBox;

           foreach (Config itr in currCfg)
           {
               if ((ctrl.SelectedItem.ToString() != itr.uartBaudrate) && (ctrl.Name == "BaudCombo1" && itr.Name == "COM3") |
                   (ctrl.Name == "BaudCombo2" && itr.Name == "COM4") | (ctrl.Name == "BaudCombo3" && itr.Name == "COM5"))
                   itr.uartBaudrate = ctrl.SelectedItem.ToString();

               if ((ctrl.SelectedItem.ToString() != itr.uartProtocol) && (ctrl.Name == "UProtocCombo1" && itr.Name == "COM3") |
                   (ctrl.Name == "UProtocCombo2" && itr.Name == "COM4") | (ctrl.Name == "UProtocCombo3" && itr.Name == "COM5")) 
                   itr.uartProtocol = ctrl.SelectedItem.ToString();

               if ((ctrl.SelectedItem.ToString() != itr.ethProtocol) && (ctrl.Name == "EProtocCombo" && itr.Name == "Eth"))
               {
                   itr.ethProtocol = ctrl.SelectedItem.ToString();

                   //enable/disable textbox controls
                   if (ctrl.SelectedItem.ToString() == "ModbusTCP")
                   {
                       TcpMasterCheck.Enabled = true;
                       if (TcpMasterCheck.Checked == true && (SlvIDText.ReadOnly == true | SlvIPText.ReadOnly == true))
                       {
                           SlvIDText.ReadOnly = false;
                           SlvIPText.ReadOnly = false;
                       }
                   }
                   else if (ctrl.SelectedItem.ToString() == "Generic")
                   {
                       TcpMasterCheck.Enabled = false;
                       if (SlvIDText.ReadOnly == false | SlvIPText.ReadOnly == false)
                       {
                           SlvIDText.ReadOnly = true;
                           SlvIPText.ReadOnly = true;
                       }
                   }
               }

               if ((ctrl.SelectedItem.ToString() != itr.Type) && (ctrl.Name == "SlotTypeCombo1" && itr.Name == "Slot1") |
                   (ctrl.Name == "SlotTypeCombo2" && itr.Name == "Slot2") | (ctrl.Name == "SlotTypeCombo3" && itr.Name == "Slot3"))
                   itr.Type = ctrl.SelectedItem.ToString();
           }
        }
        #endregion

        #region Unit combobox event
        private void UnitComboEvents(object sender, EventArgs e)
        {
            ComboBox ctrl = sender as ComboBox;
           
            foreach (Config itr in currCfg)
            {
                if (ctrl.SelectedItem.ToString() != itr.SensorUnit && (ctrl.Name == "SLUnitCombo" && itr.Name == "StrainLoad") | 
                    (ctrl.Name == "PressUnitCombo" && itr.Name == "Pressure"))
                {
                    itr.SensorUnit = ctrl.SelectedItem.ToString();

                    foreach (KeyValuePair<string, string[]> keyVal in Units)
                    {
                        //convert unit
                        if (keyVal.Key == itr.SensorUnit)
                        {
                            float val = float.Parse(keyVal.Value[0]);
                            float min = itr.SenRangeUnitMin * val;
                            float max = itr.SenRangeUnitMax * val;
                            itr.SenRangeUnitMin = (float)Math.Round(min, 1);
                            itr.SenRangeUnitMax = (float)Math.Round(max, 1);
                            
                            if (ctrl.Name == "SLUnitCombo")
                            {
                                SLminMeasText.Text = itr.SenRangeUnitMin.ToString();
                                SLmaxMeasText.Text = itr.SenRangeUnitMax.ToString();
                            }

                            else if (ctrl.Name == "PressUnitCombo")
                            {
                                PressMinMeasText.Text = itr.SenRangeUnitMin.ToString();
                                PressMaxMeasText.Text = itr.SenRangeUnitMax.ToString();
                            }
                        }
                    }
                }
                        
                if(ctrl.Name == "MovUnitCombo" && itr.Name == "Moving") itr.SensorUnit = ctrl.SelectedItem.ToString();
                if (ctrl.Name == "SpdUnitCombo" && itr.Name == "Speed") itr.SensorUnit = ctrl.SelectedItem.ToString();
            }
        }
        #endregion

        #region textbox event
        private void TextEvent(object sender, KeyEventArgs e)
        {
            TextBox ctrl = sender as TextBox;
            Regex OnlNum = new Regex(@"\d");
            Regex IpReg = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

            if (e.KeyCode == Keys.Return)
            {
                foreach (Config itr in currCfg)
                {
                    //check value range (-32768 to +23767,0 to 65535) and slaveID
                    if (OnlNum.IsMatch(ctrl.Text) && !IpReg.IsMatch(ctrl.Text))
                    {
                        int val = Int32.Parse(ctrl.Text);
                        if ((val >= ushort.MinValue | val <= ushort.MaxValue) | (val >= short.MinValue | val <= short.MaxValue))
                        {
                            if ((ctrl.Name == "SlotMinRanText1" && itr.Name == "Slot1") |
                                (ctrl.Name == "SlotMinRanText2" && itr.Name == "Slot2") |
                                (ctrl.Name == "SlotMinRanText3" && itr.Name == "Slot3")) itr.ModRangeUnitMin = val;

                            if ((ctrl.Name == "SlotMaxRanText1" && itr.Name == "Slot1") |
                                (ctrl.Name == "SlotMaxRanText2" && itr.Name == "Slot2") |
                                (ctrl.Name == "SlotMaxRanText3" && itr.Name == "Slot3")) itr.ModRangeUnitMax = val;
                                
                            if ((ctrl.Name == "SLminMeasText" && itr.Name == "StrainLoad") |
                                (ctrl.Name == "PressMinMeasText" && itr.Name == "Pressure")) itr.SenRangeUnitMin = val;

                            if ((ctrl.Name == "SLmaxMeasText" && itr.Name == "StrainLoad") |
                                (ctrl.Name == "PressMaxMeasText" && itr.Name == "Pressure")) itr.SenRangeUnitMax = val;

                            if (ctrl.Name == "SlvIDText" && itr.Name == "Eth") itr.slaveID = int.Parse(ctrl.Text);
                        }
                    }

                    //check ip address
                    else if (IpReg.IsMatch(ctrl.Text) && (ctrl.Name == "SlvIPText" && itr.Name == "Eth")) itr.slaveIP = ctrl.Text;

                    //restore previous text
                    else RestorePrevText(ctrl, itr);
                }
            }
        }
        #endregion

        #region textbox lost focus
        private void TextLostFocus(object sender, EventArgs e)
        {
            TextBox ctrl = sender as TextBox;

            foreach (Config itr in currCfg) { RestorePrevText(ctrl, itr); }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            XMLFileWriter fw = new XMLFileWriter();
            fw.WriteFile<Hardware>("System_Disk2\\StandGA\\System\\hardware.xml", currCfg);

            //return currCfg to main form
            MainForm form = this.Owner as MainForm;
            form.MainForm_Init(currCfg);
            this.Close();
        }
        #endregion

        #region lir-916 button click
        private void LirSetting_Click(object sender, EventArgs e)
        {

            foreach (Config itr in currCfg)
            {
                if (itr.Name == "COM3" && itr.uartProtocol != "Generic")
                {
                    LirProgForm form = new LirProgForm(itr.uartProtocol, itr.uartBaudrate);
                    form.Show();
                    break;
                }
                else Error.instance.HandleWarningMessage("Select lir protocol for COM3");
            }
        }
        #endregion

        #region cancel button click
        private void CancelBtn_Click(object sender, EventArgs e) { this.Close(); }
        #endregion

        #region modbus master checkbox event
        private void TcpMasterCheck_CheckStateChanged(object sender, EventArgs e)
        {
            foreach (Config itr in currCfg)
            {
                if (itr.Name == "Eth")
                {
                    if (TcpMasterCheck.Checked == true)
                    {
                        itr.asMaster = true;
                        SlvIDText.ReadOnly = false;
                        SlvIPText.ReadOnly = false;
                    }
                    else
                    {
                        itr.asMaster = false;
                        SlvIDText.ReadOnly = true;
                        SlvIPText.ReadOnly = true;
                    }
                }
                break;
            }
        }
        #endregion
    }
}