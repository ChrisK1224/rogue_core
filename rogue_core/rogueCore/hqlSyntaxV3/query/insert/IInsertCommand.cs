using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.query.insert
{
    interface IInsertCommand
    {
        public void PreFill(QueryMetaData metaData, string defaultName);
        IReadOnlyRogueRow Execute(IMultiRogueRow parentRow, IORecordID recordID);
        List<string> UnsetParams();
    }
}
