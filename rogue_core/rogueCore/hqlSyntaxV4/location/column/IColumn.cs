using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column
{
    public interface IColumn : ILocation, IPotentialParam, ICalcable
    {
        string columnName { get; }
        string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows);
        //String RetrieveStringValue(Dictionary<String, IReadOnlyRogueRow> rows);
        //ColumnRowID columnRowID { get; }
        //String colTableRefName { get; }
        //bool isConstant { get; }
        //void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
    }
    public static class ColExtender
    {
        public static string[] Splitters(this IColumn col)
        {
            return new string[1] { "." };
        }
        public static string ColSeparator(this IColumn col)
        {
            return ".";
        }
    }
    public interface IPotentialParam
    {
       // IEnumerable<string> UnsetParams();
    }
}
