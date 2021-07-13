//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using rogue_core.rogueCore.hqlSyntaxV3.segments;
//using rogueCore.hqlSyntaxV3.group;
//using rogueCore.hqlSyntaxV3.segments;
//using rogueCore.hqlSyntaxV3.segments.level;
//using rogueCore.hqlSyntaxV3.segments.snippet;
//using rogueCore.hqlSyntaxV3.segments.table;
//using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

//namespace rogueCore.hqlSyntaxV3.filledSegments
//{
//    //public class SelectHQLStatement : CoreQueryStatement
//    //{
        
//    //    //List<ILevelGroup> filledGroups = new List<ILevelGroup>();
//    //    internal List<KeyValuePair<int, IMultiRogueRow>> hierarchyGrid = new List<KeyValuePair<int, IMultiRogueRow>>();
//    //    public SelectHQLStatement(string humanHQL) : base(humanHQL) {  }
//    //    public SelectHQLStatement() : base() { }
//    //    internal static string GetQueryByID(int storedProcID)
//    //    {
//    //        var filledQry = new SelectHQLStatement("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\" SELECT * ");
//    //        filledQry.Fill();
//    //        return filledQry.TopRows().First().GetValue("QUERY_TXT");
//    //    }
//    //    public override void Fill()
//    //    {
//    //        //Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
//    //        groups = new MultiSymbolSegment<PlainList<ILevelGroup>, ILevelGroup>(SymbolOrder.symbolbefore, finalQuery, keys, (x, y) => new segments.table.LevelGroup(x, y), this).segmentItems;
//    //        //Console.WriteLine("Segment Split: " + stopwatch2.ElapsedMilliseconds);
//    //        foreach (ILevelGroup thsGroup in groups)
//    //        {
//    //            thsGroup.Fill();
//    //            //filledGroups.Add(thsGroup.Fill());
//    //        }
//    //        if (isExecutable)
//    //        {
//    //            CodeCaller.RunProcedure(executableName, TopRows().ToArray());
//    //        }
//    //        //stopwatch2.Stop();
//    //        //Console.WriteLine("Query Data Load: " + stopwatch2.ElapsedMilliseconds);
//    //        //return this;
//    //    }
//    //    internal List<KeyValuePair<int, IMultiRogueRow>> IterateRows(Action<rowstatus, IMultiRogueRow> finalOutput = null)
//    //    {
//    //        if (finalOutput == null)
//    //        {
//    //            finalOutput = (rowstatus stat, IMultiRogueRow row) => { };
//    //        }
//    //        foreach (var topRow in TopRows())
//    //        {
//    //            LoopHierachy(topRow, 0, finalOutput);
//    //        }
//    //        return hierarchyGrid;
//    //    }
//    //    internal void LoopHierachy(IMultiRogueRow topRow, int currLvl, Action<rowstatus, IMultiRogueRow> finalOutput)
//    //    {
//    //        finalOutput(rowstatus.open,topRow);
//    //        hierarchyGrid.Add(new KeyValuePair<int, IMultiRogueRow>(currLvl,topRow));
//    //        currLvl++;
//    //        foreach (var childRow in topRow.childRows)
//    //        {
//    //            LoopHierachy(childRow, currLvl, finalOutput);
//    //        }
//    //        finalOutput(rowstatus.close,topRow);
//    //    }
//    //    public enum rowstatus
//    //    {
//    //        open, close
//    //    }
//    //    public StringBuilder PrintQuery()
//    //    {
//    //        //*print from top down
//    //        StringBuilder strBuild = new StringBuilder();
//    //        foreach (IMultiRogueRow topRow in TopRows())
//    //        {
//    //            strBuild = LoopPrintHierachy(topRow, 0, strBuild);
//    //        }
//    //        return strBuild;
//    //    }
//    //    StringBuilder LoopPrintHierachy(IMultiRogueRow topRow, int currLvl, StringBuilder stringBuild)
//    //    {
//    //        stringBuild.Append(topRow.PrintRow(false));
//    //        currLvl++;
//    //        foreach (var childRow in topRow.childRows)
//    //        {
//    //            LoopPrintHierachy(childRow, currLvl, stringBuild);
//    //        }
//    //        return stringBuild;
//    //    }
//    //}
//}