using rogueCore.hqlSyntaxV3.segments;
using rogueCore.StoredProcedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.StoredProcedures.StoredQuery.UISegment
{
    static class StaticUISegments
    {
        //static Dictionary<int, UISegmentBuilder> queries = new Dictionary<int, UISegmentBuilder>() { { 1, OutlineQueryItem() }, { 2, menuQueryItem()}, { 3, rowInsertQueryItem() }, { 4, columnInsertQueryItem() } };
        internal const int outlineQryID = 7436;
        internal static StoredUIQuery GetQueryByID(int queryID, Dictionary<string,string> qryParams = null)
        {
            switch (queryID)
            {
                case outlineQryID:
                    return OutlineQueryItem();
                case 2:
                    return menuQueryItem();
                case 3:
                    return rowInsertQueryItem(qryParams);
                case 4:
                    return columnInsertQueryItem(qryParams);
                case 5:
                    return tableHeaderQueryItem(qryParams);
                case 6:
                    return navHeaderQueryItem();
                case 7:
                    return queryBuilderHeader();
                case 8:
                    return queryBuilderMenuQueryItem();
                case 9:
                    return QueryBuilderText();
                case 10:
                    return QueryResultsTree(qryParams);
                case 11:
                    return queryTableSelect(qryParams);
                case 12:
                    return queryCreateIOObject(qryParams);
                case 13:
                    return queryObjectChildren(qryParams);
            }
            return null;
        }
        internal static StoredUIQuery OutlineQueryItem()
        {
            //string outlineQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/pageOutline.txt").Replace(Environment.NewLine, " ");
            var qry = new SelectHQLStatement("FROM HQL_QUERIES SELECT QUERY_TXT WHERE ROGUECOLUMNID = \"7436\"").Fill();
            string qryText = qry.TopRows().First().GetValue("QUERY_TXT");
            //''FilledSelectRow hckeck = val[0].Value;
            //string qryText = val[0].Value.GetValue("QUERY_TXT");
            //[0].Value.values["QUERY_TXT"].Value;
            //String outlineQry = File.ReadAllText("/queries/HQLDeveloperY:\\RogueDatabase\\HQLDeveloper\\pageOutline.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(qryText, outlineQryID, "outline");
        }   
        internal static StoredUIQuery menuQueryItem()
        {
            String menuQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/menuQry.txt").Replace(Environment.NewLine, " ");
            //String menuQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\menuQry.txt").Replace(Environment.NewLine, " ");
            StoredUIQuery qry = new StoredUIQuery(menuQry, 2, "@SIDEMENU");
            qry.selectDependentQueryIDs.Add(5, "@TABLEID");
            qry.selectDependentQueryIDs.Add(3, "@TABLEID");
            qry.selectDependentQueryIDs.Add(11, "@TABLEID");
            qry.selectDependentQueryIDs.Add(4, "@TABLEID");
            return qry;
        }
        internal static StoredUIQuery queryBuilderMenuQueryItem()
        {
            String menuQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/QueryBuilder/menuQry.txt").Replace(Environment.NewLine, " ");
            //String menuQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\QueryBuilder\\menuQry.txt").Replace(Environment.NewLine, " ");
            StoredUIQuery qry = new StoredUIQuery(menuQry, 8, "@SIDEMENU");
            return qry;
        }
        internal static StoredUIQuery rowInsertQueryItem(Dictionary<string,string> qryParams)
        {
            if(qryParams == null)
            {
                qryParams = new Dictionary<String, String>() { { "@TABLEID", "-1010" } };
            }
            //String rowInsertQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\rowInsert.txt").Replace(Environment.NewLine, " ");
            String rowInsertQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/rowInsert.txt").Replace(Environment.NewLine, " ");
            StoredUIQuery qry = new StoredUIQuery(rowInsertQry, 3, "@BODYSECTION1", qryParams);
            qry.selectDependentQueryIDs.Add(5, "@TABLEID");
            qry.selectDependentQueryIDs.Add(3, "@TABLEID");
            qry.selectDependentQueryIDs.Add(11, "@TABLEID");
            return qry;
        }
        internal static StoredUIQuery QueryBuilderText()
        {
            String rowInsertQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/QueryBuilder/queryBuilderText.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(rowInsertQry, 9, "SECTION3");
        }
        internal static StoredUIQuery QueryResultsTree(Dictionary<string,string> qryParams)
        {
            return new StoredUIQuery(qryParams["query"], 10, "@BODYSECTION2");
        }
        internal static StoredUIQuery columnInsertQueryItem(Dictionary<string,string> qryParams)
        {
            if (qryParams == null)
            {
                qryParams = new Dictionary<String, String>() { { "@TABLEID", "-1010" } };
            }
            String columnInsertQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/columnInsert.txt").Replace(Environment.NewLine, " ");
            //String columnInsertQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\columnInsert.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(columnInsertQry, 4, "@BODYSECTION3", qryParams);
        }
        internal static StoredUIQuery tableHeaderQueryItem(Dictionary<string,string> qryParams)
        {
            if (qryParams == null)
            {
                qryParams = new Dictionary<String, String>() { { "@TABLEID", "-1010" } };
            }
            String tableHeaderQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/tableHeader.txt").Replace(Environment.NewLine, " ");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\tableHeader.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(tableHeaderQry, 5, "@BODYHEADER",  qryParams);
        }
        internal static StoredUIQuery navHeaderQueryItem()
        {
            String tableHeaderQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/navHeader.txt").Replace(Environment.NewLine, " ");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\navHeader.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(tableHeaderQry, 6, "@PAGEHEADER");
        }
        internal static StoredUIQuery queryBuilderHeader()
        {
            String tableHeaderQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/QueryBuilder/queryBuilderHeader.txt").Replace(Environment.NewLine, " ");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\QueryBuilder\\queryBuilderHeader.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(tableHeaderQry, 7, "@BODYHEADER");
        }
        internal static StoredUIQuery queryTableSelect(Dictionary<string,string> qryParams)
        {
            if (qryParams == null)
            {
                qryParams = new Dictionary<String, String>() { { "@TABLEID", "-1010" } };
            }
            String tableSelectQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/tableEdit.txt").Replace(Environment.NewLine, " ");
            StoredUIQuery qry = new StoredUIQuery(tableSelectQry, 11, "@BODYSECTION2",  qryParams);
            qry.selectDependentQueryIDs.Add(5, "@TABLEID");
            qry.selectDependentQueryIDs.Add(3, "@TABLEID");
            qry.selectDependentQueryIDs.Add(11, "@TABLEID");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\QueryBuilder\\queryBuilderHeader.txt").Replace(Environment.NewLine, " ");
            return qry;
        }
        internal static StoredUIQuery queryCreateIOObject(Dictionary<string,string> qryParams){
            String createIOObjectSelectQry = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/createIOObject.txt").Replace(Environment.NewLine, " ");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\QueryBuilder\\queryBuilderHeader.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(createIOObjectSelectQry, 12, "@BODYSECTION1",  qryParams);
        }
        internal static StoredUIQuery queryObjectChildren(Dictionary<string,string> qryParams){
            String objectChildren = File.ReadAllText(Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "HQLDeveloper/objectChildrenTable.txt").Replace(Environment.NewLine, " ");
            //String tableHeaderQry = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\QueryBuilder\\queryBuilderHeader.txt").Replace(Environment.NewLine, " ");
            return new StoredUIQuery(objectChildren, 13, "@BODYSECTION2",  qryParams);
        }
        //static IUIControl WebTranslater(String controlName)
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
        //        case "image":
        //            return new RImage();
        //        //Image img = new Image();
        //        ////Uri imgPath = new Uri(ths_control.Value(), UriKind.Absolute);
        //        ////img.Source = new BitmapImage(imgPath);
        //        //img.Width = 30;
        //        //img.Height = 30;
        //        //retElement = img;qq2
        //        // break;
        //        case "treeviewnode":
        //            control = new RTreeViewNode();
        //            break;
        //        case "emptytreeviewnode":
        //            control = new REmptyTreeViewNode();
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
        //        case "headertablerow":
        //            control = new RHeaderRow();
        //            break;
        //        case "headertablecell":
        //            control = new RHeaderCell();
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
        //        case "displaycell":
        //            return new RDisplayCell();
        //        case "displayrow":
        //            return new RDisplayRow();
        //        case "displaytable":
        //            return new RDisplayTable();
        //        case "navbar":
        //            return new RNavBar();
        //        case "navitem":
        //            return new RNavItem();
        //        default:
        //            return null;
        //    }
        //    return control;
        //}
        //static IUIAttribute WebAttributeTranslator(String attributeType, String attributeValue)
        //{
        //    switch (attributeType)
        //    {
        //        case "widthpixels":
        //            return new WidthPixels(attributeValue);
        //        case "heightpixels":
        //            return new HeightPixels(attributeValue);
        //        case "fontsize":
        //            return new FontSize(attributeValue);
        //        case "mouseclick":
        //            return new MouseClick(attributeValue);
        //        case "mousedoubleclick":
        //            return new MouseDoubleClick(attributeValue);
        //        case "text":
        //            return new Text(attributeValue);
        //        case "orientation":
        //            return null;
        //        case "idname":
        //            return new NameID(attributeValue);
        //        case "columnspan":
        //            return new ColumnSpan(attributeValue);
        //        case "rowspan":
        //            return new RowSpan(attributeValue);
        //        case "widthpercent":
        //            return new WidthPercent(attributeValue);
        //        case "heightpercent":
        //            return new HeightPercent(attributeValue);
        //        case "fontweight":
        //            return null;
        //        case "backgroundcolor":
        //            return new BackgroundColor(attributeValue);
        //        case "cssclass":
        //            return new CssClass(attributeValue);
        //        case "imagepath":
        //            return new ImagePath(attributeValue);
        //        case "paddingleft":
        //            return new PaddingLeft(attributeValue);
        //        default:
        //            return null;
        //    }
        //}
    }
}
