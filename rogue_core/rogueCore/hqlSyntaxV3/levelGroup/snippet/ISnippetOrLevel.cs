using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System.Collections.Generic;

namespace rogueCore.hqlSyntaxV3.segments.snippet
{
    internal interface ISnippetOrLevel
    {
        void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        public List<ILevelStatement> levelStatements { get; }
        public List<string> UnsetParams();
    }
}