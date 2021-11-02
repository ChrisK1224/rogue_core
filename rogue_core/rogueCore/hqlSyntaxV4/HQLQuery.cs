using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV4.group;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class HQLQuery : SplitSegment
    {
        List<IHqlGroup> groups = new List<IHqlGroup>();
        QueryMetaData metaData { get; }
        string ogQry { get; }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { GroupSplitters.withKey, GroupSplitters.withEndKey}; } }
        public HQLQuery(string qry) : base(qry, new QueryMetaData())
        {
            this.ogQry = qry;
            //string patthen = @"(?<=\s)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
            //string quoteAndParenStilHasProbs = @"(?<=\s)(\FROM|\yo)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
            //metaData = new QueryMetaData();
            //qry = qry.Trim();
            //if (!qry.ToUpper().StartsWith("WITH"))
            //{
            //    qry = "WITH DEFAULT " + qry + " STANDARD ";
            //}
            this.metaData = new QueryMetaData();
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
        public IEnumerable<IMultiRogueRow> topRows { get { return metaData.TopRows(); } }
        public void Execute()
        {
            foreach(var grp in groups)
            {
                grp.Fill();
            }
        }
        public IHqlGroup ParseGroup(string groupTxt)
        {
            //if (groupTxt.EndsWith(KeyNames.openParenthensis))
            //{
            //    return new CommandGroup(groupTxt, metaData);
            //}
            //else
            //{
                return new HQLGroup(groupTxt, metaData);
            //}
        }
        public void IterateRows2(Action<IMultiRogueRow> newRowOutput = null, Action<IMultiRogueRow> endRowOutput = null)
        {
            IterateRows(topRows.ToList(), newRowOutput, endRowOutput);
        }
        public DataTable AsDataTable()
        {
            DataTable results = new DataTable();
            foreach (var col in metaData.BaseLevelSelectRow().selectColumns)
            {
                results.Columns.Add(col.columnName);
            }
            foreach (IMultiRogueRow row in topRows)
            {
                DataRow newRow = results.NewRow();
                foreach (var pair in row.GetValueList())
                {
                    newRow[pair.Key] = pair.Value;
                }
                results.Rows.Add(newRow);
            }
            return results;
        }
        public static void IterateRows(List<IMultiRogueRow> topRows, Action<IMultiRogueRow> newRowOutput = null, Action<IMultiRogueRow> endRowOutput = null)
        {
            if (newRowOutput == null)
            {
                newRowOutput = (IMultiRogueRow row) => { };
            }
            if (endRowOutput == null)
            {
                endRowOutput = (IMultiRogueRow row) => { };
            }
            foreach (var topRow in topRows)
            {
                LoopHierachy(topRow, 0, newRowOutput, endRowOutput);
            }
        }
        static void LoopHierachy(IMultiRogueRow topRow, int currLvl, Action<IMultiRogueRow> newRowOutput, Action<IMultiRogueRow> endRowOutput)
        {
            newRowOutput(topRow);
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopHierachy(childRow, currLvl, newRowOutput, endRowOutput);
            }
            endRowOutput(topRow);
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
        //public void PrintSegments()
        //{
        //    metaData.PrintSegments();
        //}
        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> SyntaxSuggestions()
        {
            return splitKeys.Select(x => x.keyTxt);
        }
    }
}
