using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM_10.ServiceMods
{
    public  class MeanNetItem
    {
        public string creater;
        //public DateTime createDate;   
        public MeanNetVerb verb;
        public List<MeanNetWord> words = new List<MeanNetWord>();

        public string toString()
        {
            string res="";
            res+=string.Format("<{0}>",verb.name);
            foreach (var w in words) res += string.Format("({0})", w.toString());
            return res;
        }
    }
}
