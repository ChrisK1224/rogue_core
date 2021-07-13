using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class REmptyTreeViewNode : RTreeViewNode
    {
        //public List<IUIControl> Header { get; set; }
        //public void Text(String text)
        //{
        //    this.attributes.Add(new Text(text));
        //}
        //public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        //public void MouseClick(String mouseClickEvent) { this.attributes.Add(new MouseClick(mouseClickEvent)); }
        //public void MouseDoubleClick(String mouseDoubleEvent) { this.attributes.Add(new MouseDoubleClick(mouseDoubleEvent)); }
        //public void SetHeader(IUIControl thsHeader) { Header.Add(thsHeader); }
        protected override string uiText { get { return "li "; } }
        public override string endTag { get { return "</li>"; } }
        internal override String StartTagText()
        {
            String buildText = "<" + uiText;
            //Text thsText = null;
            foreach (UIWebAttribute thsAtt in attributes)
            {
                buildText += thsAtt.uiText;
            }
            buildText += " >";
            foreach (IUIControl thsHeader in Header)
            {
                buildText += ((WebBaseControl)thsHeader).StartTagText() + ((WebBaseControl)thsHeader).endTag;
            }
            return buildText;
        }
        public override string elementNM { get { return Elements.emptytreeviewnode; } }
    }
}
