using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.id.rogueID;

namespace rogue_core.rogueCore.ioObject
{
    public abstract class IOObject : IioObject
    {
        protected IRecordRow ioRecordRow;
        //Dictionary<IORecordID, IORecordRow> Children;
        public IOObject(IORecordID recordID)
        {
            //if(recordID.Equals(BinaryDataTable.ioRecordTable.ioItemID)){
            //    ioRecordRow = BinaryDataTable.ioRecordTable.;
            //}else{
                ioRecordRow = BinaryDataTable.ioRecordTable.idRows[recordID];
                //IORecordRow row = new IORecordTable().rows[recordID];
                
            //}
        }
        public IORecordID ioItemID{get{return new IORecordID((int)ioRecordRow.rowID);}}
        public string FolderPath(){return ioRecordRow.FolderPath();}
        public string ObjectName(){return ioRecordRow.ObjectName();}
        public string ObjectDescription(){return ioRecordRow.ObjectDescription();}
        public string MetaRecordType() {return ioRecordRow.MetaRecordType();}
        //* FIXME This doesn decode the path so wont work */
        public virtual void Create(){ DirectoryHelper.createFileCheckNotExists(FolderPath().ToString());}
    }
    public interface IioObject
    {
        //IORecordRow ioRecordRow { get; }
        ////Dictionary<IORecordID, IORecordRow> Children;
        //public IOObject(IORecordID recordID)
        //{
        //    //if(recordID.Equals(BinaryDataTable.ioRecordTable.ioItemID)){
        //    //    ioRecordRow = BinaryDataTable.ioRecordTable.;
        //    //}else{
        //    ioRecordRow = BinaryDataTable.ioRecordTable.rows[recordID];
        //    //IORecordRow row = new IORecordTable().rows[recordID];

        //    //}
        //}
        //public IORecordID ioItemID { get; }
        //public string FolderPath();
        //public string ObjectName();
        //public string ObjectDescription();
        //public string MetaRecordType();
        //* FIXME This doesn decode the path so wont work */
       // public virtual void Create() { DirectoryHelper.createFileCheckNotExists(FolderPath().ToString()); }
    }
}