using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    //*Complex column is able to handle concated columns but with no alias name so can apply to where statements but not select.
    public class ComplexColumn
    {
        protected static string columnConcatSymbol { get; } = "&";
        protected List<ILocationColumn> columns = new List<ILocationColumn>();
        internal ILocationColumn column { get; set; }
        internal ColumnRowID BaseColumnID { get { return column.columnRowID; } }
        protected static string[] keys { get { return new String[1] { columnConcatSymbol }; } }
        string origTxt { get; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public ComplexColumn(string colTxt)
        {
            try
            {
                origTxt = colTxt;
                //this.queryStatement = queryStatement;
                //*Get rid of after finsih location column logic
                var segmentItems = new MultiSymbolSegment<PlainList<ILocationColumn>, ILocationColumn>(SymbolOrder.symbolbefore, colTxt, keys, NewColumn).segmentItems;
                column = segmentItems[0];
                for (int i = 0; i < segmentItems.Count; i++)
                {
                    columns.Add(segmentItems[i]);
                }
                LocalSyntaxParts = StandardSyntaxParts;
                //if (isEncoded)
                //{
                //    encodedTableNm = colTxt.Substring(0, colTxt.IndexOf("."));
                //    GetValue = GetValueEncoded;
                //}
                //else
                //{
                //    GetValue = GetValueStandard;
                //}
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
                foreach (var col in columns)
                {
                    col.PreFill(metaData, assumedTableNm);
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        ILocationColumn NewColumn(string colTxt)
        {
            return BaseLocation.LocationType(colTxt);
            //if (colTxt.Trim().StartsWith("\""))
            //{
            //    return new ConstLocationColumn(colTxt, queryStatement);
            //}
            //else
            //{
            //    return new LocationColumn(colTxt, queryStatement);
            //}
        }
        static string SplitOffBaseCol(string colTxt)
        {
            return new MultiSymbolString<PlainList<string>>(SymbolOrder.symbolbefore, colTxt, keys).segmentItems[0];
        }
        public string GetValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            string finalValue = "";
            foreach (ILocationColumn concatCol in columns)
            {
                finalValue += concatCol.RetrieveStringValue(rows);
            }
            return finalValue;
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var col in columns)
            {
                unsets.AddRange(col.UnsetParams());
            }
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
           
            for (int i = 0; i < columns.Count - 1; i++)
            {
                columns[i].LoadSyntaxParts(parentRow, syntaxCommands);
                syntaxCommands.GetLabel(parentRow, columnConcatSymbol + "&nbsp;", IntellsenseDecor.MyColors.blue);
            }
            columns[columns.Count - 1].LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //for (int i = 0; i < columns.Count - 1; i++)
            //{
            //    columns[i].LoadSyntaxParts(parentRow);
            //    syntaxCommands.GetLabel(parentRow, columnConcatSymbol + " ", IntellsenseDecor.MyColors.blue);
            //}
            //columns[columns.Count - 1].LoadSyntaxParts(parentRow);
        }
    }
}
