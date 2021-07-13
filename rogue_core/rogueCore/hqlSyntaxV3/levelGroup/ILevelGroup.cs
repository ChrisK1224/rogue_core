using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.group
{
    public interface ILevelGroup
    {
        void Fill();
        void PreFill(QueryMetaData metaData);
        void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        List<ILevelStatement> levelStatements { get; }
        List<string> UnsetParams();
    }
}
