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
        private const int DevWith = 32;
        private const int DevHeight = 32;
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
        public void DrawDeviceMap(byte ch, UInt16 panID, AssocList.Tree_t list)
        {
            ClearImg();
            int cdx = (myImg.Width / 2)- DevWith;//x轴从中间开始画
            int cdy = 0;
            DrawCoord(ch,panID, cdx, cdy);
            DrawLayer(list, cdx, cdy, myImg.Width,myImg.Height);
            myPictureBox.Image = myImg;
        }
        private void DrawLayer(AssocList.Tree_t list, int x, int y,  int w, int h)
        {
            float loss = 0;
            int snd;
            if (list.child != null)
            {
                int devNum = list.child.Count;
                int sw = w /(devNum+1);//分配宽度。
                for (int i = 0; i < devNum; i++)
                {
                    int rsx = sw * (devNum / 2);//以当前设备的x轴为中心，计算左边起始位置。
                    if (x >= rsx)
                        rsx = x - rsx;
                    else
                        rsx = 0;
                    int sx = (rsx + (sw * i))+sw/2;//计算每个设备的x轴位置
                    int sy = (y + (DevHeight * 3));
                    int sh = (h - (DevHeight * 3));
                    AssocList.Tree_t dev = list.child[i];
                    switch (dev.devType)
                    {
                        case ZigbeeCommon.DevType_Route:
                            DrawLine(dev.lqi,x,y,sx,sy);
                            loss = ZigbeeApi.Device.GetLossPercent(dev.addr);
                            snd = ZigbeeApi.Device.GetSendCount(dev.addr);
                            DrawRouter(dev.addr,dev.lqi,loss,snd,sx,sy);
                            DrawLayer(dev, sx, sy,sw,sh);
                            break;
                        case ZigbeeCommon.DevType_EndDev:
                            DrawLine(dev.lqi, x, y, sx, sy);
                            loss = ZigbeeApi.Device.GetLossPercent(dev.addr);
                            snd = ZigbeeApi.Device.GetSendCount(dev.addr);
                            DrawEndDevice(dev.addr, dev.lqi,loss,snd,sx, sy);
                            break;
                        default:
                            break;
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
        private void DrawLine(byte lqi,int x, int y, int ex, int ey)
        {
            Color myColor;
            if (lqi > 80)
                myColor = Color.Green;
            else if(lqi>70)
                myColor = Color.YellowGreen;
            else if (lqi > 60)
                myColor = Color.Yellow;
            else if (lqi > 50)
                myColor = Color.Orange;
            else if (lqi > 30)
                myColor = Color.OrangeRed;
            else 
                myColor = Color.Red;
            Pen black_pen = new Pen(myColor, 2);
            x += (DevWith / 2);
            y += (DevHeight);
            ex += (DevWith / 2);
            myGraphics.DrawLine(black_pen, x, y, ex, ey);//x坐标轴
        }
    }
}
