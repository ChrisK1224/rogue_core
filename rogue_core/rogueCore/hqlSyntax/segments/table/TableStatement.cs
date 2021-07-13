using files_and_folders;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.from;
using rogueCore.hqlSyntax.segments.join;
using rogueCore.hqlSyntax.segments.limit;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.where;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.table
{
   // public abstract class TableStatement : MultiSymbolSegment<StringMyList, string>, ISplitSegment
    public class TableStatement : MultiSymbolSegment<StringMyList, String>, ISplitSegment
    {
        protected override string[] keys { get; } = new string[4] { JoinClause.splitKey, WhereClause.splitKey, SelectRow.splitKey, Limit.splitKey };
        public const String splitKey = "FROM";
        public int level { get; set; } = 0;
        public From fromInfo;
        internal Boolean isEncoded { get { return fromInfo.isEncoded; } }
        public String tableRefName { get { return fromInfo.tableRefName; } }
        public String parentTableRefName { get { return joinClause.isSet ? joinClause.parentColumn.colTableRefName : ""; } }
        public JoinClause joinClause { get; protected set; }
        public SelectRow selectRow { get; protected set; }
        protected WhereClauses whereClauses { get; set; }
        //HQLMetaData metaData;
        protected Limit limit { private get; set; }
        //*temporary should just be contained in single method and discarded. Using for merge joins to get down stream rows when
        internal TableStatement(String tablePortion, HQLMetaData metaData) : base(SymbolOrder.symbolbefore, tablePortion) {
            Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            fromInfo = new From(segmentItems[firstEntrySymbol], metaData);
            //HumanHQLStatement.tableRefIDs.Add(fromInfo.tableRefName.ToUpper(), fromInfo.tableID);
            //HumanHQLStatement.currTableRefName = fromInfo.tableRefName;
            if (!metaData.encodedTableStatements.Contains(fromInfo.tableRefName))
            {
                metaData.AddChangeTableRefID(fromInfo.tableRefName.ToUpper(), fromInfo.tableID);
            }
            metaData.currTableRefName = fromInfo.tableRefName;
            joinClause = new JoinClause(segmentItems.GetValue(JoinClause.splitKey), metaData);
            //joinClause.tableRefName = tableRefName;
            whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey), metaData);
            stopwatch2.Stop();
            Console.WriteLine("VOrig TableStatementLoad:" + stopwatch2.ElapsedMilliseconds);
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //*Check for select star
            if (segmentItems.GetValue(SelectRow.splitKey).Equals("*"))
            {
                selectRow = new SelectRow(fromInfo, metaData);
            }
            else
            {
                selectRow = new SelectRow(segmentItems.GetValue(SelectRow.splitKey), metaData);
            }
            stopwatch.Stop();
            Console.WriteLine("VOrig SelectRow Load:" + stopwatch.ElapsedMilliseconds);
            limit = new Limit(segmentItems.GetValue(Limit.splitKey), metaData);
        }
        /*public void AddChildTableRef(TableStatement childTable)
        {
            //joinClause.SetChildJoin(childTable.joinClause);
            if (childTable.joinClause.evaluationType.Equals(EvaluationTypes.merge))
            {
                //selectedCols.Add(childTable.tableInfo.tableRefName, childTable.selectedCols[childTable.tableInfo.tableRefName]);
            }
        }*/
        /*public DataRows LoadDataRows(Dictionary<String, List<TableStatement>> childTableMap, DataRows topRows)
        {
            DataRows dataRows = topRows.GetChildJoinedRows(this, childTableMap[tableRefName]);
            //DataRows dataRows = new DataRows(this, childTableMap);
            /*foreach (IRogueRow thsRow in fromInfo.tableID.ToTable().StreamIRows(whereClauses))
            {
                foreach (FilledSelectRow dataRow in joinClause.GetJoinedRows(selectRow, thsRow, topRows))
                {
                    dataRows.AddRow(dataRow);
                }
            }
            //*Need to make object with list and then has dictionary just to correspond to list number as the index
            foreach (TableStatement tbl in childTableMap[tableRefName])
            {
                if (tbl.joinClause.evaluationType.Equals(EvaluationTypes.merge))
                {
                    //*need to test this logic and finish
                    if (tbl.joinClause.MergeJointype.Equals("inner"))
                    {
                        dataRows = tbl.LoadDataRows(childTableMap, dataRows);
                    }
                    else
                    {
                        //*This actually updates the top rows and leaves all unmatched rows
                        tbl.LoadDataRows(childTableMap, dataRows);
                    }
                }
                else
                {
                    tbl.LoadDataRows(childTableMap, dataRows);
                }
            }
            return dataRows;
        }*/
        public IEnumerable<KeyValuePair<IRogueRow, FilledSelectRow>> StreamValidRows(Func<IRogueRow, IEnumerable<FilledSelectRow>> parentCheck)
        {
            int rowCount = 0;
            foreach (KeyValuePair<IRogueRow, FilledSelectRow> thsRow in fromInfo.tableID.ToTable().StreamIRows(fromInfo.tableRefName, whereClauses, parentCheck).TakeWhile(x => rowCount != limit.limitRows))
            {
                yield return thsRow;
                rowCount++;
            }
        }
        protected override String ItemParse(String txt) { return txt; }
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
    }
    internal static class EvalType
    {
        internal static EvaluationTypes ByName(String evalName)
        {
            switch (evalName.ToLower())
            {
                case "=":
                    return EvaluationTypes.equal;
                case "!=":
                    return EvaluationTypes.notEqual;
                case "$":
                    return EvaluationTypes.merge;
                case "?":
                    return EvaluationTypes.valuePair;
                default:
                    return EvaluationTypes.valuePair;
            }
        }
    }
}
