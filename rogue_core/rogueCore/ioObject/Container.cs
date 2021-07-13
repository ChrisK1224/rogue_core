using System.IO;
using System;
using rogue_core.rogueCore.id.rogueID;
using FilesAndFolders;
using rogue_core.rogueCore.id.idableItem;
using System.Collections.Generic;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.bundle;
using rogue_core.rogueCore.install;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.prefilled;
using System.Linq;
using rogueCore.hqlSyntaxV3.filledSegments;

namespace rogue_core.rogueCore.ioObject
{
    public class Container : IOObject
    {
        List<IRecordRow> childObjects { get; }
        public Container(IORecordID ID) : base(ID) {
            //*Just use this with linq**
            childObjects = BinaryDataTable.ioRecordTable.idRows.Values.Where(x => x.OwnerIOITem() == ID.ToString()).ToList();
        }
        public IioObject ConvertToIOObject(string metaRecordType, IORecordID thsID)
        {
            if (metaRecordType == SystemIDs.ObjectTypes.table.name)
            {
                return new BinaryDataTable(thsID);
            }
            else if (metaRecordType == SystemIDs.ObjectTypes.bundle.name)
            {
                return new RogueBundle(thsID);
            }
            else if (metaRecordType == SystemIDs.ObjectTypes.database.name)
            {
                return new RogueDatabase<DataRowID>(thsID);
            }
            return null;
        }
        protected IioObject GetIOObject(String foreignID, String description, IORecordID metaOwnerID, string metaRecordType, String parentFolderPath)
        {
            var foundRow = childObjects.Where(x => x.OwnerIOITem() == metaOwnerID.ToString() && x.ObjectNameID() == foreignID.ToUpper().ToString()).FirstOrDefault();
            if (foundRow != null)
            {
                return ConvertToIOObject(foundRow.MetaRecordType(), new IORecordID((int)foundRow.rowID));
            }
            else
            {
                var newRecord = BinaryDataTable.ioRecordTable.NewWriteRow();
                String filePath = "";
                //*Changed from above so that it only writes relative path and not full path not sure if FUll path is needed so need to verify*
                String newFolderPath = parentFolderPath.Replace("\\","/") + Path.DirectorySeparatorChar + newRecord.rowID.ToString();
                //* TODO Linux only terrible way to fix file path
                DirectoryHelper.createFolderNotExists(RootVariables.rootPath + newFolderPath);
                if (metaRecordType == SystemIDs.ObjectTypes.table.name)
                {
                    filePath = newFolderPath + Path.DirectorySeparatorChar + BinaryDataTable.CreateTableName(newRecord.rowID.ToString()); //newRecord.rowID.ToString() + ".rogue";
                    DirectoryHelper.createFileCheckNotExists(RootVariables.rootPath + filePath);
                    newRecord.NewWritePair(SystemIDs.Columns.metaTablePathID, filePath);
                }
                newRecord.NewWritePair(SystemIDs.Columns.ownerIOItemID, metaOwnerID.ToString());
                newRecord.NewWritePair(SystemIDs.Columns.metaForeignID, foreignID);
                newRecord.NewWritePair(SystemIDs.Columns.metaDescriptionID, description);
                newRecord.NewWritePair(SystemIDs.Columns.metaFolderPathID, newFolderPath);
                //*NEED TO MAKE SURE metaRecordType writes correctly**
                newRecord.NewWritePair(SystemIDs.Columns.metaRecordTypeID, metaRecordType);
                BinaryDataTable.ioRecordTable.Write();
                childObjects.Add(new IORecordRow(newRecord));
                return ConvertToIOObject(metaRecordType, new IORecordID(newRecord.rowID.ToInt()));
                
                //return newIOItem;
            }
        }        
        //*This class is used to create uniqueID of the string and metarecordType. It allows same name of different object but not same name same object. This way there is no error or invalid name only returns the right type or create new. Never invalid sin
        public static void CreateObject(string parentRogueID, string name, string desc)
        {
            string objTyp = BinaryDataTable.ioRecordTable.idRows[new IORecordID(parentRogueID)].MetaRecordType();
            if (objTyp.ToLower() == "database")
            {
                RogueDatabase<DataRowID> thsDb = new RogueDatabase<DataRowID>(new IORecordID(parentRogueID));
                thsDb.GetTable(name, desc);
            }
            else if (parentRogueID == "-1004")
            {
                RogueBundle rootBundle = new RogueBundle(new IORecordID(parentRogueID));
                rootBundle.GetBundle(name, desc);
            }
            else if (objTyp.ToLower() == "bundle")
            {
                RogueBundle thsBundle = new RogueBundle(new IORecordID(parentRogueID));
                thsBundle.GetDatabase<DataRowID>(name, desc);
            }
        }
    }
}