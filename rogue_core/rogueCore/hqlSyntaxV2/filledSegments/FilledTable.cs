using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using static rogueCore.hqlSyntaxV2.segments.table.TableStatement;
using static rogueCore.hqlSyntaxV2.segments.join.JoinClause;
using rogueCore.hqlSyntaxV2.segments.where;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    public class FilledTable : TableStatement
    {
        //*First is child Join CLause  to be indexed whihc is the parent Join of all children. Second in is value (decodedValue) then list of indexs that correspond to list of selectRow
        internal Dictionary<ColumnRowID, Dictionary<int, List<IRogueRow>>> indexedRows = new Dictionary<ColumnRowID, Dictionary<int, List<IRogueRow>>>();
        internal List<IRogueRow> rows { get; private set; }  = new List<IRogueRow>();
        
        //Func<IRogueRow, IEnumerable<MultiRogueRow>> IndexedParentGroup;
        //Func<IRogueRow, MultiRogueRow, MultiRogueRow> NewRow;
        //internal SelectRow selectRow;
        //Dictionary<int, List<MultiRogueRow>> indexedParentRows;
        //*For when Encoded segment is used and parent is just one row to set full table info
        /// <summary>
        /// This is for manual creation of Filled Table usually form TableGroupInfo
        /// </summary>
        /// <param name="tableRefName"></param>
        /// <param name="joinClause"></param>
        /// <param name="parentTbl"></param>
        //public FilledTable(HQLMetaData metaData, String tableRefName, JoinClause joinClause, FilledTable parentTbl)
        //{
        //    this.parentTbl = parentTbl;
        //    parentRows = () => parentTbl.rows;
        //    if (joinClause.isSet == false)
        //    {
        //        IndexedParentGroup = JoinAllFilter;
        //        NewRow = EqualSelectRow;
        //    }
        //    else
        //    {
        //        //this.parentMatchCol = joinClause.parentColumn.columnRowID;
        //        switch (joinClause.evaluationType)
        //        {
        //            case EvaluationTypes.equal:
        //                NewRow = EqualSelectRow;
        //                break;
        //            case EvaluationTypes.merge:
        //                NewRow = MergeSelectRow;
        //                break;
        //        }
        //        if (joinClause.joinAll)
        //        {
        //            IndexedParentGroup = JoinAllFilter;
        //        }
        //        else
        //        {
        //            IndexedParentGroup = StandardFilter;
        //        }
        //    }
        //}
        /// <summary>
        /// This is for a standard filled table creation and how the majority are created.
        /// </summary>
        /// <param name="metaData"></param>
        /// <param name="thsTbl"></param>
        /// <param name="indexedColumns"></param>
        /// <param name="parentTbl"></param>
        /// <summary>
        /// This is for the runtime generated queries where one statement will turn into many using upper row's values.  So it only takes in the parent row which was used to generate this new tableStatement
        /// </summary>
        /// <param name="thsTable"></param>
        /// <param name="indexedColumns"></param>
        /// <param name=""></param>
        /// <param name="parentRow"></param>
        //public FilledTable(TableStatement thsTbl, List<JoinClause> indexedColumns, FilledSelectRow parentRow, FilledTable parentTbl, int level)
        //{
        //    parentRows = () => new List<FilledSelectRow>() { parentRow };
        //    this.parentTbl = parentTbl;
        //    this.parentRow = parentRow;
        //    this.tableRefName = thsTbl.tableRefName;
        //    //this.selectRow = thsTbl.selectRow;
        //    this.joinClause = thsTbl.joinClause;
        //    this.level = level;
        //    if (thsTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
        //    {
        //        foreach (JoinClause thsJoin in parentTbl.indexedRows.Keys)
        //        {
        //            indexedColumns.Add(thsJoin);
        //        }
        //    }
        //    //*This should never be false since an encoded table will always have a parent
        //    if (joinClause.isSet == false)
        //    {
        //        IndexedParentGroup = JoinAllFilter;
        //        NewRow = EqualSelectRow;
        //    }
        //    else
        //    {
        //        //this.parentMatchCol = thsTbl.joinClause.parentColumn.columnRowID;
        //        switch (thsTbl.joinClause.evaluationType)
        //        {
        //            case EvaluationTypes.equal:
        //                NewRow = EqualSelectRow;
        //                break;
        //            case EvaluationTypes.merge:
        //                NewRow = MergeSelectRow;
        //                break;
        //        }
        //        if (joinClause.joinAll)
        //        {
        //            IndexedParentGroup = JoinAllFilter;
        //        }
        //        else
        //        {
        //            IndexedParentGroup = StandardFilter;
        //        }
        //    }
        //    //*Fix hee to send parent rows into stream valid rows*** Might need to look at indexparentGroup first 
        //    foreach (KeyValuePair<IRogueRow, FilledSelectRow> thsRow in thsTbl.StreamValidRows(IndexedParentGroup))
        //    {
        //        //*Get rid of this is redundant
        //        //foreach (FilledSelectRow parentRow in IndexedParentGroup(thsRow))
        //        //{
        //        AddRow(NewRow(thsRow.Key, thsRow.Value));
        //        //}
        //    }
        //}
        /// <summary>
        /// This is to make a shell of filledtable for rootTable in HQLMetaData Only
        /// /// </summary>
        /// <param name="baseRow"></param>
        //internal FilledTable(HQLMetaData metaData, FilledSelectRow baseRow)
        //{
        //    this.level = -1;
        //    rows.Add(baseRow);
        //    IndexedParentGroup = JoinAllFilter;
        //    NewRow = EqualSelectRow;
        //}
        internal FilledTable(string hqlTxt, HQLMetaData metaData) : base(hqlTxt, metaData) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexedColumns"></param>
        /// <param name="parentTbl"></param>
        FilledTable() : base() { }
        internal static FilledTable MasterTable()
        {
            return new FilledTable();
        }
        //internal override IEnumerable<IRogueRow> StreamRows()
        //{
        //    var indexedColumns = metaData.ChildTableJoinRefs(tableRefName);
        //    //IndexedParentGroup = (joinClause.joinAll) ?  (Func<IRogueRow, IEnumerable<MultiRogueRow >>)JoinAllFilter : StandardFilter;
        //    int rowCount = 0;
        //    foreach (IRogueRow testRow in fromInfo.tableID.ToTable().StreamIRows().TakeWhile(x => rowCount != limit.limitRows))
        //    {
        //        if (IsValidRow(tableRefName, testRow, whereClauses))
        //        {
        //            yield return testRow;
        //        }
        //        rowCount++;
        //    }
        //}
        //public IEnumerable<KeyValuePair<IRogueRow, MultiRogueRow>> StreamValidRows(Func<string, IRogueRow, WhereClauses, bool> IsValidRow)
        //{
        //    int rowCount = 0;
        //    foreach (IRogueRow testRow in fromInfo.tableID.ToTable().StreamIRows().TakeWhile(x => rowCount != limit.limitRows))
        //    {
        //        if (IsValidRow(tableRefName, testRow, whereClauses))
        //        {
        //            yield return testRow;
        //        }
        //        //*Turn both below foreach and if into a func sent in from Filled Table??
        //        //foreach (MultiRogueRow parentRow in IndexedParentGroup(testRow))
        //        //{
        //        //    if (whereClauses.CheckRow(tableRefName, testRow, parentRow))
        //        //    {
        //        //        yield return new KeyValuePair<IRogueRow, MultiRogueRow>(testRow, parentRow);
        //        //    }
        //        //}
        //        rowCount++;
        //    }
        //}
        /// <summary>
        /// Blank filled table if there are no parent rows just to have placeholder
        /// </summary>
        /// <param name="mergeTbl"></param>
        internal void MergeTable(FilledTable mergeTbl)
        {
            rows.AddRange(mergeTbl.rows);
            
        }
        public void AddRow(IRogueRow newRow) 
        {
            //rows.Add(newRow);
            //*Index the column(s) that are used in child joins. and add this new column to its indexed value. Need to check if value exists and add to list if so or create new list
            //foreach (JoinClause thsChildJoinRef in indexedRows.Keys)
            //{
            //    int indexParentValue = int.Parse(newRow.tableRefRows[thsChildJoinRef.parentColumn.colTableRefName].ITryGetValue(thsChildJoinRef.parentColumn.columnRowID).WriteValue());
            //    List<IRogueRow> foundList = indexedRows[thsChildJoinRef].FindAddIfNotFound(indexParentValue);
            //    foundList.Add(newRow);
            //}
        }
        //public IEnumerable<MultiRogueRow> GetIndexedRows(JoinClause childJoinClause, int indexedValue)
        //{
        //    return indexedRows[childJoinClause].TryFindReturn(indexedValue);
        //}
        //IEnumerable<MultiRogueRow> JoinAllFilter(IRogueRow row)
        //{
        //    //return parentTbl.Rows();
        //    return indexedParentRows[0];
        //}
        //IEnumerable<MultiRogueRow> StandardFilter(IRogueRow row)
        //{
        //    //*might need to do and TryFindReturn and return empty list if not found. I think will need this for a valid situation but leaving until situation arises
        //    return indexedParentRows[joinClause.localColumn.CalcValue(row)];
        //}
    }
}