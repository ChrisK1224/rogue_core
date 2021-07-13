using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.streaming;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;

namespace rogueCore.hqlSyntaxV3
{
    public class QueryMetaData
    {
        List<ITableStatement> tableRefIDs = new List<ITableStatement>();
        List<ILevelStatement> levelRefIDs = new List<ILevelStatement>();
        ILevelStatement rootLevel;
        List<string> activeParams = new List<string>();
        public IList<string> allParams { get { return activeParams.AsReadOnly(); } }
        public QueryMetaData(ILevelStatement rootLevel, List<ILevelStatement> levelRefIDs, List<ITableStatement> tableRefIDs) 
        {
            this.rootLevel = rootLevel;
            this.levelRefIDs = levelRefIDs;
            this.tableRefIDs = tableRefIDs;
        }
        public int GetTableIDByRefName(string tableName)
        {
            return tableRefIDs.Where(tbl => tbl.tableRefName == tableName).First().tableID;
        }
        public ILevelStatement GetLevelByRefName(string levelName)
        {
            if(levelName == "")
            {
                return rootLevel;
            }
            else
            {
                return levelRefIDs.Where(lvl => lvl.levelName.ToUpper() == levelName.ToUpper()).First();
            }
        }
        internal Dictionary<string, List<ILocationColumn>> LevelIndexedRows(List<ITableStatement> tables)
        {
            var indexesPerTbl = new Dictionary<string, List<ILocationColumn>>();
            foreach (var thsTable in tables)
            {
                List<ILocationColumn> indexes = new List<ILocationColumn>();
                //*All table statements that have tableRefName as parent Get their parentColumn
                tableRefIDs.Where(tbl => !tbl.joinClause.joinAll && tbl.joinClause.parentTableRef == thsTable.tableRefName).ToList().ForEach(tbl => indexes.Add(tbl.joinClause.parentColumn));
                foreach (ITableStatement thsTbl in tableRefIDs)
                {
                    thsTbl.IndexedWhereColumns.Where(col => col.colTableRefName == thsTable.tableRefName).ToList().ForEach(col => indexes.Add(col));
                }
                indexes = indexes.Distinct().ToList();
                indexesPerTbl.Add(thsTable.tableRefName, indexes);
            }
            return indexesPerTbl;
        }
        internal List<ILevelStatement> ChildLevels(string levelName)
        {
            List<ILevelStatement> childLevels = new List<ILevelStatement>();
            foreach(var lvl in levelRefIDs.Where(lvl => lvl.parentLevelRefName.ToUpper() == levelName.ToUpper()))
            {
                childLevels.Add(lvl);
            }
            return childLevels;
        }
        internal void AddUnsetParams(List<string> unsets)
        {
            activeParams.AddRange(unsets);
        }
        //internal string currTableRef { get; private set; }
        //public void AddTableRef(string tableRefName, int tableID)
        //{
        //    refIDs.Add(tableRefName, tableID);
        //    currTableRef = tableRefName;
        //}
        //internal int TryGetID(string tableRefNM)
        //{
        //    int parentID = 0;
        //    refIDs.TryGetValue(tableRefNM, out parentID);
        //    return parentID;
        //}
        //internal int GetID(string tableRefNM)
        //{
        //    return refIDs[tableRefNM];
        //}
        //internal void AddChangeTableRefID(string key, int tableID)
        //{
        //    if (refIDs.ContainsKey(key))
        //    {
        //        refIDs[key] = tableID;
        //    }
        //    else
        //    {
        //        refIDs.Add(key, tableID);
        //        currTableRef = key;
        //    }
        //}
        //internal string LastTableRefName()
        //{
        //    return lastTableRef;
        //}
        //internal int LastTableRefID()
        //{
        //    return refIDs[lastTableRef];
        //}
    }
}
