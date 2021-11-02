using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class AddSeconds : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "ADD_SECONDS"; } }
        public string columnName { get { return name; } }
        public AddSeconds(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            var dt = DateTime.Parse(commandParams[0].GetValue(rows));
            int seconds = int.Parse(commandParams[1].GetValue(rows));
            dt = dt.AddSeconds(seconds);
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
