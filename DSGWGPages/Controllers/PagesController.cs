using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DSGWGPages.Controllers
{
    public class PagesController : Controller
    {
        //网络状态
        public IActionResult NetworkStatus()
        {
            return View();
        }
        //设备硬件状态
        public IActionResult DevHandwareStatus()
        {
            return View();
        }
        //参数设置
        public IActionResult ProgramConfig()
        {
            return View();
        }
       
        //挂载设备
        public IActionResult MountedDevice()
        {
            return View();
        }
        //WiFi设备
        public IActionResult WiFiStatus()
        {
            return View();
        }
        //板载4G模块
        public IActionResult Onboard4GModel()
        {
            return View();
        }
        //板载4G路由器
        public IActionResult Onboard4GRouter()
        {
            return View();
        }
        //1GHz传感器
        public IActionResult A_1GHzSensor()
        {
            return View();
        }
        //1.7GHz传感器
        public IActionResult A_1_7GHzSensor()
        {
            return View();
        }
        //账号设置
        public IActionResult AccountSet()
        {
            return View();
        }
        //系统工具
        public IActionResult SysTools()
        {
            ViewBag.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }
        //系统日志
        public IActionResult SysLogs()
        {
            return View();
        }

    }
}