using System;
using System.Linq;
using System.IO;
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
        public virtual string Type { get; set; }
        public virtual string SensorUnit { get; set; }
        public virtual float SenRangeUnitMin { get; set; }
        public virtual float SenRangeUnitMax { get; set; }
        public virtual int ModRangeUnitMin { get; set; }
        public virtual int ModRangeUnitMax { get; set; }
        public virtual int slaveID { get; set; }
        public virtual bool asMaster { get; set; }
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
        public override bool asMaster { get; set; }
        
        public tConfig() { }
        
        public tConfig(string ethName, string protocol, int ID, string slaveAddr, bool isMaster)
            :base(ethName)
        {
            slaveID = ID;
            ethProtocol = protocol;
            slaveIP = slaveAddr;
            asMaster = isMaster;
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
        public override float SenRangeUnitMin { get; set; }
        public override float SenRangeUnitMax { get; set; }

        public sConfig() { }

        public sConfig(string sensorName, string unit, float measureMin, float measureMax)
            :base(sensorName)
        {
            SensorUnit = unit;
            SenRangeUnitMin = measureMin;
            SenRangeUnitMax = measureMax;
        }
    }

    static class ConfigClone
    {
        public static Hardware Clone(Hardware obj)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(typeof(Hardware));
                xs.Serialize(ms, obj);
                ms.Position = 0;
                return (Hardware)xs.Deserialize(ms);
            }
        }
    }
}