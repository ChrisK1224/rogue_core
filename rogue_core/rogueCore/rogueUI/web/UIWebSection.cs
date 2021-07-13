//using rogueCore.hqlSyntax;
//using rogueCore.rogueUI.web.element;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace rogueCore.rogueUI.web
//{
//    public class UIWebSection : UISection
//    {
//        internal string finalHTML;
//        internal UIWebSection(FilledSelectRow sectionRow, Dictionary<int,string> pageParams) : base(sectionRow, pageParams) { }
//        protected override IUIElement UITranslator(string elementName, string elementValue)
//        {
//            //Type t = typeof(rogueCore.UI.WebUI.WebControls.WebBaseControl);
//            //MethodInfo methodInfo = t.GetMethod("RLabel");

//            //if (methodInfo != null)
//            //{

//            //}
//            if (elementName != null)
//            {
//                elementName = elementName.ToLower();
//            }
//            switch (elementName)
//            {
//                case "textbox":
//                    return new RTextBox();
//                case "textarea":
//                    return new RTextArea();
//                case "label":
//                    return new RLabel();
//                case "button":
//                    return new RButton();
//                case "image":
//                    return new RImage();
//                case "treeviewnode":
//                    return new RTreeViewNode();
//                case "emptytreeviewnode":
//                    return new REmptyTreeViewNode();
//                case "treeview":
//                    return new RTreeView();
//                case "groupbox":
//                    return new RGroupBox();
//                case "tablerow<maintainonrow>":
//                case "tablerow":
//                    return new RTableRow();
//                case "tablecell":
//                    return new RTableCell();
//                case "headertablegroup":
//                    return new RHeaderGroup();
//                case "isselected":
//                    return new IsSelected("");
//                case "datarowgroup":
//                    return new RDataRowGroup();
//                case "headertablecell":
//                    return new RHeaderCell();
//                case "table":
//                    return new RTable();
//                case "dropdownlist":
//                    return new RDropDownList();
//                case "listitem":
//                    return new RListItem();
//                case "displaycell":
//                    return new RDisplayCell();
//                case "displayrow":
//                    return new RDisplayRow();
//                case "displaytable":
//                    return new RDisplayTable();
//                case "navbar":
//                    return new RNavBar();
//                case "navitem":
//                    return new RNavItem();
//                case "widthpixels":
//                    return new WidthPixels(elementValue);
//                case "heightpixels":
//                    return new HeightPixels(elementValue);
//                case "fontsize":
//                    return new FontSize(elementValue);
//                case "mouseclick":
//                    return new MouseClick(elementValue);
//                case "mousedoubleclick":
//                    return new MouseDoubleClick(elementValue);
//                case "text":
//                    return new Text(elementValue);
//                case "orientation":
//                    return null;
//                case "idname":
//                    return new NameID(elementValue);
//                case "scrollx":
//                    return new OverflowScrollX(elementValue);
//                case "scrolly":
//                    return new OverflowScrollY(elementValue);
//                case "columnspan":
//                    return new ColumnSpan(elementValue);
//                case "rowspan":
//                    return new RowSpan(elementValue);
//                case "widthpercent":
//                    return new WidthPercent(elementValue);
//                case "heightpercent":
//                    return new HeightPercent(elementValue);
//                case "fontweight":
//                    return null;
//                case "backgroundcolor":
//                    return new BackgroundColor(elementValue);
//                case "cssclass":
//                    return new CssClass(elementValue);
//                case "imagepath":
//                    return new ImagePath(elementValue);
//                case "paddingleft":
//                    return new PaddingLeft(elementValue);
//                case "itemvalue":
//                    return new ItemValue(elementValue);
//                case "breakline":
//                    return new RBreakline();
//                case "nowrap":
//                    return new RNoWrap(elementValue);
//                case "listview":
//                    return new RListView();
//                case "listviewitem":
//                    return new RListViewItem();
//                case "onchange":
//                    return new DDLChangeEvent(elementValue);
//                default:
//                    return null;
//            }
//        }
//        //internal static string GenerateSegmentHTML(WebBaseControl thsControl, StringBuilder html)
//        //{
//        //    html.Append(thsControl.StartTagText());
//        //    foreach (WebBaseControl child in thsControl.childControls)
//        //    {
//        //        GenerateSegmentHTML(child, html);
//        //    }
//        //    html.Append(thsControl.endTag);
//        //    return html.ToString();
//        //}
//        internal static string LoopGenerateSegmentHTML(WebBaseControl thsControl, StringBuilder html)
//        {
//            //BuildControls();
//            // var html = new StringBuilder();
//            //WebBaseControl thsControl = (WebBaseControl)this.topControl;
//            html.Append(thsControl.StartTagText());
//            foreach (WebBaseControl child in thsControl.childControls)
//            {
//                LoopGenerateSegmentHTML(child, html);
//            }
//            html.Append(thsControl.endTag);
//            return html.ToString();
//        }
//        protected override void BuildSpecificFramework()
//        {
//            WebBaseControl thsControl = (WebBaseControl)this.topControl;
//            finalHTML = LoopGenerateSegmentHTML(thsControl, new StringBuilder());
//        }
//    }
//}
