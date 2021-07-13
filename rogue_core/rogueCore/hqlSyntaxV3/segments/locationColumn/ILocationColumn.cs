using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments
{
    public interface ILocationColumn : IPotentialParam
    {
        //String CalcStringValue(IRogueRow thsRow);
        //Func<Dictionary<string, IRogueRow>, string> DelCalcStringValue;
        string columnName { get; }
        
        //string upperColumnName { get; }
        String RetrieveStringValue(Dictionary<String, IReadOnlyRogueRow> rows);
        //String RetrieveStringValue(IRogueRow row);
        ColumnRowID columnRowID { get; }
        String colTableRefName { get; }
        //string upperColTableRefName { get; }
        bool isConstant { get; }
        void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        public void PreFill(QueryMetaData metaData, string assumedTblName);
        //void SecondaryLoad(string thsLevelName, Dictionary<string, IFrom> tableList);
        //bool isEncoded { get; }
        //bool isStar { get; }
    }
    public interface IPotentialParam
    {
        IEnumerable<string> UnsetParams();
    }
}
