using rogue_core.rogueCore.hqlSyntaxV4.group;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class HQLQuery
    {
        QueryMetaData metaData { get; }        
        public HQLQuery(string qry)
        {
            //string patthen = @"(?<=\s)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
            //string quoteAndParenStilHasProbs = @"(?<=\s)(\FROM|\yo)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
            metaData = new QueryMetaData();
            new HQLGroup(qry, metaData);
        }
        public void Execute()
        {
            foreach(var grp in metaData.groups)
            {
                grp.Fill();
            }
        }
        public StringBuilder PrintQuery()
        {
            //*print from top down
            StringBuilder strBuild = new StringBuilder();
            foreach (IMultiRogueRow topRow in metaData.TopRows())
            {
                strBuild = LoopPrintHierachy(topRow, 0, strBuild);
            }
            return strBuild;
        }
        StringBuilder LoopPrintHierachy(IMultiRogueRow topRow, int currLvl, StringBuilder stringBuild)
        {
            stringBuild.Append(topRow.PrintRow(false));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl, stringBuild);
            }
            return stringBuild;
        }
        public void PrintSegments()
        {
            metaData.PrintSegments();
        }
    }
}
