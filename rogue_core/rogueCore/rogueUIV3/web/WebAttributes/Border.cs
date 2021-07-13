using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class Border : UIWebAttribute, IStyleAttribute
    {
        public Border(String border) { Value = border; }
        public override String uiText { get { return "border"; } }
        public override string elementNM { get { return Attributes.border; } }
    }
}
