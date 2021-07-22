using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    //*Complex column is able to handle concated columns but with no alias name so can apply to where statements but not select.
    public class ComplexColumn : SplitSegment
    {
        protected List<IColumn> _columns { get; }  = new List<IColumn>();
        public IList<IColumn> columns { get { return _columns.AsReadOnly(); } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.colConcat }; } }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public ComplexColumn(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            try
            {
                splitList.ForEach(x => _columns.Add(NewColumn(x.Value, metaData)));
                //foreach (string colSeg in this.SplitList(colTxt).Select(x => x.Value))
                //{
                //    columns.Add(NewColumn(colSeg));
                //}
                //var segmentItems = new MultiSymbolSegment<PlainList<ILocationColumn>, ILocationColumn>(SymbolOrder.symbolbefore, colTxt, keys, NewColumn).segmentItems;
                //column = columns[0];
                //for (int i = 0; i < segmentItems.Count; i++)
                //{
                //    columns.Add(segmentItems[i]);
                //}
                //LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                //LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        //internal void PreFill(QueryMetaData metaData, string assumedTableNm)
        //{
        //    try
        //    {
        //        foreach (var col in columns)
        //        {
        //            col.PreFill(metaData, assumedTableNm);
        //        }
        //        LocalSyntaxParts = StandardSyntaxParts;
        //    }
        //    catch(Exception ex)
        //    {
        //        LocalSyntaxParts = ErrorSyntaxParts;
        //    }
        //}
        IColumn NewColumn(string colTxt, QueryMetaData metaData)
        {
            return BaseColumn.ParseColumn(colTxt, metaData);
        }
        public string GetValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            string finalValue = "";
            foreach (IColumn concatCol in columns)
            {
                finalValue += concatCol.RetrieveStringValue(rows.ToSingleEnum());
            }
            return finalValue;
        }
        //public List<string> UnsetParams()
        //{
        //    List<string> unsets = new List<string>();
        //    foreach(var col in columns)
        //    {
        //        unsets.AddRange(col.UnsetParams());
        //    }
        //    return unsets;
        //}
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        public override string PrintDetails()
        {
            return "";
        }
        //public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        //{           
        //    for (int i = 0; i < columns.Count - 1; i++)
        //    {
        //        columns[i].LoadSyntaxParts(parentRow, syntaxCommands);
        //        syntaxCommands.GetLabel(parentRow, columnConcatSymbol + "&nbsp;", IntellsenseDecor.MyColors.blue);
        //    }
        //    columns[columns.Count - 1].LoadSyntaxParts(parentRow, syntaxCommands);
        //}
        //public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        //{
        //    syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        //}
    }
}
