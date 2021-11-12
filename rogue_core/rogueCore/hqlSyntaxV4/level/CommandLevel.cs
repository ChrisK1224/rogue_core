using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level
{
    public abstract class CommandLevel : HQLLevel
    {       
        public CommandLevel(string groupTxt, QueryMetaData metaData) : base(groupTxt, metaData, true)
        {

        }
        protected override ISelectRow ParseSelectRow(string key, string txt, QueryMetaData metaData)
        {
            return new ManualSelectRow(UICommand.uiCommandRowTableId, metaData);
        }
        protected override abstract List<HQLTable> ParseTable(List<string> txtLst, QueryMetaData metaData);
    }
}
