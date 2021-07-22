using files_and_folders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class DateToEpoch : CommandLocation, ICommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "DATE_TO_EPOCH"; } }
        public string columnName { get { return name; } }
        public string commandTxt { get; }
        public SelectRow selectRow { get; set; }
        public DateToEpoch(string comTxt, QueryMetaData metaData) : base(comTxt, metaData)
        {
            this.commandTxt = comTxt;
            this.Initialize(metaData);
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        {
            return DateHelper.DateToEpoch(DateTime.Parse(commandParams[0].GetValue(parentRows))).ToString();
            //return "";
        }
    }
}
