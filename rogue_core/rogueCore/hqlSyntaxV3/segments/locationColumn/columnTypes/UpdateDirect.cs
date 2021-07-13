using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn
{
    class UpdateDirect : StandardColumn
    {
        //public ColumnRowID columnRowID { get; } = -1012;
        //public string colTableRefName { get; } = "";
        //public string columnName { get; } = "";
        //public bool isConstant { get; } = false;
        //public bool isEncoded { get; } = false;
        //public bool isStar { get; } = false;
        //public ColumnDirect(string colTblName)
        //{
        //    this.colTableRefName = colTblName;
        //    columnRowID = new ColumnRowID(0);
        //}
        internal UpdateDirect(ColumnRowID columnRowID) : base(columnRowID)
        {
            //this.columnRowID = columnRowID;
        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> parentRows)
        {
                return GetValue(parentRows.ElementAt(0).Value);           
        }
        //internal ColumnDirect(ColumnRowID columnRowID, string colTableRefName)
        //{
        //    this.columnRowID = columnRowID;
        //    this.colTableRefName = colTableRefName;
        //}
        //public string RetrieveStringValue(Dictionary<string, IRogueRow> rows)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
