using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class Float : UIWebAttribute, IStyleAttribute
    {
        public Float(String direction) { Value = direction; }
        public override String uiText { get { return "float"; } }
        public override string elementNM { get { return Attributes.orientationFloat; } }
    }
}

