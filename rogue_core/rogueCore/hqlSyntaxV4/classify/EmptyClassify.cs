using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.classify
{
    class EmptyClassify : SplitSegment, IClassify
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public EmptyClassify(string txt, QueryMetaData metaData) : base(txt, metaData) {  }
        public override string PrintDetails()
        {
            return "";
        }
        public void ClassifyRow(IMultiRogueRow row) { }
    }
}
