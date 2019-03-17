using System;
using System.IO;

namespace DSGWGPages
{
    public class LogHelper
    {
        public static string rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/Logs/";
        private static object lc = new object();
        public static void CheckDir()
        {
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        public static void Log(string tagName,string content)
        {
            lock (lc)
            {
                try
                {
                    CheckDir();
                    DateTime now = DateTime.Now;
                    string str = now.ToString("[HH:mm:ss] ")+"<"+tagName+"> "+content+"\r\n";
                    string filePath = rootPath + now.ToString("yyyy_MM_dd") + ".txt";
                    File.AppendAllText(filePath,str );
                }
                catch(Exception e)
                {

                }
            }
        }
    }
}
