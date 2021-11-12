using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    public abstract class UICommand : CommandFrom
    {
        public override IORecordID tableId {get{return uiCommandRowTableId;} }
        public const int uiColumn_ControlType = 2795495;
        public const int uiColumn_ControlValue = 2795501;
        public const int uiColumn_ControlStyle = 2795498;
        public const int uiCommandRowTableId = 2795491;
        public UICommand(string cmdTxt, QueryMetaData metaData) : base(cmdTxt, metaData) { }
    }
}
