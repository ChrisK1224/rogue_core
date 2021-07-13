using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{ 
    class TabSpace : WebBaseControl
    {
        protected override string uiText { get { return " &nbsp; "; } }
        public override string endTag { get { return ""; } }
        public override string elementNM { get { return Elements.tabspace; } }
    }
}
