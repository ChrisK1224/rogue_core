using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.binary.prefilled
{
    public class IORecordTable : PreFilledTable<IORecordRow>
    {
        List<IRecordRow> allRows { get; set; }
        public Dictionary<long, IRecordRow> idRows { get; set; }
        public ReadOnlyCollection<IRecordRow> rows { get { return allRows.AsReadOnly(); } }
        public IORecordTable() : base()
        {
            idRows = new Dictionary<long, IRecordRow>();
            allRows = new List<IRecordRow>();
            filePath = install.RootVariables.rootPath + new IORecordRecordRow().FolderPath()  + Path.DirectorySeparatorChar + CreateTableName(ioItemID.ToString()); //"BIN_" + tableID.ToString() + ".rogue";
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            complexWordTable = new ComplexWordTable(ioItemID, install.RootVariables.rootPath + new IORecordRecordRow().FolderPath());
            foreach (var row in StreamDataRows())
            {
                //if (row.rowID.ToInt() != -1010 && row.rowID.ToInt() != -1011)
                //{
                    var ioRow = new IORecordRow(row);
                    allRows.Add(ioRow);
                    idRows.Add(ioRow.rowID, ioRow);
                //}
            }       
        }
        //void InitializeRefRows()
        //{
        //    idRows = new Dictionary<long, IRecordRow>();
        //    allRows = new List<IRecordRow>();
        //    //var rec = new IORecordRecordRow();
        //    //var col = new ColumnRecordRow();
        //    //allRows.Add(rec);
        //    //idRows.Add(rec.rowID, rec);
        //    //allRows.Add(col);
        //    //idRows.Add(col.rowID, col);
        //}
        public override void DeleteRewrite(List<IReadOnlyRogueRow> deletedRows)
        {
            foreach (var row in deletedRows)
            {
                var ioRecordRow = new IORecordRow(row);
                if(ioRecordRow.MetaRecordType().ToUpper() == "TABLE")
                {
                    FileInfo fileInfo = new FileInfo(ioRecordRow.FullFilePath());
                    string newPath = fileInfo.DirectoryName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fileInfo.Name) + "_DELETED" + fileInfo.Extension;
                    File.Move(fileInfo.FullName, newPath);
                }
            }
            idRows = new Dictionary<long, IRecordRow>();
            allRows = new List<IRecordRow>();
            base.DeleteRewrite(deletedRows);
        }
        public override void UpdateRewrite()
        {
            idRows = new Dictionary<long, IRecordRow>();
            allRows = new List<IRecordRow>();
            base.UpdateRewrite();
        }
        public override void Write()
        {            
            using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
            {
                foreach (var row in writeRows)
                {
                    writer.Write(row.WriteBytes());
                    var ioRow = new IORecordRow(row);
                    allRows.Add(ioRow);
                    idRows.Add(ioRow.rowID, ioRow);
                }
            }
            writeRows.Clear();
        }
        public String GuessTableIDByName(String tblName)
        {
            var retRows = rows.Where(x => x.ObjectNameID() == tblName.ToUpper()).ToList();
            if (retRows.Count > 1)
            {
                throw new Exception("NON UNIQUE Table NAME");
            }
            return retRows[0].rowID.ToString();
        }
        public String GetTableNameByRogueID(long tableID)
        {
            return rows.Where(x => x.rowID == tableID).FirstOrDefault().ObjectName();
            //return "-1015 = @METAFOREIGNID";
            //return new HQLQuery(GetTableNameByIDQry.Replace("@IORECORDID", tableID.ToString())).hierarchyGrid.FirstRowFirstValue();
        }
        public IEnumerable<IRecordRow> GetChildOfTable(int parentTableID)
        {
            foreach (var rowPair in  rows.Where(x => x.OwnerIOITem().ToString() == parentTableID.ToString()))
            {
                yield return rowPair;
            }
            //foreach (var rowPair in new HQLQuery(childrenOfTable.Replace("@PARENTTABLEID", parentTableID.ToString())).hierarchyGrid)
            //{
            //    yield return rowPair.Value;
            //}
        }
        public List<long> GetTableIDsList()
        {
            return rows.Select(x => x.rowID).ToList();
            //var tableIDs = new List<long>();
            //foreach (var thsRowPair in rows)
            //{
            //    tableIDs.Add(thsRowPair.rowID);
            //}
            //return tableIDs;
        }
        internal RogueDatabase<DataRowID> GetDatabaseByFullName(string fullDBName)
        {
            string[] IORecords = fullDBName.Split('.');
            //int[] ids = new int[IORecords.Length];
            long currParentRowID = 0;
            for (int i = 0; i < IORecords.Length; i++)
            {
                var row = rows.Where(x => x.rowID == currParentRowID && x.ObjectNameID() == IORecords[i].ToUpper()).FirstOrDefault();
                //HQLQuery idQry = new HQLQuery(tableCircleQuery.Replace("@PARENTROWID", currParentRowID.ToString()).Replace("@METAFOREIGNID", IORecords[i]));
                //HierarchyRow row = idQry.hierarchyGrid[0].Value;
                //String test= row.baseRow.IGetBasePair(new ColumnRowID(-1012));
                currParentRowID = row.rowID;
                //ids[i] = currParentRowID.ToInt();
            }
            RogueDatabase<DataRowID> thsDB = new RogueDatabase<DataRowID>((int)currParentRowID);
            return thsDB;
        }
        private int EncodeFullTableName(String fullTableName)
        {
            string[] IORecords = fullTableName.Split('.');
            //int[] ids = new int[IORecords.Length];
            long currParentRowID = 0;
            for (int i = 0; i < IORecords.Length; i++)
            {
                var row = rows.Where(x => x.rowID == currParentRowID && x.ObjectNameID() == IORecords[i].ToUpper()).FirstOrDefault();
                //HQLQuery idQry = new HQLQuery(tableCircleQuery.Replace("@PARENTROWID", currParentRowID.ToString()).Replace("@METAFOREIGNID", IORecords[i]));
                //HierarchyRow row = idQry.hierarchyGrid[0].Value;
                //String test= row.baseRow.IGetBasePair(new ColumnRowID(-1012));
                currParentRowID = row.rowID;
                //ids[i] = currParentRowID.ToInt();
            }
            return (int)currParentRowID;
            //var qry = new HQLQuery(tableCircleQuery);
            //HierarchyGrid results =    qry.hierarchyGrid;

            // owner = 1016, foreignID = 1015
            //int lvl = 0;
            //foreach (var thsRow in results)
            //{
            //    //String blah = thsRow.Value.baseRow.IGetBasePair(-1015).DisplayValue();
            //    //String lbah2 = thsRow.Value.baseRow.IGetBasePair(-1016).DisplayValue();
            //    //if (thsRow.Value.baseRow.IGetBasePair(-1015).DisplayValue().Equals(IORecords[lvl]))
            //    //{
            //    //    String ss = "SFD";
            //    //}
            //    //if (thsRow.Value.baseRow.IGetBasePair(-1016).DisplayValue().Equals(currParentRowID.ToString()))
            //    //{
            //    //    String s = "SDF";
            //    //}
            //    if (thsRow.Key == lvl && thsRow.Value.baseRow.IGetBasePair(-1015).DisplayValue().Equals(IORecords[lvl]) && thsRow.Value.baseRow.IGetBasePair(-1016).DisplayValue().Equals(currParentRowID.ToString()))
            //    {
            //        if(lvl == IORecords.Length)
            //        {
            //            return new IORecordID(thsRow.Value.rowID.ToInt());
            //        }
            //        currParentRowID = new UnKnownID(thsRow.Value.baseRow.IGetBasePair(-1016).WriteValue());
            //        lvl++;
            //    }

            //}
            //return null;
            //results.TransformByLevelAndIndex(new List<ColumnRowID>() { -1015 });
            //for(int i =0; i < IORecords.Length;i++)
            //{
            //    RowID thisLevelRowID =
            //        //results.transformedGrid[i][new ColumnRowID(-1015)][IORecords[i]].rowID;

            //}
            //KeyValuePair<int, String> currRound = new KeyValuePair<int, String>(0, IORecords[0]);
            //int p = 0;
            //foreach(var rowPair in results)
            //{
            //    if(rowPair.Key.Equals(currRound.Key) && rowPair.Value.GetValueByColID(new ColumnRowID(-1015)).Equals(currRound.Value))
            //    {
            //        currRound = new KeyValuePair<int, String>(p, IORecords[0]);
            //        p++;
            //    }
            //}

        }
        public IORecordID DecodeTableName(String fullTableName)
        {
            string[] IORecords = fullTableName.Split('.');
            return DecodeTableName(IORecords);
            //long currParentRowID = 0;
            //for (int i = 0; i < IORecords.Length; i++)
            //{
            //    var row = rows.Where(x => x.rowID == currParentRowID && x.ObjectNameID() == IORecords[i].ToUpper()).FirstOrDefault();
            //    //HQLQuery idQry = new HQLQuery(tableCircleQuery.Replace("@PARENTROWID", currParentRowID.ToString()).Replace("@METAFOREIGNID", IORecords[i]));
            //    //HierarchyRow row = idQry.hierarchyGrid[0].Value;
            //    currParentRowID = row.rowID;
            //}
            //return new IORecordID((int)currParentRowID);
        }
        public IORecordID DecodeTableName(string[] IORecords)
        {
            //string[] IORecords = fullTableName.Split('.');
            long currParentRowID = 0;
            for (int i = 0; i < IORecords.Length; i++)
            {
                var row = rows.Where(x => (x.OwnerIOITem() == currParentRowID.ToString() || x.OwnerIOITem() == SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString()) && x.ObjectNameID() == IORecords[i].ToUpper()).FirstOrDefault();
                //HQLQuery idQry = new HQLQuery(tableCircleQuery.Replace("@PARENTROWID", currParentRowID.ToString()).Replace("@METAFOREIGNID", IORecords[i]));
                //HierarchyRow row = idQry.hierarchyGrid[0].Value;
                currParentRowID = row.rowID;
            }
            return new IORecordID((int)currParentRowID);
        }
        public String GetDecodedFullTableName(IORecordID thsTbl)
        {
            //HQLQuery idQry = new HQLQuery(fullTableNameCircleQuery.Replace("@IORECORDID", thsTbl.ToString()));
            //HierarchyRow row = idQry.hierarchyGrid[0].Value;
            var row = rows.Where(x => x.rowID == thsTbl.ToDecodedRowID().ToInt()).FirstOrDefault();
            String parentRowID = row.OwnerIOITem();
            String objectName = row.ObjectNameID();
            String final = GetFullNameLoop(objectName, parentRowID);
            return final;
        }
        private String GetFullNameLoop(String objectName, String parentID)
        {
            //HQLQuery idQry = new HQLQuery(fullTableNameCircleQuery.Replace("@IORECORDID", parentID.ToString()));
            //HierarchyRow row = idQry.hierarchyGrid[0].Value;
            var row = rows.Where(x => x.rowID.ToString() == parentID).FirstOrDefault();
            String currParentRowID = row.OwnerIOITem();
            String objectNameAddition = row.ObjectName();
            objectName = objectNameAddition + "." + objectName;
            if (currParentRowID != "0")
            {
                return GetFullNameLoop(objectName, currParentRowID);
            }
            return objectName;
        }
    }
    public class IORecordRow : IRecordRow
    {
        IReadOnlyRogueRow baseRow { get; }
        public IORecordRow(IReadOnlyRogueRow baseRow)
        {
            this.baseRow = baseRow;
        }
        
        public long rowID { get { return baseRow.rowID.ToInt(); } }
        public string OwnerIOITem()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.ownerIOItemID);
        }
        public string MetaRecordType()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaRecordTypeID);
        }
        public string ObjectName()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaForeignID);
        }
        public string ObjectNameID()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaForeignID).ToUpper();
        }        
        public string ObjectDescription()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaDescriptionID);
        }
        public string FolderPath()
        {
            //string folder = baseRow.GetValueByColumn(SystemIDs.Columns.metaFolderPathID);
            //string file = baseRow.GetValueByColumn(SystemIDs.Columns.metaTablePathID);
            //string name = baseRow.GetValueByColumn(SystemIDs.Columns.metaForeignID);
            //string desc = baseRow.GetValueByColumn(SystemIDs.Columns.metaDescriptionID);
            //string typ = baseRow.GetValueByColumn(SystemIDs.Columns.metaRecordTypeID);
            //baseRow.PrintRow();
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaFolderPathID).Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }
        public string FilePath()
        {
            return baseRow.GetValueByColumn(SystemIDs.Columns.metaTablePathID);
        }
        public string FullFilePath()
        {
            return install.RootVariables.rootPath + Path.DirectorySeparatorChar + baseRow.GetValueByColumn(SystemIDs.Columns.metaTablePathID);
        }
    }
    public interface IRecordRow
    {       
        public long rowID { get; }
        public string OwnerIOITem();
        public string MetaRecordType();
        public string ObjectName();
        public string ObjectNameID();
        public string ObjectDescription();
        public string FolderPath();
        public string FilePath();
    }
    public class ColumnRecordRow : IRecordRow
    {
        public long rowID { get { return SystemIDs.IOTableRecords.Tables.SystemTables.columnTableID; } }
        public string OwnerIOITem()
        {
            return SystemIDs.IOTableRecords.Databases.metaDatabase.ToString();
        }
        public string MetaRecordType()
        {
            return "Table";
        }
        public string ObjectName()
        {
            return "Column";
        }
        public string ObjectNameID()
        {
            return "COLUMN";
        }
        public string ObjectDescription()
        {
            return "Table that stores all Columns";
        }
        public string FolderPath()
        {
            return Path.DirectorySeparatorChar + SystemIDs.IOTableRecords.Bundles.systemBundleID.ToString() + Path.DirectorySeparatorChar + SystemIDs.IOTableRecords.Databases.metaDatabase.ToString() + Path.DirectorySeparatorChar + SystemIDs.IOTableRecords.Tables.SystemTables.columnTableID.ToString();
        }
        public string FilePath()
        {
            throw new Exception("Unset variable");
            //return Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Bundles.systemBundleID.ToString() + Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Databases.metaDatabase.ToString() + Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Tables.SystemTables.columnTableID.ToString() + Path.DirectorySeparatorChar + System
        }
    }
    public class IORecordRecordRow : IRecordRow
    {
        public long rowID { get { return SystemIDs.IOTableRecords.Tables.SystemTables.ioRecordTableID; } }
        public string OwnerIOITem()
        {
            return SystemIDs.IOTableRecords.Databases.metaDatabase.ToString();
        }
        public string MetaRecordType()
        {
            return "Table";
        }
        public string ObjectName()
        {
            return "IORecords";
        }
        public string ObjectNameID()
        {
            return "IORECORDS";
        }
        public string ObjectDescription()
        {
            throw new Exception();
        }
        public string FolderPath()
        {
            return Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Bundles.systemBundleID.ToString() + Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Databases.metaDatabase.ToString() + Path.DirectorySeparatorChar.ToString() + SystemIDs.IOTableRecords.Tables.SystemTables.ioRecordTableID.ToString();
        }
        public string FilePath()
        {
            throw new Exception();
        }

    }
}
