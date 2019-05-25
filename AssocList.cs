using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySerialPorts
{
    class AssocList
    {
        public class NeighborInfo_t
        {
            public UInt16 addr;
            public byte   rxlqi;
            public byte   devType;
        }
        public class DeviceAssocInfo_t
        {
            public UInt16   addr;
            public int      startIndex;
            public Boolean  finished;
            
            public List<NeighborInfo_t> Neighbor;

            public int x;
            public int y;
            public int text_x;
            public int text_y;
        }
        private static List<DeviceAssocInfo_t> myDeviceList = new List<DeviceAssocInfo_t>();
        private static Mutex                myMutex = new Mutex();

        public void MessageInput(byte[] data, int len)
        {
            UInt16 srcAddr;
            int startID = 0;
            int index = 4;//start of data
            srcAddr = ZigbeeCommon.BtoU16(data, index); index += 2;
            if (data[index++] == 0x00)//if succees
            {
                byte totalNum = data[index++];
                byte startDevID = data[index++];
                byte curNum = data[index++];
                for (int i = 0; i < curNum; i++)
                {
                    index += 16;
                    UInt16 DevAddr = ZigbeeCommon.BtoU16(data, index);
                    index += 2;
                    byte devType = (byte)((data[index] & 0x03) + 1);
                    index += 3;
                    byte lqi = data[index++];
                    if ((devType == 0x02) && (CheckExist(DevAddr) == false))
                        ZigbeeApi.Instance.ReqAssoc(DevAddr, 0);
                    startID = AddDeviceAssoc(srcAddr, devType, DevAddr, lqi);
                }
                if (startID < totalNum)
                    ZigbeeApi.Instance.ReqAssoc(srcAddr, (byte)startID);
                else
                    SetFinishedFlag(srcAddr,true);
            }
        }
        public static byte GetDeviceType(UInt16 Addr)
        {
            byte devType = 0xFF;
            LockList();
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                foreach (NeighborInfo_t info in dev.Neighbor)
                {
                    if (info.addr == Addr)
                    {
                        devType = info.devType;
                        break;
                    }
                }
            }
            ReleaseList();
            return devType;
        }
        public static void LockList()
        {
            myMutex.WaitOne();
        }
        public static void ReleaseList()
        {
            myMutex.ReleaseMutex();
        }
        public static void ClearList()
        {
           myDeviceList = new List<DeviceAssocInfo_t>();
        }
        public static List<DeviceAssocInfo_t> GetAssocList()
        {
            return myDeviceList;
        }
        public static void SetTextPoint(UInt16 Addr, int x, int y)
        {
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                if (dev.addr == Addr)
                {
                    dev.text_x = x;
                    dev.text_y = y;
                    break;
                }
            }
        }
        public static DeviceAssocInfo_t GetDeviceAssocInfo(UInt16 Addr)
        {
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                if (dev.addr == Addr)
                {
                    return dev;
                }
            }
            return null;
        }
        private static bool CheckExist(UInt16 Addr)
        {
            bool ret = false;
            LockList();
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                if (dev.addr == Addr)
                {
                    ret = true;
                    break;
                }
            }
            ReleaseList();
            return ret;
        }
        private static int AddDeviceAssoc(UInt16 Addr, byte DevType, UInt16 NeighborAddr, byte NeighborLqi)
        {
            bool has = false;
            int index = 0;
            if (CheckExist(Addr) == false)
            {
                LockList();
                DeviceAssocInfo_t neighbor = new DeviceAssocInfo_t();
                neighbor.addr = Addr;
                neighbor.finished = false;
                neighbor.startIndex = 0;
                neighbor.Neighbor = new List<NeighborInfo_t>();
                neighbor.x = 0;
                neighbor.y = 0;
                myDeviceList.Add(neighbor);
                ReleaseList();
            }
            LockList();
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                if (dev.addr == Addr)
                {
                    foreach (NeighborInfo_t info in dev.Neighbor)
                    {
                        if (info.addr == NeighborAddr)
                        {
                            has = true;
                            info.rxlqi = NeighborLqi;//update lqi;
                            break;
                        }
                    }
                    if (has == false)
                    {
                        NeighborInfo_t n = new NeighborInfo_t();
                        n.addr = NeighborAddr;
                        n.rxlqi = NeighborLqi;
                        n.devType = DevType;
                        dev.Neighbor.Add(n);
                    }
                    dev.startIndex++;
                    index = dev.startIndex;
                    break;
                }
            }
            ReleaseList();
            return index;
        }
        private static void SetFinishedFlag(UInt16 Addr, bool IsFinished)
        {
            LockList();
            foreach (DeviceAssocInfo_t dev in myDeviceList)
            {
                if (dev.addr == Addr)
                {
                    dev.finished = IsFinished;
                    break;
                }
            }
            ReleaseList();
        }
    }
}
