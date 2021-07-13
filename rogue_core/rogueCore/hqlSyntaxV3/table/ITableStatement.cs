using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.segments.limit;
using rogueCore.hqlSyntaxV3.segments.where;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.segments.join.JoinClause;

namespace rogueCore.hqlSyntaxV3.segments.table
{
    //*SHOULD PROBABLY get rid of most of these being public like join clause where clause limit etc. Shouldn't be needed anywhere but wihtin and main meth it oopens to IlevelStatement
    public interface ITableStatement
    {
        //public IFrom fromInfo { get; }
        public String tableRefName { get; }
        public string displayTableRefName { get; }
        public String parentTableRefName { get; }
        public IJoinClause joinClause { get; }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow);
        List<ILocationColumn> IndexedWhereColumns { get; }
        IORecordID tableID { get; }
        void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        void PreFill(QueryMetaData metaData);
        public List<string> UnsetParams();
    }
    public static class ITableStatementExtensions
    {
        public static string[] SplitKeys(this ITableStatement thsTable)
        {
            return new string[3] { JoinClause.splitKey, WhereClause.splitKey, Limit.splitKey };
        }
        public static IJoinClause ParseJoinClause(this ITableStatement thsTable, string joinTxt)
        {
            joinTxt = joinTxt.Trim();
            if (joinTxt.Contains("*"))
            {
                return new JoinAllClause(joinTxt);
            }
            else if (joinTxt.Equals(""))
            {
                return new EmptyJoinClause();
            }
            else if (joinTxt.StartsWith(JoinTypes.to.GetStringValue()))
            {
                return new JoinToClause(joinTxt);
            }
            else
            {
                return new JoinClause(joinTxt);
            }
        }
    }    
    public static class EvalType
    {
        public enum EvaluationTypes
        {
            [StringValue("=")] equal = 1,
            [StringValue("!=")] notEqual = 2
        }
        internal static EvaluationTypes ByName(String evalName)
        {
            switch (evalName.ToLower())
            {
                case "=":
                    return EvaluationTypes.equal;
                case "!=":
                    return EvaluationTypes.notEqual;
                default:
                    return EvaluationTypes.equal;
            }
        }
    }
}
