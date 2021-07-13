using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;

namespace rogueCore.hqlSyntaxV3.segments.where
{
    public class DirectWhereClause : IWhereClause
    {
        public ILocationColumn evalColumnRowID { get; private set; }
        string checkValue;
        EvaluationTypes evalType;
        Dictionary<EvaluationTypes, Func<String, Dictionary<string, IReadOnlyRogueRow>, Boolean>> compare;
        internal DirectWhereClause(ColumnRowID colID, string checkValue)
        {
            this.checkValue = checkValue;
            this.evalColumnRowID = new UpdateDirect(colID);
            evalType = EvaluationTypes.equal;
            SetCompareOptions();
        }
        void SetCompareOptions()
        {
            compare = new Dictionary<EvaluationTypes, Func<String, Dictionary<string, IReadOnlyRogueRow>, Boolean>>();
            compare.Add(EvaluationTypes.equal, EqualCompare);
            compare.Add(EvaluationTypes.notEqual, NotEqualCompare);

            //iRogueRowCompare = new Dictionary<EvaluationTypes, Func<String, IRogueRow, Boolean>>();
            //iRogueRowCompare.Add(EvaluationTypes.equal, EqualCompare);
            //iRogueRowCompare.Add(EvaluationTypes.notEqual, NotEqualCompare);
        }
        public bool IsValid(string thsTableRef, IReadOnlyRogueRow thsRow, IMultiRogueRow parentRow)
        {
            string rowStrValue = evalColumnRowID.RetrieveStringValue(new Dictionary<string, IReadOnlyRogueRow>() { { thsTableRef, thsRow } });
            //String rowStrValue = thsRow.GetValueByColumn(evalColumnRowID.columnRowID).ToUpper();
            return compare[evalType](rowStrValue, null);
        }
        Boolean EqualCompare(String value, Dictionary<string, IReadOnlyRogueRow> parents)
        {
            return checkValue.ToUpper().Equals(value);
        }
        Boolean NotEqualCompare(String value, Dictionary<string, IReadOnlyRogueRow> parents)
        {
            return !checkValue.ToUpper().Equals(value);
        }
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
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
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            //syntaxCommands.GetLabel(parentRow, WhereClause.splitKey + " ", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            
        }

        public void PreFill(QueryMetaData metaData, string assumedTableNm)
        {
            
        }
        public List<string> UnsetParams()
        {
            return new List<string>();
        }
    }
}
