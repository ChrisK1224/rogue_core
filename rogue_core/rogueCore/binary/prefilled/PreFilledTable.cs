using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace rogue_core.rogueCore.binary.prefilled
{
    public class  PreFilledTable<RowType> : BinaryDataTable
    {
        
        public PreFilledTable(IORecordID tableID) : base(tableID) { }
        protected PreFilledTable(string itemID) : base(itemID) { }
        protected PreFilledTable() : base() { }
    }
}
