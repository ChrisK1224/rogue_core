using rogue_core.rogueCore.hqlSyntaxV4.select;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public interface ICommandLocation
    {
        string commandTxt { get; }
        SelectRow selectRow { get; set; }
    }
    static class commandExtender
    { 
        public static void Initialize(this ICommandLocation comLoc, QueryMetaData metaData)
        {
            string colTxt = FilesAndFolders.stringHelper.get_string_between_2(comLoc.commandTxt, "(", ")");
            comLoc.selectRow = new SelectRow(colTxt, metaData);
        }
    }

}
