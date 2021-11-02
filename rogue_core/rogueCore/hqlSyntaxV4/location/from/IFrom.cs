using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    public interface IFrom : ILocation
    {
        public string idName { get; }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow);
        //public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause);
    }
}