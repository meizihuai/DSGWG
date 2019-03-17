using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DSGWGPages.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "DSG监测网关";
            string token = "";
            Request.Cookies.TryGetValue("token", out token);
            if (token == null)
            {
                return RedirectToAction("Login", "Default");
            }
            if (token == "")
            {
                return RedirectToAction("Login", "Default");
            }
            if (token != ConfigHelper.GetKey("token"))
            {
                return RedirectToAction("Login", "Default");
            }
            return View();
        }
        public IActionResult Login()
        {
            Response.Cookies.Delete("token");
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm]string password)
        {
            bool isLoginSuccess = false;
            if (password != null)
            {
                string rightPassword = ConfigHelper.GetKey("password");
                if (rightPassword == null) rightPassword = "12345678";
                if (password == rightPassword)
                {
                    isLoginSuccess = true;
                }
                else
                {
                    ViewBag.loginMsg = "密码错误";
                }
            }
            else
            {
                ViewBag.loginMsg = "请输入密码";
            }
            if (isLoginSuccess)
            {
                string token = ConfigHelper.GetKey("token");
                if (token == "")
                {
                    token = System.Guid.NewGuid().ToString("N");
                    ConfigHelper.SetKey("token", token);
                }
               
                Response.Cookies.Append("token", token);
                return RedirectToAction("index", "Default");
            }
            else
            {
                Response.Cookies.Delete("token");
                return View();
            }                            
        }
    }
}