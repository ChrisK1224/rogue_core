using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using files_and_folders;
using rogueCore.hqlSyntaxV2.segments;
using rogueCore.hqlSyntaxV2.segments.select;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Linq;
using rogueCore.hqlSyntaxV2.segments.levelConversion;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    /// <summary>
    /// This class is used before selectRow so its essentially directly from a table which means its still encoded. It can have parent and children rows and also represent multiple RogueRow when a merge is used. When multiple rows are present due to a merge they are referenced by the tableRefName which is why its a dictionary of String and then rogue row
    /// </summary>
    public class MultiRogueRow 
    {
        string levelName;
        int levelNum;
        internal List<MultiRogueRow> childRows = new List<MultiRogueRow>();
        public Dictionary<string, IRogueRow> tableRefRows = new Dictionary<string, IRogueRow>();
        List<string> tableRefMergeIndicator = new List<string>();
        internal Dictionary<string, KeyValuePair<SelectColumn, string>> values = new Dictionary<string, KeyValuePair<SelectColumn, string>>();
        internal IRogueRow baseRow;
        //SelectRow selectRow;
        //Dictionary<string, List<SelectColumn>> columns;
        internal SelectRow selectRow;
        internal MultiRogueRow parentRow;
        HQLMetaData metaData;
        internal MultiRogueRow(string levelName,int levelNum, IRogueRow firstRow, MultiRogueRow parentRow, SelectRow selectRow, HQLMetaData metaData)
        {
            this.metaData = metaData;
            this.levelName = levelName;
            this.levelNum = levelNum;
            this.parentRow = parentRow;
            this.selectRow = selectRow;
            tableRefMergeIndicator.Add(levelName);
            baseRow = firstRow;
            if(parentRow != null)
            {
                parentRow.childRows.Add(this);
                tableRefRows = new Dictionary<string, IRogueRow>(parentRow.tableRefRows);
            }
            else
            {
                tableRefRows = new Dictionary<string, IRogueRow>();
            }
            //*Calculate all select Columns that are of a higher level. Since after this values are only calculated on per table basis.
            foreach (string thsHigherTblName in selectRow.HigherLevelColTables)
            {
                LoadTableSelects(thsHigherTblName);
            }
            tableRefRows.Add(levelName, firstRow);
            //*NOT Running when table is higher level than this because its not in curr level. Need to look at curr AND ABOVE**
            LoadTableSelects(levelName);
            //rowConversion(this);
        }
        internal MultiRogueRow() { }
        internal static MultiRogueRow MasterRow() { return new MultiRogueRow() ; }
        //internal void AddMergeRow(string tableRefName, IRogueRow mergeRow)
        //{
        //    tableRefRows[tableRefName] = mergeRow;
        //}
        MultiRogueRow Clone()
        {
            MultiRogueRow cloneRow = new MultiRogueRow();
            cloneRow.parentRow = this.parentRow;
            cloneRow.tableRefRows = new Dictionary<string, IRogueRow>(tableRefRows);
            cloneRow.levelName = this.levelName;
            cloneRow.tableRefMergeIndicator = new List<String>(this.tableRefMergeIndicator);
            cloneRow.baseRow = this.baseRow;
            cloneRow.levelNum = this.levelNum;
            cloneRow.values = new Dictionary<string, KeyValuePair<SelectColumn, string>>(this.values);
            cloneRow.selectRow = this.selectRow;
            //cloneRow.columns = this.columns;
            cloneRow.metaData = this.metaData;
            int parentIndex = parentRow.childRows.IndexOf(this);
            parentRow.childRows.Insert(parentIndex+1,cloneRow);
            return cloneRow;
        }
        internal DecodedRowID GetValue(string tableRefName, ColumnRowID colID)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            return new DecodedRowID(int.Parse(tableRefRows[tableRefName].ITryGetValue(colID).WriteValue()));
        }
        internal string GetValue(string colName)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            KeyValuePair<SelectColumn, string> ret = new KeyValuePair<SelectColumn, string>(null, "");
            colName = colName.ToUpper();
            values.TryGetValue(colName, out ret);
            return ret.Value;
            //return tableRefRows.Values.First().ITryGetValue(colID).DisplayValue();
        }
        internal MultiRogueRow MergeRow(string tableRefName, IRogueRow newRow, List<MultiRogueRow> levelRows)
        {
            if (!tableRefMergeIndicator.Contains(tableRefName))
            {
                tableRefMergeIndicator.Add(tableRefName);
                tableRefRows.Add(tableRefName, newRow);
                LoadTableSelects(tableRefName);
                return this;
            }
            else
            {
                MultiRogueRow newMultiRow = this.Clone();
                newMultiRow.tableRefRows[tableRefName] = newRow;
                //newMultiRow.values.Remove(tableRefName);
                newMultiRow.LoadTableSelects(tableRefName);
                //* FIXME Probaby need to clear values here that are of tableRefNamePotential bug
                //newMultiRow.LoadTableSelects(tableRefName);
                //* Add this new cloned multirow to the full list of level rows
               // int index = levelRows.IndexOf(this);
                levelRows.Add(newMultiRow);
                return newMultiRow;
            }
        }
        internal MultiRogueRow ManualChildRow(MultiRogueRow manualRow)
        {
            this.childRows.Add(manualRow);
            manualRow.parentRow = this;
            manualRow.levelNum = this.levelNum + 1;
            return manualRow;
        }
        //internal void LoadTableSelects(string tableRefName)
        //{
        //    foreach (var col in selectRow.encodedCols.TryFindReturn(tableRefName))
        //    {
        //        col.ModifyEncodedValues(col.GetValue(tableRefRows));
        //    }
        //    foreach (SelectColumn col in selectRow.TableRefIndexedColumns.TryFindReturn(tableRefName))
        //    {
        //        values.Add(col.columnName.ToUpper(), new KeyValuePair<SelectColumn, string>(col, col.GetValue(tableRefRows)));
        //    }
        //}
        internal void LoadTableSelects(string tableRefName)
        {
            //SelectRow testRow = selectRow;
            //var colsForTable = selectRow.TableRefIndexedColumns;
            //if (selectRow.isEncoded)
            //{
            //    //colsForTable = selectRow.ModifiedEncodedColsPerTable(tableRefName, tableRefRows);
            //    string newRowTxt = metaData.DecodedStatement(selectRow.origStatement, this);
            //    testRow = new SelectRow(newRowTxt, metaData);
            //}
            //foreach (SelectColumn col in testRow.TableRefIndexedColumns.TryFindReturn(tableRefName))
            foreach (SelectColumn col in selectRow.TableRefIndexedColumns.TryFindReturn(tableRefName))
            {
                values[col.columnName.ToUpper()] = new KeyValuePair<SelectColumn, string>(col, col.GetValue(tableRefRows));
            }
        }
        //internal void OverrideEncodedSelectCols(Dictionary<string, List<SelectColumn>> overrideCols)
        //{
        //    columns = overrideCols;
        //}
        internal void LoadValues(SelectRow rowOutline)
        {
            values = new Dictionary<string, KeyValuePair<SelectColumn, string>>();
            foreach (SelectColumn col in rowOutline.SelectColumns.Values)
            {
                values.Add(col.columnName.ToUpper(), new KeyValuePair<SelectColumn, string>(col, col.GetValue(tableRefRows)));
            }
        }
        internal void LoadEncodedValues(SelectRow rowOutline, HQLMetaData metaData)
        {
            //Stopwatch stopwatch2 = Stopwatch.StartNew();
            string newRowTxt = metaData.DecodedStatement(rowOutline.origStatement, this);
            rowOutline = new SelectRow(newRowTxt, metaData);
            //stopwatch2.Stop();
            //Console.WriteLine("Time for One SelectRow Calc:" + stopwatch2.ElapsedMilliseconds);
            LoadValues(rowOutline);
        }
        public string PrintRow(bool fullRow = false)
        {
            string retLine = "";
            Console.Write(indent(levelNum) + this.levelName + "|");
            retLine += indent(levelNum) + this.levelName + "|";
            foreach (var thsPair in values)
            {
                Console.Write(thsPair.Key);
                retLine += thsPair.Key;
                Console.Write(":");
                retLine += ":";
                Console.Write(thsPair.Value.Value + " , ");
                retLine += thsPair.Value.Value + " , ";
                if (fullRow)
                {
                    Console.Write(" BASE: ");
                    retLine += " BASE: ";
                    this.baseRow.PrintRow();
                    //retLine += 
                }
            }
            Console.WriteLine("");
            retLine += Environment.NewLine;
            Console.WriteLine(indent(levelNum) + "*******************************");
            //retLine += Environment.NewLine + indent(levelNum) + "*******************************";
            return retLine;
        }
        static string indent(int indentCount)
        {
            string retStr = "";
            for (int tab = 0; tab < indentCount; tab++) { retStr += "\t"; }
            return retStr;
        }
    }
}
