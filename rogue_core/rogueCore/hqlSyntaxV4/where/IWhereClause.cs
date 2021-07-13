using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.where
{
    public interface IWhereClause
    {
        public bool CheckWhereClause(string thsTblRef, IReadOnlyRogueRow thsRow, IMultiRogueRow fullRow = null);
        public bool CheckWhereClause(IMultiRogueRow fullRow);
        List<IColumn> evalColumns { get; }
    }
}
