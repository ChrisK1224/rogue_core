using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    public interface ICalcableFromId
    {
        IORecordID CalcTableID(IMultiRogueRow row);
    }
}
