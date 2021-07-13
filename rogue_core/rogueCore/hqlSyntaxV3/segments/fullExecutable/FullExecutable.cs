using FilesAndFolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.fullExecutable
{
    class FullExecutable
    {
        internal string executableName;
        internal string paramQry;
        internal FullExecutable(string execSegment)
        {
            executableName = stringHelper.GetStringBetween(execSegment, "(", ",").Trim();
            paramQry = stringHelper.get_string_between_2(execSegment, "[", "]");
        }
    }
}
