using files_and_folders;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.classify;
using rogue_core.rogueCore.hqlSyntaxV4.insert;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.row;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level
{
    public class HQLLevel : SplitSegment, IQueryableDataSet
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LevelSplitters.fromKey, LevelSplitters.selectKey, LevelSplitters.classifyKey, TableSplitters.whereKey, LevelSplitters.combineKey, LevelSplitters.insertKey, LevelSplitters.deleteKey }; } }
        public List<IMultiRogueRow> rows { get; set; } = new List<IMultiRogueRow>();
        public string dataSetName { get { return lvlName.ToUpper(); } }
        public string lvlName { get; }
        HQLTable levelTable { get { return tables[0]; } }
        public string parentLvlName { get; }
        public List<HQLTable> tables { get; } = new List<HQLTable>();
        List<HQLLevel> childLevels = new List<HQLLevel>();
        HQLLevel parentLevel { get; }
        SelectRow selectRow { get; }
        public List<string> columnNames { get { return selectRow.selectColumns.Select(x => x.columnName).ToList(); } }
        IClassify classify { get; }
        IWhereClause whereClause { get; }
        public int levelNum { get; }
        public QueryMetaData queryData { get; }
        public Dictionary<string, List<IColumn>> indexesPerTable { private get; set; } = new Dictionary<string, List<IColumn>>();
        public Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>> indexedRows { get; set; } = new Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>>();
        public HQLLevel(string lvlTxt, QueryMetaData metaData) : base(lvlTxt, metaData)
        {
            this.queryData = metaData;
            splitList.Where(x => x.Key != KeyNames.select &&  x.Key != KeyNames.where && x.Key != KeyNames.classify).ToList().ForEach(x => tables.Add(new HQLTable(x.Value, metaData)));
            var rowTxt = splitList.Where(x => x.Key == KeyNames.select).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            selectRow = new SelectRow(rowTxt, metaData);
            classify = ParseClassifyClause(splitList.Where(x => x.Key == KeyNames.classify).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            whereClause = ParseWhereClause(splitList.Where(x => x.Key == KeyNames.where).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            lvlName = levelTable.idName;
            parentLvlName = queryData.ParentLevel(levelTable.joinClause.parentTableName).lvlName;
            parentLevel = metaData.ParentLevel(parentLvlName);
            levelNum = metaData.AddLevel(this);
        }
        HQLLevel(QueryMetaData metaData) : base("", metaData) 
        { 
            lvlName = "";
            levelNum = 0;
            var masterRow = MultiRogueRow.MasterRow();
            rows.Add(masterRow);
        }
        //public Func<string, IReadOnlyRogueRow, IMultiRogueRow, bool>  PreCheckWhereClause { get { return whereClause.CheckWhereClause; } }
        internal static HQLLevel MasterLevel(QueryMetaData metaData)
        {            
            return new HQLLevel(metaData);  
        }
        IWhereClause ParseWhereClause(string whereTxt, QueryMetaData metaData)
        {
            whereTxt = whereTxt.Trim();
            if (whereTxt == "")
            {
                return new EmptyWhereClause(whereTxt, metaData);
            }
            else
            {
                return new WhereClause(whereTxt, metaData);
            }
        }
        IClassify ParseClassifyClause(string classifyTxt, QueryMetaData metaData)
        {
            classifyTxt = classifyTxt.Trim();
            if (classifyTxt == "")
            {
                return new EmptyClassify(classifyTxt, metaData);
            }
            else
            {
                return new Classify(classifyTxt, metaData);
            }
        }
        internal void InitializeIndexedRows()
        {
            foreach (var thsTable in tables)
            {
                List<IColumn> indexes = new List<IColumn>();
                //**Index any child tables that use this table as join
                queryData.ChildTablesForIndexing(thsTable.idName).ForEach(tbl => indexes.Add(((JoinClause)tbl.joinClause).parentColumn));
                //**Index any table that has a where clause ein this table. This hsould be moved to HQLLevel when where does
                //foreach (HQLTable thsTbl in queryData.AllTables())
                //{
                //    thsTbl.IndexedWhereColumns.Where(col => ((IColWithOwnerTable)col).colTableRefName == thsTable.idName).ToList().ForEach(col => indexes.Add(col));
                //}
                indexes = indexes.Distinct().ToList();
                indexesPerTable.Add(thsTable.idName, indexes);
            }
        }
        public void AddChildLevel(HQLLevel lvl)
        {
            childLevels.Add(lvl);
        }
        public void Fill()
        {
            InitializeIndexedRows();
            LoadTable(levelTable, parentLevel, NewLevelRow);
            //LoadTable2(levelTable, parentLevel);
            for (int i = 1; i < tables.Count; i++)
            {
                LoadTable(tables[i], this, MergeWithLevelRow);
                //LoadTable2(tables[i], this);
            }
            for(int i = rows.Count-1; i >= 0; i--)
            {
                if (!whereClause.CheckWhereClause(rows[i]))
                {
                    rows[i].RemoveFromParent();
                    rows.Remove(rows[i]);
                }
                else
                {
                    classify.ClassifyRow(rows[i]);
                }
            }
            foreach(var childLevel in childLevels)
            {
                childLevel.Fill();
            }
        }
        void LoadTable(HQLTable topTbl, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> AddRow)
        {
            indexesPerTable[topTbl.idName].ForEach(col => indexedRows.FindAddIfNotFound(col));
            foreach (IMultiRogueRow newRow in topTbl.FilterAndStreamRows(parentLvl,whereClause, AddRow))
            {
                IndexThsRow(topTbl.idName, newRow);
            }
        }
        //void LoadTable2(HQLTable topTbl, HQLLevel parentLvl)
        //{
        //    foreach(var row in parentLvl.rows)
        //    {
        //        topTbl.LoadTableRows(row.tableRefRows);
        //    }
        //}
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
            foreach (IColumn thsIndexCol in indexesPerTable[tableRefName])
            {
                string indexParentValue = newRow.GetValue(thsIndexCol);
                var foundList = indexedRows[thsIndexCol].FindAddIfNotFound(indexParentValue);
                foundList.Add(newRow);
            }
        }
        public override string PrintDetails()
        {
            return "LevelName:" + lvlName + ",LeveNum:" + levelNum + ", ParentLevelName:" + parentLvlName;
        }
    }
}
