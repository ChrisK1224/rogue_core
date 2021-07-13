using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.where
{
    //public abstract class WhereGroup : MultiSymbolSegment<ListKeyPairs<WhereClause>, WhereClause>
    //: MultiSymbolSegment<ListKeyPairs<WhereClause>, WhereClause>
    public class WhereGroup 
    {
        protected string andMarker { get { return " AND "; } }
        protected string orMarker { get { return " OR "; } }
        protected string[] keys { get { return new string[2] { andMarker, orMarker }; } }
        ListKeyPairs<WhereClause> segmentItems;
        internal List<ILocationColumn> evalColumnRowIDs { get { List<ILocationColumn> lstCols = new List<ILocationColumn>(); segmentItems.ForEach(whereClause => lstCols.Add(whereClause.Value.evalColumnRowID)); return lstCols; } }
        public WhereGroup(String txt, HQLMetaData metaData)
        {
            segmentItems = new MultiSymbolSegment<ListKeyPairs<WhereClause>, WhereClause>(SymbolOrder.symbolafter, txt, keys,(x,y) => new WhereClause(x,y), metaData).segmentItems;
        }
        internal WhereGroup(){ segmentItems = new ListKeyPairs<WhereClause>(); }
        //protected WhereGroup() { }
        internal void AddDirectWhereClause(ColumnRowID thsCol, string val)
        {
            segmentItems.Add(andMarker, new WhereClause(thsCol, val));
        }
        /*public Boolean IsGroupValidOLD(IRogueRow checkRow)
        {
            Boolean IsBreak = false;
            foreach (var valPair in segmentItems)
            {
                IsBreak = valPair.Value.IsValid(checkRow);
            }
            return IsBreak;
        }*/
        //* TODO  Redudant code between two isGroupValids one just doens't use a parentRow
        public Boolean IsGroupValid(string thsTableRef, IRogueRow thsRow)
        {
            Boolean isApproved = true;
            if (segmentItems[0].Key.Equals(andMarker))
            {
                foreach (var thsWhereFilter in segmentItems)
                {
                    if (!thsWhereFilter.Value.IsValid(thsTableRef,thsRow))
                    {
                        return false;
                    }
                }
            }
            else if (segmentItems[0].Key.Equals(orMarker))
            {
                isApproved = false;
                foreach (var thsWhereFilter in segmentItems)
                {
                    if ((thsWhereFilter.Value.IsValid(thsTableRef,thsRow)))
                    {
                        return true;
                    }
                }
            }
            else{
                return segmentItems[0].Value.IsValid(thsTableRef,thsRow);
            }
            return isApproved;
        }
        public Boolean IsGroupValid(string thsTableRef, IRogueRow thsRow, MultiRogueRow fullRow)
        {
            Boolean isApproved = true;
            if (segmentItems[0].Key.Equals(andMarker))
            {
                foreach (var thsWhereFilter in segmentItems)
                {
                    if (!thsWhereFilter.Value.IsValid(thsTableRef,thsRow, fullRow))
                    {
                        return false;
                    }
                }
            }
            else if (segmentItems[0].Key.Equals(orMarker))
            {
                isApproved = false;
                foreach (var thsWhereFilter in segmentItems)
                {
                    if ((thsWhereFilter.Value.IsValid(thsTableRef,thsRow, fullRow)))
                    {
                        return true;
                    }
                }
            }
            else {
                return segmentItems[0].Value.IsValid(thsTableRef, thsRow, fullRow);
            }
            return isApproved;
        }
    }
}