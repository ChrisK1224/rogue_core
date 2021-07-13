using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;

namespace rogueCore.hqlSyntaxV3.row
{
    public class ManualMultiRogueRow : IMultiRogueRow
    {
        public int levelNum { get; set; }
        public string levelName { get; private set; } = "AUTO";
        public List<IMultiRogueRow> childRows { get; } = new List<IMultiRogueRow>();
        public Dictionary<string, IReadOnlyRogueRow> tableRefRows => throw new NotImplementedException();
        public SelectRow selectRow => throw new NotImplementedException();
        Dictionary<string, string> vals = new Dictionary<string, string>();
        public IMultiRogueRow parentRow { get; set; }
        public ManualMultiRogueRow() { }
        public ManualMultiRogueRow(IMultiRogueRow parentRow) { this.parentRow = parentRow; levelNum = parentRow.levelNum + 1; parentRow.ManualChildRow(this); }
        public string GetValue(string colName)
        {
            if (vals.ContainsKey(colName))
            {
                return vals[colName.ToUpper()];
            }
            else
            {
                return "";
            }            
        }
        public string GetValue(string tableRefName, ColumnRowID colID)
        {
            throw new NotImplementedException();
        }
        public string GetValueAt(int index)
        {
            return vals.ElementAt(index).Value;
        }
        public string GetValue(ILocationColumn col)
        {
            return vals[col.columnName];
        }
        public string WhereClauseGet(ComplexColumn whereCol, IReadOnlyRogueRow testRow, string thsTableRef)
        {
            var allRows = new Dictionary<string, IReadOnlyRogueRow>() { { thsTableRef, testRow } };
            //allRows.FindChangeIfNotFound(thsTableRef, testRow);
            return whereCol.GetValue(allRows);
        }
        public IMultiRogueRow ManualChildRow(IMultiRogueRow childRow)
        {
            this.childRows.Add(childRow);
            //childRow.SetParent(this);
            //childRow.parentRow = this;
            childRow.levelNum = this.levelNum + 1;
            return childRow;
        }
        public void SetParent(IMultiRogueRow parentRow) { this.parentRow = parentRow; }
        public IMultiRogueRow MergeRow(string tableRefName, IReadOnlyRogueRow newRow, List<IMultiRogueRow> levelRows)
        {
            throw new NotImplementedException();
        }
        public void Add(string key, string val)
        {
            vals.Add(key, val);
        }
        public string PrintRow(bool fullRow)
        {
            string retLine = "";
            Console.Write(indent(levelNum) + "MANUALROW" + "|");
            retLine += indent(levelNum) + "MANUALROW" + "|";
            foreach (var thsPair in vals)
            {
                Console.Write(thsPair.Key);
                retLine += thsPair.Key;
                Console.Write(":");
                retLine += ":";
                Console.Write(thsPair.Value + " , ");
                retLine += thsPair.Value + " , ";
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
            //Console.WriteLine(indent(0) + "*******************************");
            //retLine += Environment.NewLine + indent(levelNum) + "*******************************";
            return retLine;
        }
        static string indent(int indentCount)
        {
            string retStr = "";
            for (int tab = 0; tab < indentCount; tab++) { retStr += "\t"; }
            return retStr;
        }
        public IEnumerable<KeyValuePair<string, string>> GetValueList()
        {
            foreach(var pair in vals)
            {
                yield return pair;
            }
        }
        public IEnumerable<IReadOnlyRogueRow> InvertRow(SelectRow invertedSelectRow, Dictionary<ColumnRowID, IReadOnlyRogueRow> columns, ComplexWordTable complexWordTable)
        {
            foreach (SelectColumn col in invertedSelectRow.selectColumns.Values)
            {
                var pairRogueRow = new ManualBinaryRow();
                foreach (var pair in columns[col.BaseColumnID].GetPairs())
                {
                    pairRogueRow.AddPair(pair.Key, pair.Value);
                }
                pairRogueRow.AddPair(8676, col.columnName);
                pairRogueRow.AddPair(8637, tableRefRows[col.upperColumnTableRefName].rowID.ToString());
                pairRogueRow.AddPair(8619, col.GetValue(tableRefRows));
                yield return pairRogueRow;
            }
        }
    }
}
