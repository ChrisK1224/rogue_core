using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RNavItem : WebBaseControl
    {
        protected override string uiText { get { return "li class=\"nav-item\"><a class=\"nav-link\" href=\"#\""; } }
        public override string endTag { get { return "</a></li>"; } }
        public override string elementNM { get { return Elements.navitem; } }
    }
}
