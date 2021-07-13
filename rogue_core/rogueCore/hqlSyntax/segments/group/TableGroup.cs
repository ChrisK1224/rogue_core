using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntax.segments.join;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.table.human;
using static rogueCore.hqlSyntax.MutliSegmentEnum;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.table
{
    //public abstract class TableGroup : MultiSymbolSegment<DictionaryListValues<String>, String>, ISplitSegment
    public class TableGroup
    {
        protected TableGroupInfo groupInfo { get; set; }
        protected Dictionary<String, TableStatement> tableStatements = new Dictionary<String, TableStatement>();
        protected string[] keys { get { return new string[2] { TableStatement.splitKey, TableGroupInfo.splitKey }; } }
        //protected TableGroup(SymbolOrder thsOrder, String humanHQL) : base(thsOrder, humanHQL)
        HQLMetaData metaData;
        public TableGroup(String humanHQL, HQLMetaData metaData)
        {
            this.metaData = metaData;
            var segmentItems = new MultiSymbolSegmentNew<DictionaryListValues<String>, String>(SymbolOrder.symbolbefore, humanHQL, keys, metaData).segmentItems;
            groupInfo = new TableGroupInfo(segmentItems[TableGroupInfo.splitKey][0], metaData);
            foreach (String thsTableSeg in segmentItems[TableStatement.splitKey])
            {
                TableStatement newTbl = new TableStatement(thsTableSeg, metaData);
                tableStatements.Add(newTbl.tableRefName, newTbl);
            }
        }
        void SetTableLevels(TableStatement topTbl)
        {
            foreach (TableStatement tbl in tableStatements.Values.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == topTbl.tableRefName))
            {
                tbl.level = topTbl.level + 1;
                SetTableLevels(tbl);
            }
        }
        /*Dictionary<String, List<TableStatement>> SetChildMap()
        {
            Dictionary<String, List<TableStatement>> childTableMap = new Dictionary<string, List<TableStatement>>();
            foreach (TableStatement tbl in tableStatements.Values)
            {
                childTableMap.Add(tbl.tableRefName, tableStatements.Values.ToList().Where(lst => (lst.joinClause.isSet == true && lst.joinClause.parentColumn.colTableRefName == tbl.tableRefName)).ToList());
            }
            return childTableMap;
        }*/
        /*protected void LoadChildRows(TableStatement tbl, Dictionary<ColumnRowID,Dictionary<int, Li st<FilledSelectRow>>> topRows)
        {
            
            List<TableStatement> childTables = tableStatements.Values.Where(t => (t.joinClause.isSet== true && t.joinClause.parentColumn.colTableRefName == tbl.tableRefName)).ToList();
            Dictionary<ColumnRowID,Dictionary<int, List<FilledSelectRow>>> newTopRows = tbl.LoadDataRows(topRows, childTables);
            foreach(TableStatement childTbl in childTables)
            {
                LoadChildRows(childTbl, newTopRows)
            }
        }*/
        /*public void LoadGroupDataOld(ref Dictionary<String, FilledTable> existingPieces)
        {
            //childJoinMap = SetChildMap();
            tableStatements.Values.Where(v => v.joinClause.isSet == false).ToList().ForEach(x => SetTableLevels(x));
            List<FilledSelectRow> groupRows = new List<FilledSelectRow>();
            foreach (var tbl in tableStatements.Values.Where(v => v.joinClause.isSet == false))
            {
                FilledSelectRow baseRow = FilledSelectRow.BaseRow();
                var topRows = new FilledTable(baseRow);
                groupRows.AddRange(LoadDataRows(tbl, topRows).Rows());
            }
            //PrintTopTableGroup(groupRows);
            List<FilledSelectRow> finalRows = groupInfo.Transformer(groupRows);
            var transformedRows = groupInfo.Transformer(groupRows);
            PrintTopTableGroup(transformedRows);
        }*/
        public void LoadGroupData(Dictionary<String, FilledTable> existingPieces)
        {
            tableStatements.Values.Where(v => v.joinClause.isSet == false).ToList().ForEach(x => SetTableLevels(x));
            List<FilledSelectRow> groupRows = new List<FilledSelectRow>();
            foreach (TableStatement loadTbl in tableStatements.Values.OrderBy(x => x.level))
            {
                List<JoinClause> indexes = tableStatements.Values.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == loadTbl.tableRefName).Select(x => x.joinClause).Distinct().ToList();
                FilledTable parentTbl; 
                existingPieces.TryGetValue(loadTbl.parentTableRefName, out parentTbl);
                if (metaData.encodedTableStatements.Contains(loadTbl.tableRefName) && parentTbl != null)
               // if (loadTbl.isEncoded)
                {
                    foreach(FilledSelectRow parentRow in parentTbl.rows)
                    {
                        string finalStatement =  loadTbl.origStatement;

                        foreach(var replaceColPair in new MultiSymbolSegmentNew<DictionaryValues<SelectColumn>, SelectColumn>(SymbolOrder.randombetweensymbols, loadTbl.origStatement, new string[2] { "{","}"}, metaData).segmentItems)
                        {
                            string replaceColVal = replaceColPair.Value.GetValue(parentRow.tableRefRows);
                            finalStatement = finalStatement.Replace("{" + replaceColPair.Key + "}", replaceColVal );
                        }
                        //* FIXME this should run once and then merge tables especially IndexedRows should be passed to each new FilledTable since this needs to be added upon to work when child table is not JOINALL might work for not assuming JOINALL
                        TableStatement runtimeTbl = new TableStatement(finalStatement, metaData);
                        FilledTable newTbl = new FilledTable(runtimeTbl, indexes, parentRow, parentTbl, loadTbl.level);
                        //* TODO shorten this
                        if (existingPieces.ContainsKey(loadTbl.tableRefName))
                        {
                            existingPieces[loadTbl.tableRefName].MergeTable(newTbl);
                        }
                        else
                        {
                            existingPieces.Add(loadTbl.tableRefName, newTbl);
                        }
                    }
                    //FilledTable newTbl = new FilledTable(loadTbl, indexes, parentTbl);
                    //existingPieces.Add(loadTbl.tableRefName, newTbl);
                }
                else if(loadTbl.level ==0)
                {
                    if(parentTbl == null)
                    {
                        parentTbl = metaData.rootTable;
                    }
                    FilledTable newTbl = new FilledTable(metaData, loadTbl, indexes, parentTbl);
                    existingPieces.Add(loadTbl.tableRefName, newTbl);
                }
                else if (parentTbl != null)
                {
                    FilledTable newTbl = new FilledTable(metaData, loadTbl, indexes, parentTbl);
                    existingPieces.Add(loadTbl.tableRefName, newTbl);
                }
                else
                {
                    string f = "WFTF";
                }
                //FilledTable newTbl = new FilledTable(loadTbl, indexes, parentTbl);
                //existingPieces.Add(loadTbl.tableRefName, newTbl);
                //if (loadTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
                //{
                //    //existingPieces[loadTbl.parentTableRefName] = newTbl;
                //    //newTbl.level = existingPieces[loadTbl.parentTableRefName].level;
                //}
            }
            int maxLvl = tableStatements.Values.ToList().Max(obj => obj.level);
            tableStatements.Values.Where(x => x.level == maxLvl && existingPieces.ContainsKey(x.tableRefName)).ToList().ForEach(x => groupRows.AddRange(existingPieces[x.tableRefName].rows));
            var transformedRows = groupInfo.Transformer(groupRows, existingPieces, tableStatements);
            //PrintTopTableGroup(transformedRows);
        }
        public List<string> FormatQueryTxt()
        {
            tableStatements.Values.Where(v => v.joinClause.isSet == false).ToList().ForEach(x => SetTableLevels(x));
            List<string> lines = new List<string>();
            lines.Add("|");
            foreach (TableStatement loadTbl in tableStatements.Values.Where(x => x.level == 0))
            {
                
                //List<JoinClause> indexes = tableStatements.Values.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == loadTbl.tableRefName).Select(x => x.joinClause).Distinct().ToList();

                //if (loadTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
                //{
                //    //existingPieces[loadTbl.parentTableRefName] = newTbl;
                //    //newTbl.level = existingPieces[loadTbl.parentTableRefName].level;

                //}
                lines.Concat(LoopChildTables(loadTbl, lines));
            }
            lines.Add(groupInfo.origStatement());
            return lines;
        }
        List<string> LoopChildTables(TableStatement topTbl, List<string> lines)
        {
            string formattedLine = "";
            for (int i = 0; i < topTbl.level; i++)
            {
                formattedLine += "\t";
            }
            formattedLine += TableStatement.splitKey + "  " + topTbl.origStatement;
            lines.Add(formattedLine);
            List<TableStatement> childTables = tableStatements.Values.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == topTbl.tableRefName).Select(x => x).Distinct().ToList();
            
            foreach(TableStatement childTbl in childTables)
            {
                LoopChildTables(childTbl, lines);
            }
            return lines;
        }
        /*List<FilledSelectRow> HierarchyTableTransform(List<FilledSelectRow> allRows)
        {
            List<FilledSelectRow> tblList = new List<FilledSelectRow>();
            FilledSelectRow headerRow = new FilledSelectRow(groupInfo.groupRefName + "_HeaderRow");
            tblList.Add(headerRow);
            //*Set header row just use first row since assumed all have same columns
            foreach (var pair in allRows[0].values)
            {
                FilledSelectRow headerCol = new FilledSelectRow(groupInfo.groupRefName + "_HeaderCol");
                headerCol.values.Add("COLUMNNAME", pair.Key);
                headerRow.childRows.Add(headerCol);
            }
            //*Iterate each row 
            foreach (FilledSelectRow origRow in allRows)
            {
                FilledSelectRow dataRow = new FilledSelectRow(groupInfo.groupRefName + "_DataRow");
                tblList.Add(dataRow);
                foreach (var pair in origRow.values)
                {
                    FilledSelectRow dataCol = new FilledSelectRow(groupInfo.groupRefName + "_DataPairCol");
                    dataCol.values.Add("VALUE", pair.Value);
                    dataRow.childRows.Add(dataCol);
                }
            }
            return tblList;
        }*/
        void PrintTopTableGroup(IEnumerable<FilledSelectRow> topRows)
        {
            foreach (var topRow in topRows)
            {
                LoopPrintHierachy(topRow, 0);
            }
        }
        void LoopPrintHierachy(FilledSelectRow topRow, int currLvl)
        {
            topRow.PrintRow(currLvl);
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl);
            }
        }
        /*FilledTable LoadDataRows(TableStatement thsTbl, FilledTable topRows)
        {
            FilledTable dataRows = topRows.GetChildJoinedRows(thsTbl, childJoinMap[thsTbl.tableRefName]);
            dataTableRows.Add(thsTbl.tableRefName, dataRows);
            //*Need to make object with list and then has dictionary just to correspond to list number as the index
            foreach (TableStatement tbl in childJoinMap[thsTbl.tableRefName])
            {
                if (tbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
                {
                    //*need to test this logic and finish
                    if (tbl.joinClause.MergeJointype.Equals("inner"))
                    {
                        dataRows = LoadDataRows(tbl, dataRows);
                    }
                    else
                    {
                        //*This actually updates the top rows and leaves all unmatched rows
                        LoadDataRows(tbl, dataRows);
                    }
                }
                else
                {
                    LoadDataRows(tbl, dataRows);
                }
            }
            return dataRows;
        }
        //*Not sure if should do this here or from joinClause using static allTables.
        /*protected void AddTableStatement(TableStatement newTbl)
        {
            tableStatements.Add(newTbl.tableRefName, newTbl);
            /*if (newTbl.joinClause.isSet)
            {
                TableStatement parentTbl = tableStatements[newTbl.joinClause.parentColumn.colTableRefName];
                parentTbl.AddChildTableRef(newTbl);
            }
        }*/
    }
}
