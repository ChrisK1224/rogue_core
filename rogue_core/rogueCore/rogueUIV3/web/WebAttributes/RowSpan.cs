using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class RowSpan : UIWebAttribute
    {
        public RowSpan(String rowSpan) { Value = rowSpan; }
        public override string uiText { get{ return " rowspan=\"" + Value + "\""; } }
        public override string elementNM { get { return Attributes.rowspan; } }
    }
}
