using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    public interface ISelectColumn
    {
        public string columnName { get; }
        public string upperColumnName { get; }
        public string colTableRefName { get; }
        public void ResetColName(string colName);
        public string GetValue(Dictionary<string, IReadOnlyRogueRow> tableRefRows);
        public ColumnRowID baseColumnID { get; }
    }
}
