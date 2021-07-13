using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    class DirectFrom : IIdableFrom
    {       
        public IORecordID tableId { get; private set; }
        public string idName => throw new NotImplementedException();
        public IJoinClause joinClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WhereClause whereClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILimit limit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string[] splitters => throw new NotImplementedException();
        public string defaultName => throw new NotImplementedException();
        public QueryMetaData queryData => throw new NotImplementedException();
        public string origTxt => throw new NotImplementedException();

        public bool IsIdable { get { return true; } }

        public DirectFrom(IORecordID tableID)
        {
            //this.tableID = tableID;
            //this.joinClause = new EmptyJoinClause();
            //tableRefName = tableID.TableName();
            //displayTableRefName = tableRefName;
            //parentTableRefName = "";
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT READY");
        }
        public string PrintDetails()
        {
            return "idName:"+ idName + "," + "tableID:" + tableId.ToString();
        }
    }
}
