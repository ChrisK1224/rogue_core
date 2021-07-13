using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public abstract class StandardLocation : BaseLocation, IOptionalDirect
    {
        //protected List<string> periodParts { get; }
        public StandardLocation(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            //periodParts = splitList.Where(x => x.Key == KeyNames.period).Select(x => x.Value).ToList();
            ////*Set column Table Ref Name if possible. Required if this is a join clause column
            //if (items.Length == 2)
            //{
            //    colTableRefName = items[0].ToUpper();
            //}
            //columnName = aliasName.GetName();
            //if (aliasName.isNameSet)
            //{
            //    name = aliasName.Name.ToUpper();
            //}

        }
        
        //public virtual void PreFill(QueryMetaData metaData, string assumedTblName)
        //{
        //    try
        //    {
        //        if (colTableRefName == "")
        //        {
        //            colTableRefName = assumedTblName.ToUpper();
        //        }
        //        if (isDirectID)
        //        {
        //            ID = new ColumnRowID(items[0]);
        //        }
        //        else
        //        {
        //            int ownerTableID = metaData.GetTableIDByRefName(colTableRefName.ToUpper());
        //            //string str = items[items.Length - 1];
        //            //var blah = BinaryDataTable.columnTable.GetColumnIDByNameAndOwnerID(items[items.Length - 1], ownerTableID);
        //            ID = new ColumnRowID((int)BinaryDataTable.columnTable.GetColumnIDByNameAndOwnerID(items[items.Length - 1], ownerTableID));
        //        }
        //        if (name == "")
        //        {
        //            name = ID.ToColumnName();
        //        }
        //        name = columnName.ToUpper();
        //        colTableRefName = colTableRefName.ToUpper();
        //        upperColumnName = columnName.ToUpper();
        //        metaData.AddUnsetParams(UnsetParams().ToList());
        //        LocalSyntaxParts = StandardSyntaxParts;
        //    }
        //    catch(Exception ex)
        //    {
        //        LocalSyntaxParts = ErrorSyntaxParts;
        //    }
        //}
        //protected StandardColumn(ColumnRowID columnRowID)
        //protected StandardColumn(ColumnRowID columnRowID, string colTableRefName)
        //{ 
        //    this.colTableRefName = colTableRefName.ToUpper();
        //    //name = ID.ToColumnName().ToUpper();
        //    this.upperColumnName = columnName;
        //}
        //protected StandardColumn(ColumnRowID columnRowID, string columnName, string colTableRefName)
        //{
        //    this.colTableRefName = colTableRefName.ToUpper();
        //    //name = columnName; upperName = name.ToUpper();
        //}
        //public virtual string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> parentRows)
        //{
        //    if (parentRows.ContainsKey(colTableRefName))
        //    {
        //        return GetValue(parentRows[colTableRefName]); ;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
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