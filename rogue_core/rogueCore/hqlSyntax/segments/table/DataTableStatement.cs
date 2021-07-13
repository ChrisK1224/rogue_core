using System;
using System.Collections.Generic;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.join;
using rogueCore.hqlSyntax.segments.select;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.table
{
   /* abstract partial class TableStatement : MultiSymbolSegment<StringMyList, string>, ISplitSegment
    {
        DataRows dataRows;
        Func<IRogueRow, DataRows, IEnumerable<FilledSelectRow>> IndexedParentGroup;
        Func<IRogueRow, FilledSelectRow, FilledSelectRow> NewRow;
        void LoadDataRows()
        {
            //childTableMap[thsTbl.tableRefName].Select(x => x.joinClause.parentColumn.columnRowID).Distinct().ToList().ForEach(x => indexes.Add(x, new Dictionary<int, List<FilledSelectRow>>()));
            if (joinClause.isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                switch (joinClause.evaluationType)
                {
                    case EvaluationTypes.equal:
                        NewRow = EqualSelectRow;
                        break;
                    case EvaluationTypes.merge:
                        NewRow = MergeSelectRow;
                        break;
                }
                if (joinClause.joinAll)
                {
                    IndexedParentGroup = JoinAllFilter;
                }
                else
                {
                    IndexedParentGroup = StandardFilter;
                }
            }
        }
        //*For tables with no parent
        public void LoadTopDataRows()
        {
            FilledSelectRow baseRow = FilledSelectRow.BaseRow();
            dataRows = new DataRows(baseRow);
            IndexedParentGroup = JoinAllFilter;
            NewRow = EqualSelectRow;
            dataRows.AddRow(baseRow);
        }
        /*public IEnumerable<FilledSelectRow> GetIndexedRows(JoinClause childJoinClause, int indexedValue)
        {
            return indexes[childJoinClause].TryFindReturn(indexedValue);
        }
        public TableStatement GetChildJoinedRows(TableStatement childTbl, List<TableStatement> childTableMap)
        {
            foreach (var thsChild in childTableMap)
            {
                childTbl.indexes.Add(thsChild.joinClause, new Dictionary<int, List<FilledSelectRow>>());
            }
            if (childTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
            {
                foreach (JoinClause thsJoin in this.indexes.Keys)
                {
                    childTbl.indexes.Add(thsJoin, new Dictionary<int, List<FilledSelectRow>>());
                }
            }
            foreach (IRogueRow thsRow in childTbl.fromInfo.tableID.ToTable().StreamIRows(childTbl.whereClauses))
            {
                //8BING BACK
                foreach (FilledSelectRow parentRow in childTbl.IndexedParentGroup(thsRow, this))
                {
                    FilledSelectRow newChildRow = childTbl.NewRow(thsRow, parentRow);
                    childTbl.AddRow(newChildRow);
                }
            }
            return childTbl;
        }
        IEnumerable<FilledSelectRow> JoinAllFilter(IRogueRow row, DataRows parents)
        {
            return parents.Rows();
        }
        IEnumerable<FilledSelectRow> StandardFilter(IRogueRow row, DataRows parents)
        {
            //*might need to do and TryFindReturn and return empty list if not found. I think will need this for a valid situation but leaving until situation arises
            return parents.GetIndexedRows(joinClause, joinClause.localColumn.CalcValue(row));
        }
        FilledSelectRow EqualSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return new FilledSelectRow(tableRefName, selectRow, baseRow, parentRow);
        }
        FilledSelectRow MergeSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return parentRow.MergeRow(tableRefName, selectRow, baseRow);
        }
    }
    /*class DataRows
    {
        Dictionary<JoinClause, Dictionary<int, List<FilledSelectRow>>> indexes = new Dictionary<JoinClause, Dictionary<int, List<FilledSelectRow>>>();
        List<FilledSelectRow> rows = new List<FilledSelectRow>();
        String tableRefName;
        SelectRow selectRow;
        JoinClause joinClause;
        public Func<IRogueRow, DataRows, IEnumerable<FilledSelectRow>> IndexedParentGroup;
        public Func<IRogueRow, FilledSelectRow, FilledSelectRow> NewRow;
        
        public DataRows(String tableRefName, JoinClause joinClause, SelectRow selectRow)
        {
            this.tableRefName=  tableRefName;
            this.selectRow = selectRow;
            this.joinClause = joinClause;
            if (joinClause.isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                switch (joinClause.evaluationType)
                {
                    case EvaluationTypes.equal:
                        NewRow = EqualSelectRow;
                        break;
                    case EvaluationTypes.merge:
                        NewRow = MergeSelectRow;
                        break;
                }
                if (joinClause.joinAll)
                {
                    IndexedParentGroup = JoinAllFilter;
                }
                else
                {
                    IndexedParentGroup = StandardFilter;
                }
            }
        }
        public DataRows(){
            //dataRows.Add(baseRow);
            IndexedParentGroup = JoinAllFilter;
            NewRow = EqualSelectRow;
        }
        public void AddRow(FilledSelectRow newRow)
        {
            rows.Add(newRow);
            //*Index the column(s) that are used in child joins. and add this new column to its indexed value. Need to check if value exists and add to list if so or create new list
            foreach (JoinClause thsChildJoinRef in indexes.Keys)
            {
                int indexParentValue = int.Parse(newRow.tableRefRows[thsChildJoinRef.parentColumn.colTableRefName].ITryGetValue(thsChildJoinRef.parentColumn.columnRowID).WriteValue());
                List<FilledSelectRow> foundList = indexes[thsChildJoinRef].FindAddIfNotFound(indexParentValue);
                foundList.Add(newRow);
            }
        }
        public IEnumerable<FilledSelectRow> Rows()
        {
            return rows;
        }
        IEnumerable<FilledSelectRow> JoinAllFilter(IRogueRow row, DataRows parents)
        {
            return parents.Rows();
        }
        IEnumerable<FilledSelectRow> StandardFilter(IRogueRow row, DataRows parents)
        {
            return parents.indexes[joinClause].TryFindReturn(joinClause.localColumn.CalcValue(row));
        }
        FilledSelectRow EqualSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return new FilledSelectRow(tableRefName, selectRow, baseRow, parentRow);
        }
        FilledSelectRow MergeSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return parentRow.MergeRow(tableRefName, selectRow, baseRow);
        }
    }*/
}