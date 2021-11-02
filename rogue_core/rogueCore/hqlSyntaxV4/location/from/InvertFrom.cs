using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV4.join;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    class InvertFrom : SplitSegment, IIdableFrom
    {
        public string idName => throw new NotImplementedException();       
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public IORecordID tableId => throw new NotImplementedException();
        internal InvertFrom(string hqlTxt, QueryMetaData metaData) : base(hqlTxt, metaData)
        {
            //this.queryStatement = queryStatement;
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            throw new Exception("NO FINDISHED");
            //foreach (IRogueRow thsRow in parentRow.InvertRow(invertSelectRow, columns, complexWordTable))
            //{
            //    yield return thsRow;
            //}
        }
        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT READY");
        }
        public enum ConversionTypes { rowtocolumn, hierarchytable, hierarchyheader }
        public override string PrintDetails()
        {
            return "";
        }
    }
}

