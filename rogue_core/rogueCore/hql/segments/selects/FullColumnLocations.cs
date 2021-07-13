using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.RogueCode.hql.hqlSegments.table;
using rogue_core.rogueCore.hql;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.id.rogueID.hqlIDs;
using rogue_core.rogueCore.queryResults;
using static rogue_core.RogueCode.hql.hqlSegments.where.WhereClause;

namespace rogue_core.rogueCore.hqlFilter
{
    public class FullColumnLocations
    {
        const char startColumns = '(';
        const char endColumns = ')';
        public const String humanHQLSplitter = "SELECT ";
        public Dictionary<DecodedRowID, SelectColumn> maintainOnHeadRowColumns = new Dictionary<DecodedRowID, SelectColumn>();
        public Dictionary<DecodedRowID, SelectColumn> visibleColumns = new Dictionary<DecodedRowID, SelectColumn>();
        public Dictionary<DecodedRowID, SelectColumn> allColumns = new Dictionary<DecodedRowID, SelectColumn>();
        public Dictionary<DecodedRowID, SelectColumn> columnToRowColumns = new Dictionary<DecodedRowID, SelectColumn>();
        public FullColumnLocations(String allCols, IORecordID tableID = null, String tableRefID = ""){
            String columnsPortion = stringHelper.GetStringBetween(allCols, "(",")");
            String[] cols = columnsPortion.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(String thsCol in cols){
                if (thsCol.Equals("*"))
                {
                    foreach(ColumnRowID thsColID in HQLEncoder.GetAllColumnsPerTable(tableID))
                    {
                        SelectColumn fullCol = new SelectColumn(tableRefID, thsColID);
                        AddColumn(fullCol);
                    }
                }
                else
                {
                    SelectColumn thsFullCol = new SelectColumn(thsCol);
                    AddColumn(thsFullCol);
                }
            }
        }
        public FullColumnLocations() { }
        public void AddColumn(SelectColumn newCol)
        {
            if (newCol.maintainOnHeadRow)
            {
                maintainOnHeadRowColumns.Add(newCol.columnName, newCol);
            }
            else
            {
                columnToRowColumns.Add(newCol.columnName, newCol);
                //this.Add(newCol.columnName, newCol);
            }
            if (!newCol.hidden)
            {
                visibleColumns.Add(newCol.columnName, newCol);
            }
            allColumns.Add(newCol.columnName, newCol);
        }
        public static FullColumnLocations RowToColumnCols(String tableRefName, int columnRowID)
        {
            FullColumnLocations rowToCol = new FullColumnLocations();
            rowToCol.AddColumn(SelectColumn.ColumnAsRowColumnName(tableRefName, columnRowID));
            rowToCol.AddColumn(SelectColumn.ColumnAsRowColumnValue(tableRefName, columnRowID));
            return rowToCol;
        }
        public static String HumanToEncodedHQL(String humanHQL, Dictionary<String, int> tableRefNameIDs, String tableRefName)
        {
           
            String[] columns = humanHQL.Split(',');
            FullColumnLocations thsCols;
            if (humanHQL.Trim() == "*")
            {
                thsCols = new FullColumnLocations("(*)", tableRefNameIDs[tableRefName], tableRefName);
            }
            else
            {
                thsCols = new FullColumnLocations();
                //String encodedHQL = "";
                foreach (String thsCol in columns)
                {
                    thsCols.AddColumn(SelectColumn.HumanToEncodedHQL(thsCol, tableRefNameIDs, tableRefName));
                }
            }
            return thsCols.GetHQLText();
        }
        public Dictionary<DecodedRowID, SelectColumn> GetValueColumns(TableSegment thsTable)
        {
            if (thsTable.joinClauses[0].evaluationType.Equals(EvaluationTypes.valuePair))
            {
                return maintainOnHeadRowColumns;
            }
            else
            {
                return visibleColumns;
            }
        }
        public String GetHQLText(){
            String hql =  startColumns.ToString();
            foreach(SelectColumn thsCol in this.allColumns.Values)
            {
                hql += thsCol.GetHQLText() + ",";
            }
            if(this.allColumns.Values.Count > 0)
            {
                hql = hql.Substring(0, hql.Length - 1);
            }
             hql += endColumns.ToString();
             return hql;
        }
        public String GetFullHQLText()
        {
            String hql = "";
            foreach (SelectColumn thsCol in this.allColumns.Values)
            {
                hql += thsCol.GetFullHQLText() + ",";
            }
            if (this.allColumns.Values.Count > 0)
            {
                hql = hql.Substring(0, hql.Length - 1);
            }
            return hql;
        }
    }
}