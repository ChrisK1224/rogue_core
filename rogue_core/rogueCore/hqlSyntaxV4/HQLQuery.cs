using rogue_core.rogueCore.hqlSyntaxV4.group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class HQLQuery : SplitSegment
    {
        //QueryMetaData metaData { get; }
        List<HQLGroup> groups = new List<HQLGroup>();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { GroupSplitters.withKey }; } }
        public HQLQuery(string qry) : base(qry, new QueryMetaData())
        {
            //string patthen = @"(?<=\s)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
            //string quoteAndParenStilHasProbs = @"(?<=\s)(\FROM|\yo)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
            //metaData = new QueryMetaData();
            splitList.ForEach(x => groups.Add(new HQLGroup(x.Value, new QueryMetaData())));
            //new HQLGroup(qry, metaData);
        }
        public void Execute()
        {
            foreach(var grp in groups)
            {
                grp.Fill();
            }
        }
        public StringBuilder PrintQuery()
        {
            //*print from top down
            StringBuilder strBuild = new StringBuilder();
            foreach (IMultiRogueRow topRow in groups[0].levels.First().rows)
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
        //public void PrintSegments()
        //{
        //    metaData.PrintSegments();
        //}

        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }
    }
}
