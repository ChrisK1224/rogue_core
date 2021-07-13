using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments;
using System;

namespace rogueCore.hqlSyntaxV3
{
    public abstract class OptionalSplitSegment : ISplitSegment
    {
        public Boolean isSet { get; protected set; }
        protected String segment { get;private set; }
        protected abstract void SetVariables(SelectHQLStatement metaData);
        protected OptionalSplitSegment(String segment, SelectHQLStatement metaData) 
        {
            this.segment = segment;
            if(segment != "")
            {
                isSet= true;
                SetVariables(metaData);
            }
            else
            {
                isSet = false;
            }
        }
        protected OptionalSplitSegment(){}
    }
    public interface ISplitSegment
    {
         
    }
}
