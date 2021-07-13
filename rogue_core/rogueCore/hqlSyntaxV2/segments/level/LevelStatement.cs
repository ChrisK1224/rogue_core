using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.attachment;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.levelConversion;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.level
{
    public abstract class LevelStatement : IFilledLevel
    {
        const string lvlSplitKey = "COMBINE";
        internal const string splitKey = "FROM";
        public string levelName { get { return lvlTable.tableRefName; } }
        const string startComment = "#--";
        const string endComment = "--#";
        public int levelNum { get; set; } = 0;
        internal string origStatement { get { return splitKey + " " + origTxt; } }
        string origTxt;
        internal SelectRow selectRow;
        public List<ITableStatement> allTableStatements { get { var tempLst = new List<ITableStatement>(combinedTables); tempLst.Add(lvlTable); return tempLst; } }
        public List<string> allTableNames { get { return allTableStatements.Select(tbl => tbl.tableRefName).ToList(); } }
        internal JoinClause joinClause { get { return lvlTable.joinClause; } }
        internal ITableStatement lvlTable { get; private set; }
        internal List<ITableStatement> combinedTables { get; private set; }  = new List<ITableStatement>();
        internal String parentLevelRefName { get { return joinClause.isSet ? joinClause.parentColumn.colTableRefName : ""; } }
        protected string[] keys { get; } = new string[2] { lvlSplitKey, SelectRow.splitKey };
        protected HQLMetaData metaData;
        public Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<MultiRogueRow>>>> indexedRows { get; } = new Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<MultiRogueRow>>>>();
        public Dictionary<string, Dictionary<ColumnRowID, Dictionary<int, List<MultiRogueRow>>>> indexedRows2f { get; } = new Dictionary<string, Dictionary<ColumnRowID, Dictionary<int, List<MultiRogueRow>>>>();
        public List<MultiRogueRow> rows { get; } = new List<MultiRogueRow>();
        internal static List<ITableStatement> ConvertToTableStatements(string hqlTxt, HQLMetaData metaData)
        {
            return new MultiSymbolSegment<PlainList<ITableStatement>, ITableStatement>(SymbolOrder.symbolbefore, hqlTxt, new string[] { lvlSplitKey }, NewTableStatement, metaData, SelectRow.splitKey).segmentItems;
        }
        internal static ITableStatement NewTableStatement(string hql, HQLMetaData metaData)
        {
            if (hql.Trim().ToUpper().StartsWith("EXECUTE"))
            {
                return new ExecutableTableStatement(hql, metaData);
            }
            else
            {
                return new FilledTable(hql, metaData);
            }
            //switch (hql.ToUpper().Trim().BeforeFirstSpace())
            //{
            //    case "EXECUTE":
            //        return new FilledTable(hql, metaData);
            //    default:
            //        return new FilledTable(hql, metaData);
            //}
        }
        internal LevelStatement(string hqlTxt, HQLMetaData metaData)
        {
            this.metaData = metaData;
            origTxt = hqlTxt;
            hqlTxt = RemoveComments(hqlTxt);
            //var allTableStatements = new MultiSymbolSegment<PlainList<TableStatement>, TableStatement>(SymbolOrder.symbolbefore, hqlTxt, new string[] { lvlSplitKey },(x,y) => new FilledTable(x,y), metaData, SelectRow.splitKey).segmentItems;
            var allTableStatements = ConvertToTableStatements(hqlTxt, metaData);
            lvlTable = allTableStatements[0];
            metaData.AddLevelStatement(this);
            levelNum = GetLevelNum();
            for (int i = 1; i < allTableStatements.Count; i++)
            {
                combinedTables.Add(allTableStatements[i]);
            }
            var rowItems = new MultiSymbolString<DictionaryListValues<string>>(SymbolOrder.symbolbefore, hqlTxt, new string[] { SelectRow.splitKey, LevelConversion.splitter}, metaData).segmentItems;
            string rowTxt = (rowItems.ContainsKey(SelectRow.splitKey)) ? rowItems[SelectRow.splitKey][0] : "";
            selectRow = new SelectRow(rowTxt, metaData);

            //string lvlConversionTxt = (rowItems.ContainsKey(LevelConversion.splitter)) ? rowItems[LevelConversion.splitter][0] : "";
            //levelConversion = new LevelConversion(lvlConversionTxt, levelName, levelNum, metaData);
            //foreach(string thsAttach in (rowItems.ContainsKey(Attachment.attachSplitter)) ? rowItems[Attachment.attachSplitter] : new List<string>())
            //{
            //    new Attachment(thsAttach, metaData);
            //}
            //if (segmentItems.ContainsKey(SelectRow.splitKey))
            //{
            //    if (segmentItems[SelectRow.splitKey][0] == "*")
            //    {
            //        selectRow = new SelectRow(froms, metaData);
            //    }
            //    else
            //    {
            //        selectRow = new SelectRow(segmentItems[SelectRow.splitKey][0], metaData);
            //    }
            //}
        }
        protected static string RemoveComments(string original)
        {
            string pattern = startComment + "(.*?)" + endComment;
            Regex regex = new Regex(pattern, RegexOptions.RightToLeft);
            foreach (Match match in regex.Matches(original))
            {
                original = original.Replace(startComment + match.Groups[1].Value + endComment, string.Empty);
            }
            return original;
        }
        internal int GetLevelNum()
        {
            if (!joinClause.isSet) { return 0; }
            IFilledLevel parent = metaData.levelStatements.Values.ToList().Where(x => x.levelName == joinClause.parentColumn.colTableRefName).First();
            return parent.levelNum + 1;
        }
        protected LevelStatement() { }
        // internal abstract FilledLevel Fill(Dictionary<ColumnRowID, Dictionary<string,Dictionary<int, List<MultiRogueRow>>>> indexedParentRows);
        internal abstract FilledLevel Fill(IFilledLevel parentLvl);
        public void AttachTables(List<TableStatement> newTblStatements, SelectRow selectRow)
        {
            //*So far just using attachment for CodeFilledTable could easily change to use with regular 
            throw new NotImplementedException();
        }
    }
}
