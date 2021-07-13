using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments.from
{
    public interface IFrom
    {
        string tableRefName { get; }
        IORecordID tableID { get; }
        IEnumerable<IRogueRow> StreamIRows(MultiRogueRow parentRow);
    }
}
