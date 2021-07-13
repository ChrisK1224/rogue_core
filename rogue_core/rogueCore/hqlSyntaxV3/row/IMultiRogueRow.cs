using System;
using System.Collections.Generic;
using System.Text;
using rogueCore.hqlSyntaxV3.segments.select;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;

namespace rogueCore.hqlSyntaxV3.filledSegments
{
    public interface IMultiRogueRow
    {
        int levelNum { get; set; }
        List<IMultiRogueRow> childRows { get; }
        public string GetValue(ILocationColumn col);
        public IEnumerable<KeyValuePair<string, string>> GetValueList();
        Dictionary<string, IReadOnlyRogueRow> tableRefRows { get; }
        string levelName { get; }
        string PrintRow(bool fullRow);
        string GetValue(string colName);
        string GetValueAt(int index);
        public string WhereClauseGet(ComplexColumn whereCol, IReadOnlyRogueRow testRow, string thsTableRef);
        IMultiRogueRow ManualChildRow(IMultiRogueRow manualRow);
        string GetValue(string tableRefName, ColumnRowID colID);
        IMultiRogueRow MergeRow(string tableRefName, IReadOnlyRogueRow newRow, List<IMultiRogueRow> levelRows);
        public IEnumerable<IReadOnlyRogueRow> InvertRow(SelectRow invertedSelectRow, Dictionary<ColumnRowID, IReadOnlyRogueRow> columns, ComplexWordTable complexWordTable);
    }
}
