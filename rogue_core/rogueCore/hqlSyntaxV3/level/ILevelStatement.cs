using files_and_folders;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.snippet;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace rogueCore.hqlSyntaxV3.filledSegments
{
    //public partial class FilledLevel : LevelStatement
    //{
    //    internal FilledLevel(string hqlTxt, CoreQueryStatement queryStatement) : base(hqlTxt, queryStatement) {  }
    //    FilledLevel() { }
    //    internal static FilledLevel MasterLevel()
    //    {
    //        FilledLevel masterLevel = new FilledLevel();
    //        //var masterRow = new MultiRogueRow("root", -1, null, null, null);
    //        var masterRow = MultiRogueRow.MasterRow();
    //        masterLevel.rows.Add(masterRow);
    //        return masterLevel;
    //    }
    //    internal override FilledLevel Fill(IFilledLevel parentLvl)
    //    {
    //        //Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch            
    //         LoadTable(lvlTable, parentLvl, NewLevelRow, queryStatement);
    //        //*Should probably fix this ti look for joined tables by joinClause to avoid requiring human to order correctly
    //        for(int i =0; i < combinedTables.Count; i++)
    //        {
    //            LoadTable(combinedTables[i], this, MergeWithLevelRow, queryStatement);
    //        }
    //        //Console.WriteLine("Fill Level:" + stopwatch2.ElapsedMilliseconds);
    //        return this;
    //    }
    //    void LoadTable(ITableStatement topTbl, IFilledLevel parentLvl, Func<string, IRogueRow, IMultiRogueRow, IMultiRogueRow> AddRow, CoreQueryStatement queryStatement)
    //    {
    //        var indexCols = queryStatement.RefIndexesByTable(topTbl.tableRefName);
    //        indexCols.ForEach(col => indexedRows.FindAddIfNotFound(col).Add(topTbl.tableRefName, new Dictionary<int, List<IMultiRogueRow>>()));
    //        foreach (IMultiRogueRow newRow in topTbl.FilterAndStreamRows(parentLvl, AddRow))
    //        {
    //            IndexThsRow(topTbl.tableRefName, indexCols, newRow);
    //        }
    //    }
    //    IMultiRogueRow NewLevelRow(string tblName, IRogueRow testRow, IMultiRogueRow parentRow)
    //    {
    //        var newRow = new MultiRogueRow(tblName, levelNum, testRow, parentRow, selectRow, queryStatement);
    //        rows.Add(newRow);
    //        return newRow;
    //    }
    //    IMultiRogueRow MergeWithLevelRow(string tblName, IRogueRow newRow, IMultiRogueRow parentRow)
    //    {
    //        return parentRow.MergeRow(tblName, newRow, rows);
    //    }
    //    void IndexThsRow(string tableRefName, List<ILocationColumn> indexCols, IMultiRogueRow newRow)
    //    {
    //        foreach (ILocationColumn thsIndexCol in indexCols)
    //        {
    //            //int indexParentValue;
    //            //var valPair = newRow.tableRefRows[tableRefName].ITryGetValue(thsIndexCol);
    //            int indexParentValue = newRow.GetValue(thsIndexCol).ToDecodedRowID();
    //            //if(valPair != null)
    //            //{
    //            //    indexParentValue = int.Parse(valPair.WriteValue());
    //            //}
    //            //else
    //            //{
    //            //    indexParentValue = "".ToDecodedRowID();
    //            //}
    //            var foundList = indexedRows[thsIndexCol][tableRefName].FindAddIfNotFound(indexParentValue);
    //            foundList.Add(newRow);
    //        }
    //    }
    //}
    public interface ILevelStatement
    {
        Dictionary<ILocationColumn, Dictionary<int, List<IMultiRogueRow>>> indexedRows { get; }
        //Dictionary<string, List<ILocationColumn>> indexesPerTable { set; }
        SelectRow selectRow { get; }
        List<IMultiRogueRow> rows { get; }
        string levelName { get; }
        int levelNum { get; }
        IJoinClause joinClause { get; }
        bool isTopLevel { get; }
        string parentLevelRefName { get; }
        void Fill();
        List<ITableStatement> allTableStatements { get; }
        void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        IMultiRogueRow divRow { get; }
        public void PreFill(QueryMetaData metaData);
        public List<string> UnsetParams();
    }
}
