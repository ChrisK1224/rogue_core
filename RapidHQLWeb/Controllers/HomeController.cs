using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidHQLWeb.Models;
using rogueCore.rogueUIV3.web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RapidHQLWeb.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            MyPage myPage = new MyPage();
            myPage.pageID = "YESS";
            //HttpContext.Session.SetString("pageID", "7550");
            return View();
        }
        [HttpPost]
        public JsonResult ClickEvent(string clickInfo, string frmData)
        {
            frmData = frmData.Substring(1, frmData.Length - 2);            
            Dictionary<string,string> pageContent = frmData.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Split('=')).ToDictionary(split => split[0], split => split[1]);
            //var page = UIWebPage.InstancePage(int.Parse(pageContent["ROGUEPAGEID"]), clickInfo, pageContent);
            var page = UIWebPage.InstancePage( clickInfo, pageContent);
            return Json(page.JsonSectionCollection());
        }
        [HttpGet]
        public JsonResult GetSection(string sectionNM)
        {
             UIWebPage myPage = new UIWebPage(7550); 
            //UIWebPage myPage = new UIWebPage(8455);
            var jsn = Json(myPage.JsonSectionCollection());
            return jsn;
        }
        [Route("/MapLargeDownload")]
        [HttpGet]
        public ActionResult MapLargeDownload(List<string> fileName)
        {
            return View("MapLargeDownload");
        }
        //[HttpGet]
        //public JsonResult GeContentTop(string urlPath)
        //{
        //    string myJson = new UIWebSection("7443").AsJson;
        //    return Json(myJson);
        //}
    }
}
