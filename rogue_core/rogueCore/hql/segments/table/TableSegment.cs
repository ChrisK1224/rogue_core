using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.hql;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.id.rogueID.hqlIDs;
using rogue_core.rogueCore.pair;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using static rogue_core.rogueCore.queryResults.HQLCircularQuery;
using rogue_core.rogueCore.hql.hqlSegments.where;
using rogue_core.rogueCore.hql.hqlSegments.join;
using rogue_core.RogueCode.hql.hqlSegments.where;
using static rogue_core.rogueCore.hql.hqlSegments.where.WhereClauses;
using static rogue_core.RogueCode.hql.hqlSegments.where.WhereClause;
using rogue_core.rogueCore.hql.segments.selects;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.table;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.hql.segments.tableInfo;
using rogue_core.rogueCore.hql.segments;
using rogue_core.rogueCore.binary;
using static rogue_core.rogueCore.binary.prefilled.ColumnTable;

namespace rogue_core.RogueCode.hql.hqlSegments.table
{
    public class TableSegment
    {
        internal Boolean allColumns = false;
        public Dictionary<String, LocationColumn> childJoinRefs = new Dictionary<String, LocationColumn>();
        public LevelColumns selectedCols = new LevelColumns();
        public TableInfo tableInfo;
        public String tableRefName { get { return tableInfo.tableRefName; } }
        internal WhereClauses whereClauses;
        public JoinClauses joinClauses { get; private set; }
        public int level { get; set; }
        public static String HumanToEncodedHQL(String fullHQL, Dictionary<String, int> tableRefIDs)
        {
            String encodedHQL = "";
            String[] parts = SplitHumanHQL(fullHQL);
            Boolean hasJoin = false;
            TableInfo thsTbl = TableInfo.FromHumanHQL(parts[0].AfterFirstSpace());
            tableRefIDs.Add(thsTbl.tableRefName, thsTbl.tableID);
            encodedHQL += thsTbl.GetTableHQLText();
            foreach (String thsSegment in parts)
            {
                String finalPortion = thsSegment.AfterFirstSpace();
                if (thsSegment.StartsWith(JoinClauses.humanHQLSplitter))
                {
                    encodedHQL += JoinClauses.HumanToEncodedHQL(finalPortion, tableRefIDs);
                    hasJoin = true;
                }
                else if (thsSegment.StartsWith(WhereClauses.humanHQLSplitter))
                {
                    encodedHQL += WhereClauses.HumanToEncodedHQL(finalPortion, tableRefIDs, thsTbl.tableRefName);
                }
                else if (thsSegment.StartsWith(FullColumnLocations.humanHQLSplitter))
                {
                    encodedHQL += LevelColumns.HumanToEncodedHQL(finalPortion, tableRefIDs, thsTbl.tableRefName);
                }
            }
            //*If no join clause add assumed root clause
            if (!hasJoin)
            {
                encodedHQL += WhereClauses.RootWhereClauses();
            }
            return encodedHQL;
        }
        private static String[] SplitHumanHQL(String humanHQL)
        {
            String[] splitStrings = new String[4];
            splitStrings[0] = TableInfo.humanHQLSplitter;
            splitStrings[1] = JoinClauses.humanHQLSplitter;
            splitStrings[2] = FullColumnLocations.humanHQLSplitter;
            splitStrings[3] = WhereClauses.humanHQLSplitter;
            List<int> foundIndexs = new List<int>();
            foreach (String delimiter in splitStrings)
            {
                int foundIndex = humanHQL.IndexOf(delimiter);
                if (foundIndex > -1)
                {
                    foundIndexs.Add(foundIndex);
                }
            }
            foundIndexs.Sort();
            String[] segments = new string[foundIndexs.Count];
            for (int i = 0; i < foundIndexs.Count; i++)
            {
                if (i == (foundIndexs.Count - 1))
                {
                    segments[i] = humanHQL.Substring(foundIndexs[i], humanHQL.Length - foundIndexs[i]).Trim();
                }
                else
                {
                    segments[i] = humanHQL.Substring(foundIndexs[i], foundIndexs[i + 1] - foundIndexs[i]).Trim();
                }
            }
            return segments;
        }
        public TableSegment(String tableSegment)
        {
            tableSegment = tableSegment.Trim();
            //this.matrixTableID = matrixTableID;
            // SetTableID(tableSegment);
            this.tableInfo = new TableInfo(tableSegment);
            whereClauses = new WhereClauses(tableSegment);
            //mergedColsReference.Add(matrixTableID,new FullColumnLocations(tableSegment, tableID, matrixTableID));
            selectedCols.Add(tableInfo.tableRefName, new FullColumnLocations(tableSegment, tableInfo.tableID, tableInfo.tableRefName));
            joinClauses = new JoinClauses(tableSegment);
            //this.matrixTableID = matrixTableID;
            //CheckRowToColumn()
        }
        public TableSegment(TableInfo tblInfo, WhereClauses whereRestrictions, FullColumnLocations allCols, JoinClauses joinClauses)
        {
            this.tableInfo = tblInfo;
            this.whereClauses = whereRestrictions;
            this.joinClauses = joinClauses;
            this.selectedCols.Add(tableRefName,allCols);
        }
        public String GetHQLText()
        {
            return "^" + tableInfo.GetTableHQLText() + selectedCols.GetHQLText() + whereClauses.GetHQLText() + joinClauses.GetHQLText();
        }
        // public DataFilter(List<FullColumnLocation> selectedCols, List<UpLadderRowMatch> joinClauses, List<WhereClause> whereRestrictions)
        // {
        //     this.selectedCols = selectedCols;
        //     //this.joinClauses = joinClauses;
        //     this.whereRestrictions = whereRestrictions;
        // }
        // public void LoadColumns(List<String> columnSnippets, IORecordID defaultTable)
        // {
        //     foreach (String colSnippet in columnSnippets)
        //     {
        //         //FullColumnLocation  selectedCol;
        //         if (colSnippet.Equals("*"))
        //         {
        //             allColumns = true;
        //             break;
        //         }
        //         else
        //         {
        //             //String tblSnippet = colSnippet.Substring(0, colSnippet.Length - 2);
        //             String[] snips = colSnippet.Split('.');
        //             LevelNum lvlNum = int.Parse(snips[0]);
        //             HQLTableOrderNum orderNum = int.Parse(snips[1]);
        //             ColumnRowID columnRowID = int.Parse(snips[2]);
        //             String columnAliasName = "";
        //             if (snips.Length == 4)
        //             {
        //                 columnAliasName = snips[3];
        //             }
        //             selectedCols.Add(new FullColumnLocation(orderNum, lvlNum, columnRowID, columnAliasName));
        //             if(lvlNum.Equals(thsLevelNum) && orderNum.Equals(thsTblNum)){
        //                 selectedLocalCols.Add(columnRowID);
        //             }
        //         }
        //     }
        // }
        // public Boolean checkApprovedRow(IRogueRow checkRow)
        // {
        //     Boolean isApproved = false;
        //     Boolean alreadyAdded = false;
        //     //*"Equals" should become generics using delegrates where the equlas method always takes in T value (string or rogue maybe) and returns a Boolean to say if valid based on criteria or not
        //     if(whereRestrictions.Count == 0)
        //     {
        //         isApproved = true;
        //     }
        //     foreach(WhereClause<String> thsFilter in whereRestrictions)
        //     {
        //         if(thsFilter.localRowColID == 0)
        //         {
        //             if(checkRow.partial_id == long.Parse(thsFilter.value))
        //             {
        //                 isApproved = true;
        //             }
        //         }
        //         else
        //         {
        //             if (checkRow.data_pairs[thsFilter.localRowColID].realValue.Equals(thsFilter.value))
        //             {
        //                 isApproved = true;
        //             }
        //         }
        //     }
        //     if (isApproved)
        //     {
        //         //*Usually will be just one caluse here. Each clause conains the Rows associated with the match
        //         //*For now always assuming check value here is the full ID of the row (Should update to paritalID here - update impor t logic to store paritla ID of parent since the parent ID is guranteed to be the name of the column
        //         foreach (WhereClause<RogueTableID> joinedTable in joinClauses)
        //         {
        //             if (joinedTable.localRowColID.Equals(-1))
        //             {//*This means a join all so add child row to all parents (might need tocheck logic).
        //                 foreach(RogueRow parent in parentTables.FindRealTable(joinedTable.value).baseRows.Values)
        //                 {
        //                     parent.childRows.Add(checkRow);
        //                     checkRow.parentRow = parent;
        //                     if (isResult)
        //                     {
        //                         //*Only check for and add result ROw if parent row is found
        //                         ResultRow resultRow = SyncToResultRow(checkRow);
        //                         alreadyAdded = true;
        //                         ResultRow resultParent = null;
        //                         ResultTable parentResultTbl = parentTables.FindResultTableByRealTableID(joinedTable.value);
        //                         resultParent = parentResultTbl.baseRows[parentResultTbl.mapRowToResultRow[parent.full_id]] as ResultRow;
        //                         resultParent.childRows.Add(resultRow);
        //                         resultRow.parentRow = resultParent;
        //                     }
        //                     isApproved = true;
        //                 }
        //             }
        //             else if (checkRow.data_pairs.ContainsKey(joinedTable.localRowColID))
        //             {
        //                 //**Need to change the location of SyncToResultRow otherwise it wont get added if no joins

        //                 //*CheckParentRowsList for the partial ID of the value stored in the specified column of local rows
        //                 IRogueRow parent = null;
        //                 IRogueTable realTbl = parentTables.FindRealTable(joinedTable.value);
        //                 realTbl.baseRows.TryGetValue(long.Parse(checkRow.data_pairs[joinedTable.localRowColID].realValue), out parent);
        //                 if (parent != null)
        //                 {
        //                     parent.childRows.Add(checkRow);
        //                     checkRow.parentRow = parent;
        //                     if (isResult)
        //                     {
        //                         //*Only check for and add result ROw if parent row is found
        //                         ResultRow resultRow = SyncToResultRow(checkRow);
        //                         alreadyAdded = true;
        //                         ResultRow resultParent = null;
        //                         ResultTable parentResultTbl = parentTables.FindResultTableByRealTableID(joinedTable.value);
        //                         resultParent = parentResultTbl.baseRows[parentResultTbl.mapRowToResultRow[parent.full_id]] as ResultRow;
        //                         resultParent.childRows.Add(resultRow);
        //                         resultRow.parentRow = resultParent;
        //                     }
        //                     isApproved = true;
        //                 }
        //             }
        //         }
        //     }
        //     if (isApproved && !alreadyAdded && isResult)
        //     {
        //         //resultTable.baseRows.Add(resultRow);
        //         //resultTable.mapRowToResultRow.Add(thsRow.full_id, resultRow.partial_id);
        //         SyncToResultRow(checkRow);
        //     }
        //     return isApproved;
        // }
        // public ResultRow SyncToResultRow(IRogueRow thsRow)
        // {
        //     ResultRow resultRow = resultTable.newRow();
        //     //resultRow.repRowID.Add(resultRow.full_id);
        //     if (allColumns)
        //     {
        //         resultRow.resultPairs.SyncNumberIDPairs(thsRow.data_pairs);
        //     }
        //     else
        //     {
        //         foreach (KeyValuePair<String, long> thsCol in selectedCols)
        //         {
        //             //String blah = thsRow.ownToString();
        //             if (thsCol.Key.Equals(thsRow.get_parent_id().ToString()))
        //             {
        //                 resultRow.resultPairs.Add(thsRow.data_pairs[thsCol.Value]);
        //             }
        //             else
        //             {
        //                 //*More work
        //                 resultRow.resultPairs.Add(UpLadderValue(thsRow, thsCol));
        //             }
        //         }
        //     }
        //     if (resultRow.resultPairs.Count > 0)
        //     {
        //         resultTable.baseRows.Add(resultRow);
        //         resultTable.mapRowToResultRow.Add(thsRow.full_id, resultRow.partial_id);

        //     }
        //     return resultRow;
        // }
        // private IRoguePair UpLadderValue(IRogueRow thsRow, KeyValuePair<String, long> TableAndCol)
        // {
        //     if (thsRow.parentRow.get_owner_container_id().Equals(TableAndCol.Key))
        //     {
        //         return thsRow.parentRow.data_pairs[TableAndCol.Value];
        //     }
        //     else
        //     {
        //         return UpLadderLoop(thsRow.parentRow, TableAndCol);
        //     }
        // }
        // private IRoguePair UpLadderLoop(IRogueRow thsRow, KeyValuePair<String, long> TableAndCol)
        // {
        //     if (thsRow.parentRow.get_owner_container_id().Equals(TableAndCol.Key))
        //     {
        //         return thsRow.parentRow.data_pairs[TableAndCol.Value];
        //     }
        //     else
        //     {
        //         return UpLadderLoop(thsRow.parentRow, TableAndCol);
        //     }
        // }
        // internal void LoadWhereRestrictions(List<String> whereSnippets)
        // {
        //     WhereClauses 
        //     foreach (String whereSnippet in whereSnippets)
        //     {
        //         // int charIndex = whereSnippet.IndexOfAny(WhereClause.evalChars());
        //         // ColumnRowID colID = new ColumnRowID(whereSnippet.Substring(0, charIndex));
        //         // WhereClause.EvaluationTypes thsEvalTyp = (WhereClause.EvaluationTypes)whereSnippet[charIndex];
        //         // String val = whereSnippet.Substring(charIndex + 1, (whereSnippet.Length - charIndex) - 1);
        //         whereRestrictions.Add(whereSnippet);
        //     }
        // }
        // internal void LoadJoins(List<String> joinSnippets)
        // {
        //     //*Dupe code from loadWhereRestrictions
        //     foreach (String whereSnippet in joinSnippets)
        //     {
        //         joinClauses.Add(new UpLadderRowMatch(whereSnippet));
        //         // if (whereSnippet.IndexOfAny(WhereClause<String>.evalChars()) == -1)
        //         // {
        //         //     //int charIndex = whereSnippet.IndexOfAny(WhereClause<String>.evalChars());
        //         //     //long colID = long.Parse(whereSnippet.Substring(0, charIndex));
        //         //     //WhereClause<RogueTableID>.EvaluationTypes thsEvalTyp = (WhereClause<RogueTableID>.EvaluationTypes)whereSnippet[charIndex];
        //         //     //IORecordID val = new IORecordID(whereSnippet);
        //         //     joinClauses.Add(new UpLadderRowMatch(whereSnippet);
        //         // }
        //         // else
        //         // {
        //         //     int charIndex = whereSnippet.IndexOfAny(WhereClause<String>.evalChars());
        //         //     ColumnRowID colID = new ColumnRowID(whereSnippet.Substring(0, charIndex));
        //         //     WhereClause<IORecordID>.EvaluationTypes thsEvalTyp = (WhereClause<IORecordID>.EvaluationTypes)whereSnippet[charIndex];
        //         //     IORecordID val = new IORecordID(whereSnippet.Substring(charIndex + 1, (whereSnippet.Length - charIndex) - 1));
        //         //     joinClauses.Add(new WhereClause<IORecordID>(colID, thsEvalTyp, val));
        //         // }
        //     }
        // }
        //private void LoadSnippet(List<String> whereSnippets)
        //{
        //    foreach (String whereSnippet in whereSnippets)
        //    {
        //        int charIndex = whereSnippet.IndexOfAny(WhereClause<String>.evalChars());
        //        long colID = long.Parse(whereSnippet.Substring(0, charIndex));
        //        WhereClause<String>.EvaluationTypes thsEvalTyp = (WhereClause<String>.EvaluationTypes)whereSnippet[charIndex];
        //        String val = whereSnippet.Substring(charIndex + 1, (whereSnippet.Length - charIndex) - 1);
        //        whereRestrictions.Add(new WhereClause<String>(colID, thsEvalTyp, val));
        //    }
        //}
        // internal void CheckReccurance(String txt)
        // {
        //     if (txt == "")
        //     {
        //         circularColumn = 0;
        //     }
        //     else
        //     {
        //         String colID = txt.Substring(2, txt.Length - 2);
        //         circularColumn = int.Parse(colID);
        //         if (txt.Substring(1, 1).Equals("*"))
        //         {
        //             circularDirection = LadderDirection.downladder;
        //         }
        //         else
        //         {
        //             circularDirection = LadderDirection.upladder;
        //         }
        //     }
        // }
        internal Boolean CheckAddCol(IRoguePair thsPair)
        {
            return true;
            //* TODO need to figure out how to handle the check column since it could be needed further down the query but not displayed at this point.
            // if(selectedCols.Count == 0)
            // {
            //    return true;
            // }
            // else if (selectedCols.Contains(thsPair.KeyColumnID))
            // {
            //    return true;
            // }
            // else
            // {
            //    return false;
            // }
        }
        internal Boolean CheckAddRow(IRogueRow thsRow)
        {
            Boolean isApproved = true;
            if (whereClauses.Count == 0)
            {
                isApproved = true;
            }
            else
            {
                if (whereClauses.compareType.Equals(CompareTypes.and))
                {
                    foreach (WhereClause thsFilter in whereClauses)
                    {
                        //**TEMPORARY ALL VALUES ARE CONVERTED TO UPPER BEFORE TESTING***
                        //String test = thsRow.IGetBasePair(thsFilter.localRowColID).WriteValue();
                        //DecodedRowID test2 = thsFilter.value;
                        String RowStrValue = thsRow.IGetBasePair(thsFilter.localRowColID).DisplayValue();
                        String WhereStrValue = thsFilter.thsValue;
                        DecodedRowID rowValue = RowStrValue.ToUpper().ToDecodedRowID();
                        DecodedRowID filterValue = WhereStrValue.ToUpper().ToDecodedRowID();
                        //if (!(thsRow.IGetBasePair(thsFilter.localRowColID).WriteValue() == thsFilter.value.ToString()))
                        if (!(rowValue.Equals(filterValue)))
                        {
                            return false;
                        }
                    }
                }
                else if (whereClauses.compareType.Equals(CompareTypes.or))
                {
                    isApproved = false;
                    foreach (WhereClause thsFilter in whereClauses)
                    {
                        //**TEMPORARY ALL VALUES ARE CONVERTED TO UPPER BEFORE TESTING***
                        //String test = thsRow.IGetBasePair(thsFilter.localRowColID).WriteValue();
                        //DecodedRowID test2 = thsFilter.value;
                        //String test3 = thsRow.IGetBasePair(thsFilter.localRowColID).DisplayValue();
                        //if (thsRow.IGetBasePair(thsFilter.localRowColID).WriteValue() == thsFilter.value.ToString())
                        String RowStrValue = thsRow.IGetBasePair(thsFilter.localRowColID).DisplayValue();
                        String WhereStrValue = thsFilter.thsValue;
                        DecodedRowID rowValue = RowStrValue.ToUpper().ToDecodedRowID();
                        DecodedRowID filterValue = WhereStrValue.ToUpper().ToDecodedRowID();
                        if (rowValue.Equals(filterValue))
                        {
                            return true;
                        }
                    }
                }
            }
            return isApproved;
        }
        private static List<T> SplitStringToNumList<T>(String segment)
        {
            List<T> lstSegments = new List<T>();
            foreach (String col in segment.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                T value = (T)Convert.ChangeType(col, typeof(T));
                lstSegments.Add(value);
            }
            return lstSegments;
        }
        //private void SetTableID(String tableSegment)
        //{
        //    String IORecordStr = stringHelper.GetStringBetween(tableSegment,startTableIDSeparator.ToString(),endTableIDSeparator.ToString());
        //    if (IORecordStr.Contains("."))
        //    {
        //        String[] ids = IORecordStr.Split('.');
        //        tableID = new IORecordID(ids[0]);
        //        tableRefName = ids[1];
        //    }
        //    else
        //    {
        //        tableID = new IORecordID(IORecordStr);
        //        //tableRefName = HQLEncoder.GetTableNameByID(tableID);
        //    }
        //}
        //internal void AddUpLadderColumnFilter(ColumnRowID columnRowID)
        //{
        //    if (!selectedLocalCols.Contains(columnRowID))
        //    {
        //        selectedLocalCols.Add(columnRowID);
        //    }
        //}
        //internal void AddChildJoinColumn(ColumnRowID columnRowID)
        //{
        //    if (!childJoinColumns.Contains(columnRowID))
        //    {
        //        childJoinColumns.Add(columnRowID);
        //    }
        //}
        //internal void AddChildMatrixJoinClause(int matrixTableID, FullColumnLocation parentLocation)
        //{
        //    childJoinRefs.Add(matrixTableID, parentLocation);

        //}
        //private String GetTableHQLText()
        //{
        //    return startTableIDSeparator + tableID.ToString() + "." + tableRefName + endTableIDSeparator;
        //}
        internal void AddChildMatrixJoinClause(TableSegment childTable)
        {
            childJoinRefs.Add(childTable.tableInfo.tableRefName, childTable.joinClauses[0].parentColumnRef);
            if (childTable.joinClauses[0].evaluationType.Equals(EvaluationTypes.merge))
            {
                selectedCols.Add(childTable.tableInfo.tableRefName, childTable.selectedCols[childTable.tableInfo.tableRefName]);
            }
        }
        public void WriteData(String tableRef, RowID queryID)
        {
            RogueDatabase<DataRowID> thsDB = new RogueDatabase<DataRowID>(new IORecordID((FillledIOIORecords.uiDatabase.rowID.ToString())));
            //*Table Segment
            IRogueTable segmentTable = thsDB.GetTable(SegmentWriteInfo.tableSegmentTblName, "This table is for storing the details for each table segment in a query for reuse or as a template");
            IRogueRow newSegmentRow = segmentTable.NewIWriteRow();
            newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.parentColumnRef, SegmentWriteInfo.colOwnerQueryID, queryID);
            newSegmentRow.NewWritePair(segmentTable.ioItemID,ColumnTypes.column, SegmentWriteInfo.colTableRefName, tableRef.ToDecodedRowID());
            newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colTableID, this.tableInfo.tableID.ToDecodedRowID());
            //newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colTableMatrixNum, this.matrixTableID.ToDecodedRowID());
            newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.parentColumnRef, SegmentWriteInfo.colOwnerLevelID, this.tableInfo.tableID.ToDecodedRowID());
            newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colJoinType, (((char)this.joinClauses[0].evaluationType).ToString()).ToDecodedRowID());
            //newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colParentTableMatrixNum, this.joinClauses[0].parentColumnRef.matrixTableID.ToDecodedRowID());
            newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colParentTableRefName, this.joinClauses[0].parentColumnRef.tableRefName.ToDecodedRowID());
            //newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.column, SegmentWriteInfo.colParentTableMatrixNum, table);


            //newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.parentColumnRef, SegmentWriteInfo.col, this.tableID.ToDecodedRowID());
            //newSegmentRow.NewWritePair(segmentTable.ioItemID, ColumnTypes.parentColumnRef, SegmentWriteInfo.colOwnerLevelID, tableRef.ToDecodedRowID());
            segmentTable.Write();
            //*Write Join Clauses
            IRogueTable writeJoinTable = thsDB.GetTable(JoinWriteInfo.tableJoinTblName, "This table is for storing the details for join clause within a table segment");
            IRogueRow writeJointRow = writeJoinTable.NewIWriteRow();
            writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.parentColumnRef, JoinWriteInfo.colOwnerSegmentID, newSegmentRow.rowID);
            writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colJoinType, (((char)this.joinClauses[0].evaluationType).ToString()).ToDecodedRowID());
            writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colLocalJoinColumnID, this.joinClauses[0].localColumn.columnRowID);
            writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colParentJoinColumnID, this.joinClauses[0].parentColumnRef.columnRowID);
            writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colParentJoinTableRefName, this.joinClauses[0].parentColumnRef.tableRefName.ToDecodedRowID());
            if (this.joinClauses[0].joinAll)
            {
                writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colAllJoin, 1.ToDecodedRowID());
            }
            else
            {
                writeJointRow.NewWritePair(writeJoinTable.ioItemID, ColumnTypes.column, JoinWriteInfo.colAllJoin, 0.ToDecodedRowID());
            }
            
            writeJoinTable.Write();
            //*Write Where Clauses
            IRogueTable writeWhereTable = thsDB.GetTable(WhereWriteInfo.tableWhereTblName, "This table is for storing the details for each Where Clause in a table segment");
            foreach (WhereClause thsClause in whereClauses)
            {
                IRogueRow writeWhereRow = writeWhereTable.NewIWriteRow();
                writeWhereRow.NewWritePair(writeWhereTable.ioItemID, ColumnTypes.parentColumnRef, WhereWriteInfo.colOwnerSegmentID, newSegmentRow.rowID);
                writeWhereRow.NewWritePair(writeWhereTable.ioItemID, ColumnTypes.column, WhereWriteInfo.localColumnID, thsClause.localRowColID);
                writeWhereRow.NewWritePair(writeWhereTable.ioItemID, ColumnTypes.column, WhereWriteInfo.whereValue, thsClause.value);
                writeWhereRow.NewWritePair(writeWhereTable.ioItemID, ColumnTypes.column, WhereWriteInfo.EvalutionType, ((char)thsClause.evalType).ToString().ToDecodedRowID());
            }
            writeWhereTable.Write();
            //*Write Select Cols
            IRogueTable writeSelectTable = thsDB.GetTable(SelectWriteInfo.tableSelectTblName, "This table is for storing the details for each Select Column in a table segment");
            foreach(FullColumnLocations thsCols in selectedCols.Values)
            {
                foreach(SelectColumn thsCol in thsCols.allColumns.Values)
                {
                    IRogueRow writeSelectRow = writeSelectTable.NewIWriteRow();
                    writeSelectRow.NewWritePair(writeSelectTable.ioItemID, ColumnTypes.parentColumnRef, SelectWriteInfo.colOwnerSegmentID, newSegmentRow.rowID);
                    writeSelectRow.NewWritePair(writeSelectTable.ioItemID, ColumnTypes.column, SelectWriteInfo.colColumnRowID, thsCol.columnRowID);
                    writeSelectRow.NewWritePair(writeSelectTable.ioItemID, ColumnTypes.column, SelectWriteInfo.colAliasName, thsCol.columnAliasName.ToDecodedRowID());
                    writeSelectRow.NewWritePair(writeSelectTable.ioItemID, ColumnTypes.column, SelectWriteInfo.colConstValue, thsCol.constValue.ToDecodedRowID());
                    writeSelectRow.NewWritePair(writeSelectTable.ioItemID, ColumnTypes.column, SelectWriteInfo.colTableRefName, thsCol.tableRefName.ToDecodedRowID());
                }
            }
            writeSelectTable.Write();
        }
    }
    static class SegmentWriteInfo
    {
        internal static readonly String tableSegmentTblName = "QueryTableSegment";
        internal static readonly String colTableID = "TableRogueID";
        internal static readonly String colOwnerLevelID = "OwnerLevelRogueID";
        internal static readonly String colOwnerQueryID = "OwnerQueryNameRogueID";
        //internal static readonly String colTableMatrixNum = "TableMatrixNum";
        internal static readonly String colTableRefName = "TableReferenceName";
        internal static readonly String colJoinType = "JoinType";
        //internal static readonly String colJoinType = "JoinType";
        internal static readonly String colParentTableRefName = "ParentTableRefName";
        internal static readonly String colParentTableMatrixNum = "ParentTableMatrixNum";
        internal static readonly String colParentTableJoinColID = "JoinParentTableColID";
        //internal static readonly String colParentM
    }
    static class JoinWriteInfo
    {
        internal static readonly String tableJoinTblName = "QueryJoinInfo";
        internal static readonly String colOwnerSegmentID = "OwnerSegmentRogueID";
        internal static readonly String colJoinType = "JoinType";
        internal static readonly String colParentJoinTableRefName = "ParentJoinTableRefName";
        internal static readonly String colParentJoinColumnID = "ParentJoinColumnID";
       // internal static readonly String colLocalJoinTableID = "LocalJoinColumnID";
        internal static readonly String colLocalJoinColumnID = "LocalJoinColumnID";
        internal static readonly String colAllJoin = "AllJoin";
    }
    static class SelectWriteInfo
    {
        internal static readonly String tableSelectTblName = "QuerySelectInfo";
        internal static readonly String colOwnerSegmentID = "OwnerSegmentRogueID";

        internal static readonly String colMatrixID = "ColumnMatrixID";
        internal static readonly String colTableRefName = "ColumnTableRefName";
        internal static readonly String colColumnRowID = "ColumnRowID";
        internal static readonly String colAliasName = "ColumnAliasName";
        internal static readonly String colConstValue = "ColumnConstValue";
    }
    static class WhereWriteInfo
    {

        internal static readonly String tableWhereTblName = "QueryWriteInfo";
        internal static readonly String colOwnerSegmentID = "OwnerSegmentRogueID";
        internal static readonly String localColumnID = "LocalColumnID";
        internal static readonly String EvalutionType = "EvaluationType";
        internal static readonly String whereValue = "WhereValue";
    }
}