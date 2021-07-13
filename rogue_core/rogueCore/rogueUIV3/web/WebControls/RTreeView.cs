using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RTreeView : WebBaseControl
    {
        protected override string uiText { get { return "ul id=\"myUL\" "; } }
        public override string endTag { get { return "</ul>"; } }
        public override string elementNM { get { return Elements.treeview; } }
    }
}
