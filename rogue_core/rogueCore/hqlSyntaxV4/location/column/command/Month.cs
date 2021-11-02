using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.select;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class Month : CommandLocation, ICommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "MONTH"; } }
        public string columnName { get { return name; } }
        public string commandTxt { get; }
        public SelectRow selectRow { get; set; }
        public Month(string comTxt, QueryMetaData metaData) : base(comTxt, metaData)
        {
            this.commandTxt = comTxt;
            this.Initialize(metaData);
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        {
            var date = DateTime.Parse(commandParams[0].GetValue(parentRows));
            var month = date.Month;
            return month.ToString();
        }
    }
}
