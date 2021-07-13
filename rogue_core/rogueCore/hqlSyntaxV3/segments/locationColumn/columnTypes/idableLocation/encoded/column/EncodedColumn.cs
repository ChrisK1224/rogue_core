using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.encoded.table;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes
{
    class EncodedColumn : EncodedBase<ColumnRowID>, ILocationColumn
    {
        public ColumnRowID columnRowID { get { return base.ID; } }
        public string colTableRefName { get; private set; } = "";
        public string columnName { get; private set; } = "";
        public bool isConstant { get { return false; } }
        QueryMetaData metaData;
        public EncodedColumn(string colTxt) : base(colTxt)
        {
            //*Set column Table Ref Name if possible. Required if this is a join clause column
            if (aliasName.isNameSet)
            {
                columnName = aliasName.Name.ToUpper();
            }
            if (items.Length == 2)
            {
                colTableRefName = items[0];
            }

            //colTableRefName = queryStatement.tableRefIDs.currTableRef.ToUpper();
            //upperColTableRefName = colTableRefName.ToUpper();
            //colTxt = RemoveEncoderSymbol(colTxt);
            //encodedCol = BaseLocation.LocationType(colTxt, queryStatement);
            //EncodedIDPull = (isEncoded) ? EncodedIDPull = EncodedDirectPull : EncodedNonDirectPull;
        }
        public virtual void PreFill(QueryMetaData metaData, string assumedTblName)
        {
            //if (assumedTblName == "DDLITEMTXT")
            //{
            //    string blah = base.origTxt;
            //    string bll = "SDF";
            //}
            try
            {
                this.metaData = metaData;
                //*Note encode column cannot rely on ID to Column Name for name since will be multiple columns so directID must have alias name set
                if (colTableRefName == "")
                {
                    colTableRefName = assumedTblName;
                }
                colTableRefName = colTableRefName.ToUpper();
                //if (colTableRefName == "DDLITEMTXT")
                //{
                //    string bll = "SDF";
                //}
                int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
                encodedCol.PreFill(metaData, assumedTblName);
                metaData.AddUnsetParams(UnsetParams().ToList());
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch (Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        protected override ColumnRowID NameToID(string[] ids)
        {
            string encodedColNM = ids[0];
            int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
            return new ColumnRowID((int)BinaryDataTable.columnTable.GetColumnIDByNameAndOwnerID(encodedColNM, ownerTableID));
        }
        protected override ColumnRowID DirectToID(string directID)
        {
            return new ColumnRowID(directID);
        }
        public string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> parentRows)
        {
            //string colIDorName = encodedCol.GetValue(rows[encodedCol.upperColTableRefName]);            
            string colIDorName = encodedCol.RetrieveStringValue(parentRows);
            ResetEncodedID(EncodedIDPull(colIDorName));
            return GetValue(parentRows[colTableRefName]);
        }
        string GetValue(IReadOnlyRogueRow thsRow)
        {
            return thsRow.ITryGetValueByColumn(columnRowID);
            //if (thsRow.ITryGetValue(columnRowID) != null)
            //{
            //    return thsRow.GetValueByColumn(columnRowID);
            //    // return thsRow.IGetBasePair(columnRowID).StringValue(thsRow.complexWordTable);
            //}
            //else
            //{
            //    return "";
            //}
        }
        protected override void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            base.AddSyntaxNamePart(parentRow, colTableRefName, IntellsenseDecor.MyColors.orange, syntaxCommands);
            syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            if (isDirectID)
            {
                syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            }
            syntaxCommands.GetLabel(parentRow, "{", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            encodedCol.LoadSyntaxParts(parentRow, syntaxCommands);
            syntaxCommands.GetLabel(parentRow, "}", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            if (isDirectID)
            {
                syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            }
            aliasName.LoadSyntaxParts(parentRow, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, "{", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //base.AddSyntaxPart(parentRow, colTableRefName,IntellsenseDecor.MyColors.orange, false);
            //syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //base.AddSyntaxPart(parentRow, columnName, IntellsenseDecor.MyColors.black, false);
            //syntaxCommands.GetLabel(parentRow, "}", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + colTableRefName + "&nbsp;", IntellsenseDecor.MyColors.black);

            //return syntaxCommands.GetLabel(parentRow, "&nbsp;" + String.Join('.', items) + "&nbsp;", IntellsenseDecor.MyColors.black);
        }


    }
}
