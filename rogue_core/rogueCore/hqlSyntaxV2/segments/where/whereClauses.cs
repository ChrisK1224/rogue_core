using System;
using System.Collections.Generic;
using System.Linq;
using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.where
{
    //public abstract class WhereClauses : MultiSymbolSegment<ListKeyPairs<WhereGroup>, WhereGroup>
    public class WhereClauses 
    {
        internal const string splitKey = "WHERE";
        protected String startMarker { get { return "("; } }
        protected String endMarker { get { return ")"; } }
        protected String andMarker { get { return " AND "; } }
        protected String orMarker { get { return " OR "; } }
        protected string[] keys { get { return new string[] { startMarker, endMarker }; } }
        internal List<ILocationColumn> evalColumns { get { List<ILocationColumn> lstCols = new List<ILocationColumn>(); segmentItems.ForEach(whereGroup => lstCols.AddRange(whereGroup.Value.evalColumnRowIDs)); return lstCols; } }
        ListKeyPairs<WhereGroup> segmentItems;
        internal WhereClauses(String txt, HQLMetaData metaData) 
        {
            segmentItems = new MultiSymbolSegment<ListKeyPairs<WhereGroup>, WhereGroup>(SymbolOrder.betweensymbols, FixHumanSingleGroup(txt), keys,(x,y) => new WhereGroup(x,y), metaData).segmentItems;

        }
        /// <summary>
        /// For use in update statement when adding where clauses directly through code
        /// </summary>
        internal WhereClauses() { segmentItems = new ListKeyPairs<WhereGroup>(); segmentItems.Add(andMarker, new WhereGroup()); }
        internal void AddDirectWhereClause(ColumnRowID checkCol, string checkVal)
        {
           // segmentItems.Add(new WhereGroup());
                segmentItems[0].Value.AddDirectWhereClause(checkCol, checkVal);
        }
        static String FixHumanSingleGroup(String txt)
        {
            if (!txt.Trim().StartsWith("(") && txt != "")
            {
                return "(" + txt + ")";
            }
            return txt;
        }
        //protected WhereClauses() : base() { }
        internal Boolean CheckRow(string thsTblRef, IRogueRow thsRow, MultiRogueRow fullRow)
        {
            Boolean isStillValid = true;
            foreach (var thsWhere in segmentItems)
            {
                isStillValid = thsWhere.Value.IsGroupValid(thsTblRef, thsRow, fullRow);
            }
            return isStillValid;
        }
        internal Boolean CheckRow(string thsTblRef, IRogueRow thsRow)
        {
            Boolean isStillValid = true;
            foreach (var thsWhere in segmentItems)
            {
                isStillValid = thsWhere.Value.IsGroupValid(thsTblRef, thsRow);
            }
            return isStillValid;
        }
        //internal bool Checking(Dictionary<string, Dictionary<ColumnRowID,DecodedRowID>> check)
        //{
            
        //}
        public enum CompareTypes
        {
            and = '&', or = ','
        }
    }
}