using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes
{
    //*MAKE AN IQueryable Table that has less options...
    public class ValueRefInstance : BinaryDataTable, IRogueTable
    {
        public ValueRefInstance(string filePath) : base()
        {
            this.filePath = filePath;
        }
        public void WriteValueReference(long valueId, IORecordID tableId,long colId, long rowID, long rowPosition, int complexWordIndex)
        {
            var newRow = base.NewWriteRow();
            newRow.NewWritePair(ColumnTable.record_Col_OID_Row.rowID, colId.ToString());
            newRow.NewWritePair(ColumnTable.recordIO_OID_Row.rowID, tableId.ToString());
            newRow.NewWritePair(ColumnTable.refRecordRow_OID.rowID, rowID.ToString());
            newRow.NewWritePair(ColumnTable.recordComplexIndex.rowID, complexWordIndex.ToString());
            //newRow.NewWritePair(ColumnTable.record_Position_Index.rowID, rowPosition.ToString());
            this.Write();
        }
        public override void Write()
        {
            //List<BinaryDataPair> writePairs = new List<BinaryDataPair>();
            //writeRows.ForEach(x => writePairs.AddRange(x.pairs.Values));           
            //var writeBytes = ToByteArray<BinaryDataPair>(writePairs.ToArray());
            //**This could be faster if combine all bytes from all rows then write at once***
            byte[][] arrays = new byte[writeRows.Count][];
            //*****Need to improve this writer function. BinaryWriter is open for too lon with this method and requires virtual for when writing to ValueRefInstance Table since it doesnt write value reference.
            using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
            {
                foreach (var row in writeRows)
                {
                    writer.Write(row.WriteBytes());
                }
            }
            writeRows.Clear();
        }
    }
}
