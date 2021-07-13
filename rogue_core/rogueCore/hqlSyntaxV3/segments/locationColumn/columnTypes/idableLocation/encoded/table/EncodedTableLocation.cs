using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.encoded.table
{
    class EncodedTableLocation : EncodedBase<IORecordID>, IInsertFrom
    {
        public string tableRefName { get; private set; }
        public EncodedTableLocation(string txt) : base(txt) 
        {
            ID = 0;
            if (isDirectID)
            {
                name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
            }
            else
            {
                name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
            }
            tableRefName = name.ToUpper();
        }
        public IORecordID CalcTableID(IMultiRogueRow row)
        {
            string ColOrID = row.GetValue(encodedCol);
            //string ColOrID = encodedCol.RetrieveStringValue(rows);
            return EncodedIDPull(ColOrID);
        }
        //protected void SetNameAndID(string[] parts)
        //{
        //    ID = 0;
        //    if (isDirectID)
        //    {                
        //        name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
        //    }
        //    else
        //    {
        //        name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
        //    }
        //    tableRefName = name.ToUpper();
        //}
        protected override IORecordID NameToID(string[] ids)
        {
            if (ids.Length == 1)
            {
                return new IORecordID(BinaryDataTable.ioRecordTable.GuessTableIDByName(ids[0]));
            }
            else
            {
                return BinaryDataTable.ioRecordTable.DecodeTableName(ids);
            }
        }
        protected override IORecordID DirectToID(string directID)
        {
            return new IORecordID(directID);
        }
        protected override void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
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
            //for(int i =0; i < items.Length - 1; i++)
            //{
            //    base.AddSyntaxPart(parentRow, items[i], IntellsenseDecor.MyColors.black, false);
            //    syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //}
            //base.AddSyntaxPart(parentRow, tableName, IntellsenseDecor.MyColors.orange, false);
            //syntaxCommands.GetLabel(parentRow, "}", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + colTableRefName + "&nbsp;", IntellsenseDecor.MyColors.black);

            //return syntaxCommands.GetLabel(parentRow, "&nbsp;" + String.Join('.', items) + "&nbsp;", IntellsenseDecor.MyColors.black);
        }
        public void PreFill(QueryMetaData metaData)
        {
            encodedCol.PreFill(metaData, tableRefName);
            metaData.AddUnsetParams(base.UnsetParams().ToList());
        }

        //protected override string IDToName(IORecordID directID)
        //{
        //    return directID.TableName();
        //}
    }
}
