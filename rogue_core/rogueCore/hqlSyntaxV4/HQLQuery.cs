using FilesAndFolders;
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
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { GroupSplitters.withKey, GroupSplitters.withEndKey}; } }
        public HQLQuery(string qry) : base(qry, new QueryMetaData())
        {
            //string patthen = @"(?<=\s)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
            //string quoteAndParenStilHasProbs = @"(?<=\s)(\FROM|\yo)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
            //metaData = new QueryMetaData();
            //qry = qry.Trim();
            //if (!qry.ToUpper().StartsWith("WITH"))
            //{
            //    qry = "WITH DEFAULT " + qry + " STANDARD ";
            //}
            var metaData = new QueryMetaData();
            //new HQLGroup(splitList[0].Value, metaData);            
            //var lastTxt = splitList[splitList.Count - 1];
            //var grpTxt = lastTxt.Value.AfterFirstKey(",");
            foreach(var grp in splitList.Where(x => x.Key == KeyNames.with))
            {
                var newGrp = new HQLGroup(grp.Value, metaData);
                groups.Add(newGrp);
                newGrp.topLevels.ForEach(x => metaData.AddLevel(x));
            }
            //.ToList().ForEach(x => groups.Add(new HQLGroup(x.Value,metaData)));
            //groups.ForEach(x => metaData.AddLevel(x.le)
            groups.Add(new HQLGroup(splitList.Where(x => x.Key == KeyNames.withEnd).First().Value, metaData));
            //new HQLGroup(qry, metaData);
        }
        //static 
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
            foreach (IMultiRogueRow topRow in groups[groups.Count-1].levels.First().rows)
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
