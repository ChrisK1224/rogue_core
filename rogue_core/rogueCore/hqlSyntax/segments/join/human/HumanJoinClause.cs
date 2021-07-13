using FilesAndFolders;
using rogueCore.hqlSyntax.segments.human;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.join.human
{
    //public class HumanJoinClause : JoinClause
    //{
    //    public const String splitKey = "JOIN";
    //    internal HumanJoinClause(String joinSegment) : base(joinSegment)
    //    {
    //        //SetParentRowJoinClause();
    //    }
    //    protected override void SetVariables()
    //    {
    //        String joinType = segment.BeforeFirstSpace();
    //        switch (joinType.ToUpper())
    //        {
    //            case "ON":
    //                evaluationType = EvaluationTypes.equal;
    //                break;
    //            case "MERGE":
    //                evaluationType = EvaluationTypes.merge;
    //                break;
    //            case "ROWTOCOLUMN":
    //                evaluationType = EvaluationTypes.valuePair;
    //                break;
    //            default:
    //                evaluationType = EvaluationTypes.equal;
    //                break;
    //        }
    //        String joinPortion = segment.AfterFirstSpace().Trim();
    //        String[] parts = joinPortion.Split('=');

    //        localColumn = new HumanLocationColumn(parts[0].Trim());
    //        if (!localColumn.isStar)
    //        {
    //            parentColumn = new HumanLocationColumn(parts[1].Trim());
    //        }
    //        else
    //        {
    //            parentColumn = new LocationColumn(parts[1].Split('.')[0].Trim());
    //        }
    //    }
    //}
}
