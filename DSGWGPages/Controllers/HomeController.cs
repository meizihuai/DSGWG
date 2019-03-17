using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace DSGWGPages.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index(string name)
        {
            ViewBag.loginName = "null";
            if (name != null)
            {
                string rootPath = _hostingEnvironment.WebRootPath;
                //  string path = Path.Combine(rootPath, "1.txt");
                string path = rootPath + "/1.txt";
                System.IO.File.WriteAllText(path, name);
                ViewBag.path = path;
                if (name == "login")
                {

                    return RedirectToAction("Login", "Home");
                    ViewBag.isLogin = "已登陆";
                }
                else if (name == "unlogin")
                {
                    ViewBag.isLogin = "已注销";
                }
                else
                {
                    ViewBag.isLogin = "未知";
                }
            }
           
            return View();
        }
       
        public async Task<IActionResult> Login(string username)
        {
            string value = "";
            HttpContext.Request.Cookies.TryGetValue("token", out value);
            if (username == null)
            {
                ViewBag.islogin = (value == "" )? "没有登陆" : value;
                return View();
            }
            ViewBag.islogin = (value == "") ? "没有登陆" : value;
            HttpContext.Response.Cookies.Append("token",username);
           
            return View();
        }
        public async Task<IActionResult> Logout()
        {
         
            HttpContext.Response.Cookies.Delete("token");          
            return RedirectToAction("Login", "Home");
        }

    }
}