using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.order
{
    class EmptyOrderBy : SplitSegment, IOrderBy
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public EmptyOrderBy(string hql, QueryMetaData metaData) : base(hql, metaData) { }
        public void OrderRows(List<IMultiRogueRow> rows)
        {
           
        }

        public override string PrintDetails()
        {
            return "";
        }
    }
}
