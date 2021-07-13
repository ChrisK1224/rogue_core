using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RListView : WebBaseControl
    {
        protected override string uiText { get { return "ul "; } }
        public override string endTag { get { return "</ul>"; } }
        public override string elementNM { get { return Elements.listview; } }
    }
}
