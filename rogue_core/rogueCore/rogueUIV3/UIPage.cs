using files_and_folders;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.bundle;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3
{
    public abstract class UIPage<SectionType> where SectionType : UISection
    {
        protected SectionType baseSegment { private set; get; }
        const int storedProcID = 7568;
        IMultiRogueRow pageHQLQueryRow;
        IMultiRogueRow pageResultsRow;
        public List<string> queriesTxt = new List<string>();
        public int pageID { get; private set; }
        internal List<SectionType> sections { get; private set; }
        Dictionary<int, string> currParams = new Dictionary<int, string>();
        protected UIPage(int pageID)
        {
            this.pageID = pageID;
            ResetPage();
        }
        protected UIPage() { }
        void ResetPage()
        {
            sections = new List<SectionType>();
            var filledQry = new SelectHQLStatement("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\" SELECT * ");
            filledQry.Fill();
            pageHQLQueryRow = filledQry.TopRows().First();            
            string fullPageQry = pageHQLQueryRow.GetValue("QUERY_TXT");
            fullPageQry = fullPageQry.Replace("@PAGE_OID", pageID.ToString());
            var filledQry2 = new SelectHQLStatement(fullPageQry);
            filledQry2.Fill();
            pageResultsRow = filledQry2.TopRows().First();   
            currParams.FindChangeIfNotFound(8799, pageResultsRow.GetValue("PAGE", 7548));
            baseSegment = NewSection(pageResultsRow.childRows[0], currParams);
            for (int i = 1; i < pageResultsRow.childRows.Count; i++)
            {
                SectionType thsSection = NewSection(pageResultsRow.childRows[i], currParams);
                sections.Add(thsSection);
                queriesTxt.Add(thsSection.finalQry);
            }
        }
        protected abstract void BuildPage(); 
        //public void RogueClickEventOLD(string clickInfo, Dictionary<String, String> pageContent)
        //    {
        //        string[] parts = clickInfo.Split('_');
        //        String clickAction = parts[0];
        //        int queryID = int.Parse(parts[1].ToString());
        //        String tableID = parts[2].ToString();
        //        if (clickAction.ToUpper().Equals("INSERT"))
        //        {
        //            IRogueTable tbl = new IORecordID(tableID).ToTable();
        //            IRogueRow newRow = tbl.NewIWriteRow();
        //            foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("INSERTVALUE_" + queryID.ToString() + "_" + tableID)))
        //            {
        //                String columnID = selectValues.Key.Split('_')[3];
        //                String value = selectValues.Value;
        //                newRow.NewWritePair(new ColumnRowID(columnID), value);
        //            }
        //            tbl.Write();
        //            //RefreshSegment(queryID, tableID);
        //        }
        //        else if (clickAction.ToUpper().Equals("UPDATE"))
        //        {
        //            string rowID = parts[3];
        //            UpdateStatement updateStatement = new UpdateStatement(new IORecordID(tableID));
        //            updateStatement.AddWhereClause(-1012, rowID);
        //            foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("UPDATEVALUE_" + queryID.ToString() + "_" + tableID + "_" + rowID)))
        //            {
        //                ColumnRowID columnID = new ColumnRowID(selectValues.Key.Split('_')[4]);
        //                String value = selectValues.Value;
        //                updateStatement.AddUpdateField(columnID, value);
        //            }
        //            updateStatement.ExecuteUpdate();
        //            //RefreshSegment(queryID, tableID);
        //        }
        //        else if (clickAction.ToUpper().Equals("CUSTOM"))
        //        {
        //            //RunCustomEvent(queryID, pageContent);
        //        }
        //        else
        //        {
        //            //RunCustomEvent(queryID, pageContent);
        //        }
        //    }
        public void RogueClickEvent(string clickInfo, Dictionary<string, string> pageContent)
        {
            string clickPortion;
            Dictionary<int, string> paramPairs;
            List<string> setParams = new List<string>();
            clickInfo = clickInfo.Replace((char)160, (char)32);
            if (clickInfo.Contains("*"))
            {
                clickPortion = clickInfo.Substring(0, clickInfo.IndexOf("*"));
                string paramPortion = stringHelper.GetStringBetween(clickInfo, "*", "", true);
                paramPairs = new Dictionary<int, string>();
                var segments = Regex.Split(paramPortion, MutliSegmentEnum.GetOutsideSingleQuotesPattern(new string[] { "*" }));
                var dict = segments.Select(x => Regex.Split(x, MutliSegmentEnum.GetOutsideSingleQuotesPattern(new string[] { "=" }))).ToDictionary(x => int.Parse(x[0]), x => x[2]);
                foreach(var kvp in dict)
                {
                    string value;
                    if (kvp.Value.StartsWith("'"))
                    {
                        value = kvp.Value.Substring(1, kvp.Value.Length - 2);
                    }
                    else
                    {
                        value = kvp.Value;
                    }
                    setParams.Add(value);
                    currParams.FindChangeIfNotFound(kvp.Key, value);
                    //pageContent.FindChangeIfNotFound(queryID.ToString() + "_" + kvp.Key, value);
                }
            }
            else
            {
                clickPortion = clickInfo;
            }
            string[] parts = clickPortion.Split('_');
            String clickAction = parts[0];
            int queryID = int.Parse(parts[1].ToString());
            UISection clickedSection = sections.Where(x => x.QueryOID() == queryID.ToString()).First();
            //*Could maybe just use this logic instead of setting parameters in onclicks** this just checks the page content for the id of the param and sets the value if found
            foreach(HQLParam paramOID in clickedSection.parameters.Values.Where(x => !setParams.Contains(x.ParamOID())))
            {
                string foundVal = "";
                pageContent.TryGetValue(queryID.ToString() + "_" + paramOID.ParamOID(), out foundVal);
                //if(foundVal == null)
                //{
                //    currParams.TryGetValue(int.Parse(paramOID.ParamOID()), out foundVal);
                //}                
                if (foundVal != "" && foundVal != null)
                {
                    currParams[int.Parse(paramOID.ParamOID())] = foundVal;
                }
            }
            foreach(string key in pageContent.Keys)
            {
                if (key.ToUpper().StartsWith("PARAM:"))
                {

                }
            }
            switch (clickAction.ToUpper())
            {
                //*So far just for single table insert
                case "INSERT":
                    String tableID = parts[2].ToString();
                    IRogueTable tbl = new IORecordID(tableID).ToTable();
                    IRogueRow newRow = tbl.NewWriteRow();
                    foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("INSERTVALUE_" + queryID.ToString() + "_" + tableID)))
                    {
                        String columnIDStr = selectValues.Key.Split('_')[3];
                        String value = selectValues.Value;
                        newRow.NewWritePair(new ColumnRowID(columnIDStr), value);
                    }
                    tbl.Write();
                    ResetPage();
                    break;
                case "UPDATE":
                    string rowID = parts[2];
                    foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("UPDATEVALUE_" + queryID.ToString() + "_" + rowID)))
                    {
                        ColumnRowID updateColumnID = new ColumnRowID(selectValues.Key.Split('_')[3]);
                        IORecordID updateTableID = new IORecordID(updateColumnID.ToOwnerIORecord());
                        UpdateHQLStatement updateStatement = new UpdateHQLStatement(updateTableID);
                        String value = selectValues.Value;
                        updateStatement.AddUpdateField(updateColumnID, value);
                        updateStatement.AddWhereClause(-1012, rowID);
                        updateStatement.Execute();
                    }
                    ResetPage();
                    break;
                case "UPDATESINGLE":
                    string rowSingleID = parts[2];
                    ColumnRowID singleColumnID = new ColumnRowID(parts[3]);
                    string valueKey = "UPDATEVALUE_" + queryID.ToString() + "_" + rowSingleID + "_" + singleColumnID.ToString();
                    string updateValue = pageContent[valueKey];
                    IORecordID singleTableID = new IORecordID(singleColumnID.ToOwnerIORecord());
                    UpdateHQLStatement singleStatement = new UpdateHQLStatement(singleTableID);
                    singleStatement.AddUpdateField(singleColumnID, updateValue);
                    singleStatement.AddWhereClause(-1012, rowSingleID);
                    singleStatement.Execute();
                    ResetPage();
                    break;
                case "SELECT":
                    int paramID = int.Parse(parts[2]);
                    string paramValue = parts[3];
                    currParams.FindChangeIfNotFound(paramID, paramValue);
                    List<SectionType> affectedSections = sections.Where(sect => sect.parameters.Any(pList => pList.Key == paramID)).ToList();
                    Parallel.ForEach(affectedSections, x => x.RefreshSegment(currParams));
                    break;
                case "PAGE":
                    pageID = int.Parse(parts[2]);
                    ResetPage();
                    break;
                case "RELOAD":
                    ResetPage();
                    break;
                case "SWAP":
                     var filledQry = new SelectHQLStatement("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"7621\" SELECT *");
                    filledQry.Fill();
                    IMultiRogueRow sectionQueryInfoQry = filledQry.TopRows().First();
                    string sectionQrySwap = sectionQueryInfoQry.GetValue("QUERY_TXT");
                    string sectionID = parts[2];
                    string hqlQryID = parts[3];                    
                    sectionQrySwap = sectionQrySwap.Replace("@SECTIONID", sectionID).Replace("@HQLQUERYID", hqlQryID);
                    var filledQry2 = new SelectHQLStatement(sectionQrySwap);
                    filledQry2.Fill();
                    hqlSyntaxV3.filledSegments.IMultiRogueRow sectionResultsRow = filledQry2.TopRows().First();
                    SectionType thsSect = sections.Where(sect => sect.SectionOID() == sectionID).First();
                    thsSect.ResetUISection(sectionResultsRow, currParams);
                    BuildPage();
                    break;
                case "CREATE":
                    string objectRogueID = currParams[7470];
                    string objTyp = BinaryDataTable.ioRecordTable.rows[new IORecordID(objectRogueID)].MetaRecordType();
                    if (objTyp.ToLower() == "database")
                    {
                        RogueDatabase<DataRowID> thsDb = new RogueDatabase<DataRowID>(new IORecordID(objectRogueID));
                        thsDb.GetTable(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
                    }
                    else if (objectRogueID == "-1004")
                    {
                        RogueBundle rootBundle = new RogueBundle(new IORecordID(objectRogueID));
                        rootBundle.GetBundle(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
                    }
                    else if (objTyp.ToLower() == "bundle")
                    {
                        RogueBundle thsBundle = new RogueBundle(new IORecordID(objectRogueID));
                        thsBundle.GetDatabase<DataRowID>(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
                    }
                    ResetPage();
                    break;
                case "EXECUTE":
                    
                    break;
                case "SETQUERY":
                    Dictionary<string,string> pageParams = new Dictionary<string,string>();
                    foreach(var pair in pageContent.Where(pair => pair.Key.StartsWith("PARAM:")))
                    {
                        if(pair.Value != "")
                        {
                            pageParams.Add(pair.Key.AfterFirstChar(':'), pair.Value);
                        }
                    }
                    string qry = currParams[8494];
                    currParams[8494] = new SelectHQLStatement(qry, pageParams).finalQuery;
                    ResetPage();
                    break;
            }            
        }
        protected abstract SectionType NewSection(IMultiRogueRow sectionRow, Dictionary<int, string> pageContent);
    }
}
