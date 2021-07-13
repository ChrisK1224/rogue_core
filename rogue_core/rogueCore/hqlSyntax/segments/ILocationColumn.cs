using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments
{
    public interface ILocationColumn
    {
        String CalcStringValue(IRogueRow thsRow);
        String CalcStringValue(Dictionary<String,IRogueRow> rows);
        ColumnRowID columnRowID { get; set; }
        String colTableRefName { get; set; }
    }
}
