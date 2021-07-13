using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.prefilled;

namespace rogue_core.rogueCore.id.rogueID
{
    public class IORecordID :  MetaRowID
    {   
        public IORecordID(int id) : base(id){ }
        public IORecordID(String id) : base(int.Parse(id)){}
        public static implicit operator int(IORecordID id)
        {
            return id._intVal;
        }
        public static implicit operator IORecordID(int id)
        {
            return new IORecordID(id);
        }
        
        //public RowID AsRowID() { return new RowID(_intVal);}
    }
    public static class GetTableExtensions{
        public static IRecordRow TableInfo(this IORecordID tableID){
            //return new RogueTable<DataRow,DecodedRowID,DataRowID>(tableID);
            //if (tableID == SystemIDs.IOTableRecords.Tables.SystemTables.ioRecordTableID)
            //{
            //    return new IORecordRecordRow();
            //}else if(tableID == SystemIDs.IOTableRecords.Tables.SystemTables.ioRecordTableID)
            //{
            //    return new ColumnRecordRow();
            //}
            //else
            //{
                return BinaryDataTable.ioRecordTable.idRows[tableID.ToInt()];
            //}
        }
    public static string TableName(this IORecordID tableID){
            //return new RogueTable<DataRow,DecodedRowID,DataRowID>(tableID);
            return BinaryDataTable.ioRecordTable.idRows[tableID].ObjectName();
        }
    public static IRogueTable ToTable(this IORecordID tableID){
            //return new RogueTable<DataRow,DecodedRowID,DataRowID>(tableID);
            if(tableID.ToInt().Equals(SystemIDs.IOTableRecords.Tables.SystemTables.ioRecordTableID)){
                return BinaryDataTable.ioRecordTable;
            }
            else if(tableID.ToInt().Equals(SystemIDs.IOTableRecords.Tables.SystemTables.columnTableID)){
                return BinaryDataTable.columnTable;
            }
            //else if(tableID.ToInt().Equals(-1000)){
            //    return FullTables.
            //}
            //else if(tableID.ToInt().Equals(tables.DateTable.ID)){
            //    return FullTables.dateTable;
            //}
            else{
                return new BinaryDataTable(tableID);
            }
        }
    }
}
