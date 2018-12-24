using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySerialPorts
{
    class ZigbeeApi
    {
        public static ZigbeeOutput  Output;
        public static ZigbeeInput   Input;
        public static ZigbeeDevice  Device;
        public static Ztool         myZtool;
        public static AssocList     myAssocList;
        public static System.IO.Ports.SerialPort Uart;
        public static ZigbeeApi Instance;
        public ZigbeeApi(Ztool ztool, System.IO.Ports.SerialPort sp)
        {
            Output = new ZigbeeOutput();
            Input = new ZigbeeInput();
            Device = new ZigbeeDevice();
            myAssocList = new AssocList();
            myZtool = ztool;
            Instance = this;
            Uart = sp;
        }
        public void ReqSendOnOff(UInt16 addr, byte point, byte val)
        {
            byte[] buf = new byte[14];
            int index = 0;
            byte seq = ZigbeeCommon.GetSeq();
            ZigbeeCommon.U16toB(buf, 0, addr); index += 2;
            buf[index++] = point;//2
            buf[index++] = seq;//3
            buf[index++] = 0x00;//disable response 4
            ZigbeeCommon.U16toB(buf, index, 0x0006); index += 2;//5 
            buf[index++] = 6;//

            buf[index++] = val;
            buf[index++] = 0x01;
            buf[index++] = 0x00;

            buf[index++] = 0x00;
            buf[index++] = 0x00;

            buf[index++] = 0x00;
            Output.ReqSendCommand(0x29, 0x06, seq, buf);
        }
        public void ReqSendCommand(UInt16 addr, byte point, UInt16 cID, byte[] data)
        {
            int slen = 8;
            if (data != null)
                slen += data.Length;
            byte[] buf = new byte[slen];
            int index = 0;
            byte seq = ZigbeeCommon.GetSeq();
            ZigbeeCommon.U16toB(buf, 0, addr); index += 2;
            buf[index++] = point;//2
            buf[index++] = seq;//3
            buf[index++] = 0x00;//disable response 4
            ZigbeeCommon.U16toB(buf, index, cID); index += 2;//5 
            buf[index++] = (byte)data.Length;//
            Array.Copy(data, 0, buf, index, data.Length);
            Output.ReqSendCommand(0x29, 0x06, seq, buf);
        }
        public void ReqReadNV(UInt16 item, byte offset)
        {
            byte[] buf = new byte[3];
            ZigbeeCommon.U16toB(buf, 0, item);
            buf[2] = offset;
            Output.ReqSendCommand(0x21, 0x08, 0x00, buf);
        }
        public void ReqWriteNV(UInt16 item, byte offset, byte[] data)
        {
            byte slen = 4;
            if (data != null)
                slen += (byte)data.Length;
            byte[] buf = new byte[slen];
            ZigbeeCommon.U16toB(buf, 0, item);
            buf[2] = offset;
            buf[3] = (byte)data.Length;
            if (data != null)
                Array.Copy(data,0,buf,4,data.Length);
            Output.ReqSendCommand(0x21, 0x09, 0x00, buf);
        }
        public void ReqReadNetWorkInfo()
        {
            ReqReadNV(0x0021, 22);//read panID
            Device.SetNVOperationType(ZigbeeCommon.NVReadNetWorkInfo);
            //ReqReadNV(0x0062, 0);//read preconfig key
        }
        public void ReqWriteNetWorkInfo(byte channel, UInt16 panID)
        {
            byte[] buf = new byte[13];
            buf[0] = channel;
            buf[1] = 0x00;//coordinator address
            buf[2] = 0x00;//coordinator address
            byte[] exaddr = Device.GetExAddr();
            Array.Copy(exaddr,0,buf,3,8);
            ZigbeeCommon.U16toB(buf,11,panID);
            ReqWriteNV(0x0021,22,buf);
        }
        public void ReqReadPreConfigKey()
        {
            ReqReadNV(0x0082, 0);//read panID0x0062
            Device.SetNVOperationType(ZigbeeCommon.NVReadPreConfigKey);
        }
        public void ReqWritePreConfigKey(byte[] key)
        {
            ReqWriteNV(0x0082, 1,key);//read panID0x0062
        }
        public void ReqResetSystem()
        {
            byte[] buf = new byte[1];
            buf[0] = 0x00;
            Output.ReqSendCommand(0x41, 0x00, 0x00, buf);
        }
        public void ReqAllDevicePointList()
        {
            ReqSendOnOff(0xFFFF,0x08,0x00);
        }
        public void ReqAssocList()
        {
            ReqAssoc(0x0000,0);
        }
        public void ReqAssoc(UInt16 addr, byte index)
        {
            byte[] buf = new byte[3];
            buf[0] = (byte)(addr & 0x00FF);
            buf[1] = (byte)((addr >> 8)&0x00FF);
            buf[2] = index;
            Output.ReqSendCommand(0x25, 0x31, 0x00, buf);
        }
        public void ReqPermitJoin(byte duration)
        {
            byte[] buf = new byte[5];
            buf[0] = 0x02;
            buf[1] = 0xFC;
            buf[2] = 0xFF;
            buf[3] = duration;
            buf[4] = 0x01;
            Output.ReqSendCommand(0x25, 0x36, 0x00, 1, buf);
        }
        private void MessageInput(byte[] pkg)
        {
            byte cmd0 = pkg[2];
            byte cmd1 = pkg[3];
            switch (cmd0 & 0x0F)
            {
                case 0x01://system layer
                    Output.Remove(0x21, cmd1, 0x00);
                    if ((cmd1 == 0x08)||(cmd1==0x09))
                        Device.InputMessage(pkg, pkg.Length);
                    break;
                case 0x05://zdo layer
                    Output.Remove(0x25, (byte)(cmd1&0x7F), 0x00);
                    if((cmd0&0xF0)==0x40)
                        Device.InputMessage(pkg, pkg.Length);
                    break;
                case 0x09://app layer
                    if ((cmd0 & 0xF0) == 0x40)
                    {
                        UInt16 addr = ZigbeeCommon.BtoU16(pkg,4);
                        Output.Remove(addr);
                        AppProc(pkg, pkg.Length);
                    }
                    break;
            }
        }
        public void Poll()
        {
            if (Uart != null)
            {
                if (Uart.IsOpen)
                {
                    Output.Poll();
                    Device.Poll();
                    byte[] pkg = Input.read_package();
                    if (pkg != null)
                    {
                        MessageInput(pkg);
                    }
                }
                else
                {
                    Output.Clear();
                }
            }
        }
        private void AppProc(byte[] pkg, int len)
        {
            int Index = 4;
            byte cmd0;
            byte cmd1;
            byte dlen;
            cmd0 = pkg[2];
            cmd1 = pkg[3];
            dlen = pkg[4];
            UInt16 addr = ZigbeeCommon.BtoU16(pkg, Index);
            if (Device.CheckExist(addr) == true)
            {
                string str = "app rec:";
                str += ZigbeeCommon.BtoHexStr(pkg, 0, pkg.Length);
                myZtool.UpdateRecieveTextBox(str);
                Device.ReceiveCount(addr);
            }
            else
            {
                Device.AddDevice(addr, null);
            }
        }
    }
}
