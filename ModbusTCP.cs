using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Modbus;
using Modbus.Device;
using Modbus.Data;
using Modbus.Message;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;

namespace stend
{
    class ModbusTcpMaster
    {
        private ModbusWrite write;
        private ModbusRead read;

        private ModbusIpMaster _ipMaster;

        public ModbusTcpMaster(string IpAddr, int tcpPort,ModbusFuncFactory factory)
        {
            try
            {
                TcpClient tcpClient = new TcpClient(IpAddr, tcpPort);
                _ipMaster = ModbusIpMaster.CreateIp(tcpClient);
                _ipMaster.Transport.Retries = 3;
                _ipMaster.Transport.ReadTimeout = 30000;
                
            }
            catch (Exception ex)
            {
               Error.instance.HandleExceptionLog(ex);
            }

            write = factory.CreateWriteFunc(_ipMaster);
            read = factory.CreateReadFunc(_ipMaster);
        }

        public void WriteSingleCoil(byte SlaveAddr, ushort regAddr, bool value) { if(_ipMaster != null) write.WriteSingleCoil(SlaveAddr, regAddr, value); }
        public void WriteSingleRegister(byte SlaveAddr, ushort regAddr, ushort value) { if(_ipMaster != null) write.WriteSingleRegister(SlaveAddr, regAddr, value); }
        public ushort[] ReadHoldingRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr) { return _ipMaster != null ? read.ReadHoldingRegisters(SlaveAddr, StartAddr, numAddr) : new ushort[0]; }
        public ushort[] ReadInputRegisters(byte SlaveAddr, ushort StartAddr, ushort numAddr) { return _ipMaster != null ? read.ReadInputRegisters(SlaveAddr, StartAddr, numAddr): new ushort[0]; }
    }

    class ModbusTcpSlave:IDisposable
    {
        public ModbusSlave slave { get; set; }
        private byte slaveID;
        private TcpListener slaveTcpListener;
        private IPAddress addr;

        public ModbusTcpSlave(byte ID) 
        { 
            slaveID = ID;
            addr = IPAddress.Parse("127.0.0.1");
            
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    addr = ip;
                    break;
                }
            }
        }
        public void StartSlave()
        {
            slaveTcpListener = new TcpListener(addr, 502);
            slaveTcpListener.Start();
            
            slave = Modbus.Device.ModbusTcpSlave.CreateTcp(slaveID, slaveTcpListener);
            slave.ModbusSlaveRequestReceived += new EventHandler<ModbusSlaveRequestEventArgs>(Modbus_Request_Event);
            slave.DataStore = Modbus.Data.DataStoreFactory.CreateDefaultDataStore();
            slave.Listen();
        }
        
        public void Dispose()
        {
            slave.Stop();
            slave.Dispose();
        }

         private void Modbus_Request_Event(object sender, Modbus.Device.ModbusSlaveRequestEventArgs e)
        {
            //request from master
            byte fc = e.Message.FunctionCode;
            byte[] data = e.Message.MessageFrame;
            byte[] byteStartAddress = new byte[] { data[3], data[2] };
            byte[] byteNum = new byte[] { data[5], data[4] };
            Int16 StartAddress = BitConverter.ToInt16(byteStartAddress, 0);
            Int16 NumOfPoint = BitConverter.ToInt16(byteNum, 0);
        }
    }
}