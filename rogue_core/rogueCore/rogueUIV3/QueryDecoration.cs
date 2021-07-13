using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.segments.snippet;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.rogueUIV3
{
    //class QueryDecoration
    //{
    //    public static List<IMultiRogueRow> GetUIQueryResults(IMultiRogueRow masterRow, string nonModifiedQry)
    //    {
    //        //--Do rest of logic as normal just call hierarchy from here by level name
    //        var segments = new Dictionary<string, LevelGroup>();
    //        string[] keys = new string[3] { "#--", "FROM", "SNIPPET" };
    //        var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(keys);
    //        var splits = Regex.Split(nonModifiedQry, regPattern);
    //        IMultiRogueRow lastRow = masterRow;
    //        var tempSegments = new LevelGroup();
    //        for (int i = 0; i < splits.Length; i++)
    //        {
    //            switch (splits[i].ToUpper())
    //            {
    //                case "#--":
    //                    tempSegments.Add(new CommentSection(splits[i] + " " + splits[i + 1]));
    //                    break;
    //                case "FROM":
    //                    var tbls = LevelStatement.ConvertToTableStatements(splits[i + 1], new SelectHQLStatement());
    //                    //IFrom fromInfo;
    //                    //if (splits[i + 1].StartsWith(FromConversion.convertSymbol.Trim()))
    //                    //{
    //                    //    fromInfo = new FromConversion(splits[i + 1],new HQLMetaData());
    //                    //}
    //                    //else
    //                    //{
    //                    //    fromInfo = new From(splits[i + 1], new HQLMetaData());
    //                    //}
    //                    var lvlSect = new LevelSection(splits[i] + " " + splits[i + 1]);
    //                    tempSegments.Add(lvlSect);
    //                    segments.Add(tbls[0].tableRefName, tempSegments);
    //                    tempSegments.levelName = tbls[0].tableRefName;
    //                    tempSegments.parentLvlName = tbls[0].parentTableRefName;
    //                    tempSegments.lvlSection = lvlSect;
    //                    tempSegments = new LevelGroup();
    //                    //QueryUILevel(splits[i] + " " + splits[i + 1], lastRow);
    //                    i++;
    //                    break;
    //                case "SNIPPET":
    //                    tempSegments.Add(new SnippetSection(splits[i] + " " + splits[i + 1]));
                        
    //                    //QueryUISnippet(splits[i] + " " + splits[i + 1], lastRow);
    //                    i++;
    //                    break;
    //                case "EXECUTE":
    //                    //QueryUIExecute(splits[i] + " " + splits[i + 1], lastRow);
    //                    tempSegments.Add(new ExecuteSection(splits[i] + " " + splits[i + 1]));
    //                    i++;
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
            
    //        foreach(var kvp in segments)
    //        {
    //            var lst = kvp.Value;
    //            if (lst.parentLevelName == "")
    //            {
    //                lst.LoadSegments(masterRow);
    //            }
    //            else
    //            {
    //                lst.LoadSegments(segments[lst.parentLevelName].topRow);
    //            }
    //        }
    //        if (tempSegments.Count > 0)
    //        {
    //            tempSegments.LoadSegments(masterRow);
    //        }
    //        return masterRow.childRows;
    //    }
     
    //    //static void LoopUIDecorLevels(MultiRogueRow topLvlParentRow, LevelStatement topLvl, TableGroupStatement grp)
    //    //{
    //    //    var masterRow = QueryUILevel(topLvl.origStatement, topLvlParentRow);
    //    //    foreach (LevelStatement childLvl in grp.levelStatements.Where(lvl => lvl.parentLevelRefName == topLvl.levelName))
    //    //    {
    //    //        LoopUIDecorLevels(masterRow, childLvl, grp);
    //    //    }
    //    //}
       
    //}
    class LevelGroup : List<IDecorSegment>
    {
        public string parentLevelName { get { 
                if (parentLvlName.Trim().StartsWith("@"))
                { return ""; } 
                else {
                    return parentLvlName;
                }
            } }
        internal string parentLvlName;
        public string levelName;
        public IMultiRogueRow topRow { get { return lvlSection.topRow; } }
        public LevelSection lvlSection;
        public void LoadSegments(IMultiRogueRow parentRow)
        {
            foreach(IDecorSegment thsSeg in this)
            {
                thsSeg.SetSection(parentRow);
            }
        }
    }
    class LevelSection : IDecorSegment
    {
        public IMultiRogueRow topRow { get; private set; }
        internal string levelName { get; private set; }
        internal string parentLvlName { get; private set; }
        string lvlTxt;
        internal LevelSection(string lvlTxt)
        {
            this.lvlTxt = lvlTxt;
        }
        static List<UIDecoratedTextItem> txtItems()
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem("FROM", "red", "bold"));
            items.Add(new UIDecoratedTextItem("AS", "red", "bold"));
            items.Add(new UIDecoratedTextItem("JOIN", "red", "bold"));
            items.Add(new UIDecoratedTextItem("COMBINE", "red", "bold", false, true));
            items.Add(new UIDecoratedTextItem("SELECT", "red", "bold", false, true));
            items.Add(new UIDecoratedTextItem("SNIPPET", "blue", "bold", "italic", false));
            items.Add(new UIDecoratedTextItem("WHERE", "red", "bold"));
            items.Add(new UIDecoratedTextItem(" ON ", "red", "bold"));
            items.Add(new UIDecoratedTextItem("LIMIT", "red", "bold"));
            items.Add(new UIDecoratedTextItem(",", "red", "bold"));
            items.Add(new UIDecoratedTextItem("[", "red", "bold"));
            items.Add(new UIDecoratedTextItem("];", "red", "bold"));
            return items;
        }
        public void SetSection(IMultiRogueRow parentRow)
        {
            //FilledLevel lvl = new FilledLevel(lvlTxt, new HQLMetaData());
            //levelName = lvl.levelName;
            //parentLvlName = lvl.parentLevelRefName;
            var divRow = UISection.ManualUIElementRow(parentRow, "groupbox");
            var marginleftRow = UISection.ManualUIAttributeRow(divRow, "marginleft", "25");
            var marginTopRow = UISection.ManualUIAttributeRow(divRow, "margintop", "20");
            var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(txtItems().Select(x => x.splitKey).ToArray());

            foreach (string txtLine in Regex.Split(lvlTxt, regPattern))
            {
                var uiSegs = txtItems().Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
                UIDecoratedTextItem uiSeg;
                if (uiSegs.Count == 0)
                {
                    uiSeg = UIDecoratedTextItem.Default(txtLine);
                }
                else
                {
                    uiSeg = uiSegs[0];
                    if (uiSeg.breakLineAfter) { UISection.ManualUIElementRow(divRow, "breakline", UISection.ParentRelationships.child); }
                }
                var txtRow = UISection.ManualUIElementRow(divRow, "label", UISection.ParentRelationships.child);
                var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
                var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
                var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.text);
            }
            topRow = divRow;
        }
    }
    class CommentSection: IDecorSegment
    {
        public IMultiRogueRow topRow { get; private set; }
        string commentTxt { get; set; }
        public void SetSection(IMultiRogueRow parentRow)
        {
            UISection.ManualUIElementRow(parentRow, "breakline", UISection.ParentRelationships.child);
            UISection.ManualUIElementRow(parentRow, "breakline", UISection.ParentRelationships.child);
            var txtRow = UISection.ManualUIElementRow(parentRow, "label", UISection.ParentRelationships.child);
            var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", "normal");
            var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", "gray");
            var textRow = UISection.ManualUIAttributeRow(txtRow, "text", commentTxt);
            topRow = txtRow;
        }
        internal CommentSection(string commentTxt)
        {
            this.commentTxt = commentTxt;
        }
    }
    class SnippetSection : IDecorSegment
    {
        public IMultiRogueRow topRow { get; private set; }
        string snippetTxt;
        internal SnippetSection(string snippetTxt)
        {
            this.snippetTxt = snippetTxt;
        }
        public void SetSection(IMultiRogueRow parentRow)
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem("SNIPPET", "blue", "bold", "italic", false));
            items.Add(new UIDecoratedTextItem(",", "blue", "bold"));
            items.Add(new UIDecoratedTextItem("[", "blue", "bold"));
            items.Add(new UIDecoratedTextItem("];", "blue", "bold"));

            UISection.ManualUIElementRow(parentRow, "breakline", UISection.ParentRelationships.child);
            UISection.ManualUIElementRow(parentRow, "breakline", UISection.ParentRelationships.child);
            Snippet snip = new Snippet(snippetTxt, null);

            var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(items.Select(x => x.splitKey).ToArray());
            foreach (string txtLine in Regex.Split(snippetTxt, regPattern))
            {
                var uiSegs = items.Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
                UIDecoratedTextItem uiSeg;
                if (uiSegs.Count == 0)
                {
                    uiSeg = UIDecoratedTextItem.Default(txtLine);
                }
                else
                {
                    uiSeg = uiSegs[0];
                }
                var txtRow = UISection.ManualUIElementRow(parentRow, "label", UISection.ParentRelationships.child);
                var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
                var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
                var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.splitKey);
                UISection.ManualUIAttributeRow(txtRow, "fontstyle", uiSeg.fontStyle);
            }
        }
    }
    class ExecuteSection : IDecorSegment
    {
        public IMultiRogueRow topRow { get; private set; }
        string executeTxt;
        internal ExecuteSection(string executeTxt)
        {
            this.executeTxt = executeTxt;
        }
        public void SetSection(IMultiRogueRow parentRow)
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem("EXECUTE", "orange", "bold", "italic", false));
            items.Add(new UIDecoratedTextItem(",", "orange", "bold"));
            items.Add(new UIDecoratedTextItem("(", "orange", "bold"));
            items.Add(new UIDecoratedTextItem(")", "orange", "bold"));
            Snippet snip = new Snippet(executeTxt, null);
            //var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);

            var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(items.Select(x => x.splitKey).ToArray());
            foreach (string txtLine in Regex.Split(executeTxt, regPattern))
            {
                var uiSegs = items.Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
                UIDecoratedTextItem uiSeg;
                if (uiSegs.Count == 0)
                {
                    uiSeg = UIDecoratedTextItem.Default(txtLine);
                }
                else
                {
                    uiSeg = uiSegs[0];
                }
                var txtRow = UISection.ManualUIElementRow(parentRow, "label", UISection.ParentRelationships.child);
                var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
                var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
                var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.splitKey);
                UISection.ManualUIAttributeRow(txtRow, "fontstyle", uiSeg.fontStyle);
            }
        }
    }
    interface IDecorSegment
    {
        //List<UIDecoratedTextItem> splitItems();
        void SetSection(IMultiRogueRow parentRow);
        IMultiRogueRow topRow { get; }
    }
    public class UIDecoratedTextItem
    {
        public string splitKey { get; }
        public string text { get { return splitKey + " &nbsp;"; } }
        public string textColor { get; }
        public string boldFont { get; }
        public string fontStyle { get; }
        public bool formatText { get; } = false;
        public bool breakLineAfter { get; } = false;
        public static UIDecoratedTextItem Default(string txt)
        {
            return new UIDecoratedTextItem(txt, "black", "normal", "normal", false);
        }
        public UIDecoratedTextItem(string text, string textColor, string boldFont, bool formatText = false, bool breakLineAfter = false) { this.splitKey = text; this.textColor = textColor; this.boldFont = boldFont; this.fontStyle = "normal"; this.formatText = formatText; this.breakLineAfter = breakLineAfter; }
        public UIDecoratedTextItem(string text, string textColor, string boldFont, string italic, bool breakLineAfter) { this.splitKey = text; this.textColor = textColor; this.boldFont = boldFont; this.fontStyle = italic; this.breakLineAfter = breakLineAfter; }
    }
}
