using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using IniParser;
using IniParser.Model;
namespace DSGWGPages
{
    public class IniHelper
    {
        public static string ReadKey(string filePath, string section, string key)
        {
            if (!File.Exists(filePath)) return "";
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(filePath);
                string value = data[section][key];
                return value;

                //IniFile myIni = new IniFile(filePath);
                //return myIni.ReadString(section, key, "");
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }
        public static void WriteKey(string filePath, string section, string key, string value)
        {
            if (!File.Exists(filePath)) return;
            try
            {
                /* 这里的写法，会覆盖一个新的文件，只包含一个值，慎用！！！
                 FileIniDataParser parser = new FileIniDataParser();
                 IniData data = new IniData();
                 data[section][key] = value;
                 parser.WriteFile(filePath, data);
                 */
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(filePath);
                data[section][key] = value;
                parser.WriteFile(filePath, data);

                //IniFile myIni = new IniFile(filePath);
                //myIni.WriteString(section, key, value);
            }
            catch (Exception e)
            {
               // return e.ToString();
            }
            
        }
    }
}
