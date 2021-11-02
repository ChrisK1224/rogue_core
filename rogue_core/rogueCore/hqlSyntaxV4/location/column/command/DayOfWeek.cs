using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class DayOfWeek : CommandLocation, ICommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "DAY_OF_WEEK"; } }
        public string columnName { get { return name; } }
        public string commandTxt { get; }
        public SelectRow selectRow { get; set; }
        public DayOfWeek(string comTxt, QueryMetaData metaData) : base(comTxt, metaData)
        {
            this.commandTxt = comTxt;
            this.Initialize(metaData);
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        {
            var date = DateTime.Parse(commandParams[0].GetValue(parentRows));
            var dayofWeek = ((int)date.DayOfWeek);
            return dayofWeek.ToString();
        }
    }
}
