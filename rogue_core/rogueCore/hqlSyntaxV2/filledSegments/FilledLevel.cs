using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.segments.level;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    public class FilledLevel : LevelStatement
    {
        internal FilledLevel(string hqlTxt, HQLMetaData metaData) : base(hqlTxt, metaData) {  }
        FilledLevel() { }
        internal static FilledLevel MasterLevel()
        {
            FilledLevel masterLevel = new FilledLevel();
            //var masterRow = new MultiRogueRow("root", -1, null, null, null);
            var masterRow = MultiRogueRow.MasterRow();
            masterLevel.rows.Add(masterRow);
            return masterLevel;
        }
        internal override FilledLevel Fill(IFilledLevel parentLvl)
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //levelConversion.PreFillMod(parentLvl.rows);
            if (lvlTable.isEncoded)
            {
                LoadEncodedTable(lvlTable, parentLvl, NewLevelRow);
            }
            else
            {
                LoadTable(lvlTable, parentLvl, NewLevelRow);
            }
            //*Should probably fix this ti look for joined tables by joinClause to avoid requiring human to order correctly
            for(int i =0; i < combinedTables.Count; i++)
            {
                if (combinedTables[i].isEncoded)
                {
                    
                    LoadEncodedTable(combinedTables[i],this, MergeWithLevelRow);
                }
                else
                {
                    LoadTable(combinedTables[i], this, MergeWithLevelRow);
                }
            }
            //*Might want to time this separatly
            //levelConversion.ConversionFunc(rows);
            stopwatch2.Stop();
            //Console.WriteLine("Fill Level:" + stopwatch2.ElapsedMilliseconds);
            return this;
        }
        void LoadTable(ITableStatement topTbl, IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> AddRow)
        {
            var indexCols = metaData.RefIndexesByTable(topTbl.tableRefName);
            indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(topTbl.tableRefName, new Dictionary<int, List<MultiRogueRow>>()));
            foreach (MultiRogueRow newRow in topTbl.FilterAndStreamRows(parentLvl, AddRow))
            {
                //MultiRogueRow newRow = AddRow(topTbl.tableRefName, thsRow, parentRow);
                IndexThsRow(topTbl.tableRefName, indexCols, newRow);
            }
            //* This handles encoded tables that need to maintaince iteration of first maxAmount of rows without that getting refreshed to new maxAmount after single encoded table is added
            //int snapshotRowAmount = parentLvl.rows.Count;
            ////* TODO Need to work on this to make more effeicnet probably most time consuming proc in whole project
            //foreach (IRogueRow thsRow in topTbl.StreamRows())
            //{
            //    foreach (MultiRogueRow parentRow in topTbl.MatchFilledLevelParents(parentLvl, thsRow, snapshotRowAmount))
            //    {
            //        if (topTbl.WhereClauseCheck(topTbl.tableRefName, thsRow, parentRow))
            //        {
            //            //*So bad need to find way to run new selectRow only with new parentRow. but it checks parent rows and makes copies for each rogueRow from table
            //            MultiRogueRow newRow = AddRow(topTbl.tableRefName, thsRow, parentRow);
            //            IndexThsRow(topTbl.tableRefName, indexCols, topTbl, newRow);
            //        }
            //    }
            //}
        }

        //* TODO NEED TO make tableStaement interface. Split to Encoded vs Regular table. Encoded will have separete FilterandStreamIRows
        void LoadEncodedTable(ITableStatement initialTbl, IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> AddRow)
        {
            var indexCols = metaData.RefIndexesByTable(initialTbl.tableRefName);
            indexCols.ForEach(col => indexedRows.Add(col, new Dictionary<string, Dictionary<int, List<MultiRogueRow>>>() { { initialTbl.tableRefName, new Dictionary<int, List<MultiRogueRow>>() } }));
            int snapshotRowAmount = parentLvl.rows.Count;
            List<MultiRogueRow> encodedParentRows = new List<MultiRogueRow>(parentLvl.rows);
            foreach(var encodedParentRow in encodedParentRows)
            {
                string finalStatement = metaData.DecodedStatement(initialTbl.origStatement, encodedParentRow);
                var encodeTbl = new FilledTable(finalStatement, metaData);
                //SelectRow useSelectRow = selectRow;
                //if (selectRow.isEncoded)
                //{
                //    //colsForTable = selectRow.ModifiedEncodedColsPerTable(tableRefName, tableRefRows);
                //    string newRowTxt = metaData.DecodedStatement(selectRow.origStatement, encodedParentRow);
                //    useSelectRow = new SelectRow(newRowTxt, metaData);
                //}
                //* TODO Need to work on this to make more efficienet probably most time consuming proc in whole project
                foreach (IRogueRow thsRow in encodeTbl.StreamRows())
                {
                    //*Single Row Here**
                    foreach (MultiRogueRow parentRow in encodeTbl.SingleEncodedTableJoin(encodedParentRow, thsRow))
                    {
                        if (encodeTbl.WhereClauseCheck(encodeTbl.tableRefName, thsRow, parentRow))
                        {
                            //parentRow.OverrideEncodedSelectCols(useSelectRow.TableRefIndexedColumns);
                            MultiRogueRow newRow = AddRow(encodeTbl.tableRefName, thsRow, parentRow);
                            //newRow.OverrideEncodedSelectCols(useSelectRow.TableRefIndexedColumns);
                            IndexThsRow(encodeTbl.tableRefName, indexCols, newRow);
                        }
                    }
                }
            }
        }
        MultiRogueRow NewLevelRow(string tblName, IRogueRow testRow, MultiRogueRow parentRow)
        {
            var newRow = new MultiRogueRow(tblName, levelNum, testRow, parentRow, selectRow, metaData);
            rows.Add(newRow);
            return newRow;
        }
        MultiRogueRow MergeWithLevelRow(string tblName, IRogueRow newRow, MultiRogueRow parentRow)
        {
            return parentRow.MergeRow(tblName, newRow, rows);
        }
        MultiRogueRow EncodedTableRow(string tblName, IRogueRow newRow, MultiRogueRow parentRow)
        {
            return parentRow;
        }
        void IndexThsRow(string tableRefName, List<ColumnRowID> indexCols, MultiRogueRow newRow)
        {
            foreach (ColumnRowID thsIndexCol in indexCols)
            {
                int indexParentValue;
                var valPair = newRow.tableRefRows[tableRefName].ITryGetValue(thsIndexCol);
                if(valPair != null)
                {
                    indexParentValue = int.Parse(valPair.WriteValue());
                }
                else
                {
                    indexParentValue = "".ToDecodedRowID();
                }
                var foundList = indexedRows[thsIndexCol][tableRefName].FindAddIfNotFound(indexParentValue);
                foundList.Add(newRow);
            }
        }
        internal void LoadDataIntoRows()
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch                                           //Parallel.ForEach(rows, command => command.LoadValues(selectRow, metaData));
            if (selectRow.isEncoded)
            {
                Parallel.ForEach(rows, command => command.LoadEncodedValues(selectRow, metaData));
            }
            else
            {
                Parallel.ForEach(rows, command => command.LoadValues(selectRow));
            }
            stopwatch2.Stop();
            Console.WriteLine("Parrallel RogueMultiRow to string values:" + stopwatch2.ElapsedMilliseconds);
        }
        //public void AttachTable(TableStatement newTblStatement)
        //{
        //    if (newTblStatement.isEncoded)
        //    {
        //        LoadEncodedTable(newTblStatement, this, MergeWithLevelRow);
        //    }
        //    else
        //    {
        //        LoadTable(newTblStatement, this, MergeWithLevelRow);
        //    }
        //}
        //public List<From> AllTableIDs()
        //{
        //    var froms = new List<From>();
        //    froms.Add(lvlTable.fromInfo);
        //    froms.AddRange(combinedTables.Select(x => x.fromInfo).ToList());
        //    return froms;
        //}
        //*** Move this to multiRogueRow and make values private again **
        //void TestRowToColumn()
        //{
        //    foreach(MultiRogueRow topRow in rows)
        //    {
        //        foreach(KeyValuePair<SelectColumn, string> kvp in topRow.values.Values)
        //        {
        //            IRogueRow pairRogueRow = new ManualRogueRow(0);
        //            MultiRogueRow pairRow = new MultiRogueRow(levelName + "_PAIR", levelNum + 1, pairRogueRow, topRow, SelectRow.PairSelectRow(metaData), metaData);
        //            IRogueRow keyRogueRow = new ManualRogueRow(0);
        //            keyRogueRow.NewWritePair(0, kvp.Key.columnName);
        //            MultiRogueRow ColumnRow = new MultiRogueRow(levelName + "_KEY", levelNum + 2, keyRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, levelName + "_KEY"), metaData);
        //            IRogueRow valueRogueRow = new ManualRogueRow(0);
        //            valueRogueRow.NewWritePair(0, kvp.Value);
        //            MultiRogueRow ValueRow = new MultiRogueRow(levelName + "_VALUE", levelNum + 2, valueRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, levelName + "_VALUE"), metaData);
        //        }
        //    }
        //}
        //void LoadEncodedTable(TableStatement encodedTbl,FilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> addRow)
        //{
        //    int maxAmount = rows.Count;
        //    for(int rowNum = 0; rowNum < maxAmount; rowNum++)
        //    {
        //        string finalStatement = encodedTbl.origStatement;
        //        foreach (var replaceColPair in new MultiSymbolSegment<DictionaryValues<SelectColumn>, SelectColumn>(SymbolOrder.randombetweensymbols, encodedTbl.origStatement, new string[2] { "{", "}" }, (x, y) => new SelectColumn(x, y), metaData).segmentItems)
        //        {
        //            string replaceColVal = replaceColPair.Value.GetValue(rows[rowNum].tableRefRows);
        //            finalStatement = finalStatement.Replace("{" + replaceColPair.Key + "}", replaceColVal);
        //            LoadTable(new FilledTable(finalStatement, metaData), parentLvl, addRow);
        //        }
        //    }
        //}
    }
    public interface IFilledLevel
    {
        Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<MultiRogueRow>>>> indexedRows { get; }
        List<MultiRogueRow> rows { get; }
        string levelName { get; }
        int levelNum { get; }
        //void AttachTables(List<ITableStatement> newTblStatements, SelectRow selectRow);
        List<ITableStatement> allTableStatements { get; }
        List<string> allTableNames { get; }
    }
}
