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
    public partial class Ztool : Form
    {
        ZigbeeApp myZigBeeApp = null;
        Form2 myForm2;
        Thread myForm2Thread = null;
        private static byte[] readBuf = new byte[1024];
        int readFlag=0,writeFlag=0;
        delegate void TimerDelegate();
        delegate void HandleInterfaceUpdateDelegate();//声明一个委托，此位置是尝试后的最佳位置
        TimerDelegate myTimerPoll;
        //HandleInterfaceUpdateDelegate interfaceUpdateHandle;//定义一个委托实例，此位置是尝试后的最佳位置
        //delegate void HandleSerialPortStatus(object sender, System.EventArgs e);
        //event HandleSerialPortStatus HandleStatus;

        public Ztool()
        {
            InitializeComponent();
            myZigBeeApp = new ZigbeeApp(this,sp);
            myTimerPoll = new TimerDelegate(myZigBeeApp.Poll);
            UpdatePortName();
            comboBox3.SelectedIndex = 9;
            textBox1.Text = "0";//device num
            textBox10.Text = ZigbeeCommon.TestDeviceDelay.ToString();
            textBox11.Text = ZigbeeCommon.SendTimeOut.ToString();
            textBox12.Text = ZigbeeCommon.SendTry.ToString();
            textBox4.Text = "FFFF";
            textBox5.Text = "FF";
            textBox13.Text = "0006";
            textBox6.Text = "01 01 00 00 00 00";
            comboBox2.Items.Clear();
            //interfaceUpdateHandle = new HandleInterfaceUpdateDelegate(UpdateReceiveTextBox);//实例化委托，此位置是尝试后的最佳位置
            //HandleStatus(this.sp.IsOpen,new System.EventArgs());
        }
        private void UpdatePortName()
        {
            if (sp.IsOpen == true)
            {
                MessageBox.Show("请先关闭串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                comboBox2.Items.Clear();
                string[] ArryPort = SerialPort.GetPortNames();
                if (ArryPort != null)
                {
                    for (int i = 0; i < ArryPort.Length; i++)
                    {
                       comboBox2.Items.Add(ArryPort[i]);
                    }
                }
                if (comboBox2.Items.Count > 0)
                    comboBox2.SelectedIndex = 0;
                else
                    comboBox2.Text = "";
            }
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
        public void UpdatePreConfigKey(byte[] key)
        {
            textBox14.Text = ZigbeeCommon.BtoHexStr(key,0,key.Length);
        }
        public void UpdateRecieveTextBox(String str)
        {
            String show = "["+DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("d3")+"]";
            show += str;
            show += "\r\n";
            show += textBox7.Text;
            if (show.Length > 100000)
            {
                show = show.Remove(100000);
            }
            textBox7.Text = show;
        }
        public void UpdateDeviceListTextBox(String str)
        {
            textBox8.Text = str;
        }
        public void UpdateSendTextBox(String str)
        {
            String show = "[" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("d3") + "]";
            show += str;
            show += "\r\n";
            show += textBox9.Text;
            if (show.Length > 100000)
            {
                show = show.Remove(100000);
            }
            textBox9.Text = show;
        }
        public void UptateNetWorkInfoTextBox(byte channel, UInt16 panID)
        {
            textBox2.Text = channel.ToString("X02");
            textBox3.Text = panID.ToString("X04");
        }
        public void UpdateTestCommandInfo(int devNum, UInt16 addr, byte point,UInt16 cID, byte[] data)
        {
            textBox1.Text = devNum.ToString();
            textBox4.Text = addr.ToString("X4");
            textBox5.Text = point.ToString("X2");
            textBox13.Text = cID.ToString("X4");
            textBox6.Text = ZigbeeCommon.BtoHexStr(data, 0, data.Length);
        }
        public int GetSendDelay()
        {
            int Delay = ZigbeeCommon.TestDeviceDelay;
            String str = textBox10.Text;
            if ((str != null) && (str != ""))
            {
                Delay = Convert.ToInt32(str);
            }
            else
                textBox10.Text = Delay.ToString();
            if ((Delay < 50) || (Delay > 10000))
            {
                Delay = ZigbeeCommon.TestDeviceDelay;
                textBox10.Text = Delay.ToString();
                MessageBox.Show("测试延迟时间介于50~10000ms之间！", "提示");
            }
            return Delay;
        }
        public int GetSendTimeout()
        {
            int Delay = ZigbeeCommon.SendTimeOut;
            String str = textBox11.Text;
            if ((str != null) && (str != ""))
            {
                Delay = Convert.ToInt32(str);
            }
            else
                textBox11.Text = Delay.ToString();
            if ((Delay < 50) || (Delay > 10000))
            {
                Delay = ZigbeeCommon.SendTimeOut;
                textBox11.Text = Delay.ToString();
                MessageBox.Show("发送超时时间介于50~10000ms之间！", "提示");
            }
            return Delay;
        }
        public int GetSendTry()
        {
            int cnt = ZigbeeCommon.SendTry;
            String str = textBox12.Text;
            if ((str != null) && (str != ""))
            {
                cnt = Convert.ToInt32(str);
            }
            else
                textBox12.Text = cnt.ToString();
            if ((cnt < 1) || (cnt > 9))
            {
                cnt = ZigbeeCommon.SendTry;
                textBox12.Text = cnt.ToString();
                MessageBox.Show("重发次数介于1~9之间！", "提示");
            }
            return cnt;
        }
        public Boolean CheckUartIsOpen()
        {
            return sp.IsOpen;
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
                    连接.Image = Properties.Resources.Close;
                    ZigbeeApi.Instance.ReqReadNetWorkInfo();
                    //ZigbeeApi.Instance.ReqAssocList();
                }
                else
                {
                    myZigBeeApp.ReqRun(false);
                    button5.Image = Properties.Resources.start;
                    checkBox1.Checked = false;
                    sp.Close();
                    连接.Image = Properties.Resources.Open;
                }
            }
            catch
            { MessageBox.Show("串口不存在或者被其他应用程序占用！","提示"); }
        }

        private void 发送_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp.IsOpen == true && textBox9.Text == "")
                {
                    MessageBox.Show("请输入发送数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                { 
                    sp.Write(textBox9.Text);
                    writeFlag++;
                }
            }
            catch
            {
                MessageBox.Show("串口尚未连接，" + "\n" + "数据发送失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void 清除显示_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox7.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox9.Clear();
        }

        private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int len  = sp.Read(readBuf,0,1024);
                if (len > 0)
                {
                    ZigbeeApi.Input.MessageInput(readBuf,len);
                }
                //readFlag++;
                //Invoke(interfaceUpdateHandle, readFlag.ToString(),temp);
            }
            catch
            { MessageBox.Show("串口接收错误"); }
        }

        private void 复位计数_Click(object sender, EventArgs e)
        {
            readFlag = 0;
            writeFlag = 0;
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
            label3.Text = DateTime.Now.ToLongTimeString();
            if (myZigBeeApp.GeRunLoopFlag() == false)
            {
                myZigBeeApp.ReqRun(false);
                button5.Image = Properties.Resources.start;
            }
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 清除发送区显示_Load(object sender, EventArgs e)
        {
            
        }
        private void 接收区_Enter(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void c_Popup(object sender, PopupEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke(myTimerPoll);
            //myZigBeeApp.Poll();
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            myZigBeeApp.ReqRunLoopFlag(checkBox1.Checked);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
                ZigbeeApi.Instance.ReqPermitJoin(120);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
                ZigbeeApi.Instance.ReqPermitJoin(0x00);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ZigbeeApi.Device.ClearDevice();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ZigbeeApi.Device.ClearCounter();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
            {
                if (myZigBeeApp.GeRunLoopFlag() == false)
                {
                    SendTestCommand();
                }
                else
                {
                    if (myZigBeeApp.GetRunFlag())
                    {
                        myZigBeeApp.ReqRun(false);
                        button5.Image = Properties.Resources.start;
                    }
                    else
                    {
                        myZigBeeApp.ReqRun(true);
                        button5.Image = Properties.Resources.stop;
                    }
                }
            }
        }
        private void SendTestCommand()
        {
            if (textBox4.Text == "")
                textBox4.Text = "FFFF";
            if (textBox5.Text == "")
                textBox5.Text = "FF";
            if (textBox13.Text == "")
                textBox13.Text = "0006";
            if (textBox6.Text == "")
                textBox6.Text = "01 01 00 00 00 00";
            UInt16 addr = (UInt16)Convert.ToInt16(textBox4.Text, 16);
            byte point = Convert.ToByte(textBox5.Text, 16);
            UInt16 cID = (UInt16)Convert.ToInt16(textBox13.Text,16);
            byte[] data = ZigbeeCommon.HexStrtoB(textBox6.Text);
            ZigbeeApi.Instance.ReqSendCommand(addr,point,cID, data);
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ZigbeeApi.Device.ClearDevice();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ZigbeeApi.Device.ClearCounter();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else if (myForm2Thread == null || myForm2Thread.IsAlive == false)
            {
                myForm2 = new Form2();
                myForm2Thread = new Thread(() => Application.Run(myForm2));
                myForm2Thread.Start();
                //this.WindowState = FormWindowState.Minimized;  //还原窗体
            }
        }

        private void Ztool_Deactivate(object sender, EventArgs e)
        {
           //this.TopMost = false;
           // myForm2.TopMost = true;
           // this.Activate();
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            UpdatePortName();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
                ZigbeeApi.Instance.ReqReadNetWorkInfo();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
            {
                if (textBox2.Text == "")
                    textBox2.Text = "0C";
                if (textBox3.Text == "")
                    textBox3.Text = "1234";
                byte ch = Convert.ToByte(textBox2.Text, 16);
                UInt16 panID = (UInt16)Convert.ToInt16(textBox3.Text, 16);
                ZigbeeApi.Instance.ReqWriteNetWorkInfo(ch,panID);
                ZigbeeApi.Instance.ReqResetSystem();
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
                ZigbeeApi.Instance.ReqReadPreConfigKey();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sp.IsOpen == false)
                MessageBox.Show("串口未打开！", "提示");
            else
            {
                if (textBox14.Text != "")
                {
                    byte[] key = ZigbeeCommon.HexStrtoB(textBox14.Text);
                    if(key!=null)
                        ZigbeeApi.Instance.ReqWritePreConfigKey(key);
                }
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string str = System.Environment.CurrentDirectory;
            str += "\\Resources\\readme.txt";
            System.Diagnostics.Process.Start("notepad.exe", str);
            this.WindowState = FormWindowState.Minimized;
        }

        private void 连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
