using files_and_folders;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.query;
using rogue_core.rogueCore.hqlSyntaxV3.table;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.snippet;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.hqlSyntaxV3.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.level
{
    public class LevelStatement : ILevelStatement, ISnippetOrLevel
    {
        const string lvlSplitKey = "COMBINE";
        internal const string splitKey = "FROM";
        public string levelName { get { return lvlTable.tableRefName; } }
        const string startComment = "#--";
        const string endComment = "--#";
        public int levelNum { get; set; } = 0;
        public bool isTopLevel { get { return !joinClause.isJoinSet; } }
        internal string origStatement { get { return splitKey + " " + origTxt; } }
        string origTxt;
        public SelectRow selectRow { get; private set; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxLoad;
        public List<ILevelStatement> levelStatements { get { return new List<ILevelStatement>() { this }; } }
        public List<ITableStatement> allTableStatements { get { var tempLst = new List<ITableStatement>(combinedTables); tempLst.Add(lvlTable); return tempLst; } }
        public List<string> allTableNames { get { return allTableStatements.Select(tbl => tbl.tableRefName).ToList(); } }
        public IJoinClause joinClause { get { return lvlTable.joinClause; } }
        internal ITableStatement lvlTable { get; private set; }
        internal List<ITableStatement> combinedTables { get; private set; } = new List<ITableStatement>();
        public String parentLevelRefName { get { return lvlTable.parentTableRefName; } }
        public Dictionary<string, List<ILocationColumn>> indexesPerTable {private get;set;}
        public IMultiRogueRow divRow { get; private set; }
        ILevelStatement parentLvl { get; set; }
        protected string[] keys { get; } = new string[2] { lvlSplitKey, SelectRow.splitKey };
        public List<ILocationColumn> indexColumns { private get; set; } = new List<ILocationColumn>();
        public Dictionary<ILocationColumn, Dictionary<int, List<IMultiRogueRow>>> indexedRows { get; set; } = new Dictionary<ILocationColumn, Dictionary<int, List<IMultiRogueRow>>>();
        public List<IMultiRogueRow> rows { get; } = new List<IMultiRogueRow>();
        internal LevelStatement(string hqlTxt)
        {
            try
            {
                origTxt = hqlTxt;
                hqlTxt = RemoveComments(hqlTxt);
                var allTableStatements = ConvertToTableStatements(hqlTxt);
                lvlTable = allTableStatements[0];
                //queryStatement.AddLevelStatement(this);
                //levelNum = GetLevelNum();
                for (int i = 1; i < allTableStatements.Count; i++)
                {
                    combinedTables.Add(allTableStatements[i]);
                }
                var rowItems = new MultiSymbolString<DictionaryListValues<string>>(SymbolOrder.symbolbefore, hqlTxt, new string[] { SelectRow.splitKey }).segmentItems;
                string rowTxt = (rowItems.ContainsKey(SelectRow.splitKey)) ? rowItems[SelectRow.splitKey][0] : "";
                selectRow = new SelectRow(rowTxt);
                LocalSyntaxLoad = LoadStandardSyntax;
            }
            catch(Exception ex)
            {
                //isError = true;
                LocalSyntaxLoad = LoadErrorParts;
            }
        }
        protected LevelStatement() { }
        public void PreFill(QueryMetaData metaData) 
        {
            try
            {
                parentLvl = metaData.GetLevelByRefName(parentLevelRefName);
                indexesPerTable = metaData.LevelIndexedRows(allTableStatements);
                levelNum = parentLvl.levelNum + 1;
                foreach (var tbl in allTableStatements)
                {
                    tbl.PreFill(metaData);
                }
                selectRow.PreFill(metaData, levelName);
                foreach (ILevelStatement childLevel in metaData.ChildLevels(levelName))
                {
                    childLevel.PreFill(metaData);
                }
                LocalSyntaxLoad = LoadStandardSyntax;
            }
            catch(Exception ex)
            {
                LocalSyntaxLoad = LoadErrorParts;
            }            
        }
        internal static List<ITableStatement> ConvertToTableStatements(string hqlTxt)
        {
            return new MultiSymbolSegment<PlainList<ITableStatement>, ITableStatement>(SymbolOrder.symbolbefore, hqlTxt, new string[] { lvlSplitKey }, NewTableStatement,SelectRow.splitKey).segmentItems;
        }
        internal static ITableStatement NewTableStatement(string hql)
        {
            //string afterFrom = hql.BeforeFirstSpace().Trim().ToUpper();
            //*Ready to test;;;
            string afterFrom = hql.BeforeFirstSpaceExceptInQuotesOrParenthesis().Trim().ToUpper();
            ITableStatement newTable;
            if (afterFrom.StartsWith("EXECUTE("))
            {
                return new ExecutableTableStatement(hql);
            }
            else if (afterFrom.StartsWith("CONVERT("))
            {
                return new ConversionTableStatement(hql);
            }
            else if (afterFrom.StartsWith(InsertHQLStatement.intoKey))
            {
                return new InsertHQLStatement(hql);
            }
            else if (afterFrom.StartsWith("\""))
            {
                return new ConstantTable(hql);
            }//***FIXHERE***
            else if (afterFrom.Contains("{"))
            {
                return new EncodedTableStatement(hql);
            }
            else if (afterFrom.Contains("(") && afterFrom.Contains(")"))
            {
                return CommandTableStatement.GetCommandTableType(afterFrom.BeforeFirstChar('('), hql);
                //return new CommandTableStatement(hql);
            }
            else
            {
                return new StandardTableStatement(hql);
            }
            //qry.AddTableStatement(newTable);
            //return newTable;
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
        internal static LevelStatement MasterLevel()
        {
            LevelStatement masterLevel = new LevelStatement();
            var masterRow = MultiRogueRow.MasterRow();
            masterLevel.rows.Add(masterRow);
            masterLevel.levelNum = 0;
            return masterLevel;
        }
        public void Fill()
        {
            try
            {     
                LoadTable(lvlTable, parentLvl, NewLevelRow);
                for (int i = 0; i < combinedTables.Count; i++)
                {
                    LoadTable(combinedTables[i], this, MergeWithLevelRow);
                }
            }
            catch (Exception ex)
            {
                string blha = ex.ToString();
            }            
        }
        void LoadTable(ITableStatement topTbl, ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> AddRow)
        {
            //var indexCols = queryStatement.RefIndexesByTable(topTbl.tableRefName);
            //indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(topTbl.tableRefName, new Dictionary<int, List<IMultiRogueRow>>()));
            indexesPerTable[topTbl.tableRefName].ForEach(col => indexedRows.FindAddIfNotFound(col));//.Add(new Dictionary<int, List<IMultiRogueRow>>()));
            foreach (IMultiRogueRow newRow in topTbl.FilterAndStreamRows(parentLvl, AddRow))
            {
                IndexThsRow(topTbl.tableRefName, newRow);
            }
        }
        IMultiRogueRow NewLevelRow(string tblName, IReadOnlyRogueRow testRow, IMultiRogueRow parentRow)
        {
            var newRow = new MultiRogueRow(tblName, levelNum, testRow, parentRow, selectRow);
            rows.Add(newRow);            
            return newRow;
        }
        IMultiRogueRow MergeWithLevelRow(string tblName, IReadOnlyRogueRow newRow, IMultiRogueRow parentRow)
        {
            return parentRow.MergeRow(tblName, newRow, rows);
        }
        void IndexThsRow(string tableRefName, IMultiRogueRow newRow)
        {
            foreach (ILocationColumn thsIndexCol in indexesPerTable[tableRefName])
            {
                int indexParentValue = newRow.GetValue(thsIndexCol).ToDecodedRowID();
                //var foundList = indexedRows[thsIndexCol][tableRefName].FindAddIfNotFound(indexParentValue);
                var foundList = indexedRows[thsIndexCol].FindAddIfNotFound(indexParentValue);
                foundList.Add(newRow);
            }
        }
        public List<string> UnsetParams(){
            List<string> unsets = new List<string>();
            foreach(var tbl in allTableStatements)
            {
                unsets.AddRange(tbl.UnsetParams());
            }
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxLoad(parentRow, syntaxCommands);
            //divRow = syntaxCommands.IndentedGroupBox(parentRow);
            //if (isError)
            //{
            //    syntaxCommands.GetLabel(divRow, splitKey, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //    syntaxCommands.GetLabel(divRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);

            //}
            //else
            //{
            //    syntaxCommands.GetLabel(divRow, splitKey, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    lvlTable.LoadSyntaxParts(divRow);
            //    foreach (var tbl in combinedTables)
            //    {
            //        syntaxCommands.BreakLine(divRow);
            //        syntaxCommands.GetLabel(divRow, lvlSplitKey, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //        tbl.LoadSyntaxParts(divRow);
            //    }
            //    syntaxCommands.BreakLine(divRow);
            //    selectRow.LoadSyntaxParts(divRow);
            //}
        }
        public void LoadErrorParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            divRow = syntaxCommands.IndentedGroupBox(parentRow, levelNum);
            syntaxCommands.GetLabel(divRow, splitKey + " " + origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        public void LoadStandardSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if (!isTopLevel)
            {
                parentRow = parentLvl.divRow;
            }
            divRow = syntaxCommands.IndentedGroupBox(parentRow, levelNum);
            syntaxCommands.GetLabel(divRow, splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            lvlTable.LoadSyntaxParts(divRow, syntaxCommands);
            foreach (var tbl in combinedTables)
            {
                syntaxCommands.BreakLine(divRow);
                syntaxCommands.GetLabel(divRow, lvlSplitKey + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
                tbl.LoadSyntaxParts(divRow, syntaxCommands);
            }
            syntaxCommands.BreakLine(divRow);
            selectRow.LoadSyntaxParts(divRow, syntaxCommands);
        }
    }
}
