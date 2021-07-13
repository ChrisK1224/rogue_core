using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.table
{
    //public abstract class TableGroups : MultiSymbolSegment<PlainList<TableGroup>, TableGroup>
    public class TableGroups
    {
        internal string splitter { get { return "|"; } }
        protected string[] keys { get { return new string[1] { splitter }; } }
        internal List<TableGroup> groups { get; private set; }
        public TableGroups(String fullTxt, HQLMetaData metaData) : base()
        {
            groups = new MultiSymbolSegmentNew<PlainList<TableGroup>, TableGroup>(SymbolOrder.symbolbefore, fullTxt, keys, metaData).segmentItems;
        }
    }
}