using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.segments.join.JoinClause;

namespace rogueCore.hqlSyntaxV3.segments.join
{
    class EmptyJoinClause : IJoinClause
    {
        public ILocationColumn parentColumn => throw new NotImplementedException();
        public bool joinAll { get { return true; } }
        public JoinTypes joinType { get; set; } = JoinTypes.inner;
        public string parentTableRef { get; private set; } = "";
        public bool isJoinSet { get { return false; } }
        public IEnumerable<IMultiRogueRow> JoinRows(ILevelStatement lvl, IReadOnlyRogueRow row, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                yield return lvl.rows[i];
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            
        }
        public EmptyJoinClause()
        {
            //parentTableRef = joinTxt.Split(".")[0];
        }
        public void PreFill(QueryMetaData metaData, string assumedTblName)
        {

        }
        public List<string> UnsetParams()
        {
            return new List<string>();
        }
    }
}
