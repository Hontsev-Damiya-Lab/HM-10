using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HM_10.ServiceMods
{
    class MeanNetController
    {
        public string saveName = "MeanNet.json";
        public List<MeanNetItem> items;
        public List<MeanNetVerb> verbs;

        public MeanNetController()
        {
            this.items = new List<MeanNetItem>();
            this.verbs = new List<MeanNetVerb>();
            init();
        }

        private void init()
        {
            verbs.Add(new MeanNetVerb("吃"));
        }

        public void saveMeanNet()
        {
            IOController.saveFileAsJson(saveName, this.items);
        }

        public void loadMeanNet()
        {
            this.items = IOController.getFileInfoFromJson(saveName);
        }

        public void addItem(MeanNetItem item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 去除字符串的空白内容
        /// </summary>
        /// <param name="ori"></param>
        /// <returns></returns>
        private static string removeBlanks(string ori)
        {
            string[] blanks = { "\t", " ", "　", "\r" };
            string res = ori;
            foreach (var b in blanks) res = res.Replace(b, string.Empty);
            return res;

        }


        /// <summary>
        /// 将字符串拆分为短句
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> getSentences(string str)
        {
            List<string> thisparag = new List<string>();
            //Regex regex = new Regex("[^。？！；…]+[。？！；…”’』」]*(?=[^。？！；…”’』」]*)");
            Regex regex = new Regex("[^。？！；…，；]+[。？！；…”’』」，；]*(?=[^。？！；…”’』」，；]*)");

            var res = regex.Matches(removeBlanks(str));
            foreach (var ress in res)
            {
                if (ress.ToString().Length >= 2)
                    thisparag.Add(ress.ToString());
            }
            return thisparag;
        }

        public List<MeanNetItem> analysis(string str)
        {
            List<MeanNetItem> res = new List<MeanNetItem>();
            List<string> sentences = getSentences(str);
            foreach (var s in sentences)
            {
                foreach (var v in this.verbs)
                {
                    int index = s.IndexOf(v.name);
                    if (index >= 0)
                    {
                        string sub1 = s.Substring(0, index);
                        string sub2 = s.Substring(index + v.name.Length, s.Length - index - v.name.Length);

                        MeanNetItem newitem = new MeanNetItem();
                        newitem.verb = v;
                        newitem.words = new List<MeanNetWord>();
                        newitem.words.Add(new MeanNetWord(sub1));
                        newitem.words.Add(new MeanNetWord(sub2));

                        res.Add(newitem);

                        break;
                    }
                }
            }

            return res;
        }
    }
}
