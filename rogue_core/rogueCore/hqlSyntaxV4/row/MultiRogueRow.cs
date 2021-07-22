using files_and_folders;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.row
{
    class MultiRogueRow : IMultiRogueRow
    {        
        public int levelNum { get; set; }
        public List<IMultiRogueRow> childRows { get; } = new List<IMultiRogueRow>();
        public Dictionary<string, IReadOnlyRogueRow> tableRefRows { get; private set; } = new Dictionary<string, IReadOnlyRogueRow>();
        List<string> tableRefMergeIndicator = new List<string>();
        SelectRow selectRow { get; set; }
        IMultiRogueRow parentRow { get; set; }
        public string levelName { get; set; }        
        internal MultiRogueRow(string levelName, int levelNum, IReadOnlyRogueRow firstRow, IMultiRogueRow parentRow, SelectRow selectRow)
        {
            this.levelName = levelName;
            this.levelNum = levelNum;
            this.parentRow = parentRow;
            this.selectRow = selectRow;
            tableRefMergeIndicator.Add(levelName);
            parentRow.childRows.Add(this);
            tableRefRows = new Dictionary<string, IReadOnlyRogueRow>(parentRow.tableRefRows);
            tableRefRows.Add(levelName, firstRow);
        }
        MultiRogueRow() { }
        internal static IMultiRogueRow MasterRow() { return new MultiRogueRow(); }
        MultiRogueRow Clone()
        {
            MultiRogueRow cloneRow = new MultiRogueRow();
            cloneRow.parentRow = this.parentRow;
            cloneRow.tableRefRows = new Dictionary<string, IReadOnlyRogueRow>(tableRefRows);
            cloneRow.levelName = this.levelName;
            cloneRow.tableRefMergeIndicator = new List<String>(this.tableRefMergeIndicator);
            cloneRow.levelNum = this.levelNum;
            cloneRow.selectRow = this.selectRow;
            int parentIndex = parentRow.childRows.IndexOf(this);
            parentRow.childRows.Insert(parentIndex + 1, cloneRow);
            return cloneRow;
        }
        public IEnumerable<IReadOnlyRogueRow> InvertRow(SelectRow invertedSelectRow, Dictionary<ColumnRowID, IReadOnlyRogueRow> columns, ComplexWordTable complexWordTable)
        {
            foreach (ISelectColumn col in invertedSelectRow.selectColumns)
            {
                var pairRogueRow = new ManualBinaryRow();
                foreach (var pair in columns[col.baseColumnID].GetPairs())
                {
                    //**CHANGED
                    pairRogueRow.AddPair(pair.Key, pair.Value);
                }
                pairRogueRow.AddPair(8676, col.columnName);
                //**DELTED FOR V4 nned to find effects
                //pairRogueRow.AddPair(8637, tableRefRows[col.colTableRefName].rowID.ToString());
                pairRogueRow.AddPair(8619, col.GetValue(tableRefRows.ToSingleEnum()));
                yield return pairRogueRow;
            }
        }
        public string GetValue(string tableRefName, ColumnRowID colID)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            return tableRefRows[tableRefName].ITryGetValueByColumn(colID);
            //ITryGetValue(colID).StringValue(complexWordTable);
        }
        public string GetValue(IColumn col)
        {
            return col.RetrieveStringValue(tableRefRows.ToSingleEnum());
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            //return tableRefRows[tableRefName].ITryGetValue(colID).DisplayValue();
        }
        public string GetValue(string colName)
        {
            //* TODO Fix this so that it pulls directly as DecodedRowID this is slow*****
            //SelectColumn col = null;
            //colName = colName.ToUpper();
            //try
            //{
            if (selectRow.columnsByName.ContainsKey(colName.ToUpper()))
            {
                return selectRow.columnsByName[colName.ToUpper()].GetValue(tableRefRows.ToSingleEnum());
            }
            else
            {
                return "";
            }
        }
        public IEnumerable<KeyValuePair<string, string>> GetValueList()
        {
            foreach (var col in selectRow.selectColumns)
            {
                yield return new KeyValuePair<string, string>(col.columnName, col.GetValue(tableRefRows.ToSingleEnum()));
            }
        }
        public string GetValueAt(int index)
        {
            return selectRow.selectColumns[index].GetValue(tableRefRows.ToSingleEnum());
        }
        public IMultiRogueRow MergeRow(string tableRefName, IReadOnlyRogueRow newRow, List<IMultiRogueRow> levelRows)
        {
            if (!tableRefMergeIndicator.Contains(tableRefName))
            {
                tableRefMergeIndicator.Add(tableRefName);
                tableRefRows.Add(tableRefName, newRow);
                return this;
            }
            else
            {
                MultiRogueRow newMultiRow = this.Clone();
                newMultiRow.tableRefRows[tableRefName] = newRow;
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
        public void RemoveFromParent()
        {
            parentRow.childRows.Remove(this);
        }
        public string PrintRow(bool fullRow)
        {
            string retLine = "";
            Console.Write(indent(levelNum) + this.levelName + "|");
            retLine += indent(levelNum) + this.levelName + "|";
            foreach (var col in selectRow.selectColumns)
            {
                Console.Write(col.columnName );
                retLine += col.columnName;
                Console.Write(":");
                retLine += ":";
                Console.Write(col.GetValue(tableRefRows.ToSingleEnum()) + " , ");
                retLine += col.GetValue(tableRefRows.ToSingleEnum()) + " , ";
                if (fullRow)
                {
                    Console.Write(" BASE: ");
                    retLine += " BASE: ";
                }
            }
            Console.WriteLine("");
            retLine += Environment.NewLine;
            Console.WriteLine(indent(levelNum) + "*******************************");
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
