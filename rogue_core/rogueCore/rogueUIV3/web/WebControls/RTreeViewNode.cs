using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class RTreeViewNode : WebBaseControl
    {
        
        public void Text(String text)
        {
            this.attributes.Add(new Text(text));
        }
        public void IDName(String idName){ this.attributes.Add(new NameID(idName)); }
        public void MouseClick(String mouseClickEvent) { this.attributes.Add(new MouseClick(mouseClickEvent)); }
        public void MouseDoubleClick(String mouseDoubleEvent) { this.attributes.Add(new MouseDoubleClick(mouseDoubleEvent)); }
       
        protected override string uiText { get { return "li "; } }
        public override string endTag { get{ return "</ul></li>"; } }
        internal override String StartTagText()
        {
            String buildText = "<" + uiText;
            //Text thsText = null;
            foreach (UIWebAttribute thsAtt in attributes)
            {
                buildText += thsAtt.uiText;
            }
            buildText += ">";
            buildText += "<span class=\"caret\">";
            foreach (WebBaseControl thsHeader in Header)
            {
                buildText += thsHeader.StartTagText() + thsHeader.endTag;
            }
            buildText += "</span>";
            buildText += "<ul class=\"nested\">";
            return buildText;
        }
        public override string elementNM { get { return Elements.treeviewnode; } }
    }
}
