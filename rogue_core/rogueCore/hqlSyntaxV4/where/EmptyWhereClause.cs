using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.where
{
    class EmptyWhereClause : SplitSegment, IWhereClause
    {
        
        public List<IColumn> evalColumns { get; } = new List<IColumn>();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public EmptyWhereClause(string hql, QueryMetaData metaData) : base(hql, metaData) { }
        public bool CheckWhereClause(string thsTblRef, IReadOnlyRogueRow thsRow, IMultiRogueRow fullRow = null)
        {
            return true;
        }
        public override string PrintDetails()
        {
            return "";
        }

        public bool CheckWhereClause(IMultiRogueRow fullRow)
        {
            return true;
        }
    }
}
