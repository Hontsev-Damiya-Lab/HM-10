using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HM_10.ServiceMods;

namespace HM_10
{
    public partial class MainForm : Form
    {
        private Point nowp;
        private bool isMove=false;
        private bool isRun = false;
        private delegate void sendMessageInfoDelegate(MessageInfo info);
        public delegate void sendStringDelegate(string str);
        public delegate void sendMessageDelegate(string str, MessageType type);
        public sendMessageDelegate printEvent;
        public sendStringDelegate closeEvent;
        private sendMessageInfoDelegate removeMessageEvent;
        private List<MessageInfo> messages;
        private InputController ic;

        private ControlForm cf;


        #region 注册热键的api
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        public MainForm()
        {
            InitializeComponent();

            cf = new ControlForm();
            cf.StartPosition = this.StartPosition;
            //cf.Location = this.Location;
            cf.Show();

            skinAnimatorImg1.SendToBack();
            removeMessageEvent = new sendMessageInfoDelegate(removeMessage);
            printEvent = new sendMessageDelegate(print);
            closeEvent = new sendStringDelegate(close);
            messages = new List<MessageInfo>();
            isRun = true;

            RegisterHotKey(this.Handle, 225, 0, Keys.F1);

            new Thread(workLoopMessage).Start();
        }

        /// <summary>
        /// 在界面输出一条信息
        /// </summary>
        /// <param name="str"></param>
        private void print(string str,MessageType type=MessageType.info)
        {
            newMessage(str, type);
        }

        private void waitAndExit()
        {
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private void close(string str)
        {
            if(str.Length>0)
                newMessage(str);
            notifyIcon1.Dispose();
            new Thread(waitAndExit).Start();
        }

        #region 信息管理

        /// <summary>
        /// 创建一条信息并显示出来
        /// </summary>
        /// <param name="messageString"></param>
        /// <param name="type">消息类型。用于修饰窗口样式。默认为MessageType.info</param>
        /// <param name="time">存留时间，单位为秒。使用默认时间则设为0即可</param>
        private void newMessage(string messageString, MessageType type=MessageType.info,int time=0)
        {
            RichTextBox tb = new RichTextBox();
            tb.BorderStyle = BorderStyle.None;
            tb.Text = messageString;
            tb.ReadOnly = true;
            tb.Multiline = true;
            setTextBoxSize(tb, messageString);
            setTextBoxLocation(tb);
            tb.BringToFront();
            tb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.messageBox_MouseUp);
            switch (type)
            {
                case MessageType.info:
                    tb.BackColor = Color.FromArgb(205, 216, 231);
                    break;
                case MessageType.success:
                    tb.BackColor = Color.FromArgb(202, 240, 105);
                    break;
                case MessageType.warning:
                    tb.BackColor = Color.FromArgb(254, 115, 86);
                    break;
                default:
                    break;
            }
            if (time == 0)
            {
                //根据信息长度自动设定消失时间
                time = Math.Max(messageString.Length, 60) / 20;
            }

            messages.Add(new MessageInfo(tb, messageString, time, type));
            cf.Controls.Add(tb);
        }

        private void messageBox_MouseUp(object sender, MouseEventArgs e)
        {
            print("!");
            if (e.Button == MouseButtons.Middle)
            {
                removeMessage((RichTextBox)sender);
            }
        }

        /// <summary>
        /// 根据信息长度，设置信息窗口的大小
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="messageString"></param>
        private void setTextBoxSize(RichTextBox tb, string messageString)
        {
            int length = Encoding.Default.GetBytes(messageString).Length;
            if (length <= 20) { tb.Width = length * 12; tb.Height = 30; }
            else if (length <= 100) { tb.Width = 200; tb.Height = (length + 19) * 30 / 20; }
            else { tb.Width = 200; tb.Height = 150; tb.ScrollBars = RichTextBoxScrollBars.Both; }
        }

        /// <summary>
        /// 根据已有窗口位置，设置新信息窗口显示位置。
        /// </summary>
        /// <param name="tb"></param>
        private void setTextBoxLocation(RichTextBox tb)
        {
            Random rd = new Random(System.DateTime.Now.Millisecond);

            int x = rd.Next(1, 100) <= 50 ? Math.Max(0, cf.Width / 2 - tb.Width - 30) : cf.Width / 2 + Math.Min(30, cf.Width / 2 - tb.Width);
            x += x <= cf.Width / 2 ? rd.Next(-150, -10) : rd.Next(10, 150);

            int beginy = 0;
            int yheight = 0;
            bool[] haveinfo = new bool[cf.Height];
            for (int i = 0; i < haveinfo.Length; i++) haveinfo[i] = false;
            foreach (MessageInfo info in messages)
            {
                if ((x <= cf.Width / 2 && info.textBox.Location.X <= cf.Width / 2) ||
                    (x > cf.Width / 2 && info.textBox.Location.X > cf.Width / 2))
                {
                    for (int i = Math.Min(haveinfo.Length, info.textBox.Location.Y); i < Math.Min(haveinfo.Length, info.textBox.Location.Y + info.textBox.Height); i++) haveinfo[i] = true;
                }
            }
            int tmpy = 0;
            bool begin = false;
            for (int i = 0; i < haveinfo.Length; i++)
            {
                if (haveinfo[i] == false)
                {
                    if (!begin) { begin = true; tmpy = i; }
                }
                else
                {
                    if (!begin) continue;
                    begin = false;
                    int thisHeight = i - tmpy;
                    if (thisHeight > yheight)
                    {
                        beginy = tmpy;
                        yheight = thisHeight;
                    }
                }
            }
            if (begin == true)
            {
                int thisHeight = haveinfo.Length - tmpy;
                if (thisHeight > yheight)
                {
                    beginy = tmpy;
                    yheight = thisHeight;
                }
            }
            yheight = Math.Max(yheight - tb.Height, 0);
            int y = rd.Next(beginy, beginy + yheight);

            //tb.Text = "by:" + beginy + ",ey:" + (yheight );

            tb.Location = new Point(x, y);
        }

        /// <summary>
        /// 删除一条显示着的信息
        /// </summary>
        /// <param name="message"></param>
        private void removeMessage(MessageInfo message)
        {
            messages.Remove(message);
            if (cf == null || cf.IsDisposed) return;
            cf.Controls.Remove(message.textBox);
        }

        /// <summary>
        /// 删除某个textbox对应的信息
        /// </summary>
        /// <param name="tb"></param>
        private void removeMessage(RichTextBox tb)
        {
            foreach (MessageInfo info in messages)
            {
                if (info.textBox == tb)
                {
                    messages.Remove(info);
                    tb.Dispose();
                    break;
                }
            }
        }

        /// <summary>
        /// 线程工作函数，每秒检查是否需要释放message窗口
        /// </summary>
        private void workLoopMessage()
        {
            while (isRun)
            {
                Thread.Sleep(1000);
                List<MessageInfo> removeMessages = new List<MessageInfo>();
                foreach (MessageInfo info in messages)
                {
                    if (info.leftTime <= 0)
                    {
                        removeMessages.Add(info);
                    }
                    else
                    {
                        info.leftTime--;
                    }
                }
                foreach (MessageInfo info in removeMessages)
                {
                    Invoke(removeMessageEvent, (object)info);
                }
                //Invoke(printEvent, (object)("23333333333"));
            }
        }
        #endregion

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Dispose();
            messages.Clear();
            isRun = false;
            cf.Dispose();
            this.Dispose();
        }

        #region 鼠标拖动界面

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMove = true;
                nowp = MousePosition;
            }
            else
            {
                isMove = false;
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                Location = new Point(Location.X + MousePosition.X - nowp.X, Location.Y + MousePosition.Y - nowp.Y);
                cf.Location = new Point(Location.X - cf.Width / 2 + 120, Location.Y - cf.Height / 2 + 100);
                nowp = MousePosition;
            }
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            ic = new InputController(this);
            cf.Location = new Point(Location.X - cf.Width / 2 + 120, Location.Y - cf.Height / 2 + 100);
            newMessage("初始化完毕", MessageType.success);
            MNForm f = new MNForm();
            f.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ic.input(textBox1.Text);
                textBox1.Text = "";
            }
        }

        /// <summary>
        /// 去除点击回车的噪音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        #region 响应热键
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:    //这个是window消息定义的注册的热键消息
                    if (m.WParam.ToString().Equals("225"))  //提高音量热键
                    {
                        print("F1！");
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            //注消热键(句柄,热键ID)
            UnregisterHotKey(this.Handle, 225);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
