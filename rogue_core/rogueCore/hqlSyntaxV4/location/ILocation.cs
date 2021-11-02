using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public interface ILocation : IPotentialParam, ITempBase
    {
        string PrintDetails();
        //string columnName { get; }        
        //String RetrieveStringValue(Dictionary<String, IReadOnlyRogueRow> rows);
        //ColumnRowID columnRowID { get; }
        //String colTableRefName { get; }
        //bool isConstant { get; }
        //void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
    }
    public static class ColExtender
    {
        
    }
    public interface IPotentialParam
    {
       // IEnumerable<string> UnsetParams();
    }
}
