using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RListViewItem : WebBaseControl
    {
        protected override string uiText { get { return "li "; } }
        public override string endTag { get { return "</li>"; } }
        public override string elementNM { get { return Elements.listviewitem; } }
    }
}
