using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class MouseClick : UIWebAttribute
    {
        public MouseClick(String mouseClickEvent) { Value = mouseClickEvent; }
        public override string uiText { get{ return "onclick=\"GenericClick('" + Value + "')\""; } }
        public override string elementNM { get { return Attributes.mouseclick; } }
    }
}
