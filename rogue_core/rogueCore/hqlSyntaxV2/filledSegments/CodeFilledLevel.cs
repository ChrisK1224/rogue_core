using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    //class CodeFilledLevel : IFilledLevel
    //{
    //    public Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<MultiRogueRow>>>> indexedRows { get; } = new Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<MultiRogueRow>>>>();
    //    public List<MultiRogueRow> rows { get; } = new List<MultiRogueRow>();
    //    internal List<TableStatement> combinedTables { get; private set; } = new List<TableStatement>();
    //    public string levelName { get; }
    //    public int levelNum { get; }
    //    List<ColumnRowID> indexCols;
    //    HQLMetaData metaData;
    //    internal SelectRow selectRow;
    //    public List<string> allTableNames { get; set; } = new List<string>();
    //    internal CodeFilledLevel(string levelName, int levelNum, HQLMetaData metaData) { this.levelName = levelName; this.levelNum = levelNum; this.metaData = metaData; allTableNames.Add(levelName); selectRow = new SelectRow("", metaData); }
    //    internal void FillCombinedTables()
    //    {
    //        for (int i = 0; i < combinedTables.Count; i++)
    //        {
    //            if (combinedTables[i].isEncoded)
    //            {
    //                LoadEncodedTable(combinedTables[i], this, MergeWithLevelRow);
    //            }
    //            else
    //            {
    //                LoadTable(combinedTables[i], this, MergeWithLevelRow);
    //            }
    //        }
    //    }
    //    public void AttachTables(List<TableStatement> newTblStatements,SelectRow selectRow)
    //    {
    //        this.selectRow = selectRow;
    //        foreach(var newTblStatement in newTblStatements)
    //        {
    //            combinedTables.Add(newTblStatement);
    //            //if (newTblStatement.isEncoded)
    //            //{
    //            //    LoadEncodedTable(newTblStatement, this, MergeWithLevelRow);
    //            //}
    //            //else
    //            //{
    //            //    LoadTable(newTblStatement, this, MergeWithLevelRow);
    //            //}
    //        }
    //    }
    //    internal void SetIndexes()
    //    {
    //        indexCols = metaData.RefIndexesByTable(levelName);
    //        indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(levelName, new Dictionary<int, List<MultiRogueRow>>()));
    //    }
    //    public void AddRow(MultiRogueRow thsRow)
    //    {
    //        //var indexCols = metaData.RefIndexesByTable(levelName);
    //        //indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(levelName, new Dictionary<int, List<MultiRogueRow>>()));
    //        rows.Add(thsRow);
    //        IndexThsRow(levelName, indexCols, thsRow);
    //    }
    //    void IndexThsRow(string tableRefName, List<ColumnRowID> indexCols, MultiRogueRow newRow)
    //    {
    //        foreach (ColumnRowID thsIndexCol in indexCols)
    //        {
    //            int indexParentValue;
    //            var valPair = newRow.tableRefRows[tableRefName].ITryGetValue(thsIndexCol);
    //            if (valPair != null)
    //            {
    //                indexParentValue = int.Parse(valPair.WriteValue());
    //            }
    //            else
    //            {
    //                indexParentValue = "".ToDecodedRowID();
    //            }
    //            var foundList = indexedRows[thsIndexCol][tableRefName].FindAddIfNotFound(indexParentValue);
    //            foundList.Add(newRow);
    //        }
    //    }
    //    public List<TableStatement> allTableStatements { get { return new List<TableStatement>(); } }
    //    //**NEED TO MAKE BASE CLASS FILLEDLEVEL. This is repeat code from standard filledLevel
    //    void LoadTable(TableStatement topTbl, IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> AddRow)
    //    {
    //        var indexCols = metaData.RefIndexesByTable(topTbl.tableRefName);
    //        indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(topTbl.tableRefName, new Dictionary<int, List<MultiRogueRow>>()));
    //        //* This handles encoded tables that need to maintaince iteration of first maxAmount of rows without that getting refreshed to new maxAmount after single encoded table is added
    //        int snapshotRowAmount = parentLvl.rows.Count;
    //        //* TODO Need to work on this to make more effeicnet probably most time consuming proc in whole project
    //        foreach (IRogueRow thsRow in topTbl.StreamRows())
    //        {
    //            foreach (MultiRogueRow parentRow in topTbl.MatchFilledLevelParents(parentLvl, thsRow, snapshotRowAmount))
    //            {
    //                if (topTbl.WhereClauseCheck(topTbl.tableRefName, thsRow, parentRow))
    //                {
    //                    //*So bad need to find way to run new selectRow only with new parentRow. but it checks parent rows and makes copies for each rogueRow from table
    //                    MultiRogueRow newRow = AddRow(topTbl.tableRefName, thsRow, parentRow);
    //                    IndexThsRow(topTbl.tableRefName, indexCols, topTbl, newRow);
    //                }
    //            }
    //        }
    //    }
    //    //* TODO should merge with load table above and just have generic func that handle all funcs set in tableStatement
    //    void LoadEncodedTable(TableStatement initialTbl, IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> AddRow)
    //    {
    //        var indexCols = metaData.RefIndexesByTable(initialTbl.tableRefName);
    //        indexCols.ForEach(col => indexedRows.Add(col, new Dictionary<string, Dictionary<int, List<MultiRogueRow>>>() { { initialTbl.tableRefName, new Dictionary<int, List<MultiRogueRow>>() } }));
    //        int snapshotRowAmount = parentLvl.rows.Count;
    //        List<MultiRogueRow> encodedParentRows = new List<MultiRogueRow>(parentLvl.rows);
    //        foreach (var encodedParentRow in encodedParentRows)
    //        {
    //            string finalStatement = metaData.DecodedStatement(initialTbl.origStatement, encodedParentRow);
    //            var encodeTbl = new FilledTable(finalStatement, metaData);
    //            //SelectRow useSelectRow = selectRow;
    //            //if (selectRow.isEncoded)
    //            //{
    //            //    //colsForTable = selectRow.ModifiedEncodedColsPerTable(tableRefName, tableRefRows);
    //            //    string newRowTxt = metaData.DecodedStatement(selectRow.origStatement, encodedParentRow);
    //            //    useSelectRow = new SelectRow(newRowTxt, metaData);
    //            //}
    //            //* TODO Need to work on this to make more efficienet probably most time consuming proc in whole project
    //            foreach (IRogueRow thsRow in encodeTbl.StreamRows())
    //            {
    //                //*Single Row Here**
    //                foreach (MultiRogueRow parentRow in encodeTbl.SingleEncodedTableJoin(encodedParentRow, thsRow))
    //                {
    //                    if (encodeTbl.WhereClauseCheck(encodeTbl.tableRefName, thsRow, parentRow))
    //                    {
    //                        //parentRow.OverrideEncodedSelectCols(useSelectRow.TableRefIndexedColumns);
    //                        MultiRogueRow newRow = AddRow(encodeTbl.tableRefName, thsRow, parentRow);
    //                        //newRow.OverrideEncodedSelectCols(useSelectRow.TableRefIndexedColumns);
    //                        IndexThsRow(encodeTbl.tableRefName, indexCols, encodeTbl, newRow);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    MultiRogueRow MergeWithLevelRow(string tblName, IRogueRow newRow, MultiRogueRow parentRow)
    //    {
    //        return parentRow.MergeRow(tblName, newRow, rows);
    //    }
    //    void IndexThsRow(string tableRefName, List<ColumnRowID> indexCols, TableStatement thsTbl, MultiRogueRow newRow)
    //    {
    //        foreach (ColumnRowID thsIndexCol in indexCols)
    //        {
    //            int indexParentValue;
    //            var valPair = newRow.tableRefRows[tableRefName].ITryGetValue(thsIndexCol);
    //            if (valPair != null)
    //            {
    //                indexParentValue = int.Parse(valPair.WriteValue());
    //            }
    //            else
    //            {
    //                indexParentValue = "".ToDecodedRowID();
    //            }
    //            var foundList = indexedRows[thsIndexCol][tableRefName].FindAddIfNotFound(indexParentValue);
    //            foundList.Add(newRow);
    //        }
    //    }
    //}
}
