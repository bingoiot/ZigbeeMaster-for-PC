using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySerialPorts
{
    class ZigbeeApp
    {
        ZigbeeApi   myZigbeeApi;
        Ztool       myZtool;
        private static int ShowDeviceInfoRuntime = 0;
        private static int ShowDeviceListRuntime = 0;
        private static int TestDeviceRunTime = 0;
        private static int TestDeviceIndex = 0;
        private static byte OnOff = 0;
        private static Boolean RunFlag = false;
        private static Boolean RunLoopFlag = false;
        public ZigbeeApp(Ztool zt,System.IO.Ports.SerialPort sp)
        {
            myZigbeeApi = new ZigbeeApi(zt,sp);
            myZtool = zt;
        }
        public void ReqRun(Boolean flag)
        {
            RunFlag = flag;
        }
        public Boolean GetRunFlag()
        {
            return RunFlag;
        }
        public void ReqRunLoopFlag(Boolean flag)
        {
            RunLoopFlag = flag;
        }
        public Boolean GeRunLoopFlag()
        {
            return RunLoopFlag;
        }
        public void Poll()
        {
            myZigbeeApi.Poll();
            ShowNewDeviceInfo();
            ShowDeviceList();
            if(RunFlag&& RunLoopFlag)
                TestRun();
        }
        public void TestRun()
        {
            int num;
            int SendDelay = myZtool.GetSendDelay();
            if (TestDeviceRunTime >= ZigbeeCommon.Tick)
                TestDeviceRunTime -= ZigbeeCommon.Tick;
            else
                TestDeviceRunTime = 0;
            if (TestDeviceRunTime == 0)
            {
                TestDeviceRunTime = SendDelay;
                num = ZigbeeApi.Device.GetDeviceCount();
                if (TestDeviceIndex >= num)
                    TestDeviceIndex = 0;
                ZigbeeDevice.Device_t dev = ZigbeeApi.Device.GetDevice(TestDeviceIndex);
                if (dev != null)
                {
                    if ((dev.PointList!=null)&&(dev.PointList.Count > 0))
                    {
                        for (int i = 0; i < dev.PointList.Count; i++)
                        {
                            UInt16 cID = ZigbeeApi.Device.GetOnOffcID(dev.PointList[i]);
                            if (cID != 0xFFFF)
                            {
                                ZigbeeApi.Instance.ReqSendOnOff(dev.addr, dev.PointList[i].point, OnOff);
                            }
                        }
                    }
                }
                if (TestDeviceIndex == 0)
                {
                    if (OnOff > 0)
                        OnOff = 0x00;
                    else
                        OnOff = 0x01;                  
                }
                TestDeviceIndex++;
            }
        }
        private void ShowDeviceList()
        {
            ZigbeeDevice.Device_t dev;
            String str="";
            if (ShowDeviceListRuntime >= ZigbeeCommon.Tick)
                ShowDeviceListRuntime -= ZigbeeCommon.Tick;
            else
                ShowDeviceListRuntime = 0;
            if (ShowDeviceListRuntime == 0)
            {
                ShowDeviceListRuntime = ZigbeeCommon.ShowDeviceListUpdateTime;
                int num = ZigbeeApi.Device.GetDeviceCount();
                for (int i = 0; i < num; i++)
                {
                    dev = ZigbeeApi.Device.GetDevice(i);
                    str += "Dev:" + dev.addr.ToString("X4");
                    str += " Snd:" + dev.SendCnt.ToString() + " Rec:" + dev.RecCnt.ToString() + " loss:" + dev.LossCnt.ToString();
                    float percent = dev.LossCnt;
                    if (dev.SendCnt == 0)
                        percent = 0;
                    else
                        percent = (percent / dev.SendCnt)*(float)100.0;
                    str += " loss:" + percent.ToString() + "%\r\n";
                }
                myZtool.UpdateDeviceListTextBox(str);
            }
        }
        private void ShowNewDeviceInfo()
        {
            if (ShowDeviceInfoRuntime >= ZigbeeCommon.Tick)
                ShowDeviceInfoRuntime -= ZigbeeCommon.Tick;
            else
                ShowDeviceInfoRuntime = 0;
            if (ShowDeviceInfoRuntime == 0)
            {
                ShowDeviceInfoRuntime = ZigbeeCommon.ShowDeviceInfoUpdateTime;
                ZigbeeDevice.Device_t dev = ZigbeeApi.Device.GetNewDevieInfo();
                if (dev != null)
                {
                    String str = "New Device:" + dev.addr.ToString("X4") + "\r\n";
                    foreach (ZigbeeDevice.Point_t p in dev.PointList)
                    {
                        if (p.inCLusterID != null && p.outClusterID != null)
                        {
                            str += "Point:" + p.point.ToString("X2") + "\r\n";
                            str += "device ID:" + p.devID.ToString("X4") + " ver:" + p.ver.ToString("X2") + "\r\n";
                            str += "Input  Cluster ID:" + ZigbeeCommon.U16toHexStr(p.inCLusterID, 0, p.inCLusterID.Length) + "\r\n";
                            str += "Output Cluster ID:" + ZigbeeCommon.U16toHexStr(p.outClusterID, 0, p.outClusterID.Length) + "\r\n";
                        }
                    }
                    str += "\r\n";
                    myZtool.UpdateRecieveTextBox(str);
                }
            }
        }
    }
}
