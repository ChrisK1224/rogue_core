using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV4.join
{
    class JoinToClause : SplitSegment, IJoinClause
    {
        public string parentTableName { get; private set; }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public IEnumerable<IMultiRogueRow> JoinRows(IHQLLevel lvl, IReadOnlyRogueRow row, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                yield return lvl.rows[i];
            }
        }
        public JoinToClause(string joinTxt, QueryMetaData metaData) : base(joinTxt, metaData)
        {
            parentTableName = joinTxt.AfterLastSpace();            
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
