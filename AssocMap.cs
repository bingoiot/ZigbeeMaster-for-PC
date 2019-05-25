using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MySerialPorts
{
    class AssocMap
    {
        static System.Windows.Forms.PictureBox myPictureBox;
        static Image    myImg;
        static Graphics myGraphics;
        private const int DevWith = 64;
        private const int DevHeight = 64;
        private const int DevNumPerLine = 6;
        private const int DevHeightPerLine = 4;
        private int DevStartID = 0;
        public AssocMap(System.Windows.Forms.PictureBox picbox)
        {
            myPictureBox = picbox;
            myImg = new Bitmap(picbox.Width, picbox.Height);
            myGraphics = Graphics.FromImage(myImg);
            myGraphics.Clear(Color.WhiteSmoke);
            picbox.Image = myImg;
           // DrawCoord(0,12,myImg.Width/2,0);
            //DrawRouter(12,23,(float)12.3,200,200);
            //DrawEndDevice(1,45,(float)78.2, 46, 45);

            //DrawLine(50,myImg.Width/2,0,200,200);
        }
        public void ClearImg()
        {
            myGraphics.Clear(Color.WhiteSmoke);
        }
        public void ClearImg(Color c)
        {
            myGraphics.Clear(c);
        }
        public void DrawDeviceMap(byte ch, UInt16 panID, List<AssocList.DeviceAssocInfo_t> list)
        {
            if (list.Count > 1)
            {
                ClearImg();
                int cdx = (myImg.Width / 2) - DevWith;//x轴从中间开始画
                int cdy = 0;
                DevStartID = 1;
                list[0].x = cdx;
                list[0].y = cdy;
                list[0].text_x = cdx;
                list[0].text_y = cdy;
                DrawCoord(ch, panID, cdx, cdy);
                DrawLayer(list, cdx, cdy, myImg.Width, myImg.Height);
                myPictureBox.Image = myImg;
            }
        }
        private void DrawLayer(List<AssocList.DeviceAssocInfo_t> list, int x, int y, int w, int h)
        {
            float loss = 0;
            int snd;
            byte devType = 0;
            if (list != null)
            {
                while (DevStartID < list.Count)
                {
                    int devNum = DevNumPerLine;
                    int sw = w / (devNum + 1);//分配宽度。
                    y = y + (DevHeight * DevHeightPerLine);
                    int sy = y;
                    for (int i = 0; (DevStartID < list.Count) && (i < devNum); i++)
                    {
                        int rsx = sw * (devNum / 2);//以当前设备的x轴为中心，计算左边起始位置。
                        if (x >= rsx)
                            rsx = x - rsx;
                        else
                            rsx = 0;
                        int sx = (rsx + (sw * i)) + sw / 2;//计算每个设备的x轴位置
                        AssocList.DeviceAssocInfo_t dev = list[DevStartID++];
                        devType = AssocList.GetDeviceType(dev.addr);
                        switch (devType)
                        {
                            case ZigbeeCommon.DevType_Route:
                                //DrawLine(dev.lqi,x,y,sx,sy);
                                loss = ZigbeeApi.Device.GetLossPercent(dev.addr);
                                snd = ZigbeeApi.Device.GetSendCount(dev.addr);
                                DrawRouter(dev.addr, 0xFF, loss, snd, sx, sy);
                                dev.x = sx;
                                dev.y = sy;
                                dev.text_y = sy;
                                dev.text_x = sx;
                                //DrawLayer(list, sx, sy, sw, sh);
                                break;
                            case ZigbeeCommon.DevType_EndDev:
                                // DrawLine(dev.lqi, x, y, sx, sy);
                                loss = ZigbeeApi.Device.GetLossPercent(dev.addr);
                                snd = ZigbeeApi.Device.GetSendCount(dev.addr);
                                DrawEndDevice(dev.addr, 0xFF, loss, snd, sx, sy);
                                dev.x = sx;
                                dev.y = sy;
                                dev.text_y = sy;
                                dev.text_x = sx;
                                break;
                        }
                    }
                }
                DrawLQILine(list);
            }
        }
        private void DrawLQILine(List<AssocList.DeviceAssocInfo_t> list)
        {
            int StartID = 0;
            int sx, sy, ex, ey;
            AssocList.DeviceAssocInfo_t d;
            while (StartID < list.Count)
            {
                AssocList.DeviceAssocInfo_t dev = list[StartID++];
                sx = dev.x;
                sy = dev.y;
                foreach (AssocList.NeighborInfo_t n in dev.Neighbor)
                {
                    if ((d = AssocList.GetDeviceAssocInfo(n.addr)) != null)
                    {
                        ex = d.x;
                        ey = d.y;
                        DrawLine(n.rxlqi, sx, sy, ex, ey);
                        DrawStringLqi(dev.addr,n.addr, n.rxlqi, dev.text_x, dev.text_y, ex, ey);
                        AssocList.SetTextPoint(n.addr, d.text_x, d.text_y+25);
                    }
                }
            }
        }
        private void DrawRouter(UInt16 addr, byte lqi, float loss,int snd,int x, int y)
        {
            Brush bush = new SolidBrush(Color.Coral);
            myGraphics.FillEllipse(bush, x, y, DevWith, DevHeight);

            Font f = new Font("Arial", 12);
            PointF pf = new PointF(x+(DevWith/4), y+(DevHeight/4));
            myGraphics.DrawString("R", f, Brushes.White, pf);

            f = new Font("Arial", 8);
            pf = new PointF(x+ DevWith, y);
            String str = "dev:" + addr.ToString("X4") + "\r\n" + "lqi:" + lqi.ToString() + "\r\n";
            str += "snd:" + snd.ToString()+"\r\n";
            str += "loss:" + loss.ToString() + "%" + "\r\n";
            myGraphics.DrawString(str, f, Brushes.Green, pf);
        }
        private void DrawCoord(byte channel, UInt16 panID, int x, int y)
        {
            Brush bush = new SolidBrush(Color.OrangeRed);
            myGraphics.FillEllipse(bush, x, y, DevWith, DevHeight);//

            Font f = new Font("Arial", 12);
            PointF pf = new PointF(x + (DevWith / 4), y + (DevHeight / 4));
            myGraphics.DrawString("C", f, Brushes.White, pf);

            f = new Font("Arial", 8);
            pf = new PointF(x + DevWith, y);
            String str = "ch:" + channel.ToString("X2") + "\r\n" + "panID:"+panID.ToString("X4");
            myGraphics.DrawString(str, f, Brushes.Green, pf);
        }
        private void DrawEndDevice(UInt16 addr, byte lqi, float loss, int snd, int x, int y)
        {
            Brush bush = new SolidBrush(Color.SeaGreen);//填充的颜色
            myGraphics.FillEllipse(bush, x, y, DevWith, DevHeight);

            Font f = new Font("Arial", 12);
            PointF pf = new PointF(x + (DevWith / 4), y + (DevHeight / 4));
            myGraphics.DrawString("E", f, Brushes.White, pf);

            f = new Font("Arial", 8);
            pf = new PointF(x + DevWith, y);
            String str = "dev:" + addr.ToString("X4") + "\r\n" + "lqi:" + lqi.ToString() + "\r\n";
            str += "snd:" + snd.ToString() + "\r\n";
            str += "loss:" + loss.ToString() + "%" + "\r\n";
            myGraphics.DrawString(str, f, Brushes.Green, pf);
        }
        private void DrawStringLqi(UInt16 srcAddr, UInt16 tAddr, byte lqi, int x, int y, int ex, int ey)
        {
            x += (DevWith / 2);
            y += (DevHeight / 2);
            ex += (DevWith / 2);
            ey += (DevHeight / 2);

            x = (x + ex) / 2;
            y = (y + ey) / 2;
            Font f = new Font("Arial", 8);
            PointF pf = new PointF(x, y);
            String str = lqi.ToString("X2")+"@"+srcAddr.ToString("X4")+" <-- "+ tAddr.ToString("X4") ;
            myGraphics.DrawString(str, f, Brushes.Red, pf);
        }
        private void DrawLine(byte lqi,int x, int y, int ex, int ey)
        {
            Color myColor;
            if (lqi > 70)
                myColor = Color.Green;
            else if(lqi>50)
                myColor = Color.YellowGreen;
            else if (lqi > 30)
                myColor = Color.Yellow;
            else if (lqi > 10)
                myColor = Color.Orange;
            else 
                myColor = Color.OrangeRed;
            Pen black_pen = new Pen(myColor, 2);
            x += (DevWith / 2);
            y += (DevHeight/2);
            ex += (DevWith / 2);
            ey += (DevHeight / 2);
            myGraphics.DrawLine(black_pen, x, y, ex, ey);//x坐标轴
        }
    }
}
