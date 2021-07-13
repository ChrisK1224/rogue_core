using rogue_core.rogueCore.hql.segments.columnSegment;
using rogue_core.rogueCore.row;
using System;
using FilesAndFolders;
using files_and_folders;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.binary;

namespace rogue_core.rogueCore.hql.segments.selects
{
    class ConstantValue : ILocationColumn
    {
        String value;
        public String tableRefName {get; set;}
        public ColumnRowID columnRowID { get; set; } = -1012;

        public ConstantValue(String value, String tableRefName)
        {
            this.value = value;
            this.tableRefName = tableRefName;
        }
        public ConstantValue(String snippet)
        {
            this.value = snippet.TrimFirstAndLastChar();
        }
        public string CalcStringValue(IRogueRow thsRow)
        {
            return value;
        }

        public string GetHQLText()
        {
            return "\"" + value + "\"";
        }
        public string GetHumanHQLText()
        {
            return "\"" + value + "\"";
        }
    }
}
