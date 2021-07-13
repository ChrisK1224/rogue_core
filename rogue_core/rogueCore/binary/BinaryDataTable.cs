using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.install;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using files_and_folders;
using rogue_core.rogueCore.binary.word.number;
using rogue_core.rogueCore.binary.rogueTypes.dec;
using rogue_core.rogueCore.binary.rogueTypes.emoji;
using rogue_core.rogueCore.binary.rogueTypes.date;
using rogue_core.rogueCore.binary.rogueTypes;
using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.ioObject;
using rogue_core.rogueCore.binary.word;
using System.Diagnostics;
using System.Linq;

namespace rogue_core.rogueCore.binary
{
    public class BinaryDataTable : IRogueTable, IioObject
    {
        public static NumberTable numberTable = new NumberTable();
        public static SimpleWordTable simpleTable = new SimpleWordTable();
        public static DecimalTable decimalTable = new DecimalTable();
        public static EmojiTable emojiTable = new EmojiTable();
        public static DateTable dateTable = new DateTable();
        public static IORecordTable ioRecordTable = new IORecordTable();
        public static ColumnTable columnTable = new ColumnTable();        
        protected List<IRogueRow> writeRows = new List<IRogueRow>();
        public IReadOnlyList<IRogueRow> currWriteRows{ get { return writeRows; } }
        public string filePath { get; protected set; }
        public ComplexWordTable complexWordTable { get; protected set; }
        public IORecordID ioItemID { get;  }
        public BinaryDataTable(IORecordID tableID)
        {
            this.ioItemID = tableID;
            string folderPath = RootVariables.rootPath + ioRecordTable.idRows[tableID].FolderPath();
            filePath = folderPath + Path.DirectorySeparatorChar + CreateTableName(tableID.ToString()); //"BIN_" + tableID.ToString() + ".rogue";
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            complexWordTable = new ComplexWordTable(tableID);
            //Init(tableID, RootVariables.rootPath + ioRecordTable.idRows[tableID].FolderPath());
            //this.ioItemID = tableID;
            //string folderPath = RootVariables.rootPath + ioRecordTable.idRows[tableID].FolderPath();
            //filePath = folderPath + Path.DirectorySeparatorChar + CreateTableName(tableID.ToString()); //"BIN_" + tableID.ToString() + ".rogue";
            //if (!File.Exists(filePath))
            //    File.Create(filePath).Close();
            //complexWordTable = new ComplexWordTable(tableID);
            //File.WriteAllText(filePath, "");
            //File.WriteAllText(complexWordTable.filePath, "");
            //*TEMP TRANSLATIONCODE 
            //File.Delete(filePath);
        }
        //protected void Init(IORecordID tableID, string folderPath)
        //{
        //    this.ioItemID = tableID;
        //    //string folderPath = 
        //    filePath = folderPath + Path.DirectorySeparatorChar + CreateTableName(tableID.ToString()); //"BIN_" + tableID.ToString() + ".rogue";
        //    if (!File.Exists(filePath))
        //        File.Create(filePath).Close();
        //    complexWordTable = new ComplexWordTable(tableID);
        //}
        /// <summary>
        /// Only for iorecords since needs to be open to get folderPath
        /// </summary>
        /// <param name="tableID"></param>
        protected BinaryDataTable(string itemID)
        {
            this.ioItemID = new IORecordID(itemID);
            string folderPath = RootVariables.rootPath + Path.DirectorySeparatorChar + "-1005" + Path.DirectorySeparatorChar  + "-1009" + Path.DirectorySeparatorChar  + itemID;
            filePath = folderPath + Path.DirectorySeparatorChar + CreateTableName(itemID); //"BIN_" + tableID.ToString() + ".rogue";
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            complexWordTable = new ComplexWordTable(ioItemID);
            //File.WriteAllText(filePath, "");
            //File.WriteAllText(complexWordTable.filePath, "");
            //*TEMP TRANSLATIONCODE 
            //File.Delete(filePath);
        }
        /// <summary>
        /// Temporary for direct replacement from conversion from old format
        /// </summary>
        /// <param name="tableID"></param>
        public BinaryDataTable(IORecordID tableID, string filePath)
        {
            this.ioItemID = tableID;
            //string folderPath = RootVariables.rootPath + ioRecordTable.idRows[tableID].FolderPath();
            this.filePath = filePath;
                //folderPath + Path.DirectorySeparatorChar + CreateTableName(tableID.ToString()); //"BIN_" + tableID.ToString() + ".rogue";
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            complexWordTable = new ComplexWordTable(tableID);
            
            //File.Copy(filePath, filePath + "OLD");
            //File.Copy(complexWordTable.filePath, complexWordTable.filePath + "OLD");
            File.WriteAllText(filePath, "");
            File.WriteAllText(complexWordTable.filePath, "");
            //*TEMP TRANSLATIONCODE 
            //File.Delete(filePath);
        }
        //*TEmPorary just to be able to open without having written it first. so for install
        protected BinaryDataTable()
        {
            this.ioItemID = -1010;
        }
        public static string CreateTableName(string id)
        {
            return "BIN_" + id + ".rogue"; 
        }
        public virtual void Write()
        {
            //List<BinaryDataPair> writePairs = new List<BinaryDataPair>();
            //writeRows.ForEach(x => writePairs.AddRange(x.pairs.Values));           
            //var writeBytes = ToByteArray<BinaryDataPair>(writePairs.ToArray());
            //**This could be faster if combine all bytes from all rows then write at once***
            //**So i could set the WriteValueReference position to the byte position and then when writing in bulk take in starting point of baseStream position and add to the zero based position used from the array.****
            //*****Need to improve this writer function. BinaryWriter is open for too lon with this method and requires virtual for when writing to ValueRefInstance Table since it doesnt write value reference.
            var valRefs = new SortedDictionary<long, List<BinaryDataPair>>();
            long startPosition;
            var tmr = new Stopwatch();
            tmr.Start();
            using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
            {
                List<byte> writeContent = new List<byte>();
                long position = 0;
                foreach (var row in writeRows)
                {
                    //writer.Write(row.WriteBytes());
                    //row.WriteBytes()
                    var appendArray = row.WriteBytes();
                    writeContent.AddRange(appendArray);
                    //Buffer.BlockCopy(appendArray, 0, writeContent, writeContent.Count, appendArray.Length);
                    valRefs.Add(position, row.pairs.Values.ToList());
                    position += appendArray.Length;
                }
                startPosition = writer.BaseStream.Position;
                writer.Write(writeContent.ToArray());
            }
            writeRows.Clear();
            tmr.Stop();
            var bl = tmr.ElapsedMilliseconds;
            tmr.Restart();
            //foreach (var row in valRefs)
            //{
            //    foreach (var pair in row.Value)
            //    {
            //        int p = 0;
            //        foreach (var valRef in pair.GetReferenceList(complexWordTable))
            //        {
            //            valRef.WriteValueReference(ioItemID, valRef.valueID, pair.colID, startPosition + row.Key, p);
            //            p++;
            //        }
            //    }
            //}
            tmr.Stop();
            var ll = tmr.ElapsedMilliseconds;
        }
        byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        public void AddWriteRow(IRogueRow newRow)
        {
            writeRows.Add(newRow);
        }
        public virtual IEnumerable<IReadOnlyRogueRow> StreamDataRows()
        {
            byte[] chunk;
            int CHUNK_SIZE = 100000;
            BufferedStream buff = new BufferedStream(File.Open(filePath, FileMode.Open), (16 * 1024));
            BinaryReader br = new BinaryReader(buff);
            Dictionary<long,BinaryDataPair> currPairs = null;
            Action<int> DoNothing = (x) => { };
            Action<int> SetRowPairs;
            Action<int> NewReturn = (x) => { currPairs = new Dictionary<long, BinaryDataPair>(x); SetRowPairs = DoNothing; };
            SetRowPairs = NewReturn;
            while ((chunk = br.ReadBytes(CHUNK_SIZE)).Length > 0)
            {
                var pairs = BinaryReadWrite.FromByteArray<BinaryDataPair>(chunk);
                for (int p = 0; p < pairs.Length; p++)
                {
                    SetRowPairs(pairs[p].rowCount);
                    currPairs.Add(pairs[p].colID, pairs[p]);
                    if (pairs[p].IsRowEnd())
                    {
                        yield return new BinaryDataRow(currPairs, complexWordTable);
                        SetRowPairs = NewReturn;
                    }                    
                }
            }
            br.Close();
        }
        public IRogueRow NewWriteRow()
        {
            var newRow = new BinaryDataRow(complexWordTable);
            writeRows.Add(newRow);
            return newRow;
        }
        public static byte[] ToByteArray<T>(T[] source) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(source, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                byte[] destination = new byte[source.Length * Marshal.SizeOf(typeof(T))];
                Marshal.Copy(pointer, destination, 0, destination.Length);
                return destination;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }
        //This is for updates and assumes all new rows are in writeRows 
        public virtual void UpdateRewrite()
        {
            File.WriteAllText(filePath, "");
            Write();
        }
        public virtual void DeleteRewrite(List<IReadOnlyRogueRow> deletedRows)
        {
            File.WriteAllText(filePath, "");
            Write();
        }
        public IReadOnlyRogueRow GetRowById(long rowId)
        {
            foreach(var row in StreamDataRows())
            {
                if (row.rowID.ToInt() == rowId)
                    return row;
            }
            return null;
        }
    }
}
