//using rogueCore.hqlSyntaxV2.filledSegments;
//using rogueCore.hqlSyntaxV2.segments;
//using rogueCore.hqlSyntaxV2.segments.select;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using static rogueCore.hqlSyntaxV2.filledSegments.FilledHQLQuery;

//namespace rogueCore.rogueUIV2
//{
    
//    public abstract class UISection
//    {
//        FilledHQLQuery sectionQuery;
//        List<IUIElement> lstContainers = new List<IUIElement>();
//        public IUIElement topControl { get { return lstContainers[0]; } }
//        Boolean isProcessed = false;
//        protected abstract IUIElement UITranslator(string elementName, string elementValue);
//        MultiRogueRow qryRow;
//        internal Dictionary<int, HQLParam> parameters;
//        protected UISection(MultiRogueRow qryRow, Dictionary<int,string> pageParams)
//        {
//            ResetUISection(qryRow, pageParams);
//            //this.qryRow = qryRow;
//            //foreach (FilledSelectRow paramRow in qryRow.childRows)
//            //{
//            //    parameters.Add(new HQLParam(paramRow));
//            //}
//            //SetFinalQry();
//        }
//        protected UISection(string finalQry)
//        {
//            sectionQuery = new FilledHQLQuery(finalQry);
//            //ResetUISection(qryRow, pageParams);
//            //this.qryRow = qryRow;
//            //foreach (FilledSelectRow paramRow in qryRow.childRows)
//            //{
//            //    parameters.Add(new HQLParam(paramRow));
//            //}
//            //SetFinalQry();
//        }
//        internal void ResetUISection(MultiRogueRow qryRow, Dictionary<int, string> pageParams)
//        {
//            parameters = new Dictionary<int, HQLParam>();
//            lstContainers = new List<IUIElement>();
//            this.qryRow = qryRow;
//            foreach (MultiRogueRow paramRow in qryRow.childRows)
//            {
//                var param = new HQLParam(paramRow);
//                parameters.Add(int.Parse(param.ParamOID()), param);
//            }
//            SetParameters(pageParams);
//            SetFinalQry();
//            isProcessed = false;
//        }
//        void SetFinalQry()
//        {
//            sectionQuery = new FilledHQLQuery(qryRow.values["QUERY_TXT"].Value);
//            sectionQuery.SetQueryParam("@QUERYID", QueryOID());
//            //finalQry = QueryText().Replace("@QUERYID", QueryOID());
//            foreach (HQLParam param in parameters.Values)
//            {
//                sectionQuery.SetQueryParam(param.ParamReplaceTxt(), param.value);
//            }
//        }
//        internal string QueryOID()
//        {
//            return qryRow.values["QUERY_ID"].Value;
//        }
//        //string QueryText()
//        //{
//        //    //return qryRow.values["QUERY_TXT"].Value.Replace("@QUERYID", QueryOID().ToString()).Replace("@TABLEID","-1010");
//        //    return qryRow.values["QUERY_TXT"].Value.Replace("@QUERYID", QueryOID().ToString());
//        //}
//        internal string SectionIDText()
//        {
//            return qryRow.values["SECTION_PARAM_TXT"].Value;
//        }
//        internal string SectionOID()
//        {
//            return qryRow.values["SECTION_OID"].Value;
//        }
//        //internal void RefreshSegment(Dictionary<String, String> lstParams = null)
//        //{
//        //    isProcessed = false;
//        //    SetFinalQry();
//        //    //namedControls = new Dictionary<string, IUIControl>();
//        //    //query.ResetParameters(lstParams);
//        //}
//        internal void RefreshSegment(Dictionary<int, String> lstParams)
//        {
//            isProcessed = false;
//            SetParameters(lstParams);
//            //HQLParam param = parameters.Where(p => p.Value.ParamOID() == paramID).First().Value;
//            //param.SetParamValue(paramValue);
//            SetFinalQry();
//            //namedControls = new Dictionary<string, IUIControl>();
//            //query.ResetParameters(lstParams);
//        }
//        void SetParameters(Dictionary<int, string> lstParams)
//        {
//            foreach (var pair in lstParams)
//            {
//                if (parameters.ContainsKey(pair.Key))
//                {
//                    parameters[pair.Key].SetParamValue(pair.Value);
//                }
//            }
//        }
//        void ControlRowLooper(rowstatus stat, MultiRogueRow row)
//        {
//                if (stat.Equals(rowstatus.open))
//                {
//                    String elementName;
//                    String elementValue = "";
//                    ParentRelationships parentRelation;
//                    String strRelation = row.GetValue("PARENTRELATION");
//                    System.Enum.TryParse<ParentRelationships>(strRelation, out parentRelation);
//                    if (parentRelation == ParentRelationships.attribute)
//                    {
//                        elementName = row.GetValue("attributetype");
//                        elementValue = row.GetValue("attributevalue");
//                     }
//                    else
//                    {
//                        elementName = row.GetValue("CONTROLNAME");
//                    }
//                    IUIElement uiElement = UITranslator(elementName, elementValue);
//                    if (uiElement != null)
//                    {
//                        if (parentRelation.Equals(ParentRelationships.header))
//                        {
//                            ((IUIControl)lstContainers[lstContainers.Count - 1]).SetHeader(uiElement);
//                        }
//                        else
//                        {
//                            ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(uiElement);
//                        }
//                        lstContainers.Add(uiElement);
//                    }
//                    else
//                    {
//                        lstContainers.Add(lstContainers[lstContainers.Count - 1]);
//                    }
//                }
//                else
//                {
//                    lstContainers.RemoveAt(lstContainers.Count - 1);
//                }
//            }
//        public void BuildSection()
//        {
//            if (!isProcessed)
//            {
//                lstContainers.Clear();
//                //*Tempcode
//                lstContainers.Add((IUIControl)UITranslator("groupbox", ""));
//                ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("heightpercent", "100"));
//                ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("widthpercent", "100"));
//                var qryResults = sectionQuery.Fill();
//                qryResults.IterateRows(ControlRowLooper);
//                isProcessed = true;
//                BuildSpecificFramework();
//            }
//        }
//        //public void BuildQueryDecor()
//        //{
//        //    if (!isProcessed)
//        //    {
//        //        lstContainers.Clear();
//        //        //*Tempcode
//        //        lstContainers.Add((IUIControl)UITranslator("groupbox", ""));
//        //        ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("heightpercent", "100"));
//        //        ((IUIControl)lstContainers[lstContainers.Count - 1]).SetChildContent(UITranslator("widthpercent", "100"));
//        //        sectionQuery.GetUIQueryResults();
//        //        sectionQuery.IterateRows(ControlRowLooper);
//        //        isProcessed = true;
//        //        BuildSpecificFramework();
//        //    }
//        //}
//        protected virtual void BuildSpecificFramework()
//        {

//        }
//        internal static MultiRogueRow ManualUIAttributeRow(MultiRogueRow parentRow, string attributeType,string attributeValue)
//        {
//            MultiRogueRow row = new MultiRogueRow();
//            parentRow.ManualChildRow(row);
//            row.values.Add("PARENTRELATION", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), "attribute"));
//            row.values.Add("ATTRIBUTEVALUE", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), attributeValue));
//            row.values.Add("ATTRIBUTETYPE", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), attributeType));
//            return row;
//        }
//        internal static MultiRogueRow ManualUIElementRow(MultiRogueRow parentRow, string controlName, ParentRelationships relType = ParentRelationships.child)
//        {
//            MultiRogueRow row = new MultiRogueRow();
//            parentRow.ManualChildRow(row);
//            //parentRow.childRows.Add(row);
//            //row.parentRow = parentRow;
//            row.values.Add("PARENTRELATION", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), relType.ToString()));
//            row.values.Add("CONTROLNAME", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), controlName));
//            return row;
//        }
//            //internal static MultiRogueRow ManualUIElementRowNoParent(string controlName, ParentRelationships relType = ParentRelationships.child)
//            //{
//            //    MultiRogueRow row = new MultiRogueRow();
//            //    //parentRow.ManualChildRow(row);
//            //    //parentRow.childRows.Add(row);
//            //    //row.parentRow = parentRow;
//            //    row.values.Add("PARENTRELATION", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), relType.ToString()));
//            //    row.values.Add("CONTROLNAME", new KeyValuePair<SelectColumn, string>(new SelectColumn(new LocationColumn(-1012)), controlName));
//            //    return row;
//            //}
//        public enum ParentRelationships
//        {
//            child, header, attribute
//        }
//    }
//}

