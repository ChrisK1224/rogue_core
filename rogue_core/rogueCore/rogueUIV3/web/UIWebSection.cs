using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.rogueUIV3.web.element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace rogueCore.rogueUIV3.web
{
    public class UIWebSection : UISection
    {
        public string finalHTML;
        public UIWebSection(IMultiRogueRow sectionRow, Dictionary<int,string> pageParams) : base(sectionRow, pageParams) { }
        public UIWebSection(string finalQry) : base(finalQry) { }
        protected override IUIElement UITranslator(string elementName, string elementValue)
        {
            if (elementName != null)
            {
                elementName = elementName.ToLower();
            }
            switch (elementName)
            {
                case Elements.fullHTMLOutline2Side:
                    return new FullHTMLOutline2Sides();
                case Elements.sideNavOne:
                    return new SideNavOne();               
                case "textbox":
                    return new RTextBox();
                case Elements.radiolist:
                    return new RRadioList();
                case "anchor":
                    return new Anchor();
                case "textarea":
                    return new RTextArea();
                case "label":
                    return new RLabel();
                case "button":
                    return new RButton();
                case Elements.fileupload:
                    return new FileUpload();
                case "image":
                    return new RImage();
                case "treeviewnode":
                    return new RTreeViewNode();
                case "emptytreeviewnode":
                    return new REmptyTreeViewNode();
                case "treeview":
                    return new RTreeView();
                case "groupbox":
                    return new RGroupBox();
                case "tablerow<maintainonrow>":
                case "tablerow":
                    return new RTableRow();
                case "tablecell":
                    return new RTableCell();
                case "headertablegroup":
                    return new RHeaderGroup();
                case "isselected":
                    return new IsSelected("");
                case "datarowgroup":
                    return new RDataRowGroup();
                case "headertablecell":
                    return new RHeaderCell();
                case "table":
                    return new RTable();
                case "dropdownlist":
                    return new RDropDownList();
                case "listitem":
                    return new RListItem();
                case "displaycell":
                    return new RDisplayCell();
                case "displayrow":
                    return new RDisplayRow();
                case "displaytable":
                    return new RDisplayTable();
                case "navbar":
                    return new RNavBar();
                case "navitem":
                    return new RNavItem();
                case "widthpixels":
                    return new WidthPixels(elementValue);
                case "heightpixels":
                    return new HeightPixels(elementValue);
                case "fontsize":
                    return new FontSize(elementValue);
                case "mouseclick":
                    return new MouseClick(elementValue);
                case "mousedoubleclick":
                    return new MouseDoubleClick(elementValue);
                case "text":
                    return new Text(elementValue);
                case "orientation":
                    return null;
                case Attributes.onTextChange:
                    return new OnTextChange(elementValue);
                case "idname":
                    return new NameID(elementValue);
                case "scrollx":
                    return new OverflowScrollX(elementValue);
                case "scrolly":
                    return new OverflowScrollY(elementValue);
                case "columnspan":
                    return new ColumnSpan(elementValue);
                case "rowspan":
                    return new RowSpan(elementValue);
                case "widthpercent":
                    return new WidthPercent(elementValue);
                case "heightpercent":
                    return new HeightPercent(elementValue);
                case "backgroundcolor":
                    return new BackgroundColor(elementValue);
                case "cssclass":
                    return new CssClass(elementValue);
                case "imagepath":
                    return new ImagePath(elementValue);
                case "paddingleft":
                    return new PaddingLeft(elementValue);
                case "itemvalue":
                    return new ItemValue(elementValue);
                case "breakline":
                    return new RBreakline();
                case "nowrap":
                    return new RNoWrap(elementValue);
                case "listview":
                    return new RListView();
                case "listviewitem":
                    return new RListViewItem();
                case Attributes.onchange:
                    return new DDLItemChange(elementValue);
                case "tabspace":
                    return new TabSpace();
                case "textalign":
                    return new TextAlign(elementValue);
                case "bordertop":
                    return new BorderTop(elementValue);
                case "border":
                    return new Border(elementValue);
                case "borderbottom":
                    return new BorderBottom(elementValue);
                case Attributes.margin:
                    return new Margin(elementValue);
                case "margintop":
                    return new MarginTop(elementValue);
                case "marginbottom":
                    return new MarginBottom(elementValue);
                case "float":
                    return new Float(elementValue);
                case "marginright":
                    return new MarginRight(elementValue);
                case "marginleft":
                    return new MarginLeft(elementValue);
                case "fontweight":
                    return new FontWeight(elementValue);
                case "fontcolor":
                    return new FontColor(elementValue);
                case "fontstyle":
                    return new FontStyle(elementValue);
                case Attributes.fontFamily:
                    return new FontFamily(elementValue);
                case "contenteditable":
                    return new EditableContent(elementValue);
                case "mouseclickcustom":
                    return new MouseClickCustom(elementValue);
                case Attributes.underline:
                    return new Underline(elementValue);
                case Attributes.displayType:
                    return new CssDisplayType(elementValue);
                default:
                    return null;
            }
        }
        internal static string LoopGenerateSegmentHTML(WebBaseControl thsControl, StringBuilder html)
        {            
            html.Append(thsControl.StartTagText());
            foreach (WebBaseControl child in thsControl.childControls)
            {
                LoopGenerateSegmentHTML(child, html);
            }
            html.Append(thsControl.endTag);
            return html.ToString();
        }
        protected override void BuildSpecificFramework()
        {
            WebBaseControl thsControl = (WebBaseControl)this.topControl;
            finalHTML = LoopGenerateSegmentHTML(thsControl, new StringBuilder());
        }
        public string WebJson()
        {
            StringBuilder json = new StringBuilder();
            int currLevel = 0;
            int uniqeEnforcer =4;
            Action<IMultiRogueRow> openJson = (row) =>
            {
                if (row.levelNum > currLevel)
                {
                    json.Append("\"" + uniqeEnforcer + "\" : [");
                    uniqeEnforcer++;
                }
                currLevel = row.levelNum;
                json.Append("{");
                string val = row.GetValue("PARENTRELATION").ToUpper();
                string subNM = row.GetValue("CONTROLNAME");
                bool isAttribute = false;
                if (subNM == "")
                {
                    isAttribute = true;
                    subNM = row.GetValue("ATTRIBUTETYPE");
                    val = row.GetValue("ATTRIBUTEVALUE").Replace("\r\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("\\", "\\\\").Replace("\"", "\\\"");
                }
                json.Append("\"1\":" + "\"" + subNM.ToUpper() + "\",");
                if (isAttribute || val.Equals("HEADER"))
                {
                    json.Append(" \"2\": " + "\"" + val + "\",");
                }
            };
            Action<IMultiRogueRow> closeJson = (row) =>
            {
                json.Length--;
                if (row.levelNum < currLevel)
                {
                    json.Append("]");
                }
                json.Append("},");
            };
            json.Append("{");
            sectionQuery.Fill();
            sectionQuery.IterateRows(openJson, closeJson);
            json.Length--;
            json.Append("]}");
            string bll = json.ToString();
            return json.ToString();
        }
    }
    public static class Attributes
    {
        public const string isselected = "isselected";        
        public const string widthpixels = "widthpixels";
        public const string heightpixels = "heightpixels";
        public const string fontsize = "fontsize";
        public const string mouseclick = "mouseclick";
        public const string mousedoubleclick = "mousedoubleclick";
        public const string text = "text";
        public const string orientation = "orientation";
        public const string idname = "idname";
        public const string scrollx = "scrollx";
        public const string scrolly = "scrolly";
        public const string columnspan = "columnspan";
        public const string rowspan = "rowspan";
        public const string widthpercent = "widthpercent";
        public const string heightpercent = "heightpercent";
        public const string backgroundcolor = "backgroundcolor";
        public const string cssclass = "cssclass";
        public const string imagepath = "imagepath";
        public const string paddingleft = "paddingleft";
        public const string itemvalue = "itemvalue";        
        public const string nowrap = "nowrap";
        public const string listview = "listview";
        public const string listviewitem = "listviewitem";
        public const string onchange = "onchange";
        public const string tabspace = "tabspace";
        public const string textalign = "textalign";
        public const string bordertop = "bordertop";
        public const string border = "border";
        public const string borderbottom = "borderbottom";
        public const string margintop = "margintop";
        public const string marginbottom = "marginbottom";
        public const string orientationFloat = "float";
        public const string marginright = "marginright";
        public const string marginleft = "marginleft";
        public const string margin = "margin";
        public const string fontweight = "fontweight";
        public const string fontcolor = "fontcolor";
        public const string fontstyle = "fontstyle";
        public const string contenteditable = "contenteditable";
        public const string mouseclickCustom = "mouseclickcustom";
        public const string fontFamily = "fontfamily";
        public const string underline = "underline";
        public const string displayType = "cssdisplay";
        public const string onTextChange = "ontextchange";
    }
    public static class Elements
    {
        public const string tabspace = "tabspace";
        public const string radiolist = "radiolist";
        public const string anchor = "anchor";
        public const string textbox = "textbox";
        public const string textarea = "textarea";
        public const string label = "label";
        public const string button = "button";
        public const string image = "image";
        public const string treeviewnode = "treeviewnode";
        public const string emptytreeviewnode = "emptytreeviewnode";
        public const string treeview = "treeview";
        public const string groupbox = "groupbox";
        public const string tablerow = "tablerow";
        public const string tablecell = "tablecell";
        public const string headertablegroup = "headertablegroup";
        public const string datarowgroup = "datarowgroup";
        public const string headertablecell = "headertablecell";
        public const string table = "table";
        public const string dropdownlist = "dropdownlist";
        public const string listitem = "listitem";
        public const string listview = "listview";
        public const string listviewitem = "listviewitem";
        public const string displaycell = "displaycell";
        public const string displayrow = "displayrow";
        public const string displaytable = "displaytable";
        public const string fileupload = "fileupload";
        public const string navbar = "navbar";
        public const string navitem = "navitem";
        public const string breakline = "breakline";
        public const string sideNavOne = "sidenavone";
        public const string fullHTMLOutline2Side = "fulllhtmloutline2side";
    }
}