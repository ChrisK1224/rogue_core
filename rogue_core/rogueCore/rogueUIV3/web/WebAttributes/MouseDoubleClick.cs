using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class MouseDoubleClick : UIWebAttribute
    {
        public MouseDoubleClick(String mouseDoubleClickEvent) { Value = mouseDoubleClickEvent; }
        public override string uiText { get { return " ondblclick=\"GenericClick('" + Value + "')\""; } }
        public override string elementNM { get { return Attributes.mousedoubleclick; } }
    }
}
