using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using rogue_core.rogueCore.hqlSyntaxV3.query;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.group;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.segments.snippet;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.table
{
    public class LevelGroup : ILevelGroup
    {
        protected TableGroupInfo groupInfo { get; set; }
        public List<ILevelStatement> levelStatements { get; private set; } = new List<ILevelStatement>();
        List<ISnippetOrLevel> snippetOrLevels = new List<ISnippetOrLevel>();
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        Dictionary<string, string> parameters;
        string origTxt { get; }
        //*Need to fix this to split on newline OR Space before and after*** For SNIPPET 
        string[] keys { get { return new string[4] { LevelStatement.splitKey, Snippet.snippetSplit , TableGroupInfo.splitKey, InsertHQLStatement.splitKey }; } }
        public LevelGroup(String humanHQL, Dictionary<string,string> parameters)
        {
            origTxt = humanHQL;
            try
            {
                this.parameters = parameters;
                var lstValues = new MultiSymbolString<ListKeyPairs<string>>(SymbolOrder.symbolbefore, humanHQL, keys);
                foreach (var seg in lstValues.segmentItems)
                {
                    LoadSegment(seg.Key, seg.Value);
                }
                LocalSyntaxParts = StandardSyntaxParts;
                //levelStatements = new MultiSymbolSegment<PlainList<ILevelStatement>, ILevelStatement>(SymbolOrder.symbolbefore, humanHQL, new string[] { LevelStatement.splitKey },(x,y) => new LevelStatement(x,y), queryStatement, TableGroupInfo.splitKey).segmentItems;
                //groupInfo = new TableGroupInfo(new MultiSymbolString<DictionaryListValues<string>>(SymbolOrder.symbolbefore, humanHQL, keys, queryStatement).segmentItems[TableGroupInfo.splitKey][0]);
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void PreFill(QueryMetaData metaData)
        {
            try
            {
                foreach (var lvl in levelStatements.Where(lvl => lvl.isTopLevel))
                {
                    lvl.PreFill(metaData);
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
            
            //foreach(var snip in snippetOrLevels.Where(snip => snip is Snippet))
            //{
            //    ((Snippet)snip).PreFill(metaData);
            //}
        }
        internal void LoadSegment(string segKey, string segTxt)
        {
            //try
            //{
                switch (segKey)
                {
                    case LevelStatement.splitKey:
                    case InsertHQLStatement.splitKey:
                        var lvl = new LevelStatement(segTxt);
                        levelStatements.Add(lvl);
                        snippetOrLevels.Add(lvl);
                        break;
                    case Snippet.snippetSplit:
                        var snip = new Snippet(segTxt, parameters);
                        levelStatements.AddRange(snip.levelStatements);
                        snippetOrLevels.Add(snip);
                        break;
                    case TableGroupInfo.splitKey:
                        groupInfo = new TableGroupInfo(segTxt);
                        break;
                    
                }
            //}
            //catch(Exception ex)
            //{
            //    // string exx = ex.ToString();
                
            //}            
        }
        public void Fill()
        {
            try
            {
                IEnumerable<ILevelStatement> lvls = levelStatements.OrderBy(x => x.levelNum);
                foreach (var fillLvl in lvls)
                {
                    //    if (fillLvl.levelNum == 0)
                    //    {
                    //        fillLvl.Fill();                    
                    //    }
                    //    else
                    //    {
                    //if (fillLvl.levelName == "COLID" || fillLvl.levelName == "ColID")
                    //{
                    //    string blah = "SDF";
                    //}
                    fillLvl.Fill();
                    //}

                    //}
                }
            }
            catch(Exception ex)
            {
                string bl = ex.ToString();
            }
            
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //IMultiRogueRow currParent;
            //foreach(ISnippetOrLevel snippetOrLevel in snippetOrLevels)
            //{
            //    if(snippetOrLevel is Snippet)
            //    {
            //        snippetOrLevel.LoadSyntaxParts(parentRow);
            //    }
            //    else
            //    {
            //        foreach (ILevelStatement fillLvl in snippetOrLevel.levelStatements.OrderBy(x => x.levelNum))
            //        {
            //            if (fillLvl.levelNum == 0)
            //            {
            //                fillLvl.LoadSyntaxParts(parentRow);
            //            }
            //            else
            //            {
            //                fillLvl.LoadSyntaxParts(queryStatement.ParentLevelByChildName(fillLvl.parentLevelRefName).divRow);
            //            }
            //        }
            //    }
               
            //}
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            foreach (ISnippetOrLevel snippetOrLevel in snippetOrLevels)
            {
                if (snippetOrLevel is Snippet)
                {
                    snippetOrLevel.LoadSyntaxParts(parentRow, syntaxCommands);
                }
                else
                {
                    foreach (ILevelStatement fillLvl in snippetOrLevel.levelStatements.OrderBy(x => x.levelNum))
                    {
                        fillLvl.LoadSyntaxParts(parentRow, syntaxCommands);
                        //if (fillLvl.levelNum == 0)
                        //{
                        //    fillLvl.LoadSyntaxParts(parentRow);
                        //}
                        //else
                        //{
                        //    fillLvl.LoadSyntaxParts(fillLvl.paqueryStatement.ParentLevelByChildName(fillLvl.parentLevelRefName).divRow);
                        //}
                    }
                }
            }
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            //IMultiRogueRow currParent;
            syntaxCommands.GetLabel(parentRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var itm in snippetOrLevels)
            {
                unsets.AddRange(itm.UnsetParams());
            }
            return unsets;
        }
    }
}
