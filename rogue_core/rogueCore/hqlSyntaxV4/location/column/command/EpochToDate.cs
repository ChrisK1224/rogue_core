using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;
using files_and_folders;
using System.Linq;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class EpochToDate : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "EPOCH_TO_DATE"; } }
        public string columnName { get { return name; } }
        public EpochToDate(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {

        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            return DateHelper.EpochToDate(commandParams[0].GetValue(rows)).ToString();
            //return "";
        }
    }
}
