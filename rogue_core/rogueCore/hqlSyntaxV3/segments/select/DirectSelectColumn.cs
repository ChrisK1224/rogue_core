using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.standard.column;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    class DirectSelectColumn : DirectColumn, ISelectColumn
    {
        public new String columnName { get; private set; }
        public new string upperColumnName { get; private set; }

        public ColumnRowID baseColumnID { get {return base.columnRowID; } }

        internal DirectSelectColumn(ColumnRowID colID, string colTableRefname) : base(colID, colTableRefname)
        {
            this.columnName = base.columnName;
            this.upperColumnName = base.upperColumnName;
        }
        public void ResetColName(string colName)
        {
            this.columnName = colName;
            this.upperColumnName = colName.ToUpper();
        }
        public string GetValue(Dictionary<string, IReadOnlyRogueRow> tableRefRows)
        {
            return base.RetrieveStringValue(tableRefRows);
        }
    }
}
