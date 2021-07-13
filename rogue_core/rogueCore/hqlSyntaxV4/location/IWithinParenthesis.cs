using FilesAndFolders;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public interface IWithinParenthesis
    {
        
    }
    public static class WithinExtender
    {
        //public static string TransformParentesis(this IWithinParenthesis ths, string origTxt)
        //{
        //    string newString = origTxt;
        //    //*replace open command with a comma to split correctly
        //    int index = origTxt.IndexOf(CommandLocation.openCommand);
        //    newString = origTxt.Substring(0, index) + " " + BaseLocation.colSeparator + " " + origTxt.Substring(index + CommandLocation.openCommand.Length);
        //    //*Get rid of secondClose parenthesis
        //    newString = newString.ReplaceLastOccurrence(")", "");
        //    //*Replace close command with nothing. This is fine as last param will be end of the line or the AS will get split and it will still be the end
        //    //int indexEnd = newString.LastIndexOf(CommandLocation.closeCommand);
        //    //newString = newString.Substring(0, indexEnd) + newString.Substring(indexEnd + CommandLocation.closeCommand.Length);
        //    //newString += 
        //    return newString;
        //    //eturn origTxt.get_string_between_2(openCommand, closeCommand); 
        //}
    }
}
