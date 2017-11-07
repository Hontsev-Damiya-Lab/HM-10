using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using System.Data;
using System.Data.OleDb;



namespace HM_10.ServiceMods
{


    class InputController
    {
        private string inputString;
        private string outputString;
        private MainForm mainForm;
        private DB db;

        public InputController(MainForm outputForm)
        {
            mainForm = outputForm;
            db = new DB();
        }

        private void dealChat()
        {
            if (inputString.StartsWith("-mn"))
            {
                MNForm f = new MNForm();
                f.Show();
            }
        }

        public void workSwitch()
        {
            UserCommand comm= CommandAnalysis.getUserCommandType(this.inputString);
            outputString = string.Format("you:{0}", inputString);
            output();
            switch (comm)
            {
                case UserCommand.chat:
                    dealChat();
                    break;
                case UserCommand.close:
                    close();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 输入系统一个语句，会根据语句做出对应反映。若无对应命令设置，默认输出原来语句。
        /// </summary>
        /// <param name="str"></param>
        public void input(string str)
        {
            inputString = str;
            outputString = "";
            if (str.Trim().Length > 0)
            {
                workSwitch();
                //output();
            }
            //else if (str == "-识别")
            //{
            //    OCRController ocrc = new OCRController();
            //    outputString = ocrc.getString(Image.FromFile(@"E:\图片\f813a20a19d8bc3ef0118579838ba61ea9d345fe.jpg"));
            //}
            //else if (str.Length > 1 && str[0] == '-')
            //{
            //    new Thread(workUpdateItemData).Start();
            //}
            //else
            //{
            //    new Thread(workGetItemData).Start();
            //}
            //output();
        }

        public void close()
        {
            if (DateTime.Now.Hour >= 21 || DateTime.Now.Hour <= 4)
            {
                outputString = "再见。晚安~";
            }
            else
            {
                outputString = "再见哟";
            }

            mainForm.Invoke(mainForm.closeEvent, (object)(outputString));
        }

        public void output()
        {
            mainForm.Invoke(mainForm.printEvent, (object)(outputString),(object)(MessageType.info));
        }
    }
}
