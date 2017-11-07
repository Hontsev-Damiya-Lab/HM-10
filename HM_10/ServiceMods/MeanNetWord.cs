using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM_10.ServiceMods
{
    public class MeanNetWord
    {
        public string name;
        public List<string> prenames = new List<string>();

        public MeanNetWord(string name, List<string> prenames=null)
        {
            this.name = name;
            if (prenames == null)
                this.prenames = new List<string>();
            else
                this.prenames = prenames;
        }

        public string toString()
        {
            string res = "";
            foreach (var a in prenames)
            {
                res += string.Format("{0} ", a);
            }
            res += string.Format("[{0}]", name);
            return res;
        }
    }
}
