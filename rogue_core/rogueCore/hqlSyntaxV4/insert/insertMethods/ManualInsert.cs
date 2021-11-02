using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.insert.insertMethods
{
    class ManualInsert
    {
        IRogueTable table { get; }
        List<IRogueRow> insertRows { get; }  = new List<IRogueRow>();
        public IRogueRow row { get; private set; }
        public ManualInsert(IORecordID tableId) 
        {
            table = tableId.ToTable();
        }
        public void AddKeyValuePair(String key, String value)
        {
            row.NewWritePair(table.ioItemID, key, value);
        }
        public void AddKeyValuePair(ColumnRowID colid, String value)
        {
            row.NewWritePair(colid, value);
        }
        public RowID NewRow()
        {
            row = table.NewWriteRow();            
            insertRows.Add(row);
            return row.rowID;
        }
        public List<IRogueRow> Execute()
        {
            table.Write();
            return insertRows;
        }
    }
}
