using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using files_and_folders;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;
using static rogueCore.hqlSyntaxV2.segments.table.TableStatement;

namespace rogueCore.hqlSyntaxV2.segments.where
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
        internal ILocationColumn evalColumnRowID { get { return evalColumn.column; } }
        public WhereClause(String txt, HQLMetaData metaData) 
        {
            SetCompareOptions();
            var segmentItems = new MultiSymbolString<PlainList<String>>(SymbolOrder.symbolafter, txt, keys, metaData).segmentItems;
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
        public Boolean IsValid(string thsTableRef, IRogueRow thsRow, MultiRogueRow parentRow)
        {
            Dictionary<string, IRogueRow> allRows;
            //* TODO this is terrible code expecting all parent rows need to fix this to accept an IROguerow or filled row better and not have to create dictionary and add parent. Needed for now since the new row is not added unless this is valid
            if(parentRow == null){
                allRows = new Dictionary<string, IRogueRow>();
            }else{
                allRows = new Dictionary<string, IRogueRow>(parentRow.tableRefRows);
            }
            allRows.FindChangeIfNotFound(thsTableRef, thsRow);
            //allRows.FindChangeIfNotFound(thsTableRef, thsRow);
            String rowStrValue = checkColumn.GetValue(allRows).ToUpper();
            return compare[evalType](rowStrValue, parentRow.tableRefRows);
        }
        public Boolean IsValid(string thsTableRef, IRogueRow thsRow)
        {
            String rowStrValue = checkColumn.GetValueSingleRow(thsRow).ToUpper();
            return iRogueRowCompare[evalType](rowStrValue, thsRow);
        }
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
            return evalColumn.GetValueSingleRow(row).ToUpper().Equals(value);
        }
        Boolean NotEqualCompare(String value, IRogueRow row)
        {
            return !evalColumn.GetValueSingleRow(row).ToUpper().Equals(value);
        }
    }
}
