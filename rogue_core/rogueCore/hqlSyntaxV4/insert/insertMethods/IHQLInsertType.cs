using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.insert
{
    interface IHQLInsertType
    {
        public void PreFill(QueryMetaData metaData, string defaultName);
        IEnumerable<IReadOnlyRogueRow> Execute(IMultiRogueRow row, ICalcableFromId from);
    }
}
