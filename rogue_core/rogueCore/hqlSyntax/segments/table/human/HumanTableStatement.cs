using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntax.segments.from.human;
using rogueCore.hqlSyntax.segments.join.human;
using rogueCore.hqlSyntax.segments.limit.human;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.select.human;
using rogueCore.hqlSyntax.segments.where.human;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments.table.human
{
    //public class HumanTableStatement : TableStatement
    //{
    //    public const String splitKey = "FROM";
    //    protected override string[] keys { get; } = new string[4] {HumanJoinClause.splitKey, HumanWhereClause.splitKey, SelectRow.splitKey, HumanLimit.splitKey };
    //    internal HumanTableStatement(String tblSegment) : base(tblSegment) { SetTableSegmentVariables(tblSegment); }
    //    void SetTableSegmentVariables(String tablePortion)
    //    {
    //        fromInfo = new From(segmentItems[firstEntrySymbol]);
    //        HumanHQLStatement.tableRefIDs.Add(fromInfo.tableRefName.ToUpper(), fromInfo.tableID);
    //        HumanHQLStatement.currTableRefName = fromInfo.tableRefName;
    //        joinClause = new HumanJoinClause(segmentItems.GetValue(HumanJoinClause.splitKey));
    //        //joinClause.tableRefName = tableRefName;
    //        whereClauses = new HumanWhereClauses(segmentItems.GetValue(HumanWhereClauses.splitKey));
    //        //*Check for select star
    //        if (segmentItems.GetValue(SelectRow.splitKey).Equals("*"))
    //        {
    //            selectRow = new SelectRow(fromInfo);
    //        }
    //        else
    //        {
    //            selectRow = new SelectRow(segmentItems.GetValue(SelectRow.splitKey));
    //        }
    //        limit = new HumanLimit(segmentItems.GetValue(HumanLimit.splitKey));
    //        //HQLQueryTwo.allTables.Add(this.tableRefName, this);
    //    }
    //}
}
