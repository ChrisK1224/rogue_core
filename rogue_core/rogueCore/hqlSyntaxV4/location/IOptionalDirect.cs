using FilesAndFolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public interface IOptionalDirect
    {

    }
    public static class DirectExtender
    {
        public static bool IsDirectID(this IOptionalDirect dir, string txt)
        {
            return (txt.Contains("[") && txt.Contains("]")) ? true : false;
        }
        public static string GetDirectID(this IOptionalDirect dir, string txt)
        {
            return txt.get_string_between_2("[", "]");
        }
    }
}
