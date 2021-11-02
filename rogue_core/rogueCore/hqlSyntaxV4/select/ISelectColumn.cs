using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public interface ISelectColumn  : ITempBase
    {
        public string columnName { get; }
        public string upperColumnName { get; }
        //public string colTableRefName { get; }
        public void ResetColName(string colName);
        public string GetValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> tableRefRows);
        public ColumnRowID baseColumnID { get; }
    }
}
