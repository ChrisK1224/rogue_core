using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RListItem : WebBaseControl
    {
        public void Text(String text) { this.attributes.Add(new Text(text)); }
        public void IDName(String idName){   this.attributes.Add(new NameID(idName)); }
        protected override string uiText { get { return "option "; } }
        public override string endTag { get { return "</option>"; } }
        public override string elementNM { get { return Elements.listitem; } }
    }
}
