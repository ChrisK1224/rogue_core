using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using static rogueCore.hqlSyntaxV4.join.IJoinClause;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using files_and_folders;

namespace rogueCore.hqlSyntaxV4.join
{
    public class JoinClause : SplitSegment, IJoinClause
    {
        JoinTypes joinType { get; set; } = JoinTypes.inner;
        public StandardColumn parentColumn { get;private set; }
        StandardColumn localColumn { get; set; }
        EvaluationTypes evaluationType { get; set; }
        public string parentTableRef  { get { return parentColumn.colTableRefName; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { JoinSplitters.joinOn,JoinSplitters.joinEqual }; } }
        public string parentTableName { get { return parentColumn.colTableRefName; } }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxLoad;
        public JoinClause(String segment, QueryMetaData metaData) : base(segment, metaData) 
        {
            try
            {
                switch (splitList[0].Key)
                {
                    case "ON":
                        evaluationType = EvaluationTypes.equal;
                        break;
                    case "MERGE":
                        evaluationType = EvaluationTypes.merge;
                        break;
                    case "ROWTOCOLUMN":
                        evaluationType = EvaluationTypes.valuePair;
                        break;
                    case "MERGEFULL":
                        evaluationType = EvaluationTypes.merge;
                        joinType = JoinTypes.to;
                        break;
                    default:
                        throw new Exception("Unrecognized Join Type");
                }
                localColumn = new StandardColumn(splitList[0].Value.Trim(), metaData);
                parentColumn = new StandardColumn(splitList[1].Value.Trim(), metaData);
            }
            catch(Exception ex)
            {
                //LocalSyntaxLoad = ErrorSyntaxParts; 
            }
        }
        public IEnumerable<IMultiRogueRow> JoinRows(HQLLevel filledLevel, IReadOnlyRogueRow testRow, int currRowCount)
        {
            //string test = testRow.ITryGetValueByColumn(localColumn.columnRowID);
            foreach (IMultiRogueRow row in filledLevel.indexedRows[parentColumn].TryFindReturn(testRow.ITryGetValueByColumn(localColumn.columnRowID)))
            {
                yield return row;
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxLoad(parentRow, syntaxCommands);
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}