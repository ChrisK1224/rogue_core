using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    class UICell : UICommand
    {
        public const string commandNameIDConst = "UI_CELL";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override string commandNameID { get { return CodeMatchName; } }
        //public override List<IMultiRogueRow> filteredRows { get; set; }
        public UICell(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {
            
        }

        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            throw new NotImplementedException();
        }
    }
}