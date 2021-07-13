//using files_and_folders;
//using FilesAndFolders;
//using rogue_core.rogueCore.bundle;
//using rogue_core.rogueCore.database;
//using rogue_core.rogueCore.id.rogueID;
//using rogue_core.rogueCore.row;
//using rogue_core.rogueCore.table;
//using rogue_core.rogueCore.table.encoded;
//using rogueCore.hqlSyntaxV2.filledSegments;
//using rogueCore.hqlSyntaxV2.segments.update;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace rogueCore.rogueUIV2
//{
//    public abstract class UIPage<SectionType> where SectionType : UISection
//    {
//        protected SectionType baseSegment { private set; get; }
//        const int storedProcID = 7568;
//        MultiRogueRow pageHQLQueryRow;
//        MultiRogueRow pageResultsRow;
//        int pageID; 
//        internal List<SectionType> sections { get; private set; }
//        Dictionary<int, string> currParams = new Dictionary<int, string>();
//        protected UIPage(int pageID)
//        {
//            this.pageID = pageID;
//            ResetPage();
//            //pageHQLQueryRow = new HumanHQLStatement("FROM HQL_QUERIES SELECT * WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\"").TopRows()[0];
//            //string fullPageQry = pageHQLQueryRow.values["QUERY_TXT"].Value;
//            //fullPageQry = fullPageQry.Replace("@PAGEOID", pageID.ToString());
//            //pageResultsRow = new HumanHQLStatement(fullPageQry).TopRows()[0];
//            //baseSegment = NewSection(pageResultsRow.childRows[0], new Dictionary<string, string>());
//            //for (int i = 1; i < pageResultsRow.childRows.Count; i++)
//            //{
//            //    SectionType thsSection = NewSection(pageResultsRow.childRows[i], new Dictionary<string, string>());
//            //    sections.Add(thsSection);
//            //}
//        }
//        void ResetPage()
//        {
//            //if(pageID == 0)
//            //{
//            //   pageID = this.pageID;
//            //}
//            //pageID = 7658;
//            sections = new List<SectionType>();
//            //sectionsByParam = new Dictionary<int, List<SectionType>>();
//            pageHQLQueryRow = new FilledHQLQuery("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\" SELECT * ").Fill().TopRows().First();
//            string fullPageQry = pageHQLQueryRow.values["QUERY_TXT"].Value;
//            fullPageQry = fullPageQry.Replace("@PAGE_OID", pageID.ToString());
//            pageResultsRow = new FilledHQLQuery(fullPageQry).Fill().TopRows().First();
//            ////Page Name
//            currParams.FindChangeIfNotFound(8799, pageResultsRow.GetValue("PAGE", 7548).ToDecodedString());
//            //////Page ID
//            //currParams.FindChangeIfNotFound(8804, pageResultsRow.GetValue("PAGE", 8804).ToDecodedString());
//            //////Page desc
//            //currParams.FindChangeIfNotFound(8801, pageResultsRow.GetValue("PAGE", 8801).ToDecodedString());
//            baseSegment = NewSection(pageResultsRow.childRows[0], currParams);
//            for (int i = 1; i < pageResultsRow.childRows.Count; i++)
//            {
//                SectionType thsSection = NewSection(pageResultsRow.childRows[i], currParams);
//                sections.Add(thsSection);
//            }
//            ////Page Name
//            //currParams.FindChangeIfNotFound(8799, pageID);
//            ////Page ID
//            //currParams.FindChangeIfNotFound(8804, kvp.Value);
//            ////Page desc
//            //currParams.FindChangeIfNotFound(8801, kvp.Value);
//        }
//        protected abstract void BuildPage(); 
//        //public void RogueClickEventOLD(string clickInfo, Dictionary<String, String> pageContent)
//        //    {
//        //        string[] parts = clickInfo.Split('_');
//        //        String clickAction = parts[0];
//        //        int queryID = int.Parse(parts[1].ToString());
//        //        String tableID = parts[2].ToString();
//        //        if (clickAction.ToUpper().Equals("INSERT"))
//        //        {
//        //            IRogueTable tbl = new IORecordID(tableID).ToTable();
//        //            IRogueRow newRow = tbl.NewIWriteRow();
//        //            foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("INSERTVALUE_" + queryID.ToString() + "_" + tableID)))
//        //            {
//        //                String columnID = selectValues.Key.Split('_')[3];
//        //                String value = selectValues.Value;
//        //                newRow.NewWritePair(new ColumnRowID(columnID), value);
//        //            }
//        //            tbl.Write();
//        //            //RefreshSegment(queryID, tableID);
//        //        }
//        //        else if (clickAction.ToUpper().Equals("UPDATE"))
//        //        {
//        //            string rowID = parts[3];
//        //            UpdateStatement updateStatement = new UpdateStatement(new IORecordID(tableID));
//        //            updateStatement.AddWhereClause(-1012, rowID);
//        //            foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("UPDATEVALUE_" + queryID.ToString() + "_" + tableID + "_" + rowID)))
//        //            {
//        //                ColumnRowID columnID = new ColumnRowID(selectValues.Key.Split('_')[4]);
//        //                String value = selectValues.Value;
//        //                updateStatement.AddUpdateField(columnID, value);
//        //            }
//        //            updateStatement.ExecuteUpdate();
//        //            //RefreshSegment(queryID, tableID);
//        //        }
//        //        else if (clickAction.ToUpper().Equals("CUSTOM"))
//        //        {
//        //            //RunCustomEvent(queryID, pageContent);
//        //        }
//        //        else
//        //        {
//        //            //RunCustomEvent(queryID, pageContent);
//        //        }
//        //    }
//        public void RogueClickEvent(string clickInfo, Dictionary<String, String> pageContent)
//        {
//            string clickPortion;
//            Dictionary<int, string> paramPairs;
//            List<string> setParams = new List<string>();
//            if (clickInfo.Contains("*"))
//            {
//                clickPortion = clickInfo.Substring(0, clickInfo.IndexOf("*"));
//                string paramPortion = stringHelper.GetStringBetween(clickInfo, "*", "", true);
//                paramPairs = new Dictionary<int, string>();
//                var dict = paramPortion.Split('*').Select(x => x.Split('=')).ToDictionary(x => int.Parse(x[0]), x => x[1]);
//                foreach(var kvp in dict)
//                {
//                    setParams.Add(kvp.Value);
//                    currParams.FindChangeIfNotFound(kvp.Key, kvp.Value); 
//                }
//            }
//            else
//            {
//                clickPortion = clickInfo;
//            }
//            string[] parts = clickPortion.Split('_');
//            String clickAction = parts[0];
//            int queryID = int.Parse(parts[1].ToString());
//            UISection clickedSection = sections.Where(x => x.QueryOID() == queryID.ToString()).First();
//            //*Could maybe just use this logic instead of setting parameters in onclicks** this just checks the page content for the id of the param and sets the value if found
//            foreach(HQLParam paramOID in clickedSection.parameters.Values.Where(x => !setParams.Contains(x.ParamOID())))
//            {
//                string foundVal = "";
//                pageContent.TryGetValue(queryID.ToString() + "_" + paramOID.ParamOID(), out foundVal);
//                if(foundVal != "" && foundVal != null)
//                {
//                    currParams[int.Parse(paramOID.ParamOID())] = foundVal;
//                }
//            }
//            //String tableID = parts[2].ToString();
//            switch (clickAction.ToUpper())
//            {
//                //*So far just for single table insert
//                case "INSERT":
//                    String tableID = parts[2].ToString();
//                    IRogueTable tbl = new IORecordID(tableID).ToTable();
//                    IRogueRow newRow = tbl.NewIWriteRow();
//                    foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("INSERTVALUE_" + queryID.ToString() + "_" + tableID)))
//                    {
//                        String columnIDStr = selectValues.Key.Split('_')[3];
//                        String value = selectValues.Value;
//                        newRow.NewWritePair(new ColumnRowID(columnIDStr), value);
//                    }
//                    tbl.Write();
//                    ResetPage();
//                    break;
//                case "UPDATE":
//                    //String updateTableID = parts[2].ToString();
//                    string rowID = parts[2];
//                    foreach (var selectValues in pageContent.Where(kvp => kvp.Key.StartsWith("UPDATEVALUE_" + queryID.ToString() + "_" + rowID)))
//                    {
//                        ColumnRowID updateColumnID = new ColumnRowID(selectValues.Key.Split('_')[3]);
//                        IORecordID updateTableID = new IORecordID(updateColumnID.ToOwnerIORecord());
//                        UpdateStatement updateStatement = new UpdateStatement(updateTableID);
//                        String value = selectValues.Value;
//                        updateStatement.AddUpdateField(updateColumnID, value);
//                        updateStatement.AddWhereClause(-1012, rowID);
//                        updateStatement.ExecuteUpdate();
//                    }
//                    ResetPage();
//                    break;
//                case "UPDATESINGLE":
//                    //String updateTableID = parts[2].ToString();
//                    string rowSingleID = parts[2];
//                    ColumnRowID singleColumnID = new ColumnRowID(parts[3]);
//                    string valueKey = "UPDATEVALUE_" + queryID.ToString() + "_" + rowSingleID + "_" + singleColumnID.ToString();
//                    string updateValue = pageContent[valueKey];
//                    IORecordID singleTableID = new IORecordID(singleColumnID.ToOwnerIORecord());
//                    UpdateStatement singleStatement = new UpdateStatement(singleTableID);
//                    singleStatement.AddUpdateField(singleColumnID, updateValue);
//                    singleStatement.AddWhereClause(-1012, rowSingleID);
//                    singleStatement.ExecuteUpdate();
//                    //ColumnRowID columnID = new ColumnRowID(selectValues.Key.Split('_')[3]);
//                    //IORecordID updateTableID = new IORecordID(columnID.ToOwnerIORecord());
//                    //UpdateStatement updateStatement = new UpdateStatement(updateTableID);
//                    ////String value = selectValues.Value;
//                    //updateStatement.AddUpdateField(columnID, value);
//                    //updateStatement.AddWhereClause(-1012, rowID);
//                    //updateStatement.ExecuteUpdate();
//                    ResetPage();
//                    break;
//                case "SELECT":
//                    int paramID = int.Parse(parts[2]);
//                    string paramValue = parts[3];
//                    currParams.FindChangeIfNotFound(paramID, paramValue);
//                    List<SectionType> affectedSections = sections.Where(sect => sect.parameters.Any(pList => pList.Key == paramID)).ToList();
//                    Parallel.ForEach(affectedSections, x => x.RefreshSegment(currParams));
//                    //affectedSections.ForEach(x => x.RefreshSegment(currParams));
//                    break;
//                case "PAGE":
//                    //SetPage(int.Parse(parts[2]));
//                    //ResetPage(int.Parse(parts[2]));
//                    pageID = int.Parse(parts[2]);
//                    ResetPage();
//                    break;
//                case "RELOAD":
//                    //SetPage(int.Parse(parts[2]));
//                    //ResetPage(int.Parse(parts[2]));
//                    ResetPage();
//                    break;
//                case "SWAP":
//                    MultiRogueRow sectionQueryInfoQry = new FilledHQLQuery("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"7621\" SELECT *").Fill().TopRows().First();
//                    string sectionQrySwap = sectionQueryInfoQry.values["QUERY_TXT"].Value;
//                    string sectionID = parts[2];
//                    string hqlQryID = parts[3];
//                    sectionQrySwap = sectionQrySwap.Replace("@SECTIONID", sectionID).Replace("@HQLQUERYID", hqlQryID);
//                    hqlSyntaxV2.filledSegments.MultiRogueRow sectionResultsRow = new FilledHQLQuery(sectionQrySwap).Fill().TopRows().First();
//                    SectionType thsSect = sections.Where(sect => sect.SectionOID() == sectionID).First();
//                    thsSect.ResetUISection(sectionResultsRow, currParams);
//                    BuildPage();
//                    break;
//                case "CREATE":
//                    string objectRogueID = currParams[7470];
//                    string objTyp = FullTables.ioRecordTable.rows[new IORecordID(objectRogueID)].MetaRecordType().DisplayValue();
//                    if (objTyp.ToLower() == "database")
//                    {
//                        RogueDatabase<DataRowID> thsDb = new RogueDatabase<DataRowID>(new IORecordID(objectRogueID));
//                        thsDb.GetTable(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
//                    }
//                    else if (objectRogueID == "-1004")
//                    {
//                        RogueBundle rootBundle = new RogueBundle(new IORecordID(objectRogueID));
//                        rootBundle.GetBundle(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
//                    }
//                    else if (objTyp.ToLower() == "bundle")
//                    {
//                        RogueBundle thsBundle = new RogueBundle(new IORecordID(objectRogueID));
//                        thsBundle.GetDatabase<DataRowID>(pageContent["CREATE_" + queryID.ToString() + "_" + "NAME"], pageContent["CREATE_" + queryID.ToString() + "_" + "DESC"]);
//                    }
//                    ResetPage();
//                    break;
//            }
            
//        }
//        protected abstract SectionType NewSection(MultiRogueRow sectionRow, Dictionary<int, string> pageContent);
//    }
//}
