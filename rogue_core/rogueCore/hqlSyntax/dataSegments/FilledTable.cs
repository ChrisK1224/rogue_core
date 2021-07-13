using System.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments;
using rogueCore.hqlSyntax.segments.join;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.table;
using static rogueCore.hqlSyntax.segments.table.TableStatement;
using rogueCore.hqlSyntax.segments.table.human;
using static rogueCore.hqlSyntax.segments.join.JoinClause;

namespace rogueCore.hqlSyntax
{
    public class FilledTable
    {
        //*First is child Join CLause  to be indexed whihc is the parent Join of all children. Second in is value (decodedValue) then list of indexs that correspond to list of selectRow
        Dictionary<JoinClause, Dictionary<int, List<FilledSelectRow>>> indexedRows = new Dictionary<JoinClause, Dictionary<int, List<FilledSelectRow>>>();
        internal List<FilledSelectRow> rows { get; private set; }  = new List<FilledSelectRow>();
        Func<IRogueRow, IEnumerable<FilledSelectRow>> IndexedParentGroup;
        Func<IRogueRow, FilledSelectRow, FilledSelectRow> NewRow;
        internal SelectRow selectRow;
        string tableRefName;
        JoinClause joinClause;
        public int level = 0;
        FilledTable parentTbl;
        //*For when Encoded segment is used and parent is just one row to set full table info
        FilledSelectRow parentRow;
        Func<IEnumerable<FilledSelectRow>> parentRows;
        /// <summary>
        /// This is for manual creation of Filled Table usually form TableGroupInfo
        /// </summary>
        /// <param name="tableRefName"></param>
        /// <param name="joinClause"></param>
        /// <param name="parentTbl"></param>
        public FilledTable(HQLMetaData metaData, String tableRefName, JoinClause joinClause, FilledTable parentTbl)
        {
            this.level = parentTbl.level +1;
            this.joinClause = joinClause;
            this.tableRefName = tableRefName;
            this.parentTbl = parentTbl;
            parentRows = () => parentTbl.rows;
            //if (this.parentTbl == null)
            //{
            //    this.parentTbl = metaData.rootTable;
            //}
            if (joinClause.isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                //this.parentMatchCol = joinClause.parentColumn.columnRowID;
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
        /// <summary>
        /// This is for a standard filled table creation and how the majority are created.
        /// </summary>
        /// <param name="metaData"></param>
        /// <param name="thsTbl"></param>
        /// <param name="indexedColumns"></param>
        /// <param name="parentTbl"></param>
        public FilledTable(HQLMetaData metaData, TableStatement thsTbl, List<JoinClause> indexedColumns, FilledTable parentTbl)
        {
            this.tableRefName = thsTbl.tableRefName;
            this.selectRow = thsTbl.selectRow;
            this.joinClause = thsTbl.joinClause;
            this.parentTbl = parentTbl;

            //if (this.parentTbl == null)
            //{
            //    this.parentTbl = metaData.rootTable;
            //}
            //else
            //{
                this.level = this.parentTbl.level +1;
            //}
            //*Parent table must be set first in case null
            parentRows = () => this.parentTbl.rows;
            if (thsTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
            {
                foreach (JoinClause thsJoin in this.parentTbl.indexedRows.Keys)
                {
                    indexedColumns.Add(thsJoin);
                }
            }
            //*make sure doesn't exist already
            foreach (JoinClause thsIndexCol in indexedColumns)
            {
                indexedRows.Add(thsIndexCol, new Dictionary<int, List<FilledSelectRow>>());
            }
            if (thsTbl.joinClause.isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                //this.parentMatchCol = thsTbl.joinClause.parentColumn.columnRowID;
                switch (thsTbl.joinClause.evaluationType)
                {
                    case EvaluationTypes.equal:
                        NewRow = EqualSelectRow;
                        break;
                    case EvaluationTypes.merge:
                        NewRow = MergeSelectRow;
                        break;
                }
                if (thsTbl.joinClause.joinAll)
                {
                    IndexedParentGroup = JoinAllFilter;
                }
                else
                {
                    IndexedParentGroup = StandardFilter;
                }
            }
            //*Fix hee to send parent rows into stream valid rows*** Might need to look at indexparentGroup first 
            foreach (KeyValuePair<IRogueRow, FilledSelectRow> thsRow in thsTbl.StreamValidRows(IndexedParentGroup))
            {
                //*Get rid of this is redundant
                 AddRow(NewRow(thsRow.Key, thsRow.Value));
            }
            //* TEMP Code to handle adding back parent rows that did not have a match. Basically making a full join need to add this option in qry language to specifcy full or inner or outer joins especially for merge joins
            if(thsTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge) && thsTbl.joinClause.joinType.Equals(JoinTypes.full))
            {
                foreach(FilledSelectRow unMatchedParentMergedRow in parentTbl.rows.Where(x => x.isMerged))
                {
                    unMatchedParentMergedRow.tableRefName = thsTbl.tableRefName;
                    rows.Add(unMatchedParentMergedRow);
                }
            }
        }
        /// <summary>
        /// This is for the runtime generated queries where one statement will turn into many using upper row's values.  So it only takes in the parent row which was used to generate this new tableStatement
        /// </summary>
        /// <param name="thsTable"></param>
        /// <param name="indexedColumns"></param>
        /// <param name=""></param>
        /// <param name="parentRow"></param>
        public FilledTable(TableStatement thsTbl, List<JoinClause> indexedColumns, FilledSelectRow parentRow, FilledTable parentTbl, int level)
        {
            parentRows = () => new List<FilledSelectRow>() { parentRow };
            this.parentTbl = parentTbl;
            this.parentRow = parentRow;
            this.tableRefName = thsTbl.tableRefName;
            this.selectRow = thsTbl.selectRow;
            this.joinClause = thsTbl.joinClause;
            this.level = level;
            if (thsTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
            {
                foreach (JoinClause thsJoin in parentTbl.indexedRows.Keys)
                {
                    indexedColumns.Add(thsJoin);
                }
            }
            //*This should never be false since an encoded table will always have a parent
            if (joinClause.isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                //this.parentMatchCol = thsTbl.joinClause.parentColumn.columnRowID;
                switch (thsTbl.joinClause.evaluationType)
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
            //*Fix hee to send parent rows into stream valid rows*** Might need to look at indexparentGroup first 
            foreach (KeyValuePair<IRogueRow, FilledSelectRow> thsRow in thsTbl.StreamValidRows(IndexedParentGroup))
            {
                //*Get rid of this is redundant
                //foreach (FilledSelectRow parentRow in IndexedParentGroup(thsRow))
                //{
                AddRow(NewRow(thsRow.Key, thsRow.Value));
                //}
            }
        }
        //FilledTable(HQLMetaData metaData, TableStatement thsTbl)
        //{
        //    //thsTbl.metaData;
        //    //this.thsTbl = thsTbl;
        //    //childTableMap[thsTbl.tableRefName].Select(x => x.joinClause.parentColumn.columnRowID).Distinct().ToList().ForEach(x => indexes.Add(x, new Dictionary<int, List<FilledSelectRow>>()));
        //    level = thsTbl.level;
        //    if (thsTbl.joinClause.isSet == false)
        //    {
        //        IndexedParentGroup = JoinAllFilter;
        //        NewRow = EqualSelectRow;
        //    }
        //    else
        //    {
        //        switch (thsTbl.joinClause.evaluationType)
        //        {
        //            case EvaluationTypes.equal:
        //                NewRow = EqualSelectRow;
        //                break;
        //            case EvaluationTypes.merge:
        //                NewRow = MergeSelectRow;
        //                break;
        //        }
        //        if (thsTbl.joinClause.joinAll)
        //        {
        //            IndexedParentGroup = JoinAllFilter;
        //        }
        //        else
        //        {
        //            IndexedParentGroup = StandardFilter;
        //        }
        //    }
        //}
        // void ReplaceRuntimeParams(FilledTable parentRows){
        //     var sp = QuickSplit<DictionaryListValues<string>>.BracketSplit("SFD");
        //     foreach(string pair in .segmentItems){

        //     }
        // }
        /// <summary>
        /// This is to make a shell of filledtable for rootTable in HQLMetaData Only
        /// /// </summary>
        /// <param name="baseRow"></param>
        internal FilledTable(HQLMetaData metaData, FilledSelectRow baseRow)
        {
            this.level = -1;
            rows.Add(baseRow);
            IndexedParentGroup = JoinAllFilter;
            NewRow = EqualSelectRow;
        }
        /// <summary>
        /// Blank filled table if there are no parent rows just to have placeholder
        /// </summary>
        /// <param name="mergeTbl"></param>
        //internal FilledTable(){ }
        //void Initialize()
        //{
        //    if (this.parentTbl == null)
        //    {
        //        this.parentTbl = metaData.rootTable;
        //    }
        //    else
        //    {
        //        this.level = this.parentTbl.level + 1;
        //    }
        //    //*Parent table must be set first in case null
        //    parentRows = () => this.parentTbl.rows;
        //    if (thsTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
        //    {
        //        foreach (JoinClause thsJoin in this.parentTbl.indexedRows.Keys)
        //        {
        //            indexedColumns.Add(thsJoin);
        //        }
        //    }
        //    //*make sure doesn't exist already
        //    foreach (JoinClause thsIndexCol in indexedColumns)
        //    {
        //        indexedRows.Add(thsIndexCol, new Dictionary<int, List<FilledSelectRow>>());
        //    }
        //    if (thsTbl.joinClause.isSet == false)
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
        //        if (thsTbl.joinClause.joinAll)
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
        //        AddRow(NewRow(thsRow.Key, thsRow.Value));
        //    }
        //}
        internal void MergeTable(FilledTable mergeTbl)
        {
            rows.AddRange(mergeTbl.rows);
            
        }
        public void AddRow(FilledSelectRow newRow)
        {
            rows.Add(newRow);
            //*Index the column(s) that are used in child joins. and add this new column to its indexed value. Need to check if value exists and add to list if so or create new list
            foreach (JoinClause thsChildJoinRef in indexedRows.Keys)
            {
                int indexParentValue = int.Parse(newRow.tableRefRows[thsChildJoinRef.parentColumn.colTableRefName].ITryGetValue(thsChildJoinRef.parentColumn.columnRowID).WriteValue());
                //int indexParentValue = int.Parse(newRow.baseTableRow.ITryGetValue(thsColID).WriteValue());
                List<FilledSelectRow> foundList = indexedRows[thsChildJoinRef].FindAddIfNotFound(indexParentValue);
                foundList.Add(newRow);
            }
        }
        public IEnumerable<FilledSelectRow> GetIndexedRows(JoinClause childJoinClause, int indexedValue)
        {
            return indexedRows[childJoinClause].TryFindReturn(indexedValue);
        }
        /*public FilledTable GetChildJoinedRows(TableStatement childTbl, List<TableStatement> childTableMap)
        {
            FilledTable dataRows = new FilledTable(childTbl);
            foreach (var thsChild in childTableMap)
            {
                dataRows.indexes.Add(thsChild.joinClause, new Dictionary<int, List<FilledSelectRow>>());
            }
            if (childTbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
            {
                foreach (JoinClause thsJoin in this.indexes.Keys)
                {
                    dataRows.indexes.Add(thsJoin, new Dictionary<int, List<FilledSelectRow>>());
                }
            }
            foreach (IRogueRow thsRow in childTbl.StreamValidRows())
            {
                foreach (FilledSelectRow parentRow in dataRows.IndexedParentGroup(thsRow, this))
                {
                    FilledSelectRow newChildRow = dataRows.NewRow(thsRow, parentRow);
                    dataRows.AddRow(newChildRow);
                }
            }
            return dataRows;
        }*/
        IEnumerable<FilledSelectRow> JoinAllFilter(IRogueRow row)
        {
            //return parentTbl.Rows();
            return parentRows();
        }
        IEnumerable<FilledSelectRow> StandardFilter(IRogueRow row)
        {
            //*might need to do and TryFindReturn and return empty list if not found. I think will need this for a valid situation but leaving until situation arises
            return parentTbl.GetIndexedRows(joinClause, joinClause.localColumn.CalcValue(row));
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
}