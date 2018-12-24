﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySerialPorts
{
    public partial class Form2 : Form
    {
        delegate void DelegateDrawMap();//声明一个委托，此位置是尝试后的最佳位置
        AssocMap myAssocMap = null;
        DelegateDrawMap myDrawMap;
        public Form2()
        {
            InitializeComponent();
            myDrawMap = new DelegateDrawMap(DrawMap);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            System.Drawing.Rectangle rec = Screen.GetWorkingArea(this);
            int SH = (int)(rec.Height*0.8);
            int SW = (int)(rec.Width*0.8);
            this.Width = SW;
            this.Height = SH;
            pictureBox1.Location = new Point(0,0);
            pictureBox1.Size = new Size(SW,SH);
            myAssocMap = new AssocMap(pictureBox1);
            ZigbeeApi.myAssocList.ClearList();
            ZigbeeApi.Instance.ReqAssocList();
        }
        private void Form2_Deactivate(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke(myDrawMap);
        }
        private void DrawMap()
        {
            ZigbeeApi.myAssocList.LockList();
            AssocList.Tree_t tree = ZigbeeApi.myAssocList.GetAssocList();
            if (tree != null)
                myAssocMap.DrawDeviceMap((byte)ZigbeeApi.Device.GetChannel(), (UInt16)ZigbeeApi.Device.GetPanID(), tree);
            ZigbeeApi.myAssocList.ReleaseList();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
           // ZigbeeApi.myAssocList.ClearList();
          //  ZigbeeApi.Instance.ReqAssocList();
        }
    }
}