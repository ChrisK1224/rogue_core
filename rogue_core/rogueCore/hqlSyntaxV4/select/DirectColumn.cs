using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    class DirectColumn : ISelectColumn
    {
        public string columnName { get; private set; }
        public string upperColumnName { get { return columnName.ToUpper(); } }
        public ColumnRowID baseColumnID { get; }
        string tableRefName { get; }
        public DirectColumn(string tableRefName, string columnName, ColumnRowID colId)
        {
            this.tableRefName = tableRefName;
            this.columnName = columnName;
            this.baseColumnID = colId;
        }
        public string GetValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> tableRefRows)
        {
            return tableRefRows.First()[tableRefName].ITryGetValueByColumn(baseColumnID);
        }

        public void ResetColName(string colName)
        {
            this.columnName = colName;
        }

        public IEnumerable<string> SyntaxSuggestions()
        {
            return new List<string>();
        }
    }
}
