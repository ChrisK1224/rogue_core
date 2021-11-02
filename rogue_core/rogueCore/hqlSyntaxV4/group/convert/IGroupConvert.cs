using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group.convert
{
    public interface IGroupConvert
    {
        IEnumerable<IReadOnlyRogueRow> Transform(List<IHQLLevel> topLevels);
        IORecordID tableId { get; }
    }
}
