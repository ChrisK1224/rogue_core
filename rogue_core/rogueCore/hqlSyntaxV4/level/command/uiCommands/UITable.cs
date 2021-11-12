using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    class UITable : CommandLevel
    {
        public const string commandNameIDConst = "UI_TABLE";
        public static string CodeMatchName { get { return commandNameIDConst; } }

        public string commandNameID => throw new NotImplementedException();

        public UITable(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {
            //var manualRow = new ManualMultiRogueRow();
            //manualRow.Add("Control_Type", "Table");
            //manualRow.Add("Control_Style", commandParams[0].GetValue();
        }

        //protected override ISelectRow ParseSelectRow(string txt, QueryMetaData metaData)
        //{
            
        //}

        protected override List<HQLTable> ParseTable(List<string> txt, QueryMetaData metaData)
        {
            return new List<HQLTable>() { new HQLTable(txt[0], metaData)};
        }
    }
}
