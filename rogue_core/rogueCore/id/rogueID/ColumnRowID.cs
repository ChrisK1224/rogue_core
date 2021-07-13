using rogue_core.rogueCore.binary;
using System;

namespace rogue_core.rogueCore.id.rogueID
{
    public class ColumnRowID : MetaRowID
    {
        public ColumnRowID(int id) : base(id){ }
        public ColumnRowID(String id) : base(int.Parse(id)){}
        public static implicit operator int(ColumnRowID id)
        {
            return id._intVal;
        }
        public static implicit operator ColumnRowID(int id)
        {
            return new ColumnRowID(id);
        }
    }
    public static class extensions{
        //public static IORecordID ColumnDataTypeID(this ColumnRowID colRowID){
        //    return new IORecordID(BinaryDataTable.columnTable.columnsByID[colRowID].DataType();
        //        //[ColumnCols.DataTypeID.ID].valueID);
        //}
        public static String ToColumnName(this ColumnRowID thsColID){
            return BinaryDataTable.columnTable.columnsByID[thsColID].ColumnIDName();
            //return FullTables.columnTable.rows[thsColID].ColumnNameID().DisplayValue();
        }
        public static String ToOwnerIORecord(this ColumnRowID thsColID)
        {
            return BinaryDataTable.columnTable.columnsByID[thsColID].OwnerIOItem();
        }
        //public static DecodedRowID ToColumnType(this ColumnRowID thsColID)
        //{
        //    return BinaryDataTable.columnTable.columnsByID[thsColID].ColumnType().valueID;
        //}
        //public static bool Is_Enumerated(this ColumnRowID thsColID)
        //{
        //    return BinaryDataTable.columnTable.columnsByID[thsColID].Is_Enumerated();
        //}
        //public static IORecordID ParentTableID(this ColumnRowID thsColID)
        //{
        //    return new IORecordID(BinaryDataTable.columnTable.columnsByID[thsColID].Par);
        //}
    }
}