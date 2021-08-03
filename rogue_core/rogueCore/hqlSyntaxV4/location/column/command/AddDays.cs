using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class AddDays : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "ADD_DAYS"; } }
        public string columnName { get { return name; } }
        public AddDays(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            var dt = DateTime.Parse(commandParams[0].GetValue(rows));
            int dayDiff = int.Parse(commandParams[1].GetValue(rows));
            dt = dt.AddDays(dayDiff);
            return dt.ToString("yyyy-MM-dd");
        }
    }
}
