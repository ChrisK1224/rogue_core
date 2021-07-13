using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.where
{
    interface IWhereClause
    {
        public ILocationColumn evalColumnRowID { get; }
        public Boolean IsValid(string thsTableRef, IReadOnlyRogueRow thsRow, IMultiRogueRow parentRow);
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        void PreFill(QueryMetaData metaData, string assumedTableNm);
        public List<string> UnsetParams();
    }

}
