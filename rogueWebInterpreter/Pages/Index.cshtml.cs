using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using rogue_core.rogueCore.StoredProcedures.StoredQuery.UISegment;
using rogue_ui.UIBuilder.Controls;
using rogueCore.UI;
using rogueWebInterpreter.WebUI.WebControls;

namespace rogueWebInterpreter.Pages
{
    public class IndexModel : PageModel
    {
        public static String buildHTML()
        {
            UIPageBuilder pageBuild = new UIPageBuilder(UIPageBuilder.OutlineQueryItem(), Translate);

            pageBuild.AddSegment(UIPageBuilder.menuQueryItem());
            UISegmentQuery tableHeader = UIPageBuilder.tableHeaderQueryItem();
            tableHeader.SetParameter("@TABLEID", "-1010");
            pageBuild.AddSegment(tableHeader);
            UISegmentQuery rowInsert = UIPageBuilder.rowInsertQueryItem();
            rowInsert.SetParameter("@TABLEID", "-1010");
            pageBuild.AddSegment(rowInsert);
            UISegmentQuery columnInsert = UIPageBuilder.columnInsertQueryItem();
            columnInsert.SetParameter("@TABLEID", "-1010");
            pageBuild.AddSegment(columnInsert);
            return pageBuild.BuildOutput();
        }
        public void OnGet()
        {
            //UIPageBuilder page = new UIPageBuilder()
           
        }
        static IUIControl Translate(String controlName)
        {
            IUIControl control = null;
            switch (controlName)
            {
                case "textbox":
                    control = new RTextBox();
                    break;
                case "label":
                    control = new RLabel();
                    break;
                case "button":
                    control = new RButton();
                    break;
                case "imagebutton":
                    //Image img = new Image();
                    ////Uri imgPath = new Uri(ths_control.Value(), UriKind.Absolute);
                    ////img.Source = new BitmapImage(imgPath);
                    //img.Width = 30;
                    //img.Height = 30;
                    //retElement = img;qq2
                    break;
                case "treeviewnode":
                    control = new RTreeViewNode();
                    break;
                case "treeview":
                    control = new RTreeView();
                    break;
                case "groupbox":
                    control = new RGroupBox();
                    break;
                case "tablerow<maintainonrow>":
                case "tablerow":
                    control = new RTableRow();
                    break;
                case "tablecell":
                    control = new RTableCell();
                    break;
                case "table":
                    control = new RTable();
                    break;
                case "dropdownlist":
                    control = new RDropDownList();
                    break;
                case "listitem":
                    control = new RListItem();
                    break;
                default:
                    return null;
            }
            return control;
        }
    }
}