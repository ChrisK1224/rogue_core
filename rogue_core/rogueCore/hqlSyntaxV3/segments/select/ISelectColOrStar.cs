using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    interface ISelectColOrStar
    {
        public List<ISelectColumn> generatedColumns { get; }
        public void PreFill(QueryMetaData metaData, string levelName);
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        public List<string> UnsetParams();
    }
}
