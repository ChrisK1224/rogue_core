using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.level;
using rogueCore.hqlSyntaxV2.segments.select;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.table
{
    public abstract class TableGroupStatement
    {
        protected TableGroupInfo groupInfo { get; set; }
        internal List<LevelStatement> levelStatements = new List<LevelStatement>();
        string[] keys { get { return new string[2] { LevelStatement.splitKey, TableGroupInfo.splitKey }; } }
        protected HQLMetaData metaData;
        public TableGroupStatement(String humanHQL, HQLMetaData metaData)
        {
            this.metaData = metaData;
            levelStatements = new MultiSymbolSegment<PlainList<LevelStatement>, LevelStatement>(SymbolOrder.symbolbefore, humanHQL, new string[] { LevelStatement.splitKey },(x,y) => new FilledLevel(x,y), metaData, TableGroupInfo.splitKey).segmentItems;
            groupInfo = new TableGroupInfo(new MultiSymbolString<DictionaryListValues<string>>(SymbolOrder.symbolbefore, humanHQL, keys, metaData).segmentItems[TableGroupInfo.splitKey][0], metaData);
        }
        internal abstract FilledGroup Fill();
        public List<string> FormatQueryTxt()
        {
            List<string> lines = new List<string>();
            //lines.Add("|");
            //foreach (TableStatement loadTbl in levelStatements.Values.Where(x => x.level == 0))
            //{
            //    lines.Concat(LoopChildTables(loadTbl, lines));
            //}
            lines.Add(groupInfo.origStatement());
            return lines;
        }
        //List<string> LoopChildLevels(LevelStatement topTbl, List<string> lines)
        //{
        //    string formattedLine = "";
        //    for (int i = 0; i < topTbl.levelNum; i++)
        //    {
        //        formattedLine += "\t";
        //    }
        //    formattedLine += LevelStatement.splitKey + "  " + topTbl.origStatement;
        //    lines.Add(formattedLine);
        //    List<LevelStatement> childTables = levelStatements.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == topTbl.levelName).Select(x => x).Distinct().ToList();

        //    foreach (LevelStatement childTbl in childTables)
        //    {
        //        LoopChildLevels(childTbl, lines);
        //    }
        //    return lines;
        //}
        //void PrintTopTableGroup(IEnumerable<FilledSelectRow> topRows)
        //{
        //    foreach (var topRow in topRows)
        //    {
        //        LoopPrintHierachy(topRow, 0);
        //    }
        //}
        //void LoopPrintHierachy(FilledSelectRow topRow, int currLvl)
        //{
        //    topRow.PrintRow(currLvl);
        //    currLvl++;
        //    foreach (var childRow in topRow.childRows)
        //    {
        //        LoopPrintHierachy(childRow, currLvl);
        //    }
        //}
    }
}
