using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using rogue_core.rogueCore.hqlSyntaxV2.segments;
using rogueCore.hqlSyntaxV2.segments.table;
using rogueCore.rogueUIV2;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    public class FilledHQLQuery : HQLQueryStatement
    {
        List<FilledGroup> filledGroups = new List<FilledGroup>();
        internal List<KeyValuePair<int, MultiRogueRow>> hierarchyGrid = new List<KeyValuePair<int, MultiRogueRow>>();
        public FilledHQLQuery(string humanHQL) : base(humanHQL) {  }
        internal static string GetQueryByID(int storedProcID)
        {
            var qryRow = new FilledHQLQuery("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\" SELECT * ").Fill().TopRows().First();
            return qryRow.values["QUERY_TXT"].Value;
        }
        public override FilledHQLQuery Fill()
        {
            Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //Fill();
            groups = new MultiSymbolSegment<PlainList<TableGroupStatement>, TableGroupStatement>(SymbolOrder.symbolbefore, finalQuery, keys, (x, y) => new FilledGroup(x, y), metaData).segmentItems;
            foreach (TableGroupStatement thsGroup in groups)
            {
                filledGroups.Add(thsGroup.Fill());
            }
            if (isExecutable)
            {
                CodeCaller.RunProcedure(executableName, TopRows().ToArray());
                //TopRows();
            }
            stopwatch2.Stop();
            Console.WriteLine("Query Data Load: " + stopwatch2.ElapsedMilliseconds);
            return this;
        }
        //public List<MultiRogueRow> GetUIQueryResults(MultiRogueRow masterRow)
        //{
        //    string[] keys = new string[3] { "#--", "FROM", "SNIPPET" };
        //    var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(keys);
        //    var splits = Regex.Split(nonModifiedQry, regPattern);
        //    MultiRogueRow lastRow = masterRow;
        //    for (int i =0; i <  splits.Length;i++)
        //    {
        //        switch (splits[i].ToUpper())
        //        {
        //            case "#--":
        //                QueryUIComment(splits[i] + " " + splits[i + 1], lastRow);
        //                break;
        //            case "FROM":
        //                lastRow = QueryUILevel(splits[i] + " " + splits[i + 1], lastRow);
        //                i++;
        //                break;
        //            case "SNIPPET":
        //                QueryUISnippet(splits[i] + " " + splits[i + 1], lastRow);
        //                i++;
        //                break;
        //            case "EXECUTE":
        //                QueryUIExecute(splits[i] + " " + splits[i + 1], lastRow);
        //                i++;
        //                break;
        //            default:

        //                break;
        //        }
        //    }
        //        //string removeComment = RemoveComments(nonModifiedQry);

        //        //groups = new MultiSymbolSegment<PlainList<TableGroupStatement>, TableGroupStatement>(SymbolOrder.symbolbefore, finalQuery, keys, (x, y) => new FilledGroup(x, y), metaData).segmentItems;
        //        //foreach (TableGroupStatement thsGroup in groups)
        //        //{
        //        //    foreach(var thsLvl in thsGroup.levelStatements.Where(lvl => lvl.levelNum == 0))
        //        //    {
        //        //        //LoopUIDecorLevels(masterRow, thsLvl, thsGroup);
        //        //    }
        //        //}
        //        return masterRow.childRows;
        //}
        ////MultiRogueRow QueryUIComment(string commentTxt, MultiRogueRow topRow)
        ////{
        ////    UISection.ManualUIElementRow(topRow, "breakline", UISection.ParentRelationships.child);
        ////    UISection.ManualUIElementRow(topRow, "breakline", UISection.ParentRelationships.child);
        ////    var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);
        ////    var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", "normal");
        ////    var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", "gray");
        ////    var textRow = UISection.ManualUIAttributeRow(txtRow, "text", commentTxt);
        ////    return txtRow;
        ////}
        //void QueryUISnippet(string snippetTxt, MultiRogueRow topRow)
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.Add(new UIDecoratedTextItem("SNIPPET", "blue", "bold", "italic", false));
        //    items.Add(new UIDecoratedTextItem(",", "blue", "bold"));
        //    items.Add(new UIDecoratedTextItem("[", "blue", "bold"));
        //    items.Add(new UIDecoratedTextItem("];", "blue", "bold"));

        //    UISection.ManualUIElementRow(topRow, "breakline", UISection.ParentRelationships.child);
        //    UISection.ManualUIElementRow(topRow, "breakline", UISection.ParentRelationships.child);
        //    Snippet snip = new Snippet(snippetTxt, null);
        //    //var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);
           
        //    var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(items.Select(x => x.splitKey).ToArray());
        //    foreach (string txtLine in Regex.Split(snippetTxt, regPattern))
        //    {
        //        var uiSegs = txtItems().Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
        //        UIDecoratedTextItem uiSeg;
        //        if (uiSegs.Count == 0)
        //        {
        //            uiSeg = UIDecoratedTextItem.Default(txtLine);
        //        }
        //        else
        //        {
        //            uiSeg = uiSegs[0];
        //        }
        //        var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);
        //        var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
        //        var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
        //        var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.splitKey);
        //        UISection.ManualUIAttributeRow(txtRow, "fontstyle", uiSeg.fontStyle);
        //    }
        //}
        //void QueryUIExecute(string snippetTxt, MultiRogueRow topRow)
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.Add(new UIDecoratedTextItem("EXECUTE", "orange", "bold", "italic", false));
        //    items.Add(new UIDecoratedTextItem(",", "orange", "bold"));
        //    items.Add(new UIDecoratedTextItem("(", "orange", "bold"));
        //    items.Add(new UIDecoratedTextItem(")", "orange", "bold"));
        //    Snippet snip = new Snippet(snippetTxt, null);
        //    //var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);

        //    var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(items.Select(x => x.splitKey).ToArray());
        //    foreach (string txtLine in Regex.Split(snippetTxt, regPattern))
        //    {
        //        var uiSegs = txtItems().Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
        //        UIDecoratedTextItem uiSeg;
        //        if (uiSegs.Count == 0)
        //        {
        //            uiSeg = UIDecoratedTextItem.Default(txtLine);
        //        }
        //        else
        //        {
        //            uiSeg = uiSegs[0];
        //        }
        //        var txtRow = UISection.ManualUIElementRow(topRow, "label", UISection.ParentRelationships.child);
        //        var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
        //        var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
        //        var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.splitKey);
        //        UISection.ManualUIAttributeRow(txtRow, "fontstyle", uiSeg.fontStyle);
        //    }
        //}
        //void LoopUIDecorLevels(MultiRogueRow topLvlParentRow, LevelStatement topLvl, TableGroupStatement grp)
        //{
        //    var masterRow = QueryUILevel(topLvl.origStatement, topLvlParentRow);
        //    foreach (LevelStatement childLvl in grp.levelStatements.Where(lvl => lvl.parentLevelRefName == topLvl.levelName))
        //    {
        //        LoopUIDecorLevels(masterRow, childLvl, grp);
        //    }
        //}
        //MultiRogueRow QueryUILevel(string lvlTxt, MultiRogueRow masterRow)
        //{
        //    var divRow = UISection.ManualUIElementRow(masterRow, "groupbox");
        //    var marginleftRow = UISection.ManualUIAttributeRow(divRow, "marginleft", "25");
        //    var marginTopRow = UISection.ManualUIAttributeRow(divRow, "margintop", "20");
        //    var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(txtItems().Select(x => x.splitKey).ToArray());
        //    foreach (string txtLine in Regex.Split(lvlTxt, regPattern))
        //    {
               
        //        var uiSegs = txtItems().Where(x => x.splitKey == txtLine.Trim().ToUpper()).ToList();
                
        //        UIDecoratedTextItem uiSeg;
        //        if (uiSegs.Count == 0)
        //        {
        //            uiSeg = UIDecoratedTextItem.Default(txtLine);
        //        }
        //        else
        //        {
        //           // UISection.ManualUIElementRow(divRow, "breakline", UISection.ParentRelationships.child);
        //            uiSeg = uiSegs[0];
        //            if(uiSeg.breakLineAfter) { UISection.ManualUIElementRow(divRow, "breakline", UISection.ParentRelationships.child); }
        //        }
        //        var txtRow = UISection.ManualUIElementRow(divRow, "label", UISection.ParentRelationships.child);
        //        var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "fontweight", uiSeg.boldFont);
        //        var colorRow = UISection.ManualUIAttributeRow(txtRow, "fontcolor", uiSeg.textColor);
        //        var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.text);
        //    }
        //    return divRow;
        //}
        //List<UIDecoratedTextItem> txtItems()
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.Add(new UIDecoratedTextItem("FROM", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem("AS", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem("JOIN", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem("COMBINE", "red", "bold", false, true));
        //    items.Add(new UIDecoratedTextItem("SELECT", "red", "bold", false, true));
        //    items.Add(new UIDecoratedTextItem("SNIPPET", "blue", "bold", "italic", false));
        //    items.Add(new UIDecoratedTextItem("WHERE", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem(" ON ", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem("LIMIT", "red", "bold"));
        //    items.Add(new UIDecoratedTextItem(",", "blue", "bold"));
        //    items.Add(new UIDecoratedTextItem("[", "blue", "bold"));
        //    items.Add(new UIDecoratedTextItem("];", "blue", "bold"));
        //    return items;
        //}
        internal List<KeyValuePair<int, MultiRogueRow>> IterateRows(Action<rowstatus, MultiRogueRow> finalOutput = null)
        {
            if (finalOutput == null)
            {
                finalOutput = (rowstatus stat, MultiRogueRow row) => { };
            }
            foreach (var topRow in metaData.TopRows())
            {
                LoopHierachy(topRow, 0, finalOutput);
            }
            return hierarchyGrid;
        }
        internal void LoopHierachy(MultiRogueRow topRow, int currLvl, Action<rowstatus, MultiRogueRow> finalOutput)
        {
            finalOutput(rowstatus.open,topRow);
            hierarchyGrid.Add(new KeyValuePair<int, MultiRogueRow>(currLvl,topRow));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopHierachy(childRow, currLvl, finalOutput);
            }
            finalOutput(rowstatus.close,topRow);
        }
        //*Bring BACK
        //internal static string StoredProcByID(int queryID)
        //{
        //    return new FilledHQLQuery("FROM HQL_QUERIES SELECT * WHERE ROGUECOLUMNID = \"" + queryID.ToString() + "\"").metaData.TopRows().First().values["QUERY_TXT"].Value;
        //}
        public enum rowstatus
        {
            open, close
        }
        public StringBuilder PrintQuery()
        {
            //*print from top down
            StringBuilder strBuild = new StringBuilder();
            foreach (MultiRogueRow topRow in metaData.TopRows())
            {
                strBuild = LoopPrintHierachy(topRow, 0, strBuild);
            }
            return strBuild;
        }
        StringBuilder LoopPrintHierachy(MultiRogueRow topRow, int currLvl, StringBuilder stringBuild)
        {
            stringBuild.Append(topRow.PrintRow());
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl, stringBuild);
            }
            return stringBuild;
        }
    }
}