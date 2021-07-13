using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments.limit
{
    public class Limit : OptionalSplitSegment
    {
        public const String splitKey = "LIMIT";
        public int limitRows { get; protected set; } = -1;
        public Limit(String limitSegment, HQLMetaData metaData) : base(limitSegment, metaData){ }
        protected override void SetVariables(HQLMetaData metaData)
        {
            limitRows = int.Parse(segment);
        }
    }
}
