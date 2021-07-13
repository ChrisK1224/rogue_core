using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class Text : UIWebAttribute
    {   
        public Text(String txt) {Value = txt; }
        public override string uiText { get { return " text=\"" + Value.Replace("\"", "&quot;"); } }
        public override string elementNM { get { return Attributes.text; } }
    }
}
