using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.from;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    class StarLocation : SplitSegment, ILocation
    {
        public string columnName => throw new NotImplementedException();
        public ColumnRowID columnRowID { get { return -1012; } }
        public string colTableRefName => throw new NotImplementedException();
        public bool isConstant { get { return false; } }
        public bool isEncoded { get { return false; } }
        public bool isStar { get { return true; } }
        public string upperColTableRefName => throw new NotImplementedException();
        public string upperColumnName => throw new NotImplementedException();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public StarLocation(string txt, QueryMetaData metaData) : base(txt, metaData) { }
        public IEnumerable<string> UnsetParams()
        {
            if(1 ==2)
            {
                yield return null;
            }
        }
        public string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            throw new NotImplementedException();
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            var directNmLbl = syntaxCommands.GetLabel(parentRow, "&nbsp;\"*\"&nbsp;", IntellsenseDecor.MyColors.black);
        }
        public void PreFill(QueryMetaData metaData, string assumedTableName)
        {
            //LoadColumns();
            //colTableRefName = assumedTableName;
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
