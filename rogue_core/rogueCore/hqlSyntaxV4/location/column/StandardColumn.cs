using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column
{
    public class StandardColumn : StandardLocation, IColumn, IColWithOwnerTable, IOptionalDirect
    {
        public string columnName { get; }     
        public string upperColumnName { get { return columnName.ToUpper(); } }
        public string colTableRefName { get; }
        public string upperColTableRefName { get { return colTableRefName.ToUpper(); } }
        public ColumnRowID columnRowID { get; }
        public StandardColumn(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            List<string> periodParts = splitList.Where(x => x.Key == KeyNames.period).Select(x => x.Value).ToList();
            colTableRefName = periodParts.Count == 2 ? periodParts[0].ToUpper() : metaData.GuessParentTableRefName(periodParts[0].ToUpper());
            columnRowID = (this.IsDirectID(colTxt)) ? new ColumnRowID(this.GetDirectID(colTxt)) : metaData.GetColumnByParentAndColName(colTableRefName, periodParts[periodParts.Count -1]);
            columnName = (base.GetAliasName() == "") ? BinaryDataTable.columnTable.GetColumnNameByID(columnRowID) : base.GetAliasName();
        }
        public virtual string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> parentRows)
        {
            if (parentRows.ContainsKey(upperColTableRefName))
            {
                return parentRows[upperColTableRefName].ITryGetValueByColumn(columnRowID);
            }
            else
            {
                return "";
            }
        }
        public override string PrintDetails()
        {
            return "ColumnName:" + columnName + ", OwnerTableName:" + colTableRefName;
        }
        //internal string GetValue(IReadOnlyRogueRow thsRow)
        //{
        //    //**RecentlyChangedFromBelow
        //    return thsRow.ITryGetValueByColumn(columnRowID);
        //    //if (thsRow.ITryGetValue(columnRowID) != null)
        //    //{
        //    //    return thsRow.GetValueByColumn(columnRowID);
        //    //    //return thsRow.IGetBasePair(columnRowID).DisplayValue();
        //    //}
        //    //else
        //    //{
        //    //    return "";
        //    //}
        //}
        //protected override void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        //{
        //    if(items.Length > 1)
        //    {
        //        base.AddSyntaxNamePart(parentRow, items[0], IntellsenseDecor.MyColors.orange, syntaxCommands);
        //        syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    }            
        //    if (isDirectID)
        //    {
        //        syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //        base.AddSyntaxNamePart(parentRow, items[items.Length-1], IntellsenseDecor.MyColors.black, syntaxCommands);
        //        syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    }
        //    else
        //    {
        //        base.AddSyntaxNamePart(parentRow, items[items.Length - 1], IntellsenseDecor.MyColors.black, syntaxCommands);
        //    }
        //    aliasName.LoadSyntaxParts(parentRow, syntaxCommands);
        //}
    }
}