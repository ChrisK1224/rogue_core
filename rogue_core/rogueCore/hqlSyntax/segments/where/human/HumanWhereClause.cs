using rogueCore.hqlSyntax.segments.human;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.select.human;
using rogueCore.hqlSyntax.segments.table;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.where.human
{
    //public class HumanWhereClause : WhereClause
    //{
    //    public const String splitKey = "WHERE";
        
    //    protected override string[] keys { get { return new string[1] { " " }; } }
    //    internal HumanWhereClause(String txt) : base(txt)  
    //    {

    //        //ColumnRowID thsColID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(whereParts[0], tableRefIDs[lastTableRefName]));
    //        checkColumn = new SelectColumn(segmentItems[0]);
    //        evalType = EvalType.ByName(segmentItems[1]);
    //        evalColumn = new SelectColumn(segmentItems[2]);
    //        //strValue = segmentItems[2].Substring(1, segmentItems[2].Length-2).ToUpper();
    //    }
    //    protected override string ItemParse(string txt){ return txt; }
    //}
}
