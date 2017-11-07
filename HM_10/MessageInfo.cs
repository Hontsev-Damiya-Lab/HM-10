using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace HM_10
{
    public enum MessageType
    {
        info,success,warning
    }

    class MessageInfo
    {
        public string message;
        public MessageType type;
        public int leftTime;
        public RichTextBox textBox;

        public MessageInfo(RichTextBox tb, string m, int l,MessageType t=MessageType.info)
        {
            message = m;
            leftTime = l;
            textBox = tb;
            type = t;
        }
    }
}
