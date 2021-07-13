using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.row.encoded.manual;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.level;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using files_and_folders;
using System.Text;
using rogueCore.hqlSyntaxV2.segments.select;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2
{
    public class HQLMetaData
    {
        internal Dictionary<string, int> tableRefIDs { get; } = new Dictionary<string, int>();
        internal string currTableRefName;
        internal IFilledLevel currLevel;
        internal Dictionary<string, IFilledLevel> levelStatements { get; private set; } = new Dictionary<string, IFilledLevel>();
        ITableStatement currTable;
        internal Dictionary<string, ITableStatement> tableStatements = new Dictionary<string, ITableStatement>();
        List<string> encodedTableStatements { get; set; }  = new List<string>();
        internal FilledLevel rootLevel;
        internal HQLMetaData()
        {
            rootLevel = FilledLevel.MasterLevel();
        }
        //Dictionary<string, TableStatement> tbls = new Dictionary<string, TableStatement>();
        //internal void AddTableRefID(string key, int tableID)
        //{
        //    tableRefIDs.Add(key, tableID);
        //}
        internal void AddChangeTableRefID(string key, int tableID)
        {
            if (tableRefIDs.ContainsKey(key))
            {
                tableRefIDs[key] = tableID;
            }
            else
            {
                tableRefIDs.Add(key, tableID);
            }
        }
        internal List<IFrom> CurrLevelFroms()
        {
            return currLevel.allTableStatements.Select(x => x.fromInfo).ToList();
        }
        internal List<JoinClause> ChildTableJoinRefs(string tableRefName)
        {
            return tableStatements.Values.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == tableRefName).Select(x => x.joinClause).Distinct().ToList();
        }
        internal void AddLevelStatement(IFilledLevel newStatement)
        {
            currLevel = newStatement;
            levelStatements.Add(newStatement.levelName, newStatement);
        }
        internal void AddTableStatement(ITableStatement newTable)
        {
            currTable = newTable;
            tableStatements.FindChangeIfNotFound(newTable.tableRefName, newTable);
        }
        internal bool IsEncodedTable(string tableName)
        {
             return (encodedTableStatements.Contains(tableName)) ? true : false;
        }
        internal void AddEncodedTableRef(string tableName)
        {
            if (!encodedTableStatements.Contains(tableName))
            {
                encodedTableStatements.Add(tableName);
            }
        }
        //** This method gets a list of all columnRowIDs that are referenced anywhere in the join or where clause of a query to tell the FilledTabe to index these values
        internal List<ColumnRowID> RefIndexesByTable(string tableRefName)
        {
            List<ColumnRowID> indexes = new List<ColumnRowID>();
            tableStatements.Values.Where(tbl => tbl.joinClause.isSet && !tbl.joinClause.localColumn.isStar && tbl.joinClause.parentColumn.colTableRefName == tableRefName).ToList().ForEach(tbl => indexes.Add(tbl.joinClause.parentColumn.columnRowID));
            foreach(ITableStatement thsTbl in tableStatements.Values)
            {
                thsTbl.IndexedWhereColumns.Where(col => col.colTableRefName == tableRefName).ToList().ForEach(col => indexes.Add(col.columnRowID));
            }
            indexes = indexes.Distinct().ToList();
            return indexes;
        }
        internal IEnumerable<MultiRogueRow> TopRows()
        {
            return rootLevel.rows.First().childRows;
        }
        internal string DecodedStatement(string origStatement, MultiRogueRow parentRow)
        {
            string finalStatement = origStatement.ToUpper();
            foreach (var replaceColPair in new MultiSymbolSegment<DictionaryValues<SelectColumn>, SelectColumn>(SymbolOrder.randombetweensymbols, origStatement, new string[2] { "{", "}" }, (x, y) => new SelectColumn(x, y), this).segmentItems)
            {
                string replaceColVal = replaceColPair.Value.GetValue(parentRow.tableRefRows);
                finalStatement = finalStatement.Replace("{" + replaceColPair.Key + "}", replaceColVal);
            }
            return finalStatement;
        }
        //internal List<string> HigherLevels (List<string> cols)
        //{
        //    int thsLevelNum = currLevel.levelNum;
        //    List<string> higherLevelNames = new List<string>();
        //    levelStatements.Values.ToList().Where(lvl => lvl.levelNum < thsLevelNum).ToList().ForEach(lvl => higherLevelNames.AddRange(lvl.allTableStatements.Where(tbl => cols.Contains(tbl.tableRefName)).Select(tbl => tbl.tableRefName)));
        //    return cols.Where(col => higherLevelNames.Contains(col)).ToList();
        //}
        //internal int GetLevelNum()
        //{
        //    if (!currLevel.joinClause.isSet) { return 0; }
        //    LevelStatement parent = levelStatements.Values.ToList().Where(x => x.levelName == currLevel.joinClause.parentColumn.colTableRefName).First();
        //    return parent.levelNum + 1;
        //}
    }
}
