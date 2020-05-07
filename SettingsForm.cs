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

namespace stend
{
    public partial class SettingsForm : Form
    {
        Hardware currCfg = new Hardware();
       
        
        public SettingsForm(Hardware config)
        {
            InitializeComponent();
            //currCfg = DeepClone<Hardware>(config);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            {
                ConfigFileParser parser = new ConfigFileParser();
                FileReader reader = new GenericFileReader();
                Dictionary<string, string[]> ParseItems = new Dictionary<string, string[]>();
                Dictionary <string, Items[]> items = new Dictionary<string,Items[]>();
                parser.Parse(reader.ReadFile("System_Disk2\\StandGA\\System\\settings.ini", 20));
                ParseItems = parser.GetSetting("HWSettings");

                foreach (KeyValuePair<string, string[]> keyVal in ParseItems)
                {
                   
                    Items[] tmp = new Items[keyVal.Value.Length];
                    for (int i = 0; i < keyVal.Value.Length; i++) tmp[i] = new Items { ItemName = keyVal.Value[i], unitOrId = i };
                    items.Add(keyVal.Key, tmp);
                }
            
                //Create comboboxes
                foreach (KeyValuePair<string, Items[]> keyVal in items)
                {
                    switch (keyVal.Key)
                    {
                        case "ModuleType":
                            ControlFactory.CreateControl<ComboBoxElement>(SlotTypeCB1, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(SlotTypeCB2, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(SlotTypeCB3, (Items[])keyVal.Value.Clone());
                            break;
                        case "External":
                            ControlFactory.CreateControl<ComboBoxElement>(ExtComCB1, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(ExtComCB2, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(ExtComCB3, (Items[])keyVal.Value.Clone());
                            break;
                        case "BackPlane":
                            ControlFactory.CreateControl<ComboBoxElement>(BackplCB, (Items[])keyVal.Value.Clone());
                            break;
                        case "Baudrate":
                            ControlFactory.CreateControl<ComboBoxElement>(BaudCB1, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(BaudCB2, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(BaudCB3, (Items[])keyVal.Value.Clone());
                            break;
                        case "UartProtocol":
                            ControlFactory.CreateControl<ComboBoxElement>(UProtocCB1, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(UProtocCB2, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(UProtocCB3, (Items[])keyVal.Value.Clone());
                            break;
                        case "EthProtocol":
                            ControlFactory.CreateControl<ComboBoxElement>(EProtocCB1, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(EProtocCB2, (Items[])keyVal.Value.Clone());
                            break;
                        case "LoadCellUnit":
                            ControlFactory.CreateControl<ComboBoxElement>(LCUnitCB, keyVal.Value);
                            break;
                        case "PressureUnit":
                            ControlFactory.CreateControl<ComboBoxElement>(ComprUnitCB, (Items[])keyVal.Value.Clone());
                            ControlFactory.CreateControl<ComboBoxElement>(StrUnitCB, (Items[])keyVal.Value.Clone());
                            break;
                        case "MovingUnit":
                            ControlFactory.CreateControl<ComboBoxElement>(MovUnitCB, keyVal.Value);
                            break;
                        case "SpeedUnit":
                            ControlFactory.CreateControl<ComboBoxElement>(SpdUnitCB, keyVal.Value);
                            break;
                        default: break;
                    }
                }
            }

            /*Working with config
             * ConfigManager cfgMan = new ConfigManager();
               ConfigModifier mod = new ConfigModifier();
               cfgMan.SetModifier(new ConfigOnCommand(mod));
               cfgMan.Change();
               cfgMan.UndoChanges();
            */
        }

        private void ComboBoxSelected(object sender, EventArgs e)
        {
           ComboBox ctrl = sender as ComboBox;
           Items currItem = (Items)ctrl.SelectedItem;

           /*
           switch (ctrl.Name)
           {
               case "SlotTypeCB1":
                   if(currItem.ItemName == currCfg.page[0].config[0].Name)
                       //add name to ConfigManager dictionary
                   break;

               default: break;
           }*/

          
        }

        private void TextChanges(object sender, EventArgs e)
        {
            /*
             TextBox txt = sender as TextBox;

            if (e.KeyCode == Keys.Return)
            {
                foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
                {
                    if (keyValue.Key.Name == txt.Name && keyValue.Value != txt.Text)
                    {
                        if (keyValue.Key.Tag.ToString() == "SNumCh")
                        {
                            if (OnlNum.IsMatch(txt.Text))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                MessageBox.Show("to apply the changes, save the file and reopen the window");
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '[1-99]'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }

                        else if (keyValue.Key.Tag.ToString() == "MBusIp")
                        {
                            if (IpReg.IsMatch(txt.Text))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '[0-999].[0-999].[0-999].[0-999]'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }

                        else if (keyValue.Key.Tag.ToString() == "minMeas" || keyValue.Key.Tag.ToString() == "maxMeas")
                        {
                            if (NumReg.IsMatch(txt.Text) && !(StrReg.IsMatch(txt.Text)))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '+/-9999'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }
                    }
                }
                if (textIsChanged) updateConfig(txt);
                */
        }

        private void TextLostFocus(object sender, EventArgs e)
        {
            /*TextBox txt = new TextBox();
            txt = sender as TextBox;

            foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
            {
                if (txt.Name == keyValue.Key.Name && (txt.Text == "" || txt.Text != keyValue.Value))
                    txt.Text = keyValue.Value;
            }*/
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            XMLFileWriter fw = new XMLFileWriter();
            fw.WriteFile<Hardware>("System_Disk2\\StandGA\\TestConfigs\\test.xml", currCfg);
        }

       
        private void LirSetting_Click(object sender, EventArgs e)
        {
           // LinInterfaceSetup progForm = new LinInterfaceSetup(currCfg.page[0].config[0].extInterface, currCfg.page[0].config[0].extProtocol,
              //  currCfg.page[0].config[0].extBaudrate);
            //progForm.Show();
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e) { this.Close(); }
    }

    //combobox items
    class Items : ICloneable
    {
        public string ItemName { get; set; }
        public int unitOrId { get; set; }
        public object Clone() { return this.MemberwiseClone(); }
    }

    //Controls factory methods
    abstract class ControlConstruct
    {
        protected internal abstract void LoadElement(Control control, Items[] items);
        protected internal abstract void LoadElement(Control control);
    }

    class ComboBoxElement : ControlConstruct
    {
        public ComboBoxElement() { }

        private ComboBox combo;

        protected internal override void LoadElement(Control control, Items[] items)
        {
            combo = control as ComboBox;
            //filled combo with items
            combo.DataSource = items;
            combo.DisplayMember = "ItemName";
            combo.ValueMember = "unitOrId";
        }
        protected internal override void LoadElement(Control control) {/*throw error*/}
    }
    
    class TextBoxElement: ControlConstruct
    {
	    public TextBoxElement(){}
        private TextBox text;

        protected internal override void LoadElement(Control control,Items[] items){/*throw error*/}
        protected internal override void LoadElement(Control control) { }
    }
 
    static class ControlFactory
    {
        public static void CreateControl<T>(Control control) where T : ControlConstruct, new()
        {
            try
            {
                var t = new T();
                t.LoadElement(control);
            }
            catch (TargetInvocationException e)
            {
                //Error handling
            }
        }
        public static void CreateControl<T>(Control control, Items[] items) where T : ControlConstruct, new()
	    {
		    try
		    {
			    var t = new T();
                t.LoadElement(control,items);
		    }
		    catch(TargetInvocationException e)
		    {
			    //Error handling
		    }
	    }
    }
/* 
    class TabElement
    {
        private TabPage Tab;
        private Dictionary<string, Items[]> comboItems;
        private Dictionary<int, int> numChannels;
        private Dictionary<ComboBox, string> comboConformity;
        private Dictionary<TextBox, string> textConformity;

        private Config[] Conf;
        private Regex NumReg = new Regex(@"^([+-]?(\d+))");
        private Regex StrReg = new Regex(@"[a-z]|[A-Z]");
        private Regex OnlNum = new Regex(@"^([1-9]?\d)$");
        private Regex IpReg = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

        public bool comboIsChanged { get; set; }
        public bool textIsChanged { get; set; }

        public TabElement(ref TabPage tab, Config[] cfg, Dictionary<string, Items[]> items)
        {
            Tab = tab;
            comboItems = items;
            numChannels = null;
            comboConformity = new Dictionary<ComboBox, string>();
            textConformity = new Dictionary<TextBox, string>();
            Conf = null;
            comboIsChanged = false;
            textIsChanged = false;

            LoadConfig(cfg);
        }

        public TabElement(ref TabPage tab, Config[] cfg, Dictionary<string, Items[]> items,
            Dictionary<int, int> numCh)
        {
            Tab = tab;
            comboItems = items;
            numChannels = numCh;
            comboConformity = new Dictionary<ComboBox, string>();
            textConformity = new Dictionary<TextBox, string>();
            Conf = null;
            comboIsChanged = false;
            textIsChanged = false;

            LoadConfig(cfg);
        }

        public void LoadConfig(Config[] cfg)
        {
            comboConformity.Clear();
            textConformity.Clear();
            Conf = cfg;
            LoadGui();
        }

        private void LoadGui()
        {
            //tab iteration
            foreach (Control panel in Tab.Controls)
            {
                if (panel is Panel)
                {
                    Panel currPanel = panel as Panel;
                    Config currCfg = null;

                    for (int i = 0; i < Conf.Length; i++)
                        if (panel.Tag.ToString() == Conf[i].Tag) currCfg = Conf[i];

                    //panel iteration    
                    foreach (Control control in panel.Controls)
                    {
                        if (control is ComboBox)
                        {
                            ComboBox combo = control as ComboBox;

                            //filled combo with numbers
                            switch (combo.Tag.ToString())
                            {
                                case "Slot":
                                    for (int i = 1; i <= 3; i++) combo.Items.Add(i);
                                    break;
                                case "Channel":
                                    foreach (KeyValuePair<int, int> keyValue in numChannels)
                                        if (currCfg.Slot == keyValue.Key)
                                            for (int i = 0; i < keyValue.Value; i++) combo.Items.Add(i);
                                    break;
                                default:
                                    break;
                            }

                            //filled combo with strings
                            foreach (KeyValuePair<string, string[]> keyValue in comboItems)
                            {
                                if (combo.Tag.ToString() == keyValue.Key)
                                    for (int i = 0; i < keyValue.Value.Length; i++) combo.Items.Add(keyValue.Value[i]);
                            }

                            //default combobox seletion
                            switch (combo.Tag.ToString())
                            {
                                case "Backplane":
                                    comboConformity.Add(combo, currCfg.Backplane);
                                    break;
                                case "ExtCom":
                                    comboConformity.Add(combo, currCfg.extInterface);
                                    break;
                                case "ExtBaud":
                                    comboConformity.Add(combo, currCfg.extBaudrate);
                                    break;
                                case "ExtProtocol":
                                    comboConformity.Add(combo, currCfg.extProtocol);
                                    break;
                                case "SlotType":
                                    comboConformity.Add(combo, currCfg.Type);
                                    break;
                                case "SlotRange":
                                    comboConformity.Add(combo, currCfg.Range);
                                    break;
                                case "Slot":
                                    comboConformity.Add(combo, currCfg.Slot.ToString());
                                    break;
                                case "Channel":
                                    comboConformity.Add(combo, currCfg.Channel.ToString());
                                    break;
                                case "UnitF":
                                    comboConformity.Add(combo, currCfg.Unit);
                                    break;
                                case "UnitP":
                                    comboConformity.Add(combo, currCfg.Unit);
                                    break;
                                case "UnitL":
                                    comboConformity.Add(combo, currCfg.Unit);
                                    break;
                                case "UnitS":
                                    comboConformity.Add(combo, currCfg.Unit);
                                    break;
                                default:
                                    break;
                            }
                        }

                        //default text in textbox
                        else if (control is TextBox)
                        {
                            TextBox text = control as TextBox;

                            switch (text.Tag.ToString())
                            {
                                case "MBusIp":
                                    textConformity.Add(text, currCfg.modbusIp);
                                    break;
                                case "Slot":
                                    textConformity.Add(text, currCfg.Name);
                                    break;
                                case "SNumCh":
                                    textConformity.Add(text, currCfg.numChannels.ToString());
                                    break;
                                case "minMeas":
                                    textConformity.Add(text, currCfg.MeasureValMin);
                                    break;
                                case "maxMeas":
                                    textConformity.Add(text, currCfg.MeasureValMax);
                                    break;
                            }
                        }
                    }
                }
            }
            //select combo items and fill textBoxes
            ComboCfgSel();
            TextCfgFill();
        }

        private void ComboCfgSel()
        {
            foreach (KeyValuePair<ComboBox, string> keyValue in comboConformity)
            {
                if (keyValue.Key.Tag.ToString() == "Slot" || keyValue.Key.Tag.ToString() == "Channel")
                {
                    if (keyValue.Key.Items.Count >= int.Parse(keyValue.Value))
                        keyValue.Key.SelectedItem = int.Parse(keyValue.Value);
                    else MessageBox.Show("Config is damaged, element:" + keyValue.Key.Tag.ToString());
                }
                else
                {
                    if (keyValue.Key.Items.Count >= keyValue.Key.Items.IndexOf(keyValue.Value))
                        keyValue.Key.SelectedIndex = keyValue.Key.Items.IndexOf(keyValue.Value);
                    else MessageBox.Show("Config is damaged, element:" + keyValue.Key.Tag.ToString());
                }
                keyValue.Key.SelectedIndexChanged += ComboBoxIsChanged;
            }
        }

        private void TextCfgFill()
        {
            foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
            {
                if (keyValue.Key.Tag.ToString() == "Slot") keyValue.Key.Text = keyValue.Value;

                else if (keyValue.Key.Tag.ToString() == "SNumCh")
                {
                    if (OnlNum.IsMatch(keyValue.Value)) keyValue.Key.Text = keyValue.Value;
                    else MessageBox.Show("Config is damaged, element:" + keyValue.Key.Tag.ToString());
                }

                else if (keyValue.Key.Tag.ToString() == "MBusIp")
                {
                    if (IpReg.IsMatch(keyValue.Value)) keyValue.Key.Text = keyValue.Value;
                    else MessageBox.Show("Config is damaged, element:" + keyValue.Key.Tag.ToString());
                }

                else
                {
                    if (NumReg.IsMatch(keyValue.Value) && !(StrReg.IsMatch(keyValue.Value))) keyValue.Key.Text = keyValue.Value;
                    else MessageBox.Show("Config is damaged, element:" + keyValue.Key.Tag.ToString());
                }

                keyValue.Key.Text = keyValue.Value;
                keyValue.Key.KeyUp += TextChanged;
                keyValue.Key.LostFocus += TextLostFocus;
            }
        }

        //Event handlers
        public void ComboBoxIsChanged(object sender, EventArgs e)
        {
            ComboBox ctrl = sender as ComboBox;

            foreach (KeyValuePair<ComboBox, string> keyValue in comboConformity)
            {
                if (keyValue.Key.Name == ctrl.Name && keyValue.Value == "Slot" ||
                    keyValue.Value == "Channel")
                {
                    if (int.Parse(keyValue.Value) != ctrl.SelectedIndex)
                    {
                        comboConformity[keyValue.Key] = ctrl.SelectedIndex.ToString();
                        comboIsChanged = true;
                        break;
                    }
                }

                else if (keyValue.Key.Name == ctrl.Name &&
                    keyValue.Value != ctrl.SelectedItem.ToString())
                {
                    comboConformity[keyValue.Key] = ctrl.SelectedItem.ToString();
                    comboIsChanged = true;
                    if (keyValue.Key.Tag.ToString() == "ExtProtocol" || keyValue.Key.Tag.ToString() == "ExtBaud")
                        MessageBox.Show("To apply changes, re-program the interface");
                    break;
                }
            }
            if (comboIsChanged) updateConfig(ctrl);
        }

        public void TextLostFocus(object sender, EventArgs e)
        {
            TextBox txt = new TextBox();
            txt = sender as TextBox;

            foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
            {
                if (txt.Name == keyValue.Key.Name && (txt.Text == "" || txt.Text != keyValue.Value))
                    txt.Text = keyValue.Value;
            }
        }

        public void TextChanged(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (e.KeyCode == Keys.Return)
            {
                foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
                {
                    if (keyValue.Key.Name == txt.Name && keyValue.Value != txt.Text)
                    {
                        if (keyValue.Key.Tag.ToString() == "SNumCh")
                        {
                            if (OnlNum.IsMatch(txt.Text))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                MessageBox.Show("to apply the changes, save the file and reopen the window");
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '[1-99]'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }

                        else if (keyValue.Key.Tag.ToString() == "MBusIp")
                        {
                            if (IpReg.IsMatch(txt.Text))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '[0-999].[0-999].[0-999].[0-999]'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }

                        else if (keyValue.Key.Tag.ToString() == "minMeas" || keyValue.Key.Tag.ToString() == "maxMeas")
                        {
                            if (NumReg.IsMatch(txt.Text) && !(StrReg.IsMatch(txt.Text)))
                            {
                                textConformity[keyValue.Key] = txt.Text;
                                textIsChanged = true;
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Current input does not match format '+/-9999'");
                                txt.Text = textConformity[keyValue.Key];
                            }
                        }
                    }
                }
                if (textIsChanged) updateConfig(txt);
            }
        }

        //Update functions
        private void updateConfig(Control ctrl)
        {
            Config currcfg = null;
            int origConfIndex = 0;

            for (int i = 0; i < Conf.Length; i++)
            {
                if (ctrl.Parent.Tag.ToString() == Conf[i].Tag)
                {
                    currcfg = Conf[i];
                    origConfIndex = i;
                }
            }

            if (ctrl is ComboBox)
            {
                foreach (KeyValuePair<ComboBox, string> keyValue in comboConformity)
                {
                    if (ctrl.Name == keyValue.Key.Name)
                    {
                        switch (keyValue.Key.Tag.ToString())
                        {
                            case "Backplane":
                                if (keyValue.Value != currcfg.Backplane)
                                    Conf[origConfIndex].Backplane = keyValue.Value;
                                break;
                            case "ExtCom":
                                if (keyValue.Value != currcfg.extInterface)
                                    Conf[origConfIndex].extInterface = keyValue.Value;
                                break;
                            case "ExtBaud":
                                if (keyValue.Value != currcfg.extBaudrate)
                                    Conf[origConfIndex].extBaudrate = keyValue.Value;
                                break;
                            case "ExtProtocol":
                                if (keyValue.Value != currcfg.extProtocol)
                                    Conf[origConfIndex].extProtocol = keyValue.Value;
                                break;
                            case "SlotType":
                                if (keyValue.Value != currcfg.Type)
                                    Conf[origConfIndex].Type = keyValue.Value;
                                break;
                            case "SlotRange":
                                if (keyValue.Value != currcfg.Range)
                                    Conf[origConfIndex].Range = keyValue.Value;
                                break;
                            case "Slot":
                                if (int.Parse(keyValue.Value) != currcfg.Slot)
                                    Conf[origConfIndex].Slot = int.Parse(keyValue.Value);
                                break;
                            case "Channel":
                                if (int.Parse(keyValue.Value) != currcfg.Channel)
                                    Conf[origConfIndex].Channel = int.Parse(keyValue.Value);
                                break;
                            case "UnitF":
                                if (keyValue.Value != currcfg.Unit)
                                    Conf[origConfIndex].Unit = keyValue.Value;
                                break;

                            case "UnitP":
                                if (keyValue.Value != currcfg.Unit)
                                    Conf[origConfIndex].Unit = keyValue.Value;
                                break;

                            case "UnitL":
                                if (keyValue.Value != currcfg.Unit)
                                    Conf[origConfIndex].Unit = keyValue.Value;
                                break;

                            case "UnitS":
                                if (keyValue.Value != currcfg.Unit)
                                    Conf[origConfIndex].Unit = keyValue.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (ctrl is TextBox)
            {
                foreach (KeyValuePair<TextBox, string> keyValue in textConformity)
                {
                    if (ctrl.Name == keyValue.Key.Name)
                    {
                        switch (keyValue.Key.Tag.ToString())
                        {
                            case "MBusIp":
                                if (keyValue.Value != currcfg.modbusIp)
                                    Conf[origConfIndex].modbusIp = keyValue.Value;
                                break;
                            case "SNumCh":
                                if (keyValue.Value != currcfg.numChannels.ToString())
                                    Conf[origConfIndex].numChannels = int.Parse(keyValue.Value);
                                break;

                            case "minMeas":
                                if (keyValue.Value != currcfg.MeasureValMin)
                                    Conf[origConfIndex].MeasureValMin = keyValue.Value;
                                break;

                            case "maxMeas":
                                if (keyValue.Value != currcfg.MeasureValMax)
                                    Conf[origConfIndex].MeasureValMax = keyValue.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }//end class*/
}