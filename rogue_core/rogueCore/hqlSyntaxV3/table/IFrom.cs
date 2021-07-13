using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.from
{
    public interface IFrom 
    {
        
        IORecordID tableID { get; }
        string tableRefName { get; }
        //IEnumerable<string> UnsetParams();
        //IEnumerable<IRogueRow> StreamIRows(IMultiRogueRow parentRows);
    }
}
public interface IInsertFrom
{
    string tableRefName { get; }
    public IORecordID CalcTableID(IMultiRogueRow parentRow);
    void PreFill(QueryMetaData metaData);
    //IEnumerable<string> UnsetParams();
    //IEnumerable<IRogueRow> StreamIRows(IMultiRogueRow parentRows);
}
