using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using rogueCore.hqlSyntaxV3.segments.join;
using static rogueCore.hqlSyntaxV3.segments.join.JoinClause;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.segments.join
{
    public interface IJoinClause
    {
        public ILocationColumn parentColumn { get; }
        public Boolean joinAll { get; }
        public string parentTableRef { get; }
        public bool isJoinSet { get; }
        public IEnumerable<IMultiRogueRow> JoinRows(ILevelStatement lvl, IReadOnlyRogueRow row, int rowCount);
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        public void PreFill(QueryMetaData metaData, string assumedTblName);
        public List<string> UnsetParams();
        public JoinTypes joinType { get; }
    }
}
