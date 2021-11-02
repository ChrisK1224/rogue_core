using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.update.updateTypes
{
    public class ManualUpdate
    {
        Dictionary<ColumnRowID, string> updateFields { get; } = new Dictionary<ColumnRowID, string>();
        IORecordID recordId { get; }
        DataRowID rowId { get; }
        public ManualUpdate(IORecordID tableId, DataRowID rowId)
        {
            this.recordId = tableId;
            this.rowId = rowId;
        }
        public void AddUpdateField(ColumnRowID colId, string value)
        {
            this.updateFields.Add(colId, value);
        }
        public void Execute()
        {
            var writeTable = new BinaryDataTable(recordId);
            
            foreach(IRogueRow row in writeTable.StreamDataRows())
            {
                if (row.rowID.ToInt() == rowId.ToInt())
                {
                    foreach (var thsField in updateFields)
                    {
                        row.SetValue(thsField.Key, thsField.Value);
                    }
                }
                writeTable.AddWriteRow(row);
            }
            writeTable.UpdateRewrite();
        }
    }
}
