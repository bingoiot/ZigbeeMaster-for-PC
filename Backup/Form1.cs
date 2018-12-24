#region namespace inclusions
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading; 
#endregion

namespace MySerialPorts
{
    public partial class 清除发送区显示 : Form
    {
        int readFlag=0,writeFlag=0;
        delegate void HandleInterfaceUpdateDelegate(string rFlag,string readtext);//声明一个委托，此位置是尝试后的最佳位置
        HandleInterfaceUpdateDelegate interfaceUpdateHandle;//定义一个委托实例，此位置是尝试后的最佳位置
        //delegate void HandleSerialPortStatus(object sender, System.EventArgs e);
        //event HandleSerialPortStatus HandleStatus;

        public 清除发送区显示()
        {
            InitializeComponent();
            fasong.Text = "发送："+writeFlag.ToString();
            jieshou.Text = "接收："+readFlag.ToString();
            interfaceUpdateHandle = new HandleInterfaceUpdateDelegate(UpdateReceiveTextBox);//实例化委托，此位置是尝试后的最佳位置
            //HandleStatus(this.sp.IsOpen,new System.EventArgs());
        }

        #region Methods
        /// <summary>
        /// 保存串口的设置
        /// </summary>
        private void SaveProperties()
        {
            sp.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox4.Text);
            string StopB = comboBox6.Text.Substring(0,1);//将停止位中的汉字截掉，以便和枚举变量匹配
            sp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopB);
            sp.PortName = comboBox2.Text; //确定选中的串口
            sp.BaudRate = int.Parse(comboBox3.Text);
            string DataB = comboBox5.Text.Substring(0,1);//将数据位中的汉字截掉，以便和整型变量匹配
            sp.DataBits =int.Parse(DataB);         
        }

        /// <summary>
        /// 定义一个方法，以供委托调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateReceiveTextBox(string rFlag,string readtext)
        {
            jieshou.Text = "接收：" + rFlag;
            richTextBox3.Text += readtext;
        }
        #endregion Methods

        #region Event Handlers
        private void 连接_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp.IsOpen==false)
                {
                    SaveProperties();
                    sp.Open();
                    连接.Text = "关闭";
                }
                else
                {
                    { sp.Close(); 连接.Text = "连接"; }
                }
            }
            catch
            { MessageBox.Show("串口不存在或者被其他应用程序占用！","提示"); }
        }

        private void 发送_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp.IsOpen == true && richTextBox1.Text == "")
                {
                    MessageBox.Show("请输入发送数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                { 
                    sp.Write(richTextBox1.Text);
                    writeFlag++;
                    Thread.Sleep(1000);
                    fasong.Text = "发送：" + writeFlag.ToString();
                }
            }
            catch
            {
                MessageBox.Show("串口尚未连接，" + "\n" + "数据发送失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void 清除显示_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox3.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox1.Clear();
        }

        private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string temp= sp.ReadExisting();
                readFlag++;
                Invoke(interfaceUpdateHandle, readFlag.ToString(),temp);
            }
            catch
            { MessageBox.Show("错误"); }
        }

        private void 复位计数_Click(object sender, EventArgs e)
        {
            readFlag = 0;
            jieshou.Text = "接收：" + readFlag.ToString();
            writeFlag = 0;
            fasong.Text = "发送：" + writeFlag.ToString();
        }

        #region CorrectPropertiesDuringSerialportOpen
        // 在串口打开过程中可以直接修改串口号、波特率等        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
                SaveProperties();
                try
                {
                        sp.Open();
                }
                catch
                { 连接.Text = "连接"; MessageBox.Show("串口不存在或者被其他应用程序占用！", "提示"); }
                
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
                SaveProperties();
                sp.Open();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
                SaveProperties();
                sp.Open();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
                SaveProperties();
                sp.Open();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp.IsOpen == true)
            {
                sp.Close();
                SaveProperties();
                sp.Open();
            }
        }
        #endregion CorrectPropertiesDuringSerialportOpen

        private void timer1_Tick(object sender, EventArgs e)
        {
            string date = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日";
            label2.Text = date;
            label3.Text = DateTime.Now.ToLongTimeString() ;
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        #endregion Event Handlers

        private void 连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
