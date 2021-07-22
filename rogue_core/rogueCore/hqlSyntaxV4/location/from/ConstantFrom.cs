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
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    class ConstantFrom : ConstantLocation, IFrom
    {
        public string idName { get { return tableRefName.ToUpper(); } }
        public string tableRefName { get; }
        public bool IsIdable { get { return false; } }
        public ConstantFrom(string hql, QueryMetaData metaData) : base(hql, metaData) 
        {
            tableRefName = (GetAliasName() == "") ? constValue : GetAliasName();
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int snapshotRowAmount = parentLvl.rows.Count;
            IReadOnlyRogueRow testRow = new ManualBinaryRow();
            foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
            {
                if (whereClause.CheckWhereClause(tableRefName, testRow, parentRow))
                {
                    yield return NewRow(tableRefName, testRow, parentRow);
                }
            }
        }
    }
}
