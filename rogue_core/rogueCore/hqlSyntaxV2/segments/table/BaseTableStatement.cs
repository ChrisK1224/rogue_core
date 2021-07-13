using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.limit;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.where;
using System;
using System.Collections.Generic;
using System.Linq;
using static rogueCore.hqlSyntaxV2.segments.table.TableStatement;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;
using rogueCore.hqlSyntaxV2.filledSegments;
using files_and_folders;

namespace rogueCore.hqlSyntaxV2.segments.table
{
    public abstract class BaseTableStatement : ITableStatement
    {
        protected string[] keys { get; } = new string[4] { JoinClause.splitKey, WhereClause.splitKey, SelectRow.splitKey, Limit.splitKey };
        protected HQLMetaData metaData { get; private set; }
        public string origStatement { get; private set; }
        internal Func<string, IRogueRow, MultiRogueRow, bool> CheckWhereClause { get { return whereClauses.CheckRow; } }
        public abstract IFrom fromInfo { get; }
        protected abstract void SetFromInfo(string hqlTxt, HQLMetaData metaData);
        public String tableRefName { get { return fromInfo.tableRefName.ToUpper(); } }
        public String parentTableRefName { get { return joinClause.isSet ? joinClause.parentColumn.colTableRefName : ""; } }
        public JoinClause joinClause { get; protected set; }
        protected WhereClauses whereClauses { get; private set; }
        List<IHQLSegment> segments = new List<IHQLSegment>();
        //protected abstract IFrom 
        internal Func<string, IRogueRow, MultiRogueRow, bool> WhereClauseCheck { get { return whereClauses.CheckRow; } }
        public List<ILocationColumn> IndexedWhereColumns { get { return whereClauses.evalColumns.Where(iCol => iCol.isConstant == false).ToList(); } }
        internal Func<IFilledLevel, IRogueRow, int, IEnumerable<MultiRogueRow>> MatchFilledLevelParents;
        protected Limit limit { get; set; }
        public abstract Func<IFilledLevel, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow>, IEnumerable<MultiRogueRow>> FilterAndStreamRows { get; protected set; }
        public bool isEncoded { get { return metaData.IsEncodedTable(tableRefName); } }
        protected StringMyList segmentItems;
        //*temporary should just be contained in single method and discarded. Using for merge joins to get down stream rows when
        internal BaseTableStatement(String tablePortion, HQLMetaData metaData)
        {
            this.metaData = metaData;
            segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, tablePortion, keys, metaData).segmentItems;
            SetFromInfo(tablePortion, metaData);
            origStatement = tablePortion;
            metaData.currTableRefName = fromInfo.tableRefName;
            joinClause = new JoinClause(segmentItems.GetValue(JoinClause.splitKey), metaData);
            MatchFilledLevelParents = (joinClause.isSet == false || joinClause.localColumn.isStar) ? (Func<IFilledLevel, IRogueRow, int, IEnumerable<MultiRogueRow>>)JoinAllFill : StandardJoinFill;
            whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey), metaData);
            limit = new Limit(segmentItems.GetValue(Limit.splitKey), metaData);
            metaData.AddTableStatement(this);
        }
        /// <summary>
        /// This is for creating the master table might have to add one IROGUEROW 
        /// </summary>
        protected BaseTableStatement() { MatchFilledLevelParents = JoinAllFill; }
        internal IEnumerable<IRogueRow> StreamRows()
        {
            int rowCount = 0;
            foreach (IRogueRow testRow in fromInfo.StreamIRows(null).TakeWhile(x => rowCount != limit.limitRows))
            {
                yield return testRow;
                rowCount++;
            }
        }
        internal IEnumerable<MultiRogueRow> StandardFilterAndStreamRows(IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IRogueRow testRow in fromInfo.StreamIRows(null).TakeWhile(x => rowCount != limit.limitRows))
            {
                foreach (MultiRogueRow parentRow in MatchFilledLevelParents(parentLvl, testRow, snapshotRowAmount))
                {
                    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                    {
                        yield return NewRow(tableRefName, testRow, parentRow);
                    }
                }
                rowCount++;
            }
        }
        internal IEnumerable<MultiRogueRow> ConversionFilterAndStreamRows(IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (MultiRogueRow parentRow in parentLvl.rows)
            {
                foreach (IRogueRow testRow in fromInfo.StreamIRows(parentRow).TakeWhile(x => rowCount != limit.limitRows))
                {
                    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                    {
                        yield return NewRow(tableRefName, testRow, parentRow);
                    }
                }
                rowCount++;
            }
        }
        //* TODO **IMPORTANT For perfect solution make this join func an interface that can handle these two AND one for Encoded JOIN which is the same as the below exepct the indexedRows would just be one Multiroguerow in the list. Encoded Tables are run one parentRow at at time but want to apply the same logic as if each one was in a list of its own
        IEnumerable<MultiRogueRow> StandardJoinFill(IFilledLevel filledLvl, IRogueRow testRow, int currRowCount)
        {
            foreach (MultiRogueRow row in filledLvl.indexedRows[joinClause.parentColumn.columnRowID][parentTableRefName].TryFindReturn(joinClause.localColumn.CalcValue(testRow)))
            {
                yield return row;
            }
        }
        protected IEnumerable<MultiRogueRow> JoinAllFill(IFilledLevel filledLvl, IRogueRow testRow, int currRowCount)
        {
            for (int i = 0; i < currRowCount; i++)
            {
                yield return filledLvl.rows[i];
            }
        }//*TODO terrible code fix with abouve solution
        internal IEnumerable<MultiRogueRow> SingleEncodedTableJoin(MultiRogueRow parentRow, IRogueRow testRow)
        {
            if (joinClause.isSet == false || joinClause.localColumn.isStar)
            {
                yield return parentRow;
            }
            else
            {
                //*Not sure about this logic to match single row to encoded table with regular join
                if (parentRow.tableRefRows[parentTableRefName].GetValueByColumn(joinClause.parentColumn.columnRowID) == joinClause.localColumn.CalcValue(testRow).ToDecodedString())
                {
                    yield return parentRow;
                }
            }
        }
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
    }
}
