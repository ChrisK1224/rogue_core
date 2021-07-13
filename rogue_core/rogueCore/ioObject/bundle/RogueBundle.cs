using System;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.idableItem;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.ioObject;

namespace rogue_core.rogueCore.bundle
{
    public class RogueBundle : Container
    {
        public RogueBundle(IORecordID ID) : base(ID)
        {
            
        }
        public RogueBundle GetBundle(String bundleName, String description = ""){
                var bundle = GetIOObject(bundleName, description, this.ioItemID, SystemIDs.ObjectTypes.bundle.name, this.FolderPath());
                return bundle as RogueBundle;
        }
        internal RogueDatabase<RowIDType> GetDatabase<RowIDType>(String dbName, String description = "") where RowIDType : RowID{ 
                var db = GetIOObject(dbName, description, this.ioItemID, SystemIDs.ObjectTypes.database.name, this.FolderPath());
                return db as RogueDatabase<RowIDType>;
        }
    }
}