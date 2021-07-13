using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.table;
using static rogueCore.hqlSyntax.MutliSegmentEnum;
using static rogueCore.hqlSyntax.segments.table.TableStatement;

namespace rogueCore.hqlSyntax.segments.where
{
    //public abstract class WhereClause : MultiSymbolSegment<PlainList<String>, String>, ISplitSegment
    public class WhereClause
    {
        public const String splitKey = "WHERE";

        protected string[] keys { get { return new string[1] { " " }; } }
        Dictionary<EvaluationTypes, Func<String, Dictionary<string, IRogueRow>, Boolean>> compare;
        Dictionary<EvaluationTypes, Func<String, IRogueRow, Boolean>> iRogueRowCompare;
        protected SelectColumn checkColumn { private get; set; }
        protected EvaluationTypes evalType { private get; set; }
        protected SelectColumn evalColumn { private get; set; }
        protected DecodedRowID value { private get; set; }
        public WhereClause(String txt, HQLMetaData metaData) 
        {
            SetCompareOptions();
            var segmentItems = new MultiSymbolSegmentNew<PlainList<String>, String>(SymbolOrder.symbolafter, txt, keys, metaData).segmentItems;
            checkColumn = new SelectColumn(segmentItems[0], metaData);
            evalType = EvalType.ByName(segmentItems[1]);
            evalColumn = new SelectColumn(segmentItems[2], metaData);
        }
        internal WhereClause() : base() { SetCompareOptions(); }
        /// <summary>
        /// For direct in code where clause. so far just in update
        /// </summary>
        /// <param name="checkCol"></param>
        /// <param name="checkValue"></param>
        internal WhereClause(ColumnRowID checkCol, string checkValue)
        {
            checkColumn = new SelectColumn(new LocationColumn(checkCol));
            evalType = EvaluationTypes.equal;
            evalColumn = new SelectColumn(new ConstLocationColumn(true, checkValue));
            SetCompareOptions();
        }
        void SetCompareOptions()
        {
            compare = new Dictionary<EvaluationTypes, Func<String, Dictionary<string, IRogueRow>, Boolean>>();
            compare.Add(EvaluationTypes.equal, EqualCompare);
            compare.Add(EvaluationTypes.notEqual, NotEqualCompare);

            iRogueRowCompare = new Dictionary<EvaluationTypes, Func<String, IRogueRow, Boolean>>();
            iRogueRowCompare.Add(EvaluationTypes.equal, EqualCompare);
            iRogueRowCompare.Add(EvaluationTypes.notEqual, NotEqualCompare);
        }
        public Boolean IsValid(string thsTableRef, IRogueRow thsRow, FilledSelectRow parentRow)
        {
            Dictionary<string, IRogueRow> allRows;
            //* TODO this is terrible code expecting all parent rows need to fix this to accept an IROguerow or filled row better and not have to create dictionary and add parent. Needed for now since the new row is not added unless this is valid
            if(parentRow == null){
                allRows = new Dictionary<string, IRogueRow>();
            }else{
                allRows = new Dictionary<string, IRogueRow>(parentRow.tableRefRows);
            }
            allRows.Add(thsTableRef, thsRow);
            String rowStrValue = checkColumn.GetValue(allRows).ToUpper();
            //DecodedRowID rowValue = RowStrValue.ToUpper().ToDecodedRowID();
            //DecodedRowID filterValue = strValue.ToUpper().ToDecodedRowID();
            return compare[evalType](rowStrValue, parentRow.tableRefRows);
        }
        public Boolean IsValid(string thsTableRef, IRogueRow thsRow)
        {
            
            String rowStrValue = checkColumn.GetValue(thsRow).ToUpper();
            //DecodedRowID rowValue = RowStrValue.ToUpper().ToDecodedRowID();
            //DecodedRowID filterValue = strValue.ToUpper().ToDecodedRowID();
            return iRogueRowCompare[evalType](rowStrValue, thsRow);
        }
        // public Boolean IsValid(string thsTableRef, IRogueRow thsRow)
        // {
        //     String rowStrValue = thsRow.GetValueByColumn(checkColumn.BaseColumnID).ToUpper();
        //     return iRogueRowCompare[evalType](rowStrValue, thsRow);
        // }
        // public Boolean IsValid(string thsTableRef, IRogueRow thsRow)
        // {
        //     checkColumn.GetValue()
        //     String rowStrValue = thsRow.GetValueByColumn(checkColumn.BaseColumnID)(allRows).ToUpper();
        //     //DecodedRowID rowValue = RowStrValue.ToUpper().ToDecodedRowID();
        //     //DecodedRowID filterValue = strValue.ToUpper().ToDecodedRowID();
        //     return compare[evalType](rowStrValue, fullRow.tableRefRows);
        // }
        Boolean EqualCompare(String value, Dictionary<string, IRogueRow> parents)
        {
            return evalColumn.GetValue(parents).ToUpper().Equals(value);
        }
        Boolean NotEqualCompare(String value, Dictionary<string, IRogueRow> parents)
        {
            return !evalColumn.GetValue(parents).ToUpper().Equals(value);
        }
        Boolean EqualCompare(String value, IRogueRow row)
        {
            return evalColumn.GetValue(row).ToUpper().Equals(value);
        }
        Boolean NotEqualCompare(String value, IRogueRow row)
        {
            return !evalColumn.GetValue(row).ToUpper().Equals(value);
        }
    }
}
