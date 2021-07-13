using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class ItemValue : UIWebAttribute
    {
        public ItemValue(string isSelected) { Value = isSelected; }
        public override string uiText { get { return " value=\"" + Value + "\" "; } }
        public override string elementNM { get { return Attributes.itemvalue; } }
    }
}