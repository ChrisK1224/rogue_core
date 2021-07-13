//using rogueCore.rogueUIV3.web;
using rogueCore.rogueUIV3.web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueWebUI2.Models
{
    public class PageViewModel
    {
        UIWebPage uiWebPage = new UIWebPage(7550);
        public string jsonSection { get { return uiWebPage.SectionJson("@SIDEMENU"); } }
        //public void ChangePage(int pageID) { uiWebPage = new UIWebPage(pageID); }
        public string AddFilePath { get; set; }
    }
}
