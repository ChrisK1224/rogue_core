using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.level;

namespace rogueCore.hqlSyntaxV4.join
{
    public interface IJoinClause
    {
        //public ILocationColumn parentColumn { get; }
        //public Boolean joinAll { get; }
        public string parentTableName { get; }
        //public bool isJoinSet { get; }
        public IEnumerable<IMultiRogueRow> JoinRows(HQLLevel lvl, IReadOnlyRogueRow row, int rowCount);
        //public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        //public void PreFill(QueryMetaData metaData, string assumedTblName);
        //public List<string> UnsetParams();
        //public JoinTypes joinType { get;}
        public enum JoinTypes : int
        {
            [StringValue("ON")] inner = 1,
            [StringValue("OUTER")] outer = 2,
            [StringValue("TO")] to = 3
        }
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
    }
}
