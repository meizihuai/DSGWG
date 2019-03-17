using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;

namespace DSGWGPages.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        public ActionResult<NormalResponse> GetRunTimes()
        {
            WorkHelper.runTimes++;
            string result = WorkHelper.runTimes+"";
            NormalResponse np = new NormalResponse(true, result);
            return np;
        }
        public ActionResult<NormalResponse> GetPath()
        {
            return new NormalResponse(true, ConfigHelper.rootPath);
        }
        public ActionResult<NormalResponse> SetKey(string key,string value)
        {
            ConfigHelper.SetKey(key, value);
            return new NormalResponse(true, "success");
        }
        public ActionResult<NormalResponse> GetKey(string key)
        {
            string value= ConfigHelper.GetKey(key);
            return new NormalResponse(true, "",value);
        }
        public ActionResult<NormalResponse> GetGateWayStatusInfo()
        {
            return new NormalResponse(true, "", DeviceStatus.gateWayStatusInfo);
        }
        public ActionResult<NormalResponse> GetConfigs()
        {
            Config[] cfgs= ConfigHelper.GetAllConfigs();
            if (cfgs == null)
            {
                return new NormalResponse(false, "没有任何参数记录");
            }
            else
            {
                return new NormalResponse(true, "",cfgs);
            }           
        }
        public ActionResult<NormalResponse> DeleteKey(string key)
        {
            string result = ConfigHelper.DeleteKey(key);
            bool flag = (result == "success" ? true : false);         
            return new NormalResponse(true, result);
        }
        public ActionResult<NormalResponse> GetProgramConfig()
        {
            Config[] cfgs = ConfigHelper.GetAllConfigs();
            if (cfgs == null)
            {
                return new NormalResponse(false, "没有任何参数记录");
            }
            List<ProgramConfig> ps = null;
            foreach (Config tmp in cfgs)
            {
                if (tmp.Key == "ProgramConfigs")
                {
                    try
                    {
                        ps = JsonConvert.DeserializeObject<List<ProgramConfig>>(tmp.Value);
                        break;
                    }
                    catch(Exception e)
                    {

                    }                  
                }
            }
            if (ps == null)
            {
                return new NormalResponse(false, "没有任何程序参数记录");
            }
            foreach(var p in ps)
            {
                if (p.fileExten.ToLower() == "ini")
                {
                    p.value = IniHelper.ReadKey(p.filePath, p.projectName, p.key);
                }
            }
            return new NormalResponse(true, "", ps);
        }
        [HttpPost]
        public ActionResult<NormalResponse> SetProgramConfig([FromBody] ProgramConfig ps)
        {
            try
            {
               // ProgramConfig ps = JsonConvert.DeserializeObject<ProgramConfig>(json);
                if(ps==null) return new NormalResponse(false, "设置项格式非法！");
                if(ps.name==null) return new NormalResponse(false, "名称不可为空！",ps);
                string value = ConfigHelper.GetKey("ProgramConfigs");
                List<ProgramConfig> list = JsonConvert.DeserializeObject<List<ProgramConfig>>(value);
                if (ps.fileExten.ToLower() == "ini")
                {
                    IniHelper.WriteKey(ps.filePath, ps.projectName, ps.key, ps.value);
                }
                bool isfind = false;
                for(int i = 0; i < list.Count; i++)
                {
                    if (list[i].id == ps.id )
                    {
                        list[i] = ps;
                        isfind = true;                 
                        break;
                    }
                }
                if (!isfind)
                {
                    int maxIndex = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].id > maxIndex) maxIndex = list[i].id;
                    }
                    maxIndex++;
                    ps.id = maxIndex;
                    list.Add(ps);
                }
                ConfigHelper.SetKey("ProgramConfigs", JsonConvert.SerializeObject(list),"json");
                return new NormalResponse(true, "设置成功");
            }
            catch(Exception e)
            {
                return new NormalResponse(false, "设置失败！", e.ToString());
            }         
        }
        public ActionResult<NormalResponse> DeleteProgramConfig(int id)
        {
            try
            {
                string value = ConfigHelper.GetKey("ProgramConfigs");
                List<ProgramConfig> list = JsonConvert.DeserializeObject<List<ProgramConfig>>(value);
                bool isfind = false;
                for (int i = list.Count-1; i>=0; i--)
                {
                    if (list[i].id == id)
                    {
                        list.RemoveAt(i);
                        isfind = true;
                    }
                }
                if (isfind)
                {
                    ConfigHelper.SetKey("ProgramConfigs", JsonConvert.SerializeObject(list), "json");                  
                }
                return new NormalResponse(true, "操作成功");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, "设置失败！", e.ToString());
            }
        }
        public ActionResult<NormalResponse> Reboot()
        {
            try
            {
                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    Runsh("reboot.sh");
                }).Start();

                return new NormalResponse(true,"操作成功");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, "重启失败！", e.ToString());
            }
        }
        private string RunshOrder(string order)
        {
            var psi = new ProcessStartInfo("sudo "+ order, "")
            {
                RedirectStandardOutput = true
            };    //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return "操作失败";
            }
            else
            {
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }         
            }
            return "success";
        }
        private string Runsh(string fileName)
        {
            string rootpath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/sh/";
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }
            //要执行的shell的名字
            String FileName = rootpath + "/" + fileName;
            if (!System.IO.File.Exists(FileName))
            {
                return fileName+"不存在";
            }
            //实例化一个process类
            Process process = new Process();
            
            process.StartInfo.FileName = FileName;
            //process.StartInfo.Arguments = "1,2,3";         
            //获取或设置是否在新窗口中启动该进程
            process.StartInfo.CreateNoWindow = true;
            //该值指示不能启动进程时是否向用户显示错误的对话框
            process.StartInfo.ErrorDialog = false;
            //关闭shell的使用
            process.StartInfo.UseShellExecute = false;
            process.Start();
            return "success";
          // process.WaitForExit();
            //  process.Close();
        }
    }
}
