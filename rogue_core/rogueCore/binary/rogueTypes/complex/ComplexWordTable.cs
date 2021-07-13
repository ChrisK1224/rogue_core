using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.IO;
using rogue_core.rogueCore.install;

namespace rogue_core.rogueCore.binary.word.complex
{
    public class ComplexWordTable
    {
        public string filePath { get; }
        //MemoryMappedFile memFile {get;}
        //FileStream fileStream;
        int structSize { get; }
        IORecordID tableID { get; }
        public ComplexWordTable(IORecordID tableID)
        {
            //string folderPath = RootVariables.rootPath + FullTables.ioRecordTable.rows[tableID].FolderPath().GetDecodedValue();
            //if(tableID.ToInt() == -1010)
            //{
            //    filePath = RootVariables.rootPath + Path.DirectorySeparatorChar + "-1005" + Path.DirectorySeparatorChar + "-1009" + Path.DirectorySeparatorChar + "-1010" + Path.DirectorySeparatorChar + "complex.rogueComplex";
            //}
            //else
            //{
            this.tableID = tableID;
             filePath = RootVariables.rootPath + tableID.TableInfo().FolderPath() + Path.DirectorySeparatorChar + "complex.rogueComplex";
            //}            
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            structSize = Marshal.SizeOf(typeof(ComplexWordValue));
            //fileStream = new FileStream(filePath, FileMode.Open);
            //memFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.OpenOrCreate, "FileA", 1000000, MemoryMappedFileAccess.ReadWrite);
        }
        /// <summary>
        /// This is for IORecord Table only for first run
        /// </summary>
        /// <param name="tableID"></param>
        public ComplexWordTable(IORecordID tableID, string folderPath)
        {
            //string folderPath = RootVariables.rootPath + FullTables.ioRecordTable.rows[tableID].FolderPath().GetDecodedValue();
            //if(tableID.ToInt() == -1010)
            //{
            //    filePath = RootVariables.rootPath + Path.DirectorySeparatorChar + "-1005" + Path.DirectorySeparatorChar + "-1009" + Path.DirectorySeparatorChar + "-1010" + Path.DirectorySeparatorChar + "complex.rogueComplex";
            //}
            //else
            //{
            this.tableID = tableID;
            filePath = folderPath + Path.DirectorySeparatorChar + "complex.rogueComplex";
            //}            
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            structSize = Marshal.SizeOf(typeof(ComplexWordValue));
            //fileStream = new FileStream(filePath, FileMode.Open);
            //memFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.OpenOrCreate, "FileA", 1000000, MemoryMappedFileAccess.ReadWrite);
        }
        //public void Close()
        //{
        //    fileStream.Close();
        //}
        public Int64 WriteValue(ComplexWordRow complexWordRow)
        {
            long position;            
            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                position = stream.Position;
                stream.Write(BitConverter.GetBytes(complexWordRow.complexWordCount),0,4);                
                var bytes = complexWordRow.ToBytes();
                stream.Write(bytes, 0, bytes.Length);
                //position = stream.Position;
            }
            return position;
            //FileStream stream = File.OpenRead(@"D:\FFv1\dpx1\1.dpx");
            //byte[] fileBytes = new byte[stream.Length];
            //using (var accessor = memFile.CreateViewAccessor(accesso, size))
            //{

            //}
            //    MemoryMappedFileAccess.ReadWrite, new MemoryMappedFileSecurity(), HandleInheritability.Inheritable, true))
            //{
            //    var viewStream = memoryMapped.CreateViewStream();
            //    viewStream.Write(fileBytes, 0, fileBytes.Length);
            //}
        }
        public ComplexWordRow GetValue(long start)
        {
            //Int32 size = BitConverter.ToInt32(bytes, 0);
            //size = size * structSize;
            byte[] bytes;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] sizeBytes = new byte[4];
                stream.Seek(start, SeekOrigin.Begin);
                stream.Read(sizeBytes, 0, 4);
                int size = BitConverter.ToInt32(sizeBytes, 0) * structSize;
                bytes = new byte[size];
                stream.Read(bytes, 0, size);
            }
            return new ComplexWordRow(bytes);
            //long offset = 0; // 256 megabytes 5950
            //long length = new FileInfo(filePath).Length;
            //int structSize = Marshal.SizeOf(typeof(ComplexWordValue));
            // Create the memory-mapped file. 
            //using (memFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, "FileA"))
            //{
            // Create a random access view, from the 256th megabyte (the offset) 
            // to the 768th megabyte (the offset plus length).
            //using (var accessor = memFile.CreateViewAccessor(start, size))
            //{
            //    ComplexWordValue valuePair;
            //    Int64 rowCount;
            //    BinaryDataRow newRow;
            //    // Make changes to the view.                     
            //    for (long i = 0; i < size;)
            //    {
            //        rowCount = accessor.ReadInt64(i);
            //        newRow = new BinaryDataRow((int)rowCount);
            //        i += 8;
            //        for (int j = 0; j < rowCount; j++)
            //        {
            //            accessor.Read(i, out valuePair);
            //            i += structSize;
            //            //newRow.pairs[j] = valuePair;
            //        }
            //        //yield return newRow;
            //    }
            //}
        }
        public ComplexWordRow NewValue(string value)
        {
            var row = new ComplexWordRow(tableID, value);
            long rowPosition = WriteValue(row);
            row.SetPosition(rowPosition);
            return row;
        }
    }
}
