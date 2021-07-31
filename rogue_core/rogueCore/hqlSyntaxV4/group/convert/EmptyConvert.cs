using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group.convert
{
    class EmptyConvert : SplitSegment, IGroupConvert
    {
        public const string codeMatchConst = "";
        public static string codeMatchName { get { return codeMatchConst; } }
        public IORecordID tableId { get { return 0; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public EmptyConvert(string hql,  QueryMetaData metaData) : base(hql, metaData) { }
        public override string PrintDetails()
        {
            return "";
        }
        public IEnumerable<IReadOnlyRogueRow> Transform(List<HQLLevel> levels)
        {
            if (1 == 0) { yield return null; }
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            throw new NotImplementedException();
        }
    }
}
