using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV4.join.IJoinClause;

namespace rogueCore.hqlSyntaxV4.join
{
    class EmptyJoinClause : SplitSegment, IJoinClause
    {
        public string parentTableName { get; private set; } = "";
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public IEnumerable<IMultiRogueRow> JoinRows(HQLLevel lvl, IReadOnlyRogueRow row, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                yield return lvl.rows[i];
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            
        }
        public EmptyJoinClause(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            //parentTableRef = joinTxt.Split(".")[0];
        }
        public List<string> UnsetParams()
        {
            return new List<string>();
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
