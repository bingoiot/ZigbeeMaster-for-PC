namespace MySerialPorts
{
    partial class 清除发送区显示
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(清除发送区显示));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.连接 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.停止位 = new System.Windows.Forms.Label();
            this.数据位 = new System.Windows.Forms.Label();
            this.校验位 = new System.Windows.Forms.Label();
            this.波特率 = new System.Windows.Forms.Label();
            this.串口号 = new System.Windows.Forms.Label();
            this.接收区设置 = new System.Windows.Forms.GroupBox();
            this.清除显示 = new System.Windows.Forms.LinkLabel();
            this.保存数据 = new System.Windows.Forms.LinkLabel();
            this.发送区设置 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.接收区 = new System.Windows.Forms.GroupBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.发送区 = new System.Windows.Forms.GroupBox();
            this.发送 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.sp = new System.IO.Ports.SerialPort(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.jieshou = new System.Windows.Forms.Label();
            this.复位计数 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fasong = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出程序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.接收区设置.SuspendLayout();
            this.发送区设置.SuspendLayout();
            this.接收区.SuspendLayout();
            this.发送区.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.连接);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Controls.Add(this.comboBox4);
            this.groupBox1.Controls.Add(this.comboBox5);
            this.groupBox1.Controls.Add(this.comboBox6);
            this.groupBox1.Controls.Add(this.停止位);
            this.groupBox1.Controls.Add(this.数据位);
            this.groupBox1.Controls.Add(this.校验位);
            this.groupBox1.Controls.Add(this.波特率);
            this.groupBox1.Controls.Add(this.串口号);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 219);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通讯设置";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MySerialPorts.Properties.Resources._6;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(11, 156);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 57);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // 连接
            // 
            this.连接.Location = new System.Drawing.Point(62, 173);
            this.连接.Name = "连接";
            this.连接.Size = new System.Drawing.Size(75, 23);
            this.连接.TabIndex = 1;
            this.连接.Text = "连接";
            this.连接.UseVisualStyleBackColor = true;
            this.连接.Click += new System.EventHandler(this.连接_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16"});
            this.comboBox2.Location = new System.Drawing.Point(62, 20);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(70, 20);
            this.comboBox2.TabIndex = 2;
            this.comboBox2.Text = "COM1";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.comboBox3.Location = new System.Drawing.Point(62, 48);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(70, 20);
            this.comboBox3.TabIndex = 3;
            this.comboBox3.Text = "9600";
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.comboBox4.Location = new System.Drawing.Point(62, 76);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(70, 20);
            this.comboBox4.TabIndex = 4;
            this.comboBox4.Text = "None";
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "5位",
            "6位",
            "7位",
            "8位"});
            this.comboBox5.Location = new System.Drawing.Point(62, 104);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(70, 20);
            this.comboBox5.TabIndex = 5;
            this.comboBox5.Text = "8位";
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "1位",
            "1.5位",
            "2位"});
            this.comboBox6.Location = new System.Drawing.Point(62, 132);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(70, 20);
            this.comboBox6.TabIndex = 6;
            this.comboBox6.Text = "1位";
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            // 
            // 停止位
            // 
            this.停止位.AutoSize = true;
            this.停止位.Location = new System.Drawing.Point(16, 139);
            this.停止位.Name = "停止位";
            this.停止位.Size = new System.Drawing.Size(41, 12);
            this.停止位.TabIndex = 1;
            this.停止位.Text = "停止位";
            // 
            // 数据位
            // 
            this.数据位.AutoSize = true;
            this.数据位.Location = new System.Drawing.Point(16, 111);
            this.数据位.Name = "数据位";
            this.数据位.Size = new System.Drawing.Size(41, 12);
            this.数据位.TabIndex = 3;
            this.数据位.Text = "数据位";
            // 
            // 校验位
            // 
            this.校验位.AutoSize = true;
            this.校验位.Location = new System.Drawing.Point(16, 83);
            this.校验位.Name = "校验位";
            this.校验位.Size = new System.Drawing.Size(41, 12);
            this.校验位.TabIndex = 2;
            this.校验位.Text = "校验位";
            // 
            // 波特率
            // 
            this.波特率.AutoSize = true;
            this.波特率.Location = new System.Drawing.Point(16, 55);
            this.波特率.Name = "波特率";
            this.波特率.Size = new System.Drawing.Size(41, 12);
            this.波特率.TabIndex = 1;
            this.波特率.Text = "波特率";
            // 
            // 串口号
            // 
            this.串口号.AutoSize = true;
            this.串口号.Location = new System.Drawing.Point(16, 27);
            this.串口号.Name = "串口号";
            this.串口号.Size = new System.Drawing.Size(41, 12);
            this.串口号.TabIndex = 0;
            this.串口号.Text = "串口号";
            // 
            // 接收区设置
            // 
            this.接收区设置.Controls.Add(this.清除显示);
            this.接收区设置.Controls.Add(this.保存数据);
            this.接收区设置.Location = new System.Drawing.Point(12, 246);
            this.接收区设置.Name = "接收区设置";
            this.接收区设置.Size = new System.Drawing.Size(152, 82);
            this.接收区设置.TabIndex = 2;
            this.接收区设置.TabStop = false;
            this.接收区设置.Text = "接收区设置";
            // 
            // 清除显示
            // 
            this.清除显示.AutoSize = true;
            this.清除显示.Location = new System.Drawing.Point(79, 33);
            this.清除显示.Name = "清除显示";
            this.清除显示.Size = new System.Drawing.Size(53, 12);
            this.清除显示.TabIndex = 4;
            this.清除显示.TabStop = true;
            this.清除显示.Text = "清除显示";
            this.清除显示.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.清除显示_LinkClicked);
            // 
            // 保存数据
            // 
            this.保存数据.AutoSize = true;
            this.保存数据.Location = new System.Drawing.Point(16, 33);
            this.保存数据.Name = "保存数据";
            this.保存数据.Size = new System.Drawing.Size(53, 12);
            this.保存数据.TabIndex = 3;
            this.保存数据.TabStop = true;
            this.保存数据.Text = "保存数据";
            // 
            // 发送区设置
            // 
            this.发送区设置.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.发送区设置.Controls.Add(this.linkLabel1);
            this.发送区设置.Location = new System.Drawing.Point(12, 343);
            this.发送区设置.Name = "发送区设置";
            this.发送区设置.Size = new System.Drawing.Size(152, 82);
            this.发送区设置.TabIndex = 3;
            this.发送区设置.TabStop = false;
            this.发送区设置.Text = "发送区设置";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(44, 35);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 12);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "清除显示";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // 接收区
            // 
            this.接收区.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.接收区.Controls.Add(this.richTextBox3);
            this.接收区.Location = new System.Drawing.Point(170, 12);
            this.接收区.Name = "接收区";
            this.接收区.Size = new System.Drawing.Size(308, 336);
            this.接收区.TabIndex = 6;
            this.接收区.TabStop = false;
            this.接收区.Text = "接收区";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox3.Location = new System.Drawing.Point(17, 24);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(273, 288);
            this.richTextBox3.TabIndex = 2;
            this.richTextBox3.Text = "";
            // 
            // 发送区
            // 
            this.发送区.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.发送区.Controls.Add(this.发送);
            this.发送区.Controls.Add(this.richTextBox1);
            this.发送区.Location = new System.Drawing.Point(170, 324);
            this.发送区.Name = "发送区";
            this.发送区.Size = new System.Drawing.Size(308, 101);
            this.发送区.TabIndex = 7;
            this.发送区.TabStop = false;
            this.发送区.Text = "发送区";
            // 
            // 发送
            // 
            this.发送.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.发送.Location = new System.Drawing.Point(223, 43);
            this.发送.Name = "发送";
            this.发送.Size = new System.Drawing.Size(75, 23);
            this.发送.TabIndex = 9;
            this.发送.Text = "发送";
            this.发送.UseVisualStyleBackColor = true;
            this.发送.Click += new System.EventHandler(this.发送_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(17, 23);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(193, 63);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // sp
            // 
            this.sp.BaudRate = 1200;
            this.sp.PortName = "COM5";
            this.sp.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sp_DataReceived);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.jieshou);
            this.panel1.Location = new System.Drawing.Point(278, 435);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 23);
            this.panel1.TabIndex = 3;
            // 
            // jieshou
            // 
            this.jieshou.AutoSize = true;
            this.jieshou.Location = new System.Drawing.Point(29, 3);
            this.jieshou.Name = "jieshou";
            this.jieshou.Size = new System.Drawing.Size(41, 12);
            this.jieshou.TabIndex = 0;
            this.jieshou.Text = "接收：";
            // 
            // 复位计数
            // 
            this.复位计数.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.复位计数.Location = new System.Drawing.Point(393, 435);
            this.复位计数.Name = "复位计数";
            this.复位计数.Size = new System.Drawing.Size(75, 23);
            this.复位计数.TabIndex = 0;
            this.复位计数.Text = "复位计数";
            this.复位计数.UseVisualStyleBackColor = true;
            this.复位计数.Click += new System.EventHandler(this.复位计数_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.fasong);
            this.panel2.Location = new System.Drawing.Point(187, 435);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(99, 23);
            this.panel2.TabIndex = 8;
            // 
            // fasong
            // 
            this.fasong.AutoSize = true;
            this.fasong.Location = new System.Drawing.Point(27, 3);
            this.fasong.Name = "fasong";
            this.fasong.Size = new System.Drawing.Size(41, 12);
            this.fasong.TabIndex = 1;
            this.fasong.Text = "发送：";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(12, 435);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(177, 23);
            this.panel3.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(103, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "串口调试助手";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem,
            this.退出程序ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.连接ToolStripMenuItem.Text = "连接";
            this.连接ToolStripMenuItem.Click += new System.EventHandler(this.连接ToolStripMenuItem_Click);
            // 
            // 退出程序ToolStripMenuItem
            // 
            this.退出程序ToolStripMenuItem.Name = "退出程序ToolStripMenuItem";
            this.退出程序ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出程序ToolStripMenuItem.Text = "退出程序";
            this.退出程序ToolStripMenuItem.Click += new System.EventHandler(this.退出程序ToolStripMenuItem_Click);
            // 
            // 清除发送区显示
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 472);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.复位计数);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.发送区);
            this.Controls.Add(this.接收区);
            this.Controls.Add(this.发送区设置);
            this.Controls.Add(this.接收区设置);
            this.Controls.Add(this.groupBox1);
            this.Name = "清除发送区显示";
            this.Text = "串口调试助手";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.接收区设置.ResumeLayout(false);
            this.接收区设置.PerformLayout();
            this.发送区设置.ResumeLayout(false);
            this.发送区设置.PerformLayout();
            this.接收区.ResumeLayout(false);
            this.发送区.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label 停止位;
        private System.Windows.Forms.Label 数据位;
        private System.Windows.Forms.Label 校验位;
        private System.Windows.Forms.Label 波特率;
        private System.Windows.Forms.Label 串口号;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Button 连接;
        private System.Windows.Forms.GroupBox 接收区设置;
        private System.Windows.Forms.LinkLabel 清除显示;
        private System.Windows.Forms.LinkLabel 保存数据;
        private System.Windows.Forms.GroupBox 发送区设置;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox 接收区;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.GroupBox 发送区;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button 发送;
        private System.IO.Ports.SerialPort sp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button 复位计数;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label jieshou;
        private System.Windows.Forms.Label fasong;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出程序ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
    }
}

