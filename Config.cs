using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;

namespace stend
{
    [Serializable()]
    [XmlRoot("Hardware")]
    public class Hardware:IDisposable
    {
        [XmlArrayItem("page", typeof(tabPage))]
        public tabPage[] page { get; set; }

        [NonSerialized]
        private bool disposed = false;

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
        }
        ~Hardware()
        {
            Dispose(false);
        }
    }

    [Serializable()]
    public class tabPage
    {
        [XmlAttribute]
        public string tabTag { get; set; }

        [XmlArrayItem("config",typeof(Config))]
        public Config[] config;

        public tabPage() { }

        public tabPage(string tag, int numCfgs)
        {
            tabTag = tag;
            config = new Config[numCfgs];
        }
        ~tabPage()
        {
            GC.Collect();
        }
    }

    [Serializable]
    [XmlInclude(typeof(cConfig))]
    [XmlInclude(typeof(mConfig))]
    [XmlInclude(typeof(iConfig))]
    [XmlInclude(typeof(oConfig))]
    public abstract class Config
    {
        [XmlAttribute]
        public string Tag { get; set; }

        public Config() { }

        public Config(string tag)
        {
            Tag = tag;
        }

        public virtual string Backplane { get; set; }
        public virtual string extInterface { get; set; }
        public virtual string extBaudrate { get; set; }
        public virtual string extProtocol { get; set; }
        public virtual string modbusIp { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string Range { get; set; }
        public virtual string Unit { get; set; }
        public virtual string MeasureValMin { get; set; }
        public virtual string MeasureValMax { get; set; }
        public virtual int numChannels { get; set; }
        public virtual int Channel { get; set; }
        public virtual int Slot { get; set; }
        public virtual bool Enable { get; set; }
    }

    //controller config
    public class cConfig : Config
    {
        public override string Backplane { get; set; }
        public override string extInterface { get; set; }
        public override string extBaudrate { get; set; }
        public override string extProtocol { get; set; }
        public override string modbusIp { get; set; }
        
        public cConfig() { }

        public cConfig(string tag, string backplane, string extCom, string extBaud,string extProt,string modIp)
            : base(tag)
        {
            Backplane = backplane.ToUpper();
            extInterface = extCom.ToUpper();
            extBaudrate = extBaud;
            extProtocol = extProt;
            modbusIp = modIp;
        }
    }

    //onboard module config
    public class mConfig: Config
    {
        public override string Name { get; set; }
        public override string Type { get; set; }
        public override string Range { get; set; }
        public override int numChannels { get; set; }
        public override int Slot { get; set; }

        public mConfig() { }

        public mConfig(string tag, string moduleName, string type, string range, int slot, int maxChannels )
            :base(tag)
        {
            Name = moduleName;
            Type = type;
            Range = range;
            numChannels = maxChannels;
            Slot = slot;
        }

        public mConfig(string tag, string moduleName, string type, int slot, int maxChannels )
            : base(tag)
        {
            Name = moduleName;
            Type = type;
            Range = "null";
            numChannels = maxChannels;
            Slot = slot;
        }
    }

    //inputs config
    public class iConfig: Config
    {
        public override string extInterface { get; set; }
        public override string Unit { get; set; }
        public override string MeasureValMin { get; set; }
        public override string MeasureValMax { get; set; }
        public override int Channel { get; set; }
        public override int Slot { get; set; }

        public iConfig() { }

        public iConfig(string tag, string unit, string measureMin, string measureMax, int slot, int channel)
            :base(tag)
        {
            extInterface = "null";
            Unit = unit;
            MeasureValMin = measureMin;
            MeasureValMax = measureMax;
            Channel = channel;
            Slot = slot;
        }
        public iConfig(string tag, string external, string unit): base(tag)
        {
            Slot = -1;
            extInterface = external;
            Unit = unit;
            MeasureValMin = "null";
            MeasureValMax = "null";
            Channel = -1;
        }
    }

    //Outputs config
    public class oConfig: Config
    {
        public override int Slot{ get; set; }
        public override int Channel { get; set; }
        public override bool Enable { get; set; }

        public oConfig() { }

        public oConfig(string tag, int slot, int channel,bool activate):base(tag)
        {
            Slot = slot;
            Channel = channel;
            Enable = activate;
        }
    }

    //command
    interface ICommand
    {
        void Execute();
	    void Undo();
    } 

    //Receiver
    class ConfigModifier
    {
        public void ChangeConfig()
	    {
            /*
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
         * }
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
	
	    public void UndoAll()
	    {
	    	//Reset to default
	    }
    }

    //concrete command
    class ConfigOnCommand : ICommand
    {
        ConfigModifier cfgMod;
	
	    public ConfigOnCommand(ConfigModifier modSet){ cfgMod = modSet; }
	
	    public void Execute(){cfgMod.ChangeConfig();}
	    public void Undo(){cfgMod.UndoAll();}
    }

    //Invoker
    class ConfigManager
    {
        ICommand command;
	
	    public ConfigManager(){}
	
	    public void SetModifier(ICommand com){ command = com; }
	
	    public void Change(){ command.Execute(); }
	    public void UndoChanges(){command.Undo();}
    }
}
