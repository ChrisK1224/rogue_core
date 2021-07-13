using System.Collections.Generic;
using rogueCore.hqlSyntax.segments.from.human;
using rogueCore.hqlSyntax.segments.update.updateFields.human;
using rogueCore.hqlSyntax.segments.where;
using rogueCore.hqlSyntax.segments.where.human;

namespace rogueCore.hqlSyntax.segments.update.human
{
    //public class HumanUpdateStatement : UpdateStatement
    //{
    //    protected override string[] keys { get { return new string[3] { "UPDATE", "SET", WhereClauses.splitKey }; } }
    //    internal HumanUpdateStatement(string txt) : base(txt)
    //    {
    //        fromInfo = new HumanFrom(segmentItems["UPDATE"]);
    //        HumanHQLStatement.tableRefIDs = new Dictionary<string, int>();
    //        HumanHQLStatement.tableRefIDs.Add(fromInfo.tableRefName, fromInfo.tableID);
    //        HumanHQLStatement.currTableRefName = fromInfo.tableRefName;
    //        updateFields = new HumanUpdateFields(segmentItems["SET"]);
    //        whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey));
    //        RunUpdate();
    //    }
    //    protected override string ItemParse(string txt) { return txt; }

    //}
    //public static class UpdateStatementRunner
    //{
    //    public static void RunUpdateStatement(string statement)
    //    {
    //        new UpdateStatement(statement);
    //    }
    //}
}