using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class WidthPercent : UIWebAttribute, IStyleAttribute
    {
        public WidthPercent(String widthPercent) { Value = widthPercent + "%"; }
        public override string uiText { get{ return "width"; } }
        public override string elementNM { get { return Attributes.widthpercent; } }
    }
}
