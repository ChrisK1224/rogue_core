using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using rogue_core.rogueCore.syntaxCommand;

namespace rogueCore.hqlSyntaxV3.segments.snippet
{
    class SnippetParam
    {
        internal string paramID;
        internal string paramValue;
        internal const char paramSplit = '=';
        string origTxt { get; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        internal SnippetParam(string paramHql)
        {
            try
            {
                origTxt = paramHql;
                int index = paramHql.IndexOf(paramSplit);
                paramID = paramHql.Substring(0, index).Trim().ToUpper();
                paramValue = paramHql.Substring(index + 1);
                paramValue = stringHelper.getStringBetweenFirstLastOccurance(paramValue, '"');
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();            
            if (paramValue.Contains("@"))
            {
                unsets.Add(paramValue);
            }
            //if (paramID.Contains("@"))
            //{
            //    unsets.Add(paramID);
            //}
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, paramID + SnippetParam.paramSplit, IntellsenseDecor.MyColors.blue);
            if (paramValue.Contains("@"))
            {
                syntaxCommands.GetLabel(parentRow,  "\"" + paramValue + "\"", IntellsenseDecor.MyColors.yellow);
            }
            else
            {
                syntaxCommands.GetLabel(parentRow,  "\"" + paramValue + "\"", IntellsenseDecor.MyColors.green);
            }
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //syntaxCommands.GetLabel(parentRow, SnippetParam.paramSplit + "\"" + paramValue + "\"", IntellsenseDecor.MyColors.green);
        }
    }
}
