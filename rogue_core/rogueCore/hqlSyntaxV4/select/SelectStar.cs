using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public class SelectStar : SplitSegment, ISelectColOrStar
    {
        public List<ISelectColumn> generatedColumns { get; }
        public override List<SplitKey> splitKeys => throw new NotImplementedException();
        public SelectStar(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
           
        }
        public IEnumerable<string> SyntaxSuggestions()
        {
            return new List<string>();
        }
        public override string PrintDetails()
        {
            return ", STARCOL;";
        }
    }
}
