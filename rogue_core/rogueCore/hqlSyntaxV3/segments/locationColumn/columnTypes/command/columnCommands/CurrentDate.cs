using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class CurrentDate : ColumnCommand
    {
        //public override string defaultName { get { return "CurrentDate"; } }
        public static string CodeMatchName { get { return "CURRENT_DATE";} }
        public CurrentDate(string colTxt) : base(colTxt)
        {

        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            return DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}
