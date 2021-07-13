using System;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.ioObject;

namespace rogue_core.rogueCore.database
{
    internal class RogueDatabase<RowIDType> : Container where RowIDType : RowID
    {
        // public IdableList<IORecordID, IRogueTable<RowIDType>> IDTables = new IdableList<IORecordID, IRogueTable<RowIDType>>();
        // protected IdableList<DecodedRowID, IRogueTable<RowIDType>> namedTables = new IdableList<DecodedRowID, IRogueTable<RowIDType>>();
        internal RogueDatabase(IORecordID ID) : base(ID) { }
        internal IRogueTable GetTable(String tableName, String description = "")
        {
            var tbl = GetIOObject(tableName, description, this.ioItemID, SystemIDs.ObjectTypes.table.name, this.FolderPath());
            return tbl as IRogueTable;
        }
    }
}