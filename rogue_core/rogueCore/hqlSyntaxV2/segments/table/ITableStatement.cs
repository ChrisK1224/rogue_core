using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments.table
{
    public interface ITableStatement
    {
        public IFrom fromInfo { get; }
        public String tableRefName { get; }
        public String parentTableRefName { get; }
        public JoinClause joinClause { get; }
        public Func<IFilledLevel, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow>, IEnumerable<MultiRogueRow>> FilterAndStreamRows { get; }
        public bool isEncoded { get; }
        public string origStatement { get; }
        List<ILocationColumn> IndexedWhereColumns { get; }
    }
}
