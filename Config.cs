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
    public class Hardware
    {
        [XmlArrayItem("config", typeof(Config))]
        public Config[] config;

        public Hardware() { }

        public Hardware(int numCfgs) { config = new Config[numCfgs]; }

        public IEnumerator<Config> GetEnumerator()
        {
            for (int i = 0; i < config.Length; i++)
            {
                yield return config[i];
            }
        }
    }
    
    
    [Serializable]
    [XmlInclude(typeof(uConfig))]
    [XmlInclude(typeof(tConfig))]
    [XmlInclude(typeof(mConfig))]
    [XmlInclude(typeof(sConfig))]
    public abstract class Config
    {
        [XmlAttribute]
        public string Name { get; set; }

        public Config() { }

        public Config(string name){ Name = name; }

        public virtual string uartBaudrate { get; set; }
        public virtual string uartProtocol { get; set; }        
        public virtual string ethProtocol { get; set; }
        public virtual string slaveIP { get; set; }
        public virtual string masterIP { get; set; }
        public virtual string Type { get; set; }
        public virtual string SensorUnit { get; set; }
        public virtual int SenRangeUnitMin { get; set; }
        public virtual int SenRangeUnitMax { get; set; }
        public virtual int ModRangeUnitMin { get; set; }
        public virtual int ModRangeUnitMax { get; set; }
        public virtual int slaveID { get; set; }
    }

    //uart com config
    public class uConfig : Config
    {
        public override string uartBaudrate { get; set; }
        public override string uartProtocol { get; set; }
        
        public uConfig() { }

        public uConfig(string comName, string baudrate,string protocol)
            : base(comName)
        {
            uartBaudrate = baudrate;
            uartProtocol = protocol;   
        }
    }

    //tcp config
    public class tConfig : Config
    {
        public override int slaveID { get; set; }
        public override string ethProtocol { get; set; }
        public override string slaveIP { get; set; }
        public override string masterIP { get; set; }

        public tConfig() { }
        
        public tConfig(string ethName, string protocol, int ID, string slaveAddr, string masterAddr )
            :base(ethName)
        {
            slaveID = ID;
            ethProtocol = protocol;
            slaveIP = slaveAddr;
            masterIP = masterAddr;
        }
    }

    //onboard module config
    public class mConfig: Config
    {
        public override int ModRangeUnitMin { get; set; }
        public override int ModRangeUnitMax { get; set; }
        public override string Type { get; set; }
        
        public mConfig() { }

        public mConfig(string Slot, string type, int rangeUnitMin, int rangeUnitMax)
            :base(Slot)
        {
            Type = type;
            ModRangeUnitMin = rangeUnitMin;
            ModRangeUnitMax = rangeUnitMax;
        }
    }

    //sensor config
    public class sConfig: Config
    {
        public override string SensorUnit { get; set; }
        public override int SenRangeUnitMin { get; set; }
        public override int SenRangeUnitMax { get; set; }

        public sConfig() { }

        public sConfig(string sensorName, string unit, int measureMin, int measureMax)
            :base(sensorName)
        {
            SensorUnit = unit;
            SenRangeUnitMin = measureMin;
            SenRangeUnitMax = measureMax;
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
	
	    public void UndoCurr()
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
	    public void Undo(){cfgMod.UndoCurr();}
    }

    //Invoker
    class ConfigInvoker
    {
        ICommand command;

        public ConfigInvoker() { }
	
	    public void SetModifier(ICommand com){ command = com; }
	
	    public void Change(){ command.Execute(); }
	    public void UndoChanges(){command.Undo();}
    }
}
