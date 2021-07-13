using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using files_and_folders;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.table;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using static rogueCore.hqlSyntaxV3.segments.table.EvalType;
using System.Text.RegularExpressions;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.segments.where
{
    //public abstract class WhereClause : MultiSymbolSegment<PlainList<String>, String>, ISplitSegment
    class WhereClause : IWhereClause
    {
        public const String splitKey = "WHERE";
        protected static string[] keys { get { return new string[2] { "!=", "=" }; } }
        Dictionary<EvaluationTypes, Func<String, string, Boolean>> compare;
        //Dictionary<EvaluationTypes, Func<String, IRogueRow, Boolean>> iRogueRowCompare;
        protected ComplexColumn checkColumn { private get; set; }
        protected EvaluationTypes evalType { private get; set; }
        protected ComplexColumn evalColumn { private get; set; }
        //protected DecodedRowID value { private get; set; }
        public ILocationColumn evalColumnRowID { get { return evalColumn.column; } }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        string origTxt { get; }
        public WhereClause(String txt) 
        {
            origTxt = txt;
            try
            {
                SetCompareOptions();
                var segmentItems = Regex.Split(txt, MutliSegmentEnum.GetOutsideQuotesPattern(keys));
                //var segmentItems = new MultiSymbolString<PlainList<String>>(SymbolOrder.symbolafter, txt, keys, queryStatement).segmentItems;
                checkColumn = new ComplexColumn(segmentItems[0]);
                evalType = EvalType.ByName(segmentItems[1]);
                evalColumn = new ComplexColumn(segmentItems[2]);
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void PreFill(QueryMetaData metaData, string assumedTableNm)
        {
            try
            {
                checkColumn.PreFill(metaData, assumedTableNm);
                evalColumn.PreFill(metaData, assumedTableNm);
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        /// <summary>
        /// For direct in code where clause. so far just in update
        /// </summary>
        /// <param name="checkCol"></param>
        /// <param name="checkValue"></param>
        //internal WhereClause(ColumnRowID checkCol, string checkValue)
        //{
        //    checkColumn = new ComplexColumn(new LocationColumn(checkCol));
        //    evalType = EvaluationTypes.equal;
        //    evalColumn = new ComplexColumn(new ConstLocationColumn(true, checkValue));
        //    SetCompareOptions();
        //}
        void SetCompareOptions()
        {
            compare = new Dictionary<EvaluationTypes, Func<String, string, Boolean>>();
            compare.Add(EvaluationTypes.equal, EqualCompare);
            compare.Add(EvaluationTypes.notEqual, NotEqualCompare);

            //iRogueRowCompare = new Dictionary<EvaluationTypes, Func<String, IRogueRow, Boolean>>();
            //iRogueRowCompare.Add(EvaluationTypes.equal, EqualCompare);
            //iRogueRowCompare.Add(EvaluationTypes.notEqual, NotEqualCompare);
        }
        public Boolean IsValid(string thsTableRef, IReadOnlyRogueRow thsRow, IMultiRogueRow parentRow)
        {
            string checkVal = parentRow.WhereClauseGet(checkColumn, thsRow, thsTableRef).ToUpper();
            string evalCheck = parentRow.WhereClauseGet(evalColumn, thsRow, thsTableRef).ToUpper();
            return compare[evalType](checkVal, evalCheck);
        }
        //public Boolean IsValid(string thsTableRef, IRogueRow thsRow)
        //{
        //    String rowStrValue = checkColumn.GetValue(new Dictionary<string, IRogueRow>() { { thsTableRef, thsRow } }).ToUpper();
        //    return iRogueRowCompare[evalType](rowStrValue, thsRow);
        //}
        Boolean EqualCompare(String value, string compareVal)
        {
            // return evalColumn.GetValue(parents).ToUpper().Equals(value);
            return compareVal.ToUpper().Equals(value);
        }
        Boolean NotEqualCompare(String value, string compareVal)
        {
            //return !evalColumn.GetValue(parents).ToUpper().Equals(value);
            return !compareVal.ToUpper().Equals(value);
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + WhereClause.splitKey, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //checkColumn.LoadSyntaxParts(parentRow);
            //syntaxCommands.GetLabel(parentRow, evalType.GetStringValue(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //evalColumn.LoadSyntaxParts(parentRow);
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + WhereClause.splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            checkColumn.LoadSyntaxParts(parentRow, syntaxCommands);
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + evalType.GetStringValue() + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            evalColumn.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + WhereClause.splitKey, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //checkColumn.LoadSyntaxParts(parentRow);
            //syntaxCommands.GetLabel(parentRow, evalType.GetStringValue(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //evalColumn.LoadSyntaxParts(parentRow);
        }
        public List<string> UnsetParams()
        {
            List<string> unsetParams = new List<string>();
            unsetParams.AddRange(evalColumn.UnsetParams());
            unsetParams.AddRange(checkColumn.UnsetParams());
            return unsetParams;
        }
        //Boolean EqualCompare(String value, IRogueRow row)
        //{
        //    return evalColumn.GetValue(new Dictionary<string, IRogueRow>() { { evalColumn.column.colTableRefName, row } }).ToUpper().Equals(value);
        //}
        //Boolean NotEqualCompare(String value, IRogueRow row)
        //{
        //    return !evalColumn.GetValue(new Dictionary<string, IRogueRow>() { { evalColumn.column.colTableRefName, row } }).ToUpper().Equals(value);
        //}
    }
}
