using files_and_folders;
using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class DateToEpoch : ColumnCommand
    {
        public static string CodeMatchName { get { return "DATE_TO_EPOCH"; } }
        public DateToEpoch(string colTxt) : base(colTxt)
        {

        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            return DateHelper.DateToEpoch(DateTime.Parse(paramColumns[0].GetValue(rows))).ToString();
        }
    }
}
