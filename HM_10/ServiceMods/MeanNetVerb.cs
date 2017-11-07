using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM_10.ServiceMods
{
    public enum CaseType{
        none,thing,sentence
    }

    public class MeanNetVerb
    {
        public string name;
        public CaseType case1;
        public CaseType case2;
        public CaseType case3;

        public MeanNetVerb(string name="", CaseType ct1 = CaseType.thing, CaseType ct2 = CaseType.thing, CaseType ct3 = CaseType.none)
        {
            this.name = name;
            case1 = ct1;
            case2 = ct2;
            case3 = ct3;
        }
    }
}
