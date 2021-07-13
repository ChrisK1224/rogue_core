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
        public bool IsIdable { get { return true; } }
        public bool isEncoded { get { return false; } }
        public string idName => throw new NotImplementedException();
        public IJoinClause joinClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WhereClause whereClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILimit limit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public string defaultName => throw new NotImplementedException();
        public IORecordID tableId => throw new NotImplementedException();
        internal InvertFrom(string hqlTxt, QueryMetaData metaData) : base(hqlTxt, metaData)
        {
            //this.queryStatement = queryStatement;
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            throw new NotImplementedException();
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

