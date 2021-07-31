using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.group;
using rogueCore.hqlSyntaxV4.join;
using rogue_core.rogueCore.id;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class QueryMetaData
    {
        public List<HQLGroup> groups = new List<HQLGroup>();
        List<HQLLevel> levels = new List<HQLLevel>();
        List<IQueryableDataSet> dataSets = new List<IQueryableDataSet>();
        List<HQLTable> tables = new List<HQLTable>(); 
        List<ITempBase> allSegs = new List<ITempBase>();
         
        int unnamedColCount = 0;
        public QueryMetaData() 
        {
            levels.Add(HQLLevel.MasterLevel(this));
        }
        public List<IMultiRogueRow> TopRows()
        {
            return groups[groups.Count-1].levels.First().rows[0].childRows;
        }
        public HQLLevel ParentLevel(string parentName)
        {
            return (HQLLevel)levels.Where(lvl => lvl.dataSetName == parentName.ToUpper()).FirstOrDefault();
        }
        public List<HQLLevel> ChildLevels(string parentName)
        {
            return levels.Where(lvl => ((HQLLevel)lvl).parentLvlName == parentName.ToUpper()).Select(lvl => ((HQLLevel)lvl)).ToList();
        }
        public List<HQLTable> ChildTablesForIndexing(string parentName)
        {
            return tables.Where(tbl => tbl.parentTableName.ToUpper() == parentName.ToUpper() && tbl.joinClause is JoinClause).ToList();
        }
        public List<HQLTable> AllTables()
        {
            return tables;
        }
        public void AddGroup(HQLGroup group)
        {
            groups.Add(group);
            
        }
        public HQLGroup GetGroupByName(string idName)
        {
            return this.groups.Where(x => x.idName.ToUpper() == idName.ToUpper()).FirstOrDefault();
        }
        public int AddLevel(HQLLevel level)
        {
            dataSets.Add(level);
            levels.Add(level);
            var parentLvl = levels.Where(lvl => lvl.dataSetName == level.parentLvlName.ToUpper()).FirstOrDefault();
            ((HQLLevel)parentLvl).AddChildLevel(level);
            return ((HQLLevel)parentLvl).levelNum + 1;
        }
        public void AddTable(HQLTable table)
        {
            tables.Add(table);
        }        
        public void AddSegment(ITempBase seg)
        {
            allSegs.Add(seg);
        }
        public string NextUnnamedColumn()
        {
            unnamedColCount++;
            return "UNNAMED_" + unnamedColCount.ToString();
        }
        public string DefaultTableName()
        {
            return tables[tables.Count - 1].idName;
        }
        public string GuessParentTableRefName(string columnName)
        {
            return tables.Reverse<HQLTable>().Where(x => x.DoesColumnBelong(columnName)).Select(x => x.idName).FirstOrDefault();
        }
        public ColumnRowID GetColumnByParentAndColName(string parentColName, string colName)
        {
            if(colName.ToUpper() == "ROGUECOLUMNID")
            {
                return new ColumnRowID(SystemIDs.Columns.rogueColumnID);
            }
            var tbl = tables.Where(x => x.idName == parentColName.ToUpper()).FirstOrDefault();
            var tblID = tbl.potentialTableID;
            return new ColumnRowID(int.Parse(BinaryDataTable.columnTable.GetColumnIDByNameAndOwnerID(colName.ToUpper(), long.Parse(tblID.ToString())).ToString()));
        }
        public void PrintSegments()
        {
            for(int i = 1; i < allSegs.Count; i++)
            {
                allSegs[i].PrintSegment();
            }
        }
    }
}
