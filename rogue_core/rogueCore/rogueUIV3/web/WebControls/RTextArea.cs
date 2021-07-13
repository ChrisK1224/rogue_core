using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RTextArea : WebBaseControl
    {
        public void Text(String text) { this.attributes.Add(new Text(text)); }
        public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        protected override string uiText { get { return "textarea "; } }
        public override string endTag { get { return "</textarea>"; } }
        public override string elementNM { get { return Elements.textarea; } }
    }
}
