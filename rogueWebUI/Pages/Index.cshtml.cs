using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.IO;
using rogueCore.rogueUIV3.web;
using rogueCore.hqlSyntaxV3.segments;
//using rogueCore.UI.WebUI;
//using rogueCore.rogueUI.web;

namespace rogueWebUI.Pages
{
    public class IndexModel : PageModel
    {
        public static UIWebPage pageBuilder;
        private readonly ILogger<IndexModel> _logger;
        // public static HtmlString MyHtml { get { return new HtmlString("<ul id=\"myUL\" ><li ><span class=\"caret\"><img  src=\"~/pics/bundle.png\" style=\"height: 25px; width: 25px; \"></img><label >Root</label></span><ul class=\"nested\"></ul></li></ul>"); }  }
        //public static HtmlString MyHtml { get { return new HtmlString(tempHTML); } }
        public static string tempHTML2()
        {
            string blah = System.IO.File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\qryTest.txt");
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            var qry = new rogueCore.hqlSyntaxV2.filledSegments.FilledHQLQuery(blah);
            qry.Fill();
            stopwatch.Stop();
            Console.WriteLine("QUERYTIMER: " + stopwatch.ElapsedMilliseconds);
            //qry.PrintQuery();

            Stopwatch stopwatch2 = Stopwatch.StartNew();
            UIWebSection webtest = new UIWebSection(blah);
            webtest.BuildSection();
            return webtest.finalHTML;
        }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            // CreateDB();
            //UISegmentBuilder qry = StaticUISegments.OutlineQueryItem();
            //*Outline

            //*BRING BACK
            pageBuilder = new UIWebPage(7550);
            //pageBuilder = new UIWebPage(8455);
            //pageBuilder = RogueWebPage.ByPageID(1);
            //*Nav Menu. , "SECTIONHEADER");
            //pageBuilder.AddSegment(6);
            //Tree Menu. "SECTION1"
            ///pageBuilder.AddSegment(2);
            //Table header query. 5"SECTION2"
            //pageBuilder.AddSegment(5);
            //*"Row Insert. SECTION3"
            //pageBuilder.AddSegment(3);
            //Column INsert query  "SECTION4"
            //pageBuilder.AddSegment(4);
        }
        void SetPage(int qryID, String param = "")
        {
            //*BRinGBACK
            //RogueWebPage.ByPageID(qryID, param);
            
            
            //switch (qryID)
            //{
            //    case 1:
            //        //UISegmentBuilder qry = StaticUISegments.OutlineQueryItem();
            //        //*Outline
            //        pageBuilder = new RogueWebPage(1);
            //        //*Nav Menu. , "SECTIONHEADER");
            //        pageBuilder.AddSegment(6);
            //        //Tree Menu. "SECTION1"
            //        pageBuilder.AddSegment(2);
            //        //Table header query. 5"SECTION2"
            //        pageBuilder.AddSegment(5);
            //        //*"Row Insert. SECTION3"
            //        pageBuilder.AddSegment(3);
            //        //Column INsert query  "SECTION4"
            //        pageBuilder.AddSegment(4);
            //        break;
            //    case 2:
            //        //*Outline
            //        pageBuilder = new RogueWebPage(1);
            //        //*Nav Menu. , "SECTIONHEADER");
            //        pageBuilder.AddSegment(6);
            //        //Tree Menu. "SECTION1"
            //        pageBuilder.AddSegment(8);
            //        //Table header query. 5"SECTION2"
            //        pageBuilder.AddSegment(7);
            //        //*"Row Insert. SECTION3"
            //        pageBuilder.AddSegment(9);
            //        //Column INsert query  "SECTION4"
            //       // pageBuilder.AddSegment(10, param);
            //        break;
            //    case 3:
            //        pageBuilder = new RogueWebPage(1);
            //        //*Nav Menu. , "SECTIONHEADER");
            //        pageBuilder.AddSegment(6);
            //        //Tree Menu. "SECTION1"
            //        pageBuilder.AddSegment(8);
            //        //Table header query. 5"SECTION2"
            //        pageBuilder.AddSegment(7);
            //        //*"Row Insert. SECTION3"
            //        pageBuilder.AddSegment(9);
            //        //Column INsert query  "SECTION4"
            //        pageBuilder.AddSegment(10, StaticUISegments.QueryResultsTree(RogueWebPage.WebTranslater, param));
            //        break;
            //}
        }
        public void OnPost()
        {
            String clickInfo = Request.Form["rogueClickInfo"];
            Dictionary<String, String> pageContent = new Dictionary<string, string>();
            foreach (var pair in Request.Form)
            {
                pageContent.Add(pair.Key, pair.Value[0]);
            }
            //if (clickInfo.StartsWith("PAGE")){
            //    string[] parts = clickInfo.Split("_");
            //    SetPage(int.Parse(parts[2]));
            //}
            //if (clickInfo.StartsWith("RUNQUERY"))
            //{
            //    String qry = pageContent["TEXTQUERY"];
            //    SetPage(3, qry);
            //}
            //else
            //{
                pageBuilder.RogueClickEvent(clickInfo, pageContent);
            //}
        }
        public JsonResult OnGetHQLFilter(string startStr)
        {
            if(startStr != null)
            {
                //var qry = new SelectHQLStatement(startStr);
            }
            
            List<string> blah = new List<string> { "hey", "yo", "works", "FS" };
            
            return new JsonResult(blah);
        }
        //void CreateDB()
        //{
        //    DataBundle baseBundle = new DataBundle();
        //    RogueDatabase<DataRowID> testDB = baseBundle.GetDatabase<DataRowID>("NORTHWIND", "Test Data");
        //    IRogueTable countryTbl = testDB.GetTable("COUNTRY", "List of all countries");
        //    IRogueTable stateTbl = testDB.GetTable("STATE", "List of all states");
        //    IRogueTable cityTbl = testDB.GetTable("CITY", "List of all cities");

        //    IRogueRow newCountry = countryTbl.NewIWriteRow();
        //    IRoguePair country = newCountry.NewWritePair(new ColumnRowID("COUNTRY_NM"), "USA");
        //    countryTbl.Write();

        //    IRogueRow newState = stateTbl.NewIWriteRow();
        //    newState.NewWritePair(new ColumnRowID("STATE_NM"), "OHIO");
        //    stateTbl.Write();

        //    IRogueRow newCity = cityTbl.NewIWriteRow();
        //    newCity.NewWritePair(new ColumnRowID("CITY_NM"), "CLEVELAND");
        //    cityTbl.Write();
        //}
        //static IUIControl Translate(String controlName)
        //{
        //    IUIControl control = null;
        //    switch (controlName)
        //    {
        //        case "textbox":
        //            control = new RTextBox();
        //            break;
        //        case "label":
        //            control = new RLabel();
        //            break;
        //        case "button":
        //            control = new RButton();
        //            break;
        //        case "imagebutton":
        //            //Image img = new Image();
        //            ////Uri imgPath = new Uri(ths_control.Value(), UriKind.Absolute);
        //            ////img.Source = new BitmapImage(imgPath);
        //            //img.Width = 30;
        //            //img.Height = 30;
        //            //retElement = img;qq2
        //            break;
        //        case "treeviewnode":
        //            control = new RTreeViewNode();
        //            break;
        //        case "treeview":
        //            control = new RTreeView();
        //            break;
        //        case "groupbox":
        //            control = new RGroupBox();
        //            break;
        //        case "tablerow<maintainonrow>":
        //        case "tablerow":
        //            control = new RTableRow();
        //            break;
        //        case "tablecell":
        //            control = new RTableCell();
        //            break;
        //        case "table":
        //            control = new RTable();
        //            break;
        //        case "dropdownlist":
        //            control = new RDropDownList();
        //            break;
        //        case "listitem":
        //            control = new RListItem();
        //            break;
        //        default:
        //            return null;
        //    }
        //    return control;
        //}
    }
}
