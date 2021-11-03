using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    class UITable : UICommand
    {
        public override List<IMultiRogueRow> filteredRows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string commandNameID { get; }
        public override IORecordID tableId => throw new NotImplementedException();
        public UITable(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {

        }
    }
}
