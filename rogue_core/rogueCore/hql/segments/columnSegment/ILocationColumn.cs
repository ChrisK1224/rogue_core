using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments.columnSegment
{
    public interface ILocationColumn
    {
        String CalcStringValue(IRogueRow thsRow);
        ColumnRowID columnRowID { get; set; }
        String tableRefName { get; set; }
        String GetHQLText();
        String GetHumanHQLText();
    }
}
