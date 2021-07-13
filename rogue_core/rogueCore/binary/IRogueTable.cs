using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.where;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary
{
    public interface IRogueTable
    {
        IReadOnlyList<IRogueRow> currWriteRows { get; }
        void AddWriteRow(IRogueRow thsRow);
        void Write();
        void UpdateRewrite();
        void DeleteRewrite(List<IReadOnlyRogueRow> deletedRows);
        IEnumerable<IReadOnlyRogueRow> StreamDataRows();
        IRogueRow NewWriteRow();
        IORecordID ioItemID { get; }
    }
}
