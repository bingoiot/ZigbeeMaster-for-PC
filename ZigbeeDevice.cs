using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MySerialPorts
{
    class ZigbeeDevice
    {
        public class Point_t
        {
            public byte point;
            public UInt16 devID;
            public byte ver;
            public UInt16[] inCLusterID;
            public UInt16[] outClusterID;
        }
        public class Device_t
        {
            public UInt16           addr;
            public int SendCnt;
            public int RecCnt;
            public int LossCnt;
            public byte[]           exAddr;
            public List<Point_t>    PointList;
        }
        static List<Device_t>   DeviceList = new List<Device_t>();
        static ArrayList        NewDeviceList = new ArrayList();
        static int              PollRuntime = 0;
        static byte         Channel=0;
        static UInt16       PanID=0;
        static byte[]       ExAddr = new byte[8];
        static byte[]       NwkKey = new byte[16];
        static int          NVOperationType = 0;
        public void InputMessage(byte[] data, int len)
        {
            if (data[2] == 0x61)//read or write NV  items
            {
                switch (data[3])
                {
                    case 0x08://read NV items
                        NVMessageProc(data,len);
                        break;
                    case 0x09://write NV
                        break;    
                }
            }
            else
            {
                switch (data[3])//cmd1
                {
                    case 0x85://read points
                        ReadPointResponse(data, len);
                        break;
                    case 0x84://read describe
                        ReadDescribeResponse(data, len);
                        break;
                    case 0xC1://new device joined
                        NewJoinProc(data, len);
                        break;
                    case 0xC9://new device leave
                        DeviceLeaveProc(data, len);
                        break;
                    case 0xB1://response Associte List
                        ZigbeeApi.myAssocList.MessageInput(data, len);
                        break;

                }
            }
        }
        public int GetChannel()
        {
            return Channel;
        }
        public int GetPanID()
        {
            return PanID;
        }
        public void SetNVOperationType(int type)
        {
            NVOperationType = type;
        }
        public byte[] GetExAddr()
        {
            byte[] buf = new byte[8];
            Array.Copy(ExAddr,buf,8);
            return buf;
        }
        public Device_t GetNewDevieInfo()
        {
            if (NewDeviceList.Count > 0)
            {
                UInt16 addr = (UInt16)NewDeviceList[0];
                NewDeviceList.Remove(addr);
                Device_t dev = GetDevice(addr);
                return dev;
            }
            return null;
        }
        public void LossCount(UInt16 addr)
        {
            foreach (Device_t dev in DeviceList)
            {
                if(dev.addr==addr)
                    dev.LossCnt++;
            }
        }
        public void SendCount(UInt16 addr)
        {
            foreach (Device_t dev in DeviceList)
            {
                if (dev.addr == addr)
                    dev.SendCnt++;
            }
        }
        public int GetSendCount(UInt16 addr)
        {
            foreach (Device_t dev in DeviceList)
            {
                if (dev.addr == addr)
                    return dev.SendCnt;
            }
            return 0;
        }
        public void ReceiveCount(UInt16 addr)
        {
            foreach (Device_t dev in DeviceList)
            {
                if (dev.addr == addr)
                    dev.RecCnt++;
            }
        }
        public void ClearCounter()
        {
            foreach (Device_t dev in DeviceList)
            {
                dev.RecCnt=0;
                dev.SendCnt = 0;
                dev.LossCnt = 0;
            }
        }
        public void ClearDevice()
        {
            for (int i = (DeviceList.Count-1); i >= 0; i--)
                DeviceList.RemoveAt(i);
        }
        public int GetDeviceCount()
        {
            return DeviceList.Count;
        }
        public Device_t GetDevice(int index)
        {
            if (DeviceList.Count > 0)
                return DeviceList[index];
            else
                return null;
        }
        public float GetLossPercent(UInt16 addr)
        {
            Device_t dev = GetDevice(addr);
            float f = (float)0.0;
            if (dev != null)
            {               
                if (dev.SendCnt > 0)
                    f = ((float)dev.LossCnt/dev.SendCnt)*(float)100.0;
            }
            return f;
        }
        public void Poll()
        {
            if (PollRuntime >= ZigbeeCommon.Tick)
                PollRuntime = PollRuntime - ZigbeeCommon.Tick;
            else
                PollRuntime = 0;
            if (PollRuntime == 0)
            {
                foreach(Device_t dev in DeviceList)
                {
                    if (CheckDeviceInfoComplite(dev) == false)
                    {
                        ReqActivePoints(dev.addr);
                    }
                }
                PollRuntime = ZigbeeCommon.PollTimeOut;
            }
        }
        public void AddDevice(UInt16 addr, byte[] exAddr)
        {
            if (CheckExist(addr) == false)
            {
                Device_t dev = new Device_t();
                dev.addr = addr;
                dev.exAddr = exAddr;
                dev.PointList = new List<Point_t>();
                DeviceList.Add(dev);
                ReqActivePoints(dev.addr);
            }     
        }
        private void NVMessageProc(byte[] data, int len)
        {
            switch (NVOperationType)
            {
                case ZigbeeCommon.NVReadNetWorkInfo:
                    if (data[4] == 0x00)//check success
                    {
                        byte channel = data[6];
                        UInt16 addr = ZigbeeCommon.BtoU16(data, 7);
                        Array.Copy(data, 9, ExAddr, 0, 8);
                        UInt16 panID = ZigbeeCommon.BtoU16(data, 17);
                        Channel = channel;
                        PanID = panID;
                        ZigbeeApi.myZtool.UptateNetWorkInfoTextBox(channel, panID);
                    }
                break;
                case ZigbeeCommon.NVReadPreConfigKey:
                    if (data[4] == 0x00)//check success
                    {
                        byte slen = data[5];
                        byte seq = data[6];
                        Array.Copy(data,7,NwkKey,0,16);
                        ZigbeeApi.myZtool.UpdatePreConfigKey(NwkKey);
                    }
                break;
            }
        }
        private void NewJoinProc(byte[] data, int len)
        {
            UInt16 srcAddr;
            UInt16 devAddr;
            int Index = 4;
            srcAddr = ZigbeeCommon.BtoU16(data,Index);Index += 2;
            devAddr = ZigbeeCommon.BtoU16(data, Index); Index += 2;
            byte[] exAddr = new byte[8];
            Array.Copy(data, Index, exAddr, 0,8);
            AddDevice(devAddr, exAddr);
        }
        private void DeviceLeaveProc(byte[] data, int len)
        {
            UInt16 srcAddr;
            int Index = 4;
            srcAddr = ZigbeeCommon.BtoU16(data,Index);
            Remove(srcAddr);
        }
        private void ReadPointResponse(byte[] data, int len)
        {
            UInt16 srcAddr;
            UInt16 devAddr;
            int  Index;
            byte pNum;
            byte state;
            Index = 1 + 1 + 1 + 1;
            srcAddr = ZigbeeCommon.BtoU16(data, Index); Index += 2;
            state = data[Index++];
            if (state == 0x00)//succeed
            {
                devAddr = ZigbeeCommon.BtoU16(data, Index); Index += 2;
                pNum = data[Index]; Index += 1;
                byte[] points = new byte[pNum];
                Array.Copy(data, Index, points, 0, pNum);
                for (int i = 0; i < points.Length; i++)
                    ReqDescribe(srcAddr, points[i]);
                AddPoints(srcAddr, points);
            }
        }
        private void ReadDescribeResponse(byte[] data, int len)
        {
            int Index = 4;
            UInt16 srcAddr = ZigbeeCommon.BtoU16(data,Index);Index += 2;
            byte state = data[Index++];
            if (state == 0x00)//succeed
            {
                UInt16 devAddr = ZigbeeCommon.BtoU16(data,Index);Index += 2;
                byte dlen = data[Index++];
                byte point = data[Index++];
                UInt16 profileID = ZigbeeCommon.BtoU16(data,Index);Index += 2;
                UInt16 deviceID = ZigbeeCommon.BtoU16(data, Index);Index += 2;
                byte ver = data[Index++];
                byte inNum = data[Index++];
                UInt16[] IncID = ConvertcID(data, Index, inNum);Index += (inNum * 2);
                byte outNum = data[Index++];
                UInt16[] OutcID = ConvertcID(data, Index, outNum);
                AddDescribe(srcAddr,point,deviceID,ver,IncID,OutcID);
                NewDeviceList.Add(srcAddr);
            }
        }
        public UInt16 GetOnOffcID(Point_t point)
        {
            if (point.inCLusterID != null)
            {
                for (int i = 0; i < point.inCLusterID.Length; i++)
                {
                    if ((point.inCLusterID != null) && (point.inCLusterID[i] == 0x0006))
                        return 0x0006;
                }
            }
            return 0xFFFF;
        }
        public void ReqActivePoints(UInt16 addr)
        {
            byte[] buf = new byte[4];
            ZigbeeCommon.U16toB(buf,0, addr);
            ZigbeeCommon.U16toB(buf,2, addr);
            ZigbeeApi.Output.ReqSendCommand(0x25, 0x05, 0, buf);
        }
        static void ReqDescribe(UInt16 addr, byte point)
        {
            byte[] buf = new byte[5];
            ZigbeeCommon.U16toB(buf,0,addr);
            ZigbeeCommon.U16toB(buf,2,addr);
            buf[4] = point;
            ZigbeeApi.Output.ReqSendCommand(0x25, 0x04, 0, buf);
        }
        private void Remove(UInt16 addr)
        {
            try
            {
                foreach (Device_t dev in DeviceList)
                {
                    if (dev.addr == addr)
                        DeviceList.Remove(dev);
                }
            }catch(System.InvalidOperationException ex)
            {
                return;
            }
        }
        private Device_t GetDevice(UInt16 addr)
        {
            foreach (Device_t dev in DeviceList)
            {
                if (dev.addr == addr)
                    return dev;
            }
            return null;
        }
        private Point_t GetPoint(UInt16 addr,byte point)
        {
            Device_t dev = GetDevice(addr);
            if (dev != null)
            {
                foreach (Point_t p in dev.PointList)
                {
                    if (p.point == point)
                        return p;
                }
            }
            return null;
        }
        private void RemovePoint(Device_t dev, byte point)
        {
            try
            {
                foreach (Point_t p in dev.PointList)
                {
                    if (p.point == point)
                        dev.PointList.Remove(p);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                return;
            }
        }
        private void AddPoints(UInt16 addr, byte[] point)
        {
            Device_t dev;
            dev = GetDevice(addr);
            for (int i = 0; i < point.Length; i++)
            {
                RemovePoint(dev, point[i]);
                Point_t p = new Point_t();
                p.point = point[i];
                dev.PointList.Add(p);
            }
        }
        private void AddDescribe(UInt16 addr, byte point, UInt16 devID, byte ver, UInt16[] IncID, UInt16[] OutcID)
        {
            Point_t p;
            p = GetPoint(addr,point);
            if (p != null)
            {
                p.devID = devID;
                p.ver = ver;
                p.inCLusterID = IncID;
                p.outClusterID = OutcID;
            }
        }
        public Boolean CheckExist(UInt16 addr)
        {
            foreach(Device_t dev in DeviceList)
            {
                if (dev.addr == addr)
                    return true;
            }
            return false;
        }
        private Boolean CheckDeviceInfoComplite(Device_t dev)
        {
            if (dev.PointList.Count == 0)
                return false;
            foreach (Point_t ep in dev.PointList)
            {
                if ((ep.inCLusterID == null) && (ep.outClusterID == null))
                    return false;
            }
            return true;
        }
        private UInt16[] ConvertcID(byte[] data, int startID, byte num)
        {
            int id = startID;
            UInt16[] cID = new UInt16[num];
            UInt16 tempL,tempH;
            for (int i = 0; i < num; i++)
            {
                tempL = data[id++];
                tempH = data[id++];
                cID[i] = (UInt16)(tempH<<8 | tempL);
            }
            return cID;
        }
    }
}
