using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DSGWGPages
{
    public class WorkHelper
    {
        public static int runTimes;
        public static void Run()
        {
            LogHelper.Log("MainProcess", "================MainProcess Start================");
            RunSync();
            Thread thread = new Thread(RunWork);
            thread.IsBackground = true;
            thread.Start();
        }
        private static void RunSync()
        {
            //不耗时线程，必须在网页启动前运行的程序   
            if (ConfigHelper.GetKey("password") == "") ConfigHelper.SetKey("password", "12345678");
            IniSh();
            IniGateWayStatusHelper();
        }
        private static void RunWork()
        {
            //耗时线程，一切asp.net主程序之外的线程，在此启动

        }
        private static void IniSh()
        {
            string shPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "/sh/";
            if (!Directory.Exists(shPath))
            {
                Directory.CreateDirectory(shPath);
            }
            String fileName = shPath + "/reboot.sh";
            if (!System.IO.File.Exists(fileName))
            {
                StreamWriter sw = new StreamWriter(fileName, false);
                sw.WriteLine("#! /bin/bash");
                sw.WriteLine("sudo reboot");
                sw.Close();
            }
        }
        private static void IniGateWayStatusHelper()
        {
            if (ConfigHelper.GetKey("NetTransIP") == "") ConfigHelper.SetKey("NetTransIP", "127.0.0.1");
            if (ConfigHelper.GetKey("NetTransPort") == "") ConfigHelper.SetKey("NetTransPort", "3206");
            if (ConfigHelper.GetKey("NetTransLocalPort") == "") ConfigHelper.SetKey("NetTransLocalPort", "4516");
            if (ConfigHelper.GetKey("ProgramConfigs") == "") ConfigHelper.SetKey("ProgramConfigs", GetDefaultProgramConfigs(), "json");

            string NetTransIP = ConfigHelper.GetKey("NetTransIP");
            string NetTransPort = ConfigHelper.GetKey("NetTransPort");
            string NetTransLocalPort = ConfigHelper.GetKey("NetTransLocalPort");

            GateWayStatusHelper gateWayStatusHelper = new GateWayStatusHelper(NetTransIP, int.Parse(NetTransPort), int.Parse(NetTransLocalPort));
            gateWayStatusHelper.OnGateWayStatusInfo += GateWayStatusHelper_OnGateWayStatusInfo;
            gateWayStatusHelper.StartWork();
            LogHelper.Log("GateWayStatusHelper", "gateWayStatusHelper.StartWork success!");
        }
        private static string GetDefaultProgramConfigs()
        {
            List<ProgramConfig> programConfigs = new List<ProgramConfig>();
            programConfigs.Add(new ProgramConfig(1, "服务器IP", "dsg2to1", "/usr/dsg2to1/config.ini", "ini", "SERVER", "IP", "123.207.31.37", "二合一设备"));
            programConfigs.Add(new ProgramConfig(2, "服务器端口", "dsg2to1", "/usr/dsg2to1/config.ini", "ini", "SERVER", "PORT", "3204", "二合一设备"));
            programConfigs.Add(new ProgramConfig(3, "设备ID", "dsg2to1", "/usr/dsg2to1/config.ini", "ini", "DEVICE_INFO", "ID", "DSG_DH0024", "二合一设备"));
            programConfigs.Add(new ProgramConfig(4, "转发数据目标服务器IP", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "server", "ip", "123.207.31.37", "网关设备"));
            programConfigs.Add(new ProgramConfig(5, "转发数据目标服务器端口", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "server", "port", "3204", "网关设备"));
            programConfigs.Add(new ProgramConfig(6, "转发设备端口", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "device", "port", "3200", "网关设备"));
            programConfigs.Add(new ProgramConfig(7, "网关设备ID", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "gateway", "id", "DSG-GW0100", "网关设备"));
            programConfigs.Add(new ProgramConfig(8, "网关服务器IP", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "gateway", "ip", "123.207.31.37", "网关设备"));
            programConfigs.Add(new ProgramConfig(9, "网关服务器端口", "NetTrans", "/usr/NetTranslate/config.ini", "ini", "gateway", "port", "3205", "网关设备"));
            programConfigs.Add(new ProgramConfig(10, "服务器IP", "em100", "/usr/EM100/config.ini", "ini", "SERVER", "IP", "123.207.31.37", "RF01"));
            programConfigs.Add(new ProgramConfig(11, "服务器端口", "em100", "/usr/EM100/config.ini", "ini", "SERVER", "PORT", "3204", "RF01"));
            programConfigs.Add(new ProgramConfig(12, "设备ID", "em100", "/usr/EM100/config.ini", "ini", "DEVICE_INFO", "ID", "DSG_ONE024", "RF01"));
            programConfigs.Add(new ProgramConfig(13, "服务器IP", "smphgb", "/usr/hgb/config.ini", "ini", "SERVER", "IP", "123.207.31.37", "黑广播"));
            programConfigs.Add(new ProgramConfig(14, "服务器端口", "smphgb", "/usr/hgb/config.ini", "ini", "SERVER", "PORT", "3204", "黑广播"));
            programConfigs.Add(new ProgramConfig(15, "设备ID", "smphgb", "/usr/hgb/config.ini", "ini", "DEVICE_INFO", "ID", "DSG-HGB001", "黑广播"));
            return JsonConvert.SerializeObject(programConfigs);
        }
        private static void GateWayStatusHelper_OnGateWayStatusInfo(GateWayStatusHelper.GateWayStatusInfo gateWayStatusInfo)
        {
            LogHelper.Log("GateWayStatusHelper", "OnGateWayStatusInfo");
            DeviceStatus.gateWayStatusInfo = gateWayStatusInfo;
        }
    }
}
