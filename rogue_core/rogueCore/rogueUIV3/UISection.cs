using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using rogueCore.hqlSyntaxV3.row;
using rogueCore.rogueUIV3.web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using static rogueCore.hqlSyntaxV3.segments.SelectHQLStatement;
using static rogueCore.rogueUIV3.UISection;
using rogueCore.hqlSyntaxV3;

namespace rogueCore.rogueUIV3
{

    public abstract class UISection
    {
        protected SelectHQLStatement sectionQuery { get; private set; }
        List<IUIElement> lstContainers = new List<IUIElement>();
        public IUIElement topControl { get { return lstContainers[0]; } }
        Boolean isProcessed = false;
        protected abstract IUIElement UITranslator(string elementName, string elementValue);
        IMultiRogueRow qryRow;
        internal Dictionary<int, HQLParam> parameters;
        public string finalQry { get { return sectionQuery.finalQuery; } }
        public string AsJson { get { return sectionQuery.AsJsonResult(); } }
        protected UISection(IMultiRogueRow qryRow, Dictionary<int, string> pageParams)
        {
            ResetUISection(qryRow, pageParams);
            //this.qryRow = qryRow;
            //foreach (FilledSelectRow paramRow in qryRow.childRows)
            //{
            //    parameters.Add(new HQLParam(paramRow));
            //}
            //SetFinalQry();
        }
        protected UISection(string finalQry)
        {
            sectionQuery = new SelectHQLStatement(finalQry);
            //ResetUISection(qryRow, pageParams);
            //this.qryRow = qryRow;
            //foreach (FilledSelectRow paramRow in qryRow.childRows)
            //{
            //    parameters.Add(new HQLParam(paramRow));
            //}
            //SetFinalQry();
        }
        internal void ResetUISection(IMultiRogueRow qryRow, Dictionary<int, string> pageParams)
        {
            parameters = new Dictionary<int, HQLParam>();
            lstContainers = new List<IUIElement>();
            this.qryRow = qryRow;
            foreach (IMultiRogueRow paramRow in qryRow.childRows)
            {
                var param = new HQLParam(paramRow);
                //*Accidently put a dup in should get rid of this check and ensure this cant happen
                if (!parameters.ContainsKey(int.Parse(param.ParamOID())))
                {
                    parameters.Add(int.Parse(param.ParamOID()), param);
                }
            }
            SetParameters(pageParams);
            SetFinalQry();
            isProcessed = false;
        }
        void SetFinalQry()
        {
            Dictionary<string, string> hqlParams = new Dictionary<string, string>();
            hqlParams.Add("@QUERYID", QueryOID());
            foreach (HQLParam param in parameters.Values)
            {
                if (!hqlParams.ContainsKey(param.ParamReplaceTxt()))
                {
                    hqlParams.Add(param.ParamReplaceTxt(), param.value);
                }
            }
            sectionQuery = new SelectHQLStatement(qryRow.GetValue("QUERY_TXT"), hqlParams);
            //*CHANGED  
        }
        internal string QueryOID()
        {
            return qryRow.GetValue("QUERY_ID");
        }
        internal string SectionIDText()
        {
            return qryRow.GetValue("SECTION_PARAM_TXT");
        }
        internal string SectionOID()
        {
            return qryRow.GetValue("SECTION_OID");
        }
        internal void RefreshSegment(Dictionary<int, String> lstParams)
        {
            isProcessed = false;
            SetParameters(lstParams);
            //HQLParam param = parameters.Where(p => p.Value.ParamOID() == paramID).First().Value;
            //param.SetParamValue(paramValue);
            SetFinalQry();
            //namedControls = new Dictionary<string, IUIControl>();
            //query.ResetParameters(lstParams);
        }
        void SetParameters(Dictionary<int, string> lstParams)
        {
            foreach (var pair in lstParams)
            {
                if (parameters.ContainsKey(pair.Key))
                {
                    parameters[pair.Key].SetParamValue(pair.Value);
                }
            }
        }
        void ControlRowLooper(IMultiRogueRow row)
        {
            //if(row.GetValueList().ToList().Count < 1)
            //{
            //    return;
            //}
            //Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch            

            ////Console.WriteLine("Fill Level:" + stopwatch2.ElapsedMilliseconds);
            ////if (stat.Equals(rowstatus.open))
            ////{
            //Stopwatch stopConversion = new Stopwatch();
            //stopConversion.Start();
            String elementName = "";
            String elementValue = "";
            ParentRelationships parentRelation = ParentRelationships.child;
            //Stopwatch stopwatchTryParse = new Stopwatch();
            //stopwatchTryParse.Start();
            //if(row.GetValueList().ToList().Count < 1)
            //{


            String strRelation = row.GetValue("PARENTRELATION");
            //stopwatchTryParse.Stop();
            System.Enum.TryParse<ParentRelationships>(strRelation, out parentRelation);
            // stopwatchTryParse.Stop();
            //Stopwatch stopwatchGetVal = new Stopwatch();
            // stopwatchGetVal.Start();
            if (parentRelation == ParentRelationships.attribute)
            {
                elementName = row.GetValue("attributetype");
                elementValue = row.GetValue("attributevalue");
            }
            else
            {
                elementName = row.GetValue("CONTROLNAME");
            }
            //stopwatchGetVal.Stop();
            //}


            //stopConversion.Stop();
            //Stopwatch stopwatchtrans = new Stopwatch();
            //stopwatchtrans.Start();
            IUIElement uiElement = UITranslator(elementName, elementValue);
            //uiElement.parentRelation = parentRelation;
            //stopwatchtrans.Stop();
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            if (uiElement != null)
            {
                if (parentRelation.Equals(ParentRelationships.header))
                {
                    ((IUIControl)lstContainers[lstContainers.Count - 1]).SetHeader(uiElement);
                }
                else
                {

                    ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(uiElement);
                }
                lstContainers.Add(uiElement);
            }
            else
            {
                lstContainers.Add(lstContainers[lstContainers.Count - 1]);
            }
            //stopwatch.Stop();
            //stopwatch2.Stop();
            //Console.WriteLine("Fill Level:" + stopwatch2.ElapsedMilliseconds);
            //if(stopwatch2.ElapsedMilliseconds > 60)
            //{
            //    string l = "SDF";
            //}
            //}
            //else
            //{
            //lstContainers.RemoveAt(lstContainers.Count - 1);
            //}
        }
        void EndRowLooper(IMultiRogueRow row)
        {
            //if (row.GetValueList().ToList().Count < 1)
            //{
            //lstContainers.RemoveAt(lstContainers.Count - 1);
            //return;
            //}
            lstContainers.RemoveAt(lstContainers.Count - 1);
        }
        public void BuildSection()
        {
            if (!isProcessed)
            {
                lstContainers.Clear();
                //*Tempcode
                lstContainers.Add((IUIControl)UITranslator("groupbox", ""));
                ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("heightpercent", "100"));
                ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("widthpercent", "100"));
                Stopwatch fill = new Stopwatch();
                fill.Start();
                sectionQuery.Fill();
                fill.Stop();
                long fillTime = fill.ElapsedMilliseconds;
                fill.Restart();
                sectionQuery.IterateRows(ControlRowLooper, EndRowLooper);
                fill.Stop();
                long iterateTime = fill.ElapsedMilliseconds;
                fill.Restart();
                isProcessed = true;
                BuildSpecificFramework();
                fill.Stop();
                long build = fill.ElapsedMilliseconds;
            }
        }
        protected virtual void BuildSpecificFramework()
        {

        }
        internal static IMultiRogueRow ManualUIAttributeRow(IMultiRogueRow parentRow, string attributeType, string attributeValue)
        {
            ManualMultiRogueRow row = new ManualMultiRogueRow(parentRow);
            //parentRow.ManualChildRow(row);
            row.Add("PARENTRELATION", "attribute");
            row.Add("ATTRIBUTEVALUE", attributeValue);
            row.Add("ATTRIBUTETYPE", attributeType);
            return row;
        }
        internal static IMultiRogueRow ManualUIElementRow(IMultiRogueRow parentRow, string controlName, ParentRelationships relType = ParentRelationships.child)
        {
            ManualMultiRogueRow row = new ManualMultiRogueRow(parentRow);
            //parentRow.ManualChildRow(row);
            //parentRow.childRows.Add(row);
            //row.parentRow = parentRow;
            row.Add("PARENTRELATION", relType.ToString());
            row.Add("CONTROLNAME", controlName);
            return row;
        }
        public enum ParentRelationships
        {
            child, header, attribute
        }
        //public enum ParentRelationships : int
        //{
        //    [StringValue("child")] child = 1,
        //    [StringValue("header")] header = 2,
        //    [StringValue("attribute")] attribute = 3
        //}
        public static class UIElements 
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
}

