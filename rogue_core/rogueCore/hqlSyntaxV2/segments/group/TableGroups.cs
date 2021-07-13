using rogueCore.hqlSyntaxV2.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.table
{
    //public abstract class TableGroups : MultiSymbolSegment<PlainList<TableGroup>, TableGroup>
    public class TableGroups
    {
        string splitter { get { return "|"; } }
        protected string[] keys { get { return new string[1] { splitter }; } }
        internal List<TableGroupStatement> groups { get; private set; }
        public TableGroups(String fullTxt, HQLMetaData metaData) : base()
        {
            groups = new MultiSymbolSegment<PlainList<TableGroupStatement>, TableGroupStatement>(SymbolOrder.symbolbefore, fullTxt, keys, (x,y) => new FilledGroup(x, y), metaData).segmentItems;
        }
    }
}