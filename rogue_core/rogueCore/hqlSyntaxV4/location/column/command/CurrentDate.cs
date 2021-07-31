using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class CurrentDate : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "CURRENT_DATE";} }
        public string columnName {get { return name; } }
        public CurrentDate(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            return DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}
