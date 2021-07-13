using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class Underline : UIWebAttribute, IStyleAttribute
    {
        public Underline(String fontSize) { Value = "underline"; }
        public override String uiText { get { return "text-decoration"; } }
        public override string elementNM { get { return Attributes.underline; } }
    }
}
