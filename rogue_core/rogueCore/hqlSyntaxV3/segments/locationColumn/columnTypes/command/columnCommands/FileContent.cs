using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class FileContent : ColumnCommand
    {
        public static string CodeMatchName { get { return "FILE_CONTENT"; } }
        public FileContent(string colTxt) : base(colTxt)
        {
            
        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            return File.ReadAllText(paramColumns[0].GetValue(rows));
        }
    }
}
