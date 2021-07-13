using System;
using System.Collections.Generic;
using rogue_core.RogueCode.hql.hqlSegments.table;

namespace rogue_core.rogueCore.hql.hqlSegments
{
    public class MatrixTableSegments
    {
        public Dictionary<String, TableSegment> tableSegments { get; private set; } = new Dictionary<string, TableSegment>();
        //*Int is matrixTableID for allJoins. THis is to set indexing when creating HQLTable. Set dictionary of joined values to its rowID
        private const char tableSegmentSeparator = '^';
        public MatrixTableSegments(String fullHQL)
        {
            fullHQL = fullHQL.Trim();
            string[] tableSnippets = fullHQL.Split(new[] { tableSegmentSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tableSnippets.Length; i++)
            {
                TableSegment newSegment = new TableSegment(tableSnippets[i]);
                tableSegments.Add(newSegment.tableInfo.tableRefName, newSegment);
                SetLevelAndChildRelation(newSegment);
            }
        }
        private void SetLevelAndChildRelation(TableSegment newSegment)
        {
            //* TODO Terrible code need to fix base row to not have weird issues with first base join
            if (newSegment.joinClauses[0].parentColumnRef.tableRefName.ToUpper() == "ROOT")
            {
                newSegment.level = 0;
            }
            //**This could cause isse. not sure what this logic did. else if (!(matrixLvl == 0 && newSegment.joinClauses[0].parentColumnRef.matrixTableID == 0))
            else
            {
                TableSegment parentSegment = tableSegments[newSegment.joinClauses[0].parentColumnRef.tableRefName];
                parentSegment.AddChildMatrixJoinClause(newSegment);
                newSegment.level = parentSegment.level + 1;
            }
        }
    }
}