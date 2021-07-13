//using rogueCore.hqlSyntax;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using static rogueCore.hqlSyntax.HQLQueryTwo;

//namespace rogueCore.rogueUI
//{
//    public abstract class UISection
//    {
//        List<IUIElement> lstContainers;
//        public IUIElement topControl { get { return lstContainers[0]; } }
//        Boolean isProcessed = false;
//        protected abstract IUIElement UITranslator(string elementName, string elementValue);
//        FilledSelectRow qryRow;
//        internal Dictionary<int, HQLParam> parameters;
//        string finalQry;
//        protected UISection(FilledSelectRow qryRow, Dictionary<int,string> pageParams)
//        {
//            ResetUISection(qryRow, pageParams);
//            //this.qryRow = qryRow;
//            //foreach (FilledSelectRow paramRow in qryRow.childRows)
//            //{
//            //    parameters.Add(new HQLParam(paramRow));
//            //}
//            //SetFinalQry();
//        }
//        internal void ResetUISection(FilledSelectRow qryRow, Dictionary<int, string> pageParams)
//        {
//            parameters = new Dictionary<int, HQLParam>();
//            lstContainers = new List<IUIElement>();
//            this.qryRow = qryRow;
//            foreach (FilledSelectRow paramRow in qryRow.childRows)
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
//            finalQry = QueryText().Replace("@QUERYID", QueryOID());
//            foreach (HQLParam param in parameters.Values)
//            {
//                finalQry = finalQry.Replace(param.ParamReplaceTxt(), param.value);
//            }
//        }
//        internal string QueryOID()
//        {
//            return qryRow.values["QUERY_ID"].Value;
//        }
//        string QueryText()
//        {
//            //return qryRow.values["QUERY_TXT"].Value.Replace("@QUERYID", QueryOID().ToString()).Replace("@TABLEID","-1010");
//            return qryRow.values["QUERY_TXT"].Value.Replace("@QUERYID", QueryOID().ToString());
//        }
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
//        void ControlRowLooper(rowstatus stat, FilledSelectRow row)
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
//                    }
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
//                HumanHQLStatement qryResults = new HumanHQLStatement(finalQry);
//                qryResults.IterateRows(ControlRowLooper);
//                isProcessed = true;
//                BuildSpecificFramework();
//            }
//        }
//        protected virtual void BuildSpecificFramework()
//        {

//        }
        
//        public enum ParentRelationships
//        {
//            child, header, attribute
//        }
//    }
//}

