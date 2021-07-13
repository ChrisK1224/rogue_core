using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.segments.join.JoinClause;

namespace rogueCore.hqlSyntaxV3.segments.join
{
    class JoinToClause : IJoinClause
    {
        public ILocationColumn parentColumn => throw new NotImplementedException();
        public bool joinAll { get { return true; } }
        public string parentTableRef { get; private set; }
        public JoinTypes joinType { get; set; } = JoinTypes.to;
        public bool isJoinSet { get { return true; } }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        string origTxt { get; }
        public IEnumerable<IMultiRogueRow> JoinRows(ILevelStatement lvl, IReadOnlyRogueRow row, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                yield return lvl.rows[i];
            }
        }
        public JoinToClause(string joinTxt)
        {
            origTxt = joinTxt;
            try
            {
                parentTableRef = joinTxt.AfterFirstSpace();
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch (Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands) { LocalSyntaxParts(parentRow, syntaxCommands); }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinType.GetStringValue() + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            syntaxCommands.GetLabel(parentRow, parentTableRef, IntellsenseDecor.MyColors.black);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + "&nbsp;" + origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        public void PreFill(QueryMetaData metaData, string assumedTblName)
        {

        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            if (parentTableRef.StartsWith("@"))
            {
                unsets.Add(parentTableRef);
            }
            return unsets;
        }
    }
}
