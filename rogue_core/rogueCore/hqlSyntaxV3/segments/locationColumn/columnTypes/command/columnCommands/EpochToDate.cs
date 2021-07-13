using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;
using files_and_folders;
namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class EpochToDate : ColumnCommand
    {
        public static string CodeMatchName { get { return "EPOCH_TO_DATE"; } }
        public EpochToDate(string colTxt) : base(colTxt)
        {

        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            return DateHelper.EpochToDate(paramColumns[0].GetValue(rows)).ToString();
        }
    }
}
