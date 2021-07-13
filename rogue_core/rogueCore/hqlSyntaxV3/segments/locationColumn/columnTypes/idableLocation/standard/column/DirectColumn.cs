using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.standard.column
{
    class DirectColumn : StandardColumn
    {
        public DirectColumn(ColumnRowID columnID, string colTableRefName) : base(columnID, colTableRefName) { }
        public DirectColumn(ColumnRowID columnID, string columnName, string colTableRefName) : base(columnID, columnName, colTableRefName) { }
    }
}
