using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    class UIRow : UICommand
    {
        public const string commandNameIDConst = "UI_ROW";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override string commandNameID { get { return CodeMatchName; } }
        public UIRow(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {

        }
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            var newRow = new ManualBinaryRow();
            newRow.AddPair(UICommand.uiColumn_ControlType, "table");
            newRow.AddPair(UICommand.uiColumn_ControlValue, "");
            newRow.AddPair(UICommand.uiColumn_ControlStyle, "");
            
            yield return newRow;
        }
    }
}
