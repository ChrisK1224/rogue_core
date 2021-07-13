using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.select;
using System;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntax
{
    public class FilledSelectRow : Dictionary<int, string>
    {
        public string tableRefName;
        public List<FilledSelectRow> childRows = new List<FilledSelectRow>();
        public Dictionary<string, IRogueRow> tableRefRows = new Dictionary<string, IRogueRow>();
        FilledSelectRow parentRow;
        public Dictionary<string, KeyValuePair<SelectColumn, string>> values { get; private set; } = new Dictionary<string, KeyValuePair<SelectColumn, string>>();
        public IRogueRow baseTableRow;
        internal bool isMerged = false;
        //SelectRow syntaxRow;
        //public List<string> colList {get{return syntaxRow.SelectColumns.Keys.ToList();}}
        internal FilledSelectRow() { tableRefRows = new Dictionary<string, IRogueRow>(); }
        FilledSelectRow(FilledSelectRow copyRow)
        {
            //this.tableRefName = copyRow.tableRefName;
            this.childRows = new List<FilledSelectRow>(copyRow.childRows);
            this.tableRefRows = new Dictionary<string, IRogueRow>(copyRow.tableRefRows);
            this.values = new Dictionary<string, KeyValuePair<SelectColumn, string>>(copyRow.values);
            this.parentRow = copyRow.parentRow;
            //syntaxRow = copyRow.syntaxRow;
            //this.baseTableRow = copyRow.baseTableRow;
        }
        internal FilledSelectRow(string tableRefName, SelectRow rowOutline, IRogueRow baseRow, FilledSelectRow parentRow)
        {
            this.tableRefName = tableRefName;
            this.baseTableRow = baseRow;
            tableRefRows = new Dictionary<string, IRogueRow>(parentRow.tableRefRows);
            tableRefRows.Add(tableRefName, baseRow);
            this.parentRow = parentRow;
            LoadValues(rowOutline);
            parentRow.childRows.Add(this);
            //syntaxRow = rowOutline;
        }
        internal FilledSelectRow(string tableRefName, SelectRow rowOutline, IRogueRow baseRow)
        {
            this.baseTableRow = baseRow;
            this.tableRefName = tableRefName;
            tableRefRows = new Dictionary<string, IRogueRow>();
            tableRefRows.Add(tableRefName, baseRow);
            LoadValues(rowOutline);
            //syntaxRow = rowOutline;
        }
        //*For custom transform purposes
        public FilledSelectRow(string tableRefName)
        {
            this.tableRefName = tableRefName;
            tableRefRows = new Dictionary<string, IRogueRow>();
            tableRefRows.Add(tableRefName, null);
        }
        public FilledSelectRow(string tableRefName, IRogueRow baseRow)
        {
            this.tableRefName = tableRefName;
            this.baseTableRow = baseRow;
            tableRefRows = new Dictionary<string, IRogueRow>();
            tableRefRows.Add(tableRefName, baseRow);
        }
        public FilledSelectRow(string tableRefName, FilledSelectRow parentRow, IRogueRow baseRow)
        {
            this.tableRefName = tableRefName;
            this.baseTableRow = baseRow;
            tableRefRows = new Dictionary<string, IRogueRow>(parentRow.tableRefRows);
            tableRefRows.Add(tableRefName, baseRow);
            this.parentRow = parentRow;
            parentRow.childRows.Add(this);
        }
        internal FilledSelectRow MergeRow(string thsTableRefName, SelectRow mergeRow, IRogueRow baseRow)
        {
            FilledSelectRow newChild = new FilledSelectRow(this);
            newChild.tableRefName = thsTableRefName;
            newChild.baseTableRow = baseRow;
            newChild.tableRefRows.Add(thsTableRefName, baseRow);
            newChild.LoadValues(mergeRow);
            if (!isMerged)
            {
                parentRow.childRows.Remove(this);
                //int dex = parentRow.childRows.FindIndex(this);

            }
            parentRow.childRows.Add(newChild);
            //*new this is causing THIS row to keep merged values. And so when cloned for next row up error trying to add more of the same values to THIS row since it wasn't just cloned
            //this.values = newChild.values;
            this.childRows = newChild.childRows;
            //this.Values = newChild.Values;
            //*end new
            isMerged = true;
            return newChild;
        }
        public static FilledSelectRow BaseRow()
        {
            return new FilledSelectRow();
        }
        void LoadValues(SelectRow rowOutline)
        {
            foreach (SelectColumn col in rowOutline.SelectColumns.Values)
            {
                values.Add(col.columnName.ToUpper(), new KeyValuePair<SelectColumn, string>(col, col.GetValue(tableRefRows)));
                //col.BaseColumnID;
            }
        }
        public string PrintRow(int levelNum, bool fullRow = false)
        {
            string retLine = "";
            Console.Write(indent(levelNum) + this.tableRefName + "|");
            retLine += indent(levelNum) + this.tableRefName + "|";
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
                    this.baseTableRow.PrintRow();
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
        public void SetParent(FilledSelectRow parent)
        {
            this.parentRow = parent;
        }
        public string GetValue(string colName)
        {
            KeyValuePair<SelectColumn, string> ret = new KeyValuePair<SelectColumn, string>(null, "");
            colName = colName.ToUpper();
            values.TryGetValue(colName, out ret);
            return ret.Value;
        }
    }
}