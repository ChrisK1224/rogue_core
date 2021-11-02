using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from.command
{
    class RowToColumn : CommandFrom
    {
        public override IORecordID tableId { get { return 2795480; } }
        public const string commandNameIDConst = "ROW_TO_COLUMN";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override string commandNameID { get { return CodeMatchName; } }
        public RowToColumn(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData) 
        {

        }
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            foreach (var param in commandParams)
            {
                var newRow = new ManualBinaryRow();
                //KeyValue
                newRow.AddPair(2795487, param.GetValue(parentRow));
                //KeyName
                newRow.AddPair(2795483, param.columns[param.columns.Count - 1].columnName);
                yield return newRow;
            }
        }
    }
}
