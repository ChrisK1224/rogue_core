using rogue_core.RogueCode.hql.hqlSegments.table;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments.tableGroups
{
    class segmentGroups
    {
        Dictionary<String, TableSegment> segments = new Dictionary<string, TableSegment>();
        public enum groupTypes
        {
            standard, table
        }
    }
}
