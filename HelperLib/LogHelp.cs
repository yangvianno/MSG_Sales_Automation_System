using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLib
{
    public class LogHelp
    {
        public static void Write(string log)
        {
            try
            {

                if (!Directory.Exists("ErrorLogs"))
                {
                    Directory.CreateDirectory("ErrorLogs");
                }

                string path = "ErrorLogs\\" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(DateTime.Now + " : " + log);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now + " : " + log);
                        sw.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
