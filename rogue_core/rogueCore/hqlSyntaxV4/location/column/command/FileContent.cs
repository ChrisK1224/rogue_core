using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class FileContent : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "FILE_CONTENT"; } }
        public string columnName { get { return name; } }
        public FileContent(string colTxt, QueryMetaData metaData)  : base(colTxt, metaData) { }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            return File.ReadAllText(commandParams[0].GetValue(rows));
        }
    }
}
