using rogueCore.hqlSyntax.segments;
using rogueCore.hqlSyntax.segments.from.human;
using rogueCore.hqlSyntax.segments.join.human;
using rogueCore.hqlSyntax.segments.limit.human;
using rogueCore.hqlSyntax.segments.select;
using rogueCore.hqlSyntax.segments.select.human;
using rogueCore.hqlSyntax.segments.where.human;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntax
{
    public abstract class OptionalSplitSegment : ISplitSegment
    {
        public Boolean isSet { get; protected set; }
        protected String segment { get;private set; }

        protected abstract void SetVariables(HQLMetaData metaData);
        protected OptionalSplitSegment(String segment, HQLMetaData metaData) 
        {
            this.segment = segment;
            if(segment != "")
            {
                isSet= true;
                SetVariables(metaData);
            }
            else{
                isSet = false;
            }
        }
        protected OptionalSplitSegment(){}
    }
    public interface ISplitSegment
    {
         
    }
}
