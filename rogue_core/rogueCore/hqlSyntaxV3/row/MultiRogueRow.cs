using rogue_core.rogueCore.id.rogueID;
using files_and_folders;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;

namespace rogueCore.hqlSyntaxV3.filledSegments
{
    /// <summary>
    /// This class is used before selectRow so its essentially directly from a table which means its still encoded. It can have parent and children rows and also represent multiple RogueRow when a merge is used. When multiple rows are present due to a merge they are referenced by the tableRefName which is why its a dictionary of String and then rogue row
    /// </summary>
    class MultiRogueRow : IMultiRogueRow
    {
        public string levelName { get;private  set; }
        public int levelNum { get; set; }
        public List<IMultiRogueRow> childRows { get; private set; } = new List<IMultiRogueRow>();
        public Dictionary<string, IReadOnlyRogueRow> tableRefRows { get; private set; } = new Dictionary<string, IReadOnlyRogueRow>();
        List<string> tableRefMergeIndicator = new List<string>();
        public SelectRow selectRow { get; private set; }
        IMultiRogueRow parentRow { get; set; }
        internal MultiRogueRow(string levelName,int levelNum, IReadOnlyRogueRow firstRow, IMultiRogueRow parentRow, SelectRow selectRow)
        {
            //this.metaData = metaData;
            this.levelName = levelName;
            this.levelNum = levelNum;
            this.parentRow = parentRow;
            this.selectRow = selectRow;
            tableRefMergeIndicator.Add(levelName);
            //baseRow = firstRow;
            //if(parentRow != null)
            //{
             parentRow.childRows.Add(this);
             tableRefRows = new Dictionary<string, IReadOnlyRogueRow>(parentRow.tableRefRows);
            //}
            //else
            //{
            //    tableRefRows = new Dictionary<string, IRogueRow>();
            //}
            //*Calculate all select Columns that are of a higher level. Since after this values are only calculated on per table basis.
            //foreach (string thsHigherTblName in selectRow.HigherLevelColTables)
            //{
            //    LoadTableSelects(thsHigherTblName);
            //}
            tableRefRows.Add(levelName, firstRow);
            //*NOT Running when table is higher level than this because its not in curr level. Need to look at curr AND ABOVE**
            //LoadTableSelects(levelName);
        }
        internal MultiRogueRow() { }
        internal static IMultiRogueRow MasterRow() { return new MultiRogueRow() ; }
        MultiRogueRow Clone()
        {
            MultiRogueRow cloneRow = new MultiRogueRow();
            cloneRow.parentRow = this.parentRow;
            cloneRow.tableRefRows = new Dictionary<string, IReadOnlyRogueRow>(tableRefRows);
            cloneRow.levelName = this.levelName;
            cloneRow.tableRefMergeIndicator = new List<String>(this.tableRefMergeIndicator);
            //cloneRow.baseRow = this.baseRow;
            cloneRow.levelNum = this.levelNum;
            //cloneRow.values = new Dictionary<string, KeyValuePair<ISelectColumn, string>>(this.values);
            cloneRow.selectRow = this.selectRow;
            //cloneRow.columns = this.columns;
            //cloneRow.metaData = this.metaData;
            int parentIndex = parentRow.childRows.IndexOf(this);
            parentRow.childRows.Insert(parentIndex+1,cloneRow);
            return cloneRow;
        }
        public IEnumerable<IReadOnlyRogueRow> InvertRow(SelectRow invertedSelectRow, Dictionary<ColumnRowID, IReadOnlyRogueRow> columns, ComplexWordTable complexWordTable)
        {
            foreach (ISelectColumn col in invertedSelectRow.selectColumns.Values)
            {
                var pairRogueRow = new ManualBinaryRow();
                //if (columns.ContainsKey(col.BaseColumnID))
                //{
                    foreach (var pair in columns[col.baseColumnID].GetPairs())
                    {
                        //**CHANGED
                        pairRogueRow.AddPair(pair.Key, pair.Value);
                    }
                //}
                pairRogueRow.AddPair(8676, col.columnName);
                pairRogueRow.AddPair(8637, tableRefRows[col.colTableRefName].rowID.ToString());
                //pairRogueRow.SetValue(-2, parentRow.);
                //new FilledHQLQuery("FROM COLUMN WHERE ROGUECOLUMNID = \"" + col.BaseColumnID + "\"").TopRows().First().baseRow;
                //pairRogueRow.NewWritePair(-1, col.columnName);
                pairRogueRow.AddPair(8619, col.GetValue(tableRefRows));
                //string v = col.GetValue(parentRow.tableRefRows);
                //string k = parentRow.GetValue(col.columnName.ToUpper());
                yield return pairRogueRow;
            }
        }
        public string GetValue(string tableRefName, ColumnRowID colID)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            return tableRefRows[tableRefName].ITryGetValueByColumn(colID);
            //ITryGetValue(colID).StringValue(complexWordTable);
        }
        public string GetValue(ILocationColumn col)
        {
            return col.RetrieveStringValue(tableRefRows);
        }
        public string GetValue(string colName)
        {
            if (selectRow.selectColumns.ContainsKey(colName.ToUpper()))
            {
                return selectRow.selectColumns[colName.ToUpper()].GetValue(tableRefRows);
            }
            else
            {
                return "";
            }
        }
        public IEnumerable<KeyValuePair<string,string>> GetValueList()
        {
            foreach (var col in selectRow.columnList)
            {
                yield return new KeyValuePair<string, string>(col.columnName, col.GetValue(tableRefRows));
            }
        }
        public string GetValueAt(int index)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            //KeyValuePair<ISelectColumn, string> ret = new KeyValuePair<ISelectColumn, string>(null, "");
            //colName = colName.ToUpper();
            //if (selectRow.selectColumns.Count(colName.ToUpper()))
            //{
                return selectRow.columnList[index].GetValue(tableRefRows);
            //}
            //else
            //{
            //    return "";
            //}
            //values.TryGetValue(colName, out ret);
            //return new KeyValuePair<ISelectColumn, string>(col, col.GetValue(tableRefRows));
            //return ret.Value;
        }
        public string WhereClauseGet(ComplexColumn whereCol, IReadOnlyRogueRow testRow, string thsTableRef)
        {
            var allRows = new Dictionary<string, IReadOnlyRogueRow>(tableRefRows);
            allRows.FindChangeIfNotFound(thsTableRef, testRow);
            return whereCol.GetValue(allRows);
        }
        public IMultiRogueRow MergeRow(string tableRefName, IReadOnlyRogueRow newRow, List<IMultiRogueRow> levelRows)
        {
            if (!tableRefMergeIndicator.Contains(tableRefName))
            {
                tableRefMergeIndicator.Add(tableRefName);
                tableRefRows.Add(tableRefName, newRow);
                //LoadTableSelects(tableRefName);
                return this;
            }
            else
            {
                MultiRogueRow newMultiRow = this.Clone();
                newMultiRow.tableRefRows[tableRefName] = newRow;
                //newMultiRow.LoadTableSelects(tableRefName);
                levelRows.Add(newMultiRow);
                return newMultiRow;
            }
        }
        public IMultiRogueRow ManualChildRow(IMultiRogueRow manualRow)
        {
            this.childRows.Add(manualRow);
            manualRow.levelNum = this.levelNum + 1;
            return manualRow;
        }
        public string PrintRow(bool fullRow)
        {
            string retLine = "";
            Console.Write(indent(levelNum) + this.levelName + "|");
            retLine += indent(levelNum) + this.levelName + "|";
            foreach (var col in selectRow.columnList)
            {
                Console.Write(col.columnName +"(" + col.baseColumnID + ")");
                retLine += col.columnName;
                Console.Write(":");
                retLine += ":";
                Console.Write(col.GetValue(tableRefRows) + " , ");
                retLine += col.GetValue(tableRefRows) + " , ";
                if (fullRow)
                {
                    Console.Write(" BASE: ");
                    retLine += " BASE: ";
                    //this.baseRow.PrintRow();
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
