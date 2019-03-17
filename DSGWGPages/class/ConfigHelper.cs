using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DSGWGPages
{
    public class ConfigHelper
    {
        public static string rootPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/config/";
     
        public static string configPath = rootPath + "config.json";
        private static object lc = new object();
        public static void CheckDir()
        {
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        public static Config[] GetAllConfigs()
        {
            lock (lc)
            {
                if (!File.Exists(configPath))
                {
                    return null;
                }
                string json = File.ReadAllText(configPath);
                try
                {
                    List<Config> cfgs = JsonConvert.DeserializeObject<List<Config>>(json);
                    if (cfgs != null)
                    {
                        for (int i = cfgs.Count-1; i >= 0; i--)
                        {
                            if (cfgs[i].Key== "password" || cfgs[i].Key == "token")
                            {
                                cfgs.RemoveAt(i);
                            }
                        }
                    }
                    return cfgs.ToArray();
                }
                catch (Exception e)
                {

                }
                return null;
            }
        }
        public static string GetKey(string key)
        {
            lock (lc)
            {
                if (!File.Exists(configPath))
                {
                    return "";
                }
                string json = File.ReadAllText(configPath);
                try
                {
                    Config[] cfgs = JsonConvert.DeserializeObject<Config[]>(json);
                    if (cfgs == null) return "";
                    foreach(Config cfg in cfgs)
                    {
                        if (cfg.Key == key)
                        {
                            return cfg.Value;
                        }
                    }
                }catch(Exception e)
                {

                }
                return "";
            }
        }
        public static void SetKey(string key,string value,string type="string")
        {
            if (key == null || value == null) return;
            if (key == "" || value == "") return;
            lock (lc)
            {
               
                if (!File.Exists(configPath))
                {
                    CheckDir();
                    Config cfg = new Config(key, value, type);
                    List<Config> cfgs = new List<Config>();
                    cfgs.Add(cfg);
                    string tmp = JsonConvert.SerializeObject(cfgs, Formatting.Indented);
                    File.WriteAllText(configPath, tmp);
                    return;
                }
                string json = File.ReadAllText(configPath);
                try
                {
                    List<Config> cfgs = JsonConvert.DeserializeObject<List<Config>>(json);
                    if (cfgs == null)
                    {
                        Config cfg = new Config(key, value, type);
                        List<Config> cfglist = new List<Config>();
                        cfglist.Add(cfg);
                        string tmp = JsonConvert.SerializeObject(cfglist, Formatting.Indented);
                        File.WriteAllText(configPath, tmp);
                        return;
                    }
                    bool needAdd = true;
                    foreach (Config cfg in cfgs)
                    {
                        if (cfg.Key == key)
                        {
                            cfg.Value = value;
                            needAdd = false;
                            break;
                        }
                    }
                    if (needAdd)
                    {
                        Config cfg = new Config(key, value, type);
                        cfgs.Add(cfg);
                    }
                    string str = JsonConvert.SerializeObject(cfgs, Formatting.Indented);
                    File.WriteAllText(configPath, str);
                }
                catch (Exception e)
                {

                }
            }
        }
        public static string DeleteKey(string key)
        {
            lock (lc)
            {
                if (!File.Exists(configPath))
                {
                    return "删除失败，配置文件不存在";
                }
                string json = File.ReadAllText(configPath);
                try
                {
                    List<Config> cfgs = JsonConvert.DeserializeObject<List<Config>>(json);
                    if (cfgs != null)
                    {
                       
                        for (int i = cfgs.Count - 1; i >= 0; i--)
                        {
                            if (cfgs[i].Key == key)
                            {
                                cfgs.RemoveAt(i);
                                string str = JsonConvert.SerializeObject(cfgs, Formatting.Indented);
                                File.WriteAllText(configPath, str);
                                return "success";
                            }
                        }
                        return "删除失败，没有该配置";
                    }
                    else
                    {
                        return "删除失败，配置文件无效";
                    }               
                }
                catch (Exception e)
                {
                    return "删除失败，"+e.Message;

                }
            }
        }

    }
}
