using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class RNoWrap : UIWebAttribute, IStyleAttribute
    {
        public RNoWrap(String ignore) { Value =  "nowrap"; }
        public override String uiText { get { return "wrap:off;white-space"; } }
        public override string elementNM { get { return Attributes.nowrap; } }

    }
}
