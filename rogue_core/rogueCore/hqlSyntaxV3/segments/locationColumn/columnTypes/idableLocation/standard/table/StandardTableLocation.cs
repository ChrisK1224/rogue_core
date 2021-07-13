using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.table
{
    class StandardTableLocation : IDableLocation<IORecordID>, IFrom, IInsertFrom
    {
        public IORecordID tableID { get { return base.ID; } }
        public string tableRefName { get; private set; }
        public StandardTableLocation(string txt) : base(txt) 
        {
            try
            {
                SetNameAndID(items);
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        protected IORecordID NameToID(string[] ids)
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
        protected void SetNameAndID(string[] parts)
        {
            if (isDirectID)
            {
                ID = new IORecordID(parts[0]);
                name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
            }
            else
            {
                ID = NameToID(parts);
                name = aliasName.isNameSet ? aliasName.Name : ID.TableName();
            }
            tableRefName = name.ToUpper();
        }
        protected override void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if (isDirectID)
            {
                syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
                syntaxCommands.GetLabel(parentRow, ID.ToString(), IntellsenseDecor.MyColors.black);
                syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            }
            //encodedCol.LoadSyntaxParts(parentRow);
            base.AddSyntaxNamePart(parentRow, string.Join(".", items), IntellsenseDecor.MyColors.orange, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, string.Join(".", items), IntellsenseDecor.MyColors.black);
            aliasName.LoadSyntaxParts(parentRow, syntaxCommands);
            //LocalSyntaxParts(parentRow);
        }
        public void PreFill(QueryMetaData metaData)
        {
            metaData.AddUnsetParams(base.UnsetParams().ToList());
        }
        public IORecordID CalcTableID(IMultiRogueRow parentRow)
        {
            return tableID;
        }
        //void NormSyntaxParts(IMultiRogueRow parentRow)
        //{
        //    if (isDirectID)
        //    {
        //        syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //        AddSyntaxNamePart(parentRow, ID.ToString(), IntellsenseDecor.MyColors.black);
        //        syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < items.Length - 1; i++)
        //        {
        //            AddSyntaxNamePart(parentRow, items[i], IntellsenseDecor.MyColors.black);
        //            syntaxCommands.GetLabel(parentRow, ".", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        //        }
        //        AddSyntaxNamePart(parentRow, items[items.Length - 1], IntellsenseDecor.MyColors.orange);
        //    }
        //}
    }
}
