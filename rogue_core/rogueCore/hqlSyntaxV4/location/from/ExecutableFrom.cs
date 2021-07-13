using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    //*SHOULD PROBABLY NOT BE BASE Table statement
    class ExecutableFrom : SplitSegment, IIdableFrom
    {
        public bool IsIdable { get { return false; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public ExecutableFrom(string tableTxt, QueryMetaData metaData) : base(tableTxt, metaData)
        {

        }
        public string idName => throw new NotImplementedException();
        public IJoinClause joinClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WhereClause whereClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILimit limit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string defaultName => throw new NotImplementedException();
        public IORecordID tableId => throw new NotImplementedException();

        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT READY");
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
