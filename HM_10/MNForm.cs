using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HM_10.ServiceMods;

namespace HM_10
{
    public partial class MNForm : Form
    {
        private delegate void sendStringDelegate(string str);
        MeanNetController mc;
        public MNForm()
        {
            InitializeComponent();

            mc = new MeanNetController();
        }

        private void print(string str)
        {
            if (textBox2.InvokeRequired)
            {
                sendStringDelegate printevent = new sendStringDelegate(print);
                Invoke(printevent, (object)str);
            }
            else
            {
                this.textBox2.AppendText(str);
            }
        }



        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void MNForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mc.saveMeanNet();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            textBox1.Text = "";
            var res = mc.analysis(str);
            foreach (var a in res)
            {
                print(a.toString());
            }
        }

        private void additToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string verb = additToolStripMenuItem.Text.Replace("添加谓语：", "");
            if (!string.IsNullOrEmpty(verb))
            {

            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            string str = textBox1.SelectedText;
            if (!string.IsNullOrEmpty(str))
            {
                additToolStripMenuItem.Text = string.Format("添加谓语：{0}", str);
                contextMenuStrip1.Show(MousePosition);
            }
        }
    }
}
