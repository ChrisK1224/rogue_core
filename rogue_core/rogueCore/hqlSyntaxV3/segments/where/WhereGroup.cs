using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.where
{
    //public abstract class WhereGroup : MultiSymbolSegment<ListKeyPairs<WhereClause>, WhereClause>
    //: MultiSymbolSegment<ListKeyPairs<WhereClause>, WhereClause>
    public class WhereGroup 
    {
        protected string andMarker { get { return " AND "; } }
        protected string orMarker { get { return " OR "; } }
        protected string[] keys { get { return new string[2] { andMarker, orMarker }; } }
        ListKeyPairs<IWhereClause> segmentItems;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts { get; set; }
        string origTxt { get; }
        internal List<ILocationColumn> evalColumnRowIDs { get { List<ILocationColumn> lstCols = new List<ILocationColumn>(); segmentItems.ForEach(whereClause => lstCols.Add(whereClause.Value.evalColumnRowID)); return lstCols; } }
        public WhereGroup(String txt)
        {
            origTxt = txt;
            try
            {
                segmentItems = new MultiSymbolSegment<ListKeyPairs<IWhereClause>, IWhereClause>(SymbolOrder.symbolafter, txt, keys, (x) => new WhereClause(x)).segmentItems;
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        internal WhereGroup(){ segmentItems = new ListKeyPairs<IWhereClause>(); }
        internal void PreFill(QueryMetaData metaData, string assumedTableNm)
        {
            try
            {
                foreach (var whr in segmentItems)
                {
                    whr.Value.PreFill(metaData, assumedTableNm);
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }            
        }
        internal void AddDirectWhereClause(ColumnRowID thsCol, string val)
        {
            segmentItems.Add(andMarker, new DirectWhereClause(thsCol, val));
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
        public Boolean IsGroupValid(string thsTableRef, IReadOnlyRogueRow thsRow)
        {
            Boolean isApproved = true;
            if (segmentItems[0].Key.Equals(andMarker))
            {
                foreach (var thsWhereFilter in segmentItems)
                {
                    if (!thsWhereFilter.Value.IsValid(thsTableRef,thsRow, null))
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
                    if ((thsWhereFilter.Value.IsValid(thsTableRef,thsRow, null)))
                    {
                        return true;
                    }
                }
            }
            else{
                return segmentItems[0].Value.IsValid(thsTableRef,thsRow, null);
            }
            return isApproved;
        }
        public Boolean IsGroupValid(string thsTableRef, IReadOnlyRogueRow thsRow, IMultiRogueRow fullRow)
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
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var whr in segmentItems)
            {
                unsets.AddRange(whr.Value.UnsetParams());
            }
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //if(segmentItems.Count > 1)
            //{
            //    syntaxCommands.GetLabel(parentRow, "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //    foreach(var whr in segmentItems)
            //    {
            //        whr.Value.LoadSyntaxParts(parentRow);
            //    }
            //    syntaxCommands.GetLabel(parentRow, ")", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //}
            //else
            //{
            //    segmentItems[0].Value.LoadSyntaxParts(parentRow);
            //}
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            //if (segmentItems.Count > 1)
            //{
            //    syntaxCommands.GetLabel(parentRow, "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
                for(int i =0; i < segmentItems.Count-1;i++) 
                {
                    segmentItems[i].Value.LoadSyntaxParts(parentRow, syntaxCommands);
                    syntaxCommands.GetLabel(parentRow, "&nbsp;" + segmentItems[i].Key + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
                }
                segmentItems[segmentItems.Count-1].Value.LoadSyntaxParts(parentRow, syntaxCommands);
                //syntaxCommands.GetLabel(parentRow, ")", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //}
            //else
            //{
            //    segmentItems[0].Value.LoadSyntaxParts(parentRow);
            //}
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
    }
}