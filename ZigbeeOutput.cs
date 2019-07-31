using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MySerialPorts 
{
    class ZigbeeOutput
    {
        private List<Send_t> SendList = new List<Send_t>();
        class Send_t
        {
            public int runtime;
            public byte stry;
            public byte cmd0;
            public byte cmd1;
            public byte seq;
            public byte[] data;
        };
        public ZigbeeOutput()
        {

        }
        public void ReqSendCommand(byte cmd0, byte cmd1, byte seq, byte[] data)
        {
            Send_t snd = new Send_t();
            if (ZigbeeApi.myZtool.CheckUartIsOpen() == false)
                return;
            snd.cmd0 = cmd0;
            snd.cmd1 = cmd1;
            snd.seq = seq;
            snd.runtime = 0;
            snd.stry = (byte)ZigbeeApi.myZtool.GetSendTry();
            snd.data = new byte[data.Length];
            Array.Copy(data, snd.data, data.Length);
            if (SendList != null)
            {
                if (snd.cmd0 == 0x29)
                {
                    UInt16 addr = ZigbeeCommon.BtoU16(data, 0);
                    byte point = data[2];
                    UInt16 cID = ZigbeeCommon.BtoU16(data, 5);
                    byte dlen = data[7];
                    byte[] buf = new byte[dlen];
                    Array.Copy(data, 8, buf, 0, dlen);
                    ZigbeeApi.myZtool.UpdateTestCommandInfo(ZigbeeApi.Device.GetDeviceCount(), addr, point, cID, buf);

                    ZigbeeApi.Device.SendCount(addr);
                }
                SendList.Add(snd);
            }
        }
        public void Clear()
        {
            try
            {
                for (int i = (SendList.Count-1); i >= 0 ; i--)
                {
                    SendList.RemoveAt(i);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                return;
            }
        }
        public void ReqSendCommand(byte cmd0, byte cmd1,byte seq, byte stry, byte[] data)
        {
            Send_t snd = new Send_t();
            snd.cmd0 = cmd0;
            snd.cmd1 = cmd1;
            snd.seq = seq;
            snd.runtime = ZigbeeApi.myZtool.GetSendTimeout();
            snd.stry = stry;
            snd.data = new byte[data.Length];
            Array.Copy(data, snd.data, data.Length);
            if (SendList != null)
            {
                if (snd.cmd0 == 0x29)
                {
                    UInt16 addr = ZigbeeCommon.BtoU16(data, 0);
                    byte point = data[2];
                    UInt16 cID = ZigbeeCommon.BtoU16(data,5);
                    byte dlen = data[7];
                    byte[] buf = new byte[dlen];
                    Array.Copy(data,8,buf,0,dlen); 
                    ZigbeeApi.myZtool.UpdateTestCommandInfo(ZigbeeApi.Device.GetDeviceCount(), addr, point,cID,buf);

                    ZigbeeApi.Device.SendCount(addr);
                }
                SendList.Add(snd);
            }
        }
        public void Remove(byte cmd0, byte cmd1, byte seq)
        {
            try
            {
                foreach (Send_t pkg in SendList)
                {
                    if ((pkg.cmd0 == cmd0) && (pkg.cmd1 == cmd1) && (pkg.seq == seq))
                        SendList.Remove(pkg);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                return;
            }
        }
        public void Remove(UInt16 addr)
        {
            try
            {
                foreach (Send_t pkg in SendList)
                {
                    if (pkg.cmd0 == 0x29)
                    {
                        UInt16 daddr = ZigbeeCommon.BtoU16(pkg.data, 0);
                        if(daddr == addr)
                            SendList.Remove(pkg);
                    }
                        
                }
            }
            catch (System.InvalidOperationException ex)
            {
                return;
            }
        }
        public void Poll()
        {
            try
            {
                if(SendList.Count>0)
                {
                    Send_t pkg = (Send_t)SendList[0];
                    if (pkg.runtime >= ZigbeeCommon.Tick)
                        pkg.runtime = pkg.runtime - ZigbeeCommon.Tick;
                    else
                        pkg.runtime = 0;
                    if (pkg.runtime == 0)
                    {
                        pkg.runtime = ZigbeeApi.myZtool.GetSendTimeout();
                        if (pkg.stry > 0)
                        {
                            Send(pkg.cmd0, pkg.cmd1, pkg.data);
                            pkg.stry--;
                            return;
                        }
                        else
                        {
                            if (pkg.cmd0 == 0x29)
                            {
                                UInt16 addr = ZigbeeCommon.BtoU16(pkg.data,0);
                                ZigbeeApi.Device.LossCount(addr);
                            }
                            SendList.Remove(pkg);
                        }
                    }
                }
            }
            catch (System.InvalidOperationException ex)
            {
                return;
            }
        }
        private int Send(byte cmd0, byte cmd1, byte[] data)
        {
            int slen = data.Length + 5;
            byte[] buf = new byte[slen];
            buf[0] = 0xFE;
            buf[1] = (byte)data.Length;
            buf[2] = cmd0;
            buf[3] = cmd1;
            Array.Copy(data, 0, buf, 4, data.Length);
            buf[slen - 1] = ZigbeeCommon.GetXor(buf,1,(slen-1));
            String str = "snd:"+ZigbeeCommon.BtoHexStr(buf, 0, buf.Length);
            ZigbeeApi.myZtool.UpdateSendTextBox(str);
            if(ZigbeeApi.Uart.IsOpen)
                ZigbeeApi.Uart.Write(buf,0,buf.Length);
            return 0;
        }
    }
}
