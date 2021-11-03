using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands
{
    public abstract class UICommand : CommandLevel
    {
        
        public UICommand(string cmdTxt, QueryMetaData metaData) : base(cmdTxt, metaData) { }
    }
}
