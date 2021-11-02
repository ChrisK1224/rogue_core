using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column
{
    class EncodedColumn : EncodedLocation<ColumnRowID>, IColumn, IColWithOwnerTable
    {
        public ColumnRowID columnRowID { get { return base.ID; } }
        public string colTableRefName { get;  }
        public string columnName { get; }
        public EncodedColumn(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            string startKey = (isDirect) ? "[" : EncodedLocation<ColumnRowID>.encodeStartKey;
            string testParentSpecified = splitList[0].Value.BeforeFirstChar(startKey[0]);
            colTableRefName = (testParentSpecified == "") ? metaData.DefaultTableName() : testParentSpecified;
            columnName = base.name;
            //colTableRefName = (periodParts.Count > 1) ? periodParts[0] : metaData.DefaultTableName();
            //columnName = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            //*Set column Table Ref Name if possible. Required if this is a join clause column
            //if (aliasName.isNameSet)
            //{
            //    columnName = aliasName.Name.ToUpper();
            //}
            //if (items.Length == 2)
            //{
            //    colTableRefName = items[0];
            //}
            //try
            //{
            //    this.metaData = metaData;
            //    //*Note encode column cannot rely on ID to Column Name for name since will be multiple columns so directID must have alias name set
            //    if (colTableRefName == "")
            //    {
            //        colTableRefName = assumedTblName;
            //    }
            //    colTableRefName = colTableRefName.ToUpper();
            //    int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
            //    encodedCol.PreFill(metaData, assumedTblName);
            //    metaData.AddUnsetParams(UnsetParams().ToList());
            //    LocalSyntaxParts = StandardSyntaxParts;
            //}
            //catch (Exception ex)
            //{
            //    LocalSyntaxParts = ErrorSyntaxParts;
            //}
        }
        //public virtual void PreFill(QueryMetaData metaData, string assumedTblName)
        //{
        //    //if (assumedTblName == "DDLITEMTXT")
        //    //{
        //    //    string blah = base.origTxt;
        //    //    string bll = "SDF";
        //    //}
        //    try
        //    {
        //        this.metaData = metaData;
        //        //*Note encode column cannot rely on ID to Column Name for name since will be multiple columns so directID must have alias name set
        //        if (colTableRefName == "")
        //        {
        //            colTableRefName = assumedTblName;
        //        }
        //        colTableRefName = colTableRefName.ToUpper();
        //        //if (colTableRefName == "DDLITEMTXT")
        //        //{
        //        //    string bll = "SDF";
        //        //}
        //        int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
        //        encodedCol.PreFill(metaData, assumedTblName);
        //        metaData.AddUnsetParams(UnsetParams().ToList());
        //        LocalSyntaxParts = StandardSyntaxParts;
        //    }
        //    catch (Exception ex)
        //    {
        //        LocalSyntaxParts = ErrorSyntaxParts;
        //    }
        //}
        //protected override ColumnRowID NameToID(string[] ids)
        //{
        //    string encodedColNM = ids[0];
        //    int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
        //    return new ColumnRowID((int)BinaryDataTable.columnTable.GetColumnIDByNameAndOwnerID(encodedColNM, ownerTableID));
        //}
        //protected override ColumnRowID DirectToID(string directID)
        //{
        //    return new ColumnRowID(directID);
        //}
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        {
            string colIDorName = encodedColumn.RetrieveStringValue(parentRows);
            //** Shit code to handle if the encoded column value is blank
            if(colIDorName != "")
            {
                ResetEncodedID(colIDorName);
                return parentRows.First()[colTableRefName].ITryGetValueByColumn(columnRowID);
            }
            else
            {
                return "";
            }            
        }
        protected override ColumnRowID NameToID(string ids)
        {
            return new ColumnRowID(BinaryDataTable.columnTable.GetColumnIDByFullName(ids).ToString());
        }
        protected override ColumnRowID DirectToID(string directID)
        {
            return new ColumnRowID(directID);
        }
        //string GetValue(IReadOnlyRogueRow thsRow)
        //{
        //    return thsRow.ITryGetValueByColumn(columnRowID);
        //    //if (thsRow.ITryGetValue(columnRowID) != null)
        //    //{
        //    //    return thsRow.GetValueByColumn(columnRowID);
        //    //    // return thsRow.IGetBasePair(columnRowID).StringValue(thsRow.complexWordTable);
        //    //}
        //    //else
        //    //{
        //    //    return "";
        //    //}
        //}
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<string> UnsetParams()
        {
            throw new NotImplementedException();
        }
        public override string PrintDetails()
        {
            return "ColumnName:" + columnName + ", OwnerTableName:" + colTableRefName;
        }
        //protected override void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        //{
        //    base.AddSyntaxNamePart(parentRow, colTableRefName, IntellsenseDecor.MyColors.orange, syntaxCommands);
        //    syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    if (isDirectID)
        //    {
        //        syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    }
        //    syntaxCommands.GetLabel(parentRow, "{", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    encodedCol.LoadSyntaxParts(parentRow, syntaxCommands);
        //    syntaxCommands.GetLabel(parentRow, "}", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    if (isDirectID)
        //    {
        //        syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    }
        //    aliasName.LoadSyntaxParts(parentRow, syntaxCommands);
        //}
    }
}
