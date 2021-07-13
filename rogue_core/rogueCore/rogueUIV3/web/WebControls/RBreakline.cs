using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RBreakline : WebBaseControl
    {
        protected override String uiText { get { return "br / "; } }
        public override string endTag { get { return ""; } }
        public override string elementNM { get { return Elements.breakline; } }
    }
}
