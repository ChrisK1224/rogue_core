using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public interface IMultiRogueRow
    {
        int levelNum { get; set; }
        List<IMultiRogueRow> childRows { get; }
        public string GetValue(IColumn col);
        public IEnumerable<KeyValuePair<string, string>> GetValueList();
        Dictionary<string, IReadOnlyRogueRow> tableRefRows { get; }
        string levelName { get; set; }
        string PrintRow(bool fullRow);
        string GetValue(string colName);
        string GetValueAt(int index);
        IMultiRogueRow ManualChildRow(IMultiRogueRow manualRow);
        string GetValue(string tableRefName, ColumnRowID colID);
        IMultiRogueRow MergeRow(string tableRefName, IReadOnlyRogueRow newRow, List<IMultiRogueRow> levelRows);
        public void RemoveFromParent();
        IEnumerable<IReadOnlyRogueRow> InvertRow(SelectRow invertedSelectRow, Dictionary<ColumnRowID, IReadOnlyRogueRow> columns, ComplexWordTable complexWordTable);
    }
}
