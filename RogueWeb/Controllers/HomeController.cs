using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rogueCore.rogueUIV3.web;
using RogueWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RogueWeb.Controllers
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
            return View();
        }
        [HttpGet]
        public JsonResult GetSection(string urlPath)
        {
            string myJson = new UIWebSection("7443").AsJson;
            return Json(myJson);
        }
    }
}
