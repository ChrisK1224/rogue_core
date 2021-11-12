using files_and_folders;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.classify;
using rogue_core.rogueCore.hqlSyntaxV4.insert;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.order;
using rogue_core.rogueCore.hqlSyntaxV4.row;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.select.command;
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
    public class HQLLevel : SplitSegment,IHQLLevel
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LevelSplitters.fromKey, LevelSplitters.selectKey, LevelSplitters.getKey, LevelSplitters.classifyKey, TableSplitters.whereKey, LevelSplitters.havingKey, LevelSplitters.orderKey, LevelSplitters.combineKey, LevelSplitters.insertKey, LevelSplitters.deleteKey, LevelSplitters.commandLevelKey}; } }
        public List<IMultiRogueRow> rows { get; set; } = new List<IMultiRogueRow>();
        public List<IMultiRogueRow> filteredRows { get; set; }
        public string dataSetName { get { return lvlName.ToUpper(); } }
        public string lvlName { get; }
        HQLTable levelTable { get { return tables[0]; } }
        public string parentLvlName { get; }
        public List<HQLTable> tables { get; } = new List<HQLTable>();
        List<IHQLLevel> childLevels = new List<IHQLLevel>();
        IHQLLevel parentLevel { get; }
        public ISelectRow selectRow { get; protected set; }
        public List<string> columnNames { get { return selectRow.selectColumns.Select(x => x.columnName).ToList(); } }
        IClassify classify { get; }
        IWhereClause whereClause { get; }
        IWhereClause havingClause { get; }
        IOrderBy orderBy { get; }
        public int levelNum { get; }
        public QueryMetaData queryData { get; }
        public Dictionary<string, List<IColumn>> indexesPerTable { private get; set; } = new Dictionary<string, List<IColumn>>();
        public Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>> indexedRows { get; set; } = new Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>>();
        public HQLLevel(string lvlTxt, QueryMetaData metaData, bool isCommand = false) : base(lvlTxt, metaData)
        {
            this.queryData = metaData;
            this.tables = ParseTable(splitList.Where(x => x.Key != KeyNames.select && x.Key != KeyNames.get && x.Key != KeyNames.where && x.Key != KeyNames.having && x.Key != KeyNames.classify && x.Key != KeyNames.order).ToList().Select(x => x.Value).ToList(), metaData);
                //.ForEach(x => ParseTable(x.Value, metaData));
            
            lvlName = levelTable.idName;
            parentLvlName = queryData.ParentLevel(levelTable.joinClause.parentTableName).lvlName;
            parentLevel = metaData.ParentLevel(parentLvlName);
            levelNum = metaData.AddLevel(this);
            var selectPair = splitList.Where(x => x.Key == KeyNames.select || x.Key == KeyNames.get).FirstOrDefault();
            selectRow = ParseSelectRow(selectPair.Key, selectPair.Value, metaData);
            //selectRow = new SelectRow(rowTxt, metaData);
            //selectRow = ParseSelectRow(rowTxt, metaData);
            classify = ParseClassifyClause(splitList.Where(x => x.Key == KeyNames.classify).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            whereClause = ParseWhereClause(splitList.Where(x => x.Key == KeyNames.where).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            orderBy = ParseOrder(splitList.Where(x => x.Key == KeyNames.order).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            havingClause = ParseWhereClause(splitList.Where(x => x.Key == KeyNames.having).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
        }
        protected virtual List<HQLTable> ParseTable(List<string> txtLst, QueryMetaData metaData)
        {
            //tables.Add(new HQLTable(txt, metaData));
            var thsTables = new List<HQLTable>();
            foreach(string tblTxt in txtLst)
            {
                thsTables.Add(new HQLTable(tblTxt, metaData));
            }
            return thsTables;
        }
        protected virtual ISelectRow ParseSelectRow(string keyName, string txt, QueryMetaData metaData)
        {
            switch (keyName)
            {
                case KeyNames.select:
                    return new SelectRow(txt, metaData);
                case KeyNames.get:
                    return new UITemplate(txt, metaData);
                default:
                    throw new Exception("Unrecognized Select Splitter:" + keyName);
            }            
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
        IOrderBy ParseOrder(string classifyTxt, QueryMetaData metaData)
        {
            classifyTxt = classifyTxt.Trim();
            if (classifyTxt == "")
            {
                return new EmptyOrderBy(classifyTxt, metaData);
            }
            else
            {
                return new OrderBy(classifyTxt, metaData);
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
        public void AddChildLevel(IHQLLevel lvl)
        {
            childLevels.Add(lvl);
        }
        public void Fill()
        {
            InitializeIndexedRows();
            LoadTable(levelTable, parentLevel, NewLevelRow);
            for (int i = 1; i < tables.Count; i++)
            {
                LoadTable(tables[i], this, MergeWithLevelRow);
            }
            orderBy.OrderRows(rows);
            filteredRows = new List<IMultiRogueRow>(rows);
            for(int i = filteredRows.Count-1; i >= 0; i--)
            {
                if (!whereClause.CheckWhereClause(filteredRows[i]))
                {
                    filteredRows[i].RemoveFromParent();                    
                    rows.Remove(filteredRows[i]);
                    filteredRows.Remove(filteredRows[i]);
                }
                else if (!havingClause.CheckWhereClause(filteredRows[i]))
                {
                    filteredRows[i].RemoveFromParent();
                    filteredRows.Remove(filteredRows[i]);
                }
                else
                {
                    classify.ClassifyRow(filteredRows[i]);
                }
                //rows = filteredRows;
                //orderBy.OrderRows(rows[i].childRows);
            }
            rows = filteredRows;
            foreach (var childLevel in childLevels)
            {
                childLevel.Fill();
            }
        }
        void LoadTable(HQLTable topTbl, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> AddRow)
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
        public IEnumerable<string> SyntaxSuggestions()
        {
            return splitKeys.Select(x => x.keyTxt);
        }
    }
}
