using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM_10.ServiceMods
{
    public enum UserCommand{
        chat,close
    }

    public class CommandAnalysis
    {
        static string[] ExitCommand = { "exit", "再见", "晚安", "退出" };

        private static bool BelongTo(string[] key, string target)
        {
            bool success = false;
            foreach (var k in key)
            {
                if (target.StartsWith(k))
                {
                    success = true;
                    break;
                }
            }
            return success;
        }

        public static UserCommand getUserCommandType(string input)
        {
            if (BelongTo(ExitCommand, input)) return UserCommand.close;
            else return UserCommand.chat;
        }
    }
}
