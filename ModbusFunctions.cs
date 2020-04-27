using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;

namespace stend
{
    abstract class ModbusWrite
    {
	    public abstract void WriteSingleCoil(byte SlaveAddr, ushort regAddr, bool value);
        public abstract void WriteSingleRegister(byte SlaveAddr, ushort regAddr, ushort value);
    }

    abstract class ModbusRead
    {
	    public abstract ushort[] ReadHoldingRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr);
        public abstract ushort[] ReadInputRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr);
    }

    class WriteTo : ModbusWrite
    {
        private ModbusMaster _master = null;

        public WriteTo(object obj)
	    {
		    if (obj is ModbusIpMaster) _master = obj as ModbusIpMaster;
            else if (obj is ModbusSerialMaster) _master = obj as ModbusSerialMaster;;
	    }   
    
        public override void WriteSingleCoil(byte SlaveAddr, ushort regAddr, bool value) { _master.WriteSingleCoil(SlaveAddr, regAddr, value); }
        public override void WriteSingleRegister(byte SlaveAddr, ushort regAddr, ushort value) { _master.WriteSingleRegister(SlaveAddr, regAddr, value); }
    }

    class ReadFrom : ModbusRead
    {
	    private ModbusMaster _master = null;

        public ReadFrom(object obj)
        {
            if (obj is ModbusIpMaster) _master = obj as ModbusIpMaster;
            else if (obj is ModbusSerialMaster) _master = obj as ModbusSerialMaster;
        }

        public override ushort[] ReadHoldingRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr) { return _master.ReadHoldingRegisters(SlaveAddr, StartAddr, numAddr); }
       
        public override ushort[] ReadInputRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr) { return _master.ReadInputRegisters(SlaveAddr, StartAddr, numAddr); }
    }

    abstract class ModbusFuncFactory
    {
        public abstract ModbusWrite CreateWriteFunc(object obj);
        public abstract ModbusRead CreateReadFunc(object obj);
    }

    class WriteFuncFactory : ModbusFuncFactory
    {
	    public override ModbusWrite CreateWriteFunc(object obj){return new WriteTo(obj);}
        public override ModbusRead CreateReadFunc(object obj) { return null; }
    }

    class ReadFuncFactory : ModbusFuncFactory
    {
	    public override ModbusWrite CreateWriteFunc(object obj){return null;}
        public override ModbusRead CreateReadFunc(object obj) { return new ReadFrom(obj); }
    }
}