using System;
using System.Collections.Generic;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.where
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
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        string origTxt { get; }
        internal WhereClauses(String txt) 
        {
            origTxt = txt;
            try
            {
                segmentItems = new MultiSymbolSegment<ListKeyPairs<WhereGroup>, WhereGroup>(SymbolOrder.betweensymbols, FixHumanSingleGroup(txt), keys, (x) => new WhereGroup(x)).segmentItems;
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }            
        }
        internal void PreFill(QueryMetaData metaData, string assumedTableNm)
        {
            try
            {
                foreach (var grp in segmentItems)
                {
                    grp.Value.PreFill(metaData, assumedTableNm);
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        /// <summary>
        /// For use in update statement when adding where clauses directly through code
        /// </summary>
        internal WhereClauses() { segmentItems = new ListKeyPairs<WhereGroup>(); segmentItems.Add(andMarker, new WhereGroup()); }
        internal void AddDirectWhereClause(ColumnRowID checkCol, string checkVal)
        {
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
        internal Boolean CheckRow(string thsTblRef, IReadOnlyRogueRow thsRow, IMultiRogueRow fullRow = null)
        {
            Boolean isStillValid = true;
            foreach (var thsWhere in segmentItems)
            {
                isStillValid = thsWhere.Value.IsGroupValid(thsTblRef, thsRow, fullRow);
            }
            return isStillValid;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //foreach(var grp in segmentItems)
            //{
            //    grp.Value.LoadSyntaxParts(parentRow);
            //}
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if(segmentItems.Count > 0) { syntaxCommands.GetLabel(parentRow, "&nbsp;" + WhereClause.splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder); }
            foreach (var grp in segmentItems)
            {
                if (segmentItems.Count > 1)
                {
                    syntaxCommands.GetLabel(parentRow, "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
                }
                grp.Value.LoadSyntaxParts(parentRow, syntaxCommands);
                if (segmentItems.Count > 1)
                {
                    syntaxCommands.GetLabel(parentRow, ")", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
                }
            }
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + WhereClause.splitKey + " " + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }        
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var grp in segmentItems)
            {
                unsets.AddRange(grp.Value.UnsetParams());
            }
            return unsets;
        }
        //internal Boolean CheckRow(string thsTblRef, IRogueRow thsRow)
        //{
        //    Boolean isStillValid = true;
        //    foreach (var thsWhere in segmentItems)
        //    {
        //        isStillValid = thsWhere.Value.IsGroupValid(thsTblRef, thsRow);
        //    }
        //    return isStillValid;
        //}
        public enum CompareTypes
        {
            and = '&', or = ','
        }
    }
}