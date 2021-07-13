using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.group;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.snippet 
{
    class Snippet : ISnippetOrLevel
    {
        const string paramSep = ",";
        const char snippetEnd = ']';
        const char snippetStart = '[';
        const char endCommand = ';';
        public const string snippetSplit = "SNIPPET";
        string snippetName;
        static int uniqueEnforcer = 1;
        //*Should be able to get rid of this not working for some reason
        bool isSnipsSet = false;
        List<SnippetParam> snipParams;
        static Dictionary<string, string> snippetQueries = SetQueries();
        //SelectHQLStatement queryStatement;
        string origTxt { get; }

        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public List<ILevelStatement> levelStatements { get; private set; } = new List<ILevelStatement>();
        public Snippet(string snippetHQL,Dictionary<string,string> parameters) 
        {
            origTxt = snippetHQL;
            try
            {
                snippetName = stringHelper.BeforeFirstChar(snippetHQL, snippetStart).Trim();
                string paramTxt = stringHelper.get_string_between_2(snippetHQL, "[", "]");
                snipParams = new MultiSymbolSegment<PlainList<SnippetParam>, SnippetParam>(SymbolOrder.symbolafter, paramTxt, new string[1] { paramSep }, (x) => new SnippetParam(x)).segmentItems;
                //SetQueries();
                //snippetName = stringHelper.BeforeFirstChar(snippetHQL, snippetEnd).Trim();
                //string paramTxt = stringHelper.get_string_between_2(snippetHQL, "[", "]");
                //snipParams = new MultiSymbolSegment<PlainList<SnippetParam>, SnippetParam>(SymbolOrder.symbolafter, paramTxt, new string[1] { paramSep }, (x, y) => new SnippetParam(x, y), queryStatement).segmentItems;
                string generatedLevelGroupTxt = SnipText();
                generatedLevelGroupTxt = generatedLevelGroupTxt.Replace("@UNIQUE", uniqueEnforcer.ToString());
                if (parameters != null)
                {
                    foreach (var pair in parameters)
                    {
                        generatedLevelGroupTxt = generatedLevelGroupTxt.Replace(pair.Key, pair.Value);
                    }

                }
                uniqueEnforcer++;
                LevelGroup lvl = new LevelGroup(generatedLevelGroupTxt, parameters);
                levelStatements.AddRange(lvl.levelStatements);
                LocalSyntaxParts = StandardSyntaxParts;
                //SetQueries();            
                //levelStatements = new MultiSymbolSegment<PlainList<ILevelStatement>, ILevelStatement>(SymbolOrder.symbolbefore, generatedLevelGroupTxt, new string[] { LevelStatement.splitKey }, (x, y) => new LevelStatement(x, y), queryStatement, TableGroupInfo.splitKey).segmentItems;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }

        }
        //public void PreFill(QueryMetaData metaData)
        //{
        //    foreach(ILevelStatement thsLevel in levelStatements)
        //    {
        //        thsLevel.PreFill(metaData);
        //    }
        //}
        //internal void LoadSegment(string segKey, string segTxt)
        //{
        //    switch (segKey)
        //    {
        //        case LevelStatement.splitKey:
        //            levelStatements.Add(new LevelStatement(segTxt, queryStatement));
        //            break;
        //        case Snippet.snippetSplit:
        //            levelStatements.AddRange((new Snippet(segTxt, queryStatement).levelStatements));
        //            break;
        //    }
        //}
        //internal void LoadSegment(string segKey, string segTxt)
        //{
        //    switch (segKey)
        //    {
        //        case LevelStatement.splitKey:
        //            levelStatements.Add(new LevelStatement(segTxt, queryStatement));
        //            break;
        //        case Snippet.snippetSplit:
        //            levelStatements.AddRange((new Snippet(segTxt, queryStatement).levelStatements));
        //            break;
        //        case TableGroupInfo.splitKey:
        //            groupInfo = new TableGroupInfo(segTxt);
        //            break;

        //    }
        //}
        public void Fill()
        {
            //levelGroup.Fill();
        }
        internal string SnipText()
        {
            string getSnipTxt = snippetQueries[snippetName];
            foreach (SnippetParam thsParam in snipParams)
            {
                getSnipTxt = getSnipTxt.Replace(thsParam.paramID, thsParam.paramValue);
            }
            return getSnipTxt;
        }
        static Dictionary<string, string> SetQueries()
        {
            Dictionary<string, string> qrys = new Dictionary<string, string>();
            foreach (var iRow in new IORecordID(7415).ToTable().StreamDataRows())
            {
                if (!qrys.ContainsKey(iRow.GetValueByColumn(7420)))
                {
                    qrys.Add(iRow.GetValueByColumn(7420), iRow.GetValueByColumn(7422));
                }
            }
            return qrys;
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var snipParam in snipParams)
            {
                unsets.AddRange(snipParam.UnsetParams());
            }
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, "&emsp;" + snippetSplit + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //syntaxCommands.GetLabel(parentRow, snippetName + snippetStart, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //for(int i = 0; i < snipParams.Count-1; i++)
            //{
            //    syntaxCommands.GetLabel(parentRow, snipParams[i].paramID, IntellsenseDecor.MyColors.blue);
            //    syntaxCommands.GetLabel(parentRow, SnippetParam.paramSplit + "\"" +  snipParams[i].paramValue + "\"", IntellsenseDecor.MyColors.green);
            //    syntaxCommands.GetLabel(parentRow, paramSep, IntellsenseDecor.MyColors.blue);
            //}
            //if (snipParams.Count > 0)
            //{
            //    syntaxCommands.GetLabel(parentRow, snipParams[snipParams.Count-1].paramID, IntellsenseDecor.MyColors.blue);
            //    syntaxCommands.GetLabel(parentRow, SnippetParam.paramSplit + "\"" + snipParams[snipParams.Count - 1].paramValue + "\"", IntellsenseDecor.MyColors.green);
            //}
            //syntaxCommands.GetLabel(parentRow, snippetEnd.ToString() + endCommand, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&emsp;" + snippetSplit + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            syntaxCommands.GetLabel(parentRow, snippetName + snippetStart, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            for (int i = 0; i < snipParams.Count; i++)
            {
                snipParams[i].LoadSyntaxParts(parentRow, syntaxCommands);
                if(i != snipParams.Count-1){
                    syntaxCommands.GetLabel(parentRow, paramSep, IntellsenseDecor.MyColors.blue);
                }
            }
            //if (snipParams.Count > 0)
            //{
            //    syntaxCommands.GetLabel(parentRow, snipParams[snipParams.Count - 1].paramID, IntellsenseDecor.MyColors.blue);
            //    syntaxCommands.GetLabel(parentRow, SnippetParam.paramSplit + "\"" + snipParams[snipParams.Count - 1].paramValue + "\"", IntellsenseDecor.MyColors.green);
            //}
            syntaxCommands.GetLabel(parentRow, snippetEnd.ToString() + endCommand, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
    }
}
