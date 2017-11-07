using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using HM_10.ServiceMods;

namespace HM_10.ServiceMods
{
    public class IOController
    {
        

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        public static List<MeanNetItem> getFileInfoFromJson(string fileName)
        {
            List<MeanNetItem> fi = new List<MeanNetItem>();
            Encoding encoding = Encoding.UTF8;
            using (FileStream file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(file, encoding);
                var fileContent = DeserializeJsonToList<MeanNetItem>(reader.ReadToEnd());
                foreach (var finfo in fileContent)
                {
                    fi.Add(finfo);
                }
                reader.Dispose();
            }
            return fi;
        }

        public static void saveFileAsJson(string fileName, List<MeanNetItem> fileinfo)
        {
            string saveJsonString = JsonConvert.SerializeObject(fileinfo);
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
                writer.Write(saveJsonString);
                writer.Close();
            }
        }



    }
}
