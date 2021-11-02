using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class CurrentEOD : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "CURRENT_EOD"; } }
        public string columnName { get { return name; } }
        public CurrentEOD(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            return DateTime.Today.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
