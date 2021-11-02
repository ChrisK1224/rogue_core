using FilesAndFolders;
using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class Lag : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "LAG"; } }
        public string columnName { get { return name; } }
        public QueryMetaData metaData { get; }
        List<IMultiRogueRow> levelRows { get; }
        public Lag(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) 
        {
            //this.metaData = metaData;
           levelRows = metaData.CurrentLevelRows();
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            //**so frkn bad
           // List<IMultiRogueRow>  levelRows = metaData.CurrentLevelAllRows();
            var refRows = rows.First();            
            var index = levelRows.FindIndex(x => x.tableRefRows == refRows);
            int lagNum = int.Parse(commandParams[0].GetValue(rows)) + index;
            if(lagNum < levelRows.Count && lagNum >= 0)
            {
                return commandParams[1].GetValue(levelRows[lagNum].tableRefRows.ToSingleEnum());
            }
            else
            {
                return "";
            }
        }
    }
}
