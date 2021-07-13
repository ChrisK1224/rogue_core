using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System.Linq;
using System;
using System.Collections.Generic;
using files_and_folders;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.binary.word;
using static rogue_core.rogueCore.binary.prefilled.ColumnTable;
using System.Runtime.InteropServices;
using rogue_core.rogueCore.binary.prefilled;
using System.Text;
using System.Diagnostics;

namespace rogue_core.rogueCore.binary
{
    public class BinaryDataRow : IRogueRow
    {
        public Dictionary<long, BinaryDataPair> pairs { get; private set; }
        ComplexWordTable complexWordTable { get; }
        /// <summary>
        /// This is for read Binary Pairs direct from the datatable file.
        /// **FIXTHIS Need to set rogueID datatype so don't have to convert rowID here
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="complexWordTable"></param>
        public BinaryDataRow(Dictionary<long,BinaryDataPair> pairs, ComplexWordTable complexWordTable) 
        { 
            this.pairs = pairs;
            this.complexWordTable = complexWordTable;
            rowID = new UnKnownID(int.Parse(pairs[-1012].StringValue(complexWordTable)));
        }
        /// <summary>
        /// This is for writing new pairs and creates a new row ID.
        /// </summary>
        /// <param name="complexWordTable"></param>
        public BinaryDataRow(ComplexWordTable complexWordTable)
        {
            //rowID = new UnKnownID(IDIncrement.NextID());
            //rowID = new UnKnownID(IDIncrement.NextID());
            this.complexWordTable = complexWordTable;
            pairs = new Dictionary<long, BinaryDataPair>();
            string newID = IDIncrement.NextID().ToString();
            pairs.Add(SystemIDs.Columns.rogueColumnID, new BinaryDataPair(SystemIDs.Columns.rogueColumnID, newID, complexWordTable));
            pairs.Add(SystemIDs.Columns.dateAddedID, new BinaryDataPair(SystemIDs.Columns.dateAddedID, DateTime.UtcNow.ToString(), complexWordTable));
            rowID = new UnKnownID(pairs[-1012].StringValue(complexWordTable));
        }
        /// <summary>
        /// This is for ManualBinaryRow where there is no ID default's to 0. Used in From Conversion and stuff like that
        /// </summary>
        /// <param name="complexWordTable"></param>
        /// <param name="rowID"></param>
        protected BinaryDataRow(IORecordID tableID, ComplexWordTable complexWordTable, RowID rowID)
        {
            this.complexWordTable = complexWordTable;
            this.rowID = rowID;
            pairs = new Dictionary<long, BinaryDataPair>();
            pairs.Add(SystemIDs.Columns.rogueColumnID, new BinaryDataPair(SystemIDs.Columns.rogueColumnID, rowID.ToString(), complexWordTable));
        }
        public IEnumerable<IValueReference> GetDataTypeValList()
        {
            foreach(var pair in pairs)
            {
                foreach(var valRef in pair.Value.GetReferenceList(complexWordTable))
                {
                    yield return valRef;
                }
            }
        }
        public void AddPair(BinaryDataPair pair) { }
        public RowID rowID { get; }
        public string GetValueByColumn(ColumnRowID thsCol)
        {
            return pairs[thsCol].StringValue(complexWordTable);
        }
        public IGenericValue IGetBasePair(ColumnRowID colRowID)
        {
            return pairs[colRowID];
        }
        public byte[] WriteBytes()
        {
            BinaryDataPair[] writePairs = pairs.Values.ToArray();
            writePairs[writePairs.Length - 1].SetEnd();
            writePairs.ToList().ForEach(pair => pair.rowCount = (short)pairs.Count);            
            GCHandle handle = GCHandle.Alloc(writePairs, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                byte[] destination = new byte[writePairs.Length * Marshal.SizeOf(typeof(BinaryDataPair))];
                Marshal.Copy(pointer, destination, 0, destination.Length);
                return destination;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }
        public IEnumerable<BinaryDataPair> IPairs()
        {
            foreach(var itm in pairs.Values)
            {
                yield return itm;
            }
        }
        public IEnumerable<KeyValuePair<ColumnRowID, string>> GetPairs()
        {
            foreach(var pair in pairs)
            {
                yield return new KeyValuePair<ColumnRowID, string>(new ColumnRowID((int)pair.Key), pair.Value.StringValue(complexWordTable));
            }
        }
        public string ITryGetValueByColumn(ColumnRowID colRowID)
        {
            if (pairs.ContainsKey(colRowID.ToInt()))
                return pairs[colRowID].StringValue(complexWordTable);
            else
                return "";
        }
        public IGenericValue ITryGetValue(ColumnRowID colRowID)
        {
            if (pairs.ContainsKey(colRowID.ToInt()))
                return pairs[colRowID];
            else
                return null;
        }
        public IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, RowID value, byte dataTypeID)
        {
            
            var thsColID = BinaryDataTable.columnTable.GetWriteColumn(ownerTblID, colType, columnName).rowID.ToInt();
            var newPair = new BinaryDataPair(thsColID, value.ToInt(), dataTypeID, complexWordTable);
            pairs.Add(thsColID, newPair);            
            return newPair;

        }
        public IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, string value)
        {
            var thsColID = BinaryDataTable.columnTable.GetWriteColumn(ownerTblID, colType, columnName).rowID.ToInt();
            var newPair = new BinaryDataPair(thsColID, value, complexWordTable);
            pairs.Add(thsColID, newPair);       
            return newPair;
        }
        public IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, RowID value,byte dataTypeID, IORecordID parentTableID)
        {
            var tmr = new Stopwatch();
            tmr.Start();
            ColumnRowID thsColID = BinaryDataTable.columnTable.GetWriteColumn(ownerTblID, columnName, parentTableID).rowID as ColumnRowID;
            tmr.Stop();
            var one = tmr.ElapsedMilliseconds;
            tmr.Restart();
            var newPair = new BinaryDataPair(thsColID, value.ToInt(), dataTypeID, complexWordTable);
            pairs.Add(thsColID,newPair);
            tmr.Stop();
            var two = tmr.ElapsedMilliseconds;
            //String finalValue = value.ToDecodedRowID();
          // var newPair = NewPair(thsColID, new DecodedRowID(value.ToInt()));
            //this.Add(newPair);
            return newPair;
        }
        public IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, string value, IORecordID parentTableID)
        {
            ColumnRowID thsColID = BinaryDataTable.columnTable.GetWriteColumn(ownerTblID, columnName, parentTableID).rowID as ColumnRowID;
            var newPair = new BinaryDataPair(thsColID, value, complexWordTable);
            pairs.Add(thsColID, newPair);
            //Console.WriteLine("new Column:" + columnName + "(" + thsColID + ")" + "OWNER:" + ownerTblID + "; COL TYPE: parent");
            return newPair;
        }
        public IGenericValue NewWritePair(ColumnRowID colID, string colValue)
        {
            //ColumnRowID thsColID = FullTables.columnTable.GetWriteColumn(ownerTblID, colType, columnName).rowID as ColumnRowID;
            //String finalValue = value.ToDecodedRowID();
            //var newPair = NewPair(colID, colValue);
            //this.Add(newPair);
            //ColumnRowID thsColID = BinaryDataTable.columnTable.GetWriteColumn(parentTableID).rowID as ColumnRowID;
            var newPair = new BinaryDataPair(colID, colValue, complexWordTable);
            pairs.Add(colID, newPair);
            return newPair;
        }
        public IGenericValue NewWritePair(IORecordID ownerTblID, string colNM, string colValue)
        {
            ColumnRowID colID = BinaryDataTable.columnTable.GetWriteColumn(ownerTblID, ColumnTypes.column, colNM).rowID as ColumnRowID;
            var newPair = new BinaryDataPair(colID, colValue, complexWordTable);
            pairs.Add(colID, newPair);
            return newPair;
            //    ColumnRowID colID = FullTables.columnTable.GetWriteColumn(tableID, ColumnTypes.column, colNM).rowID as ColumnRowID;
            //    var newPair = NewPair(colID, colValue);
            //    this.Add(newPair);
            //    return newPair;
        }
        public void SetValue(ColumnRowID col, string value)
        {
            //*POtential issue HERE can't assume the pair exists. could throw things out of whack
            // pairs[col.ToInt()]
            //BinaryDataPair[] copyPairs = pairs.Values.ToArray();
            //copyPairs[copyPairs.Length - 1].SetEnd();
            //copyPairs.ToList().ForEach(pair => pair.rowCount = (short)pairs.Count);

            var endPair = pairs.Values.ToList().Where(x => x.isEnd == 1).First();
            endPair.UnSetEnd();
            pairs[endPair.colID] = endPair;
            //pairs.Values.ToList().ForEach(x => x.UnSetEnd());
            var currPair = pairs.FindAddIfNotFound(col.ToInt());
            pairs[col.ToInt()] = new BinaryDataPair(currPair.rowCount, col.ToInt(), value, 0, complexWordTable);
        }
        public void PrintRow()
        {
            Console.WriteLine("_________________________");
            foreach(var pair in pairs.Values)
            {
                string test = pair.StringValue(complexWordTable);
                Console.WriteLine(new ColumnRowID((int)pair.colID).ToColumnName() + ":" + pair.StringValue(complexWordTable) + ",");
                Console.WriteLine(pair.colID + ":" + pair.StringValue(complexWordTable) + ",");
            }
            Console.WriteLine("_________________________");
        }
    }
}
