using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RTextBox : WebBaseControl
    {
        public void Text(String text){   this.attributes.Add(new Text(text)); }
        public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        protected override string uiText { get { return "input type=\"text\" "; } }
        public override string endTag { get{ return ""; } }
        internal override String StartTagText()
        {
            String buildText = "<" + uiText;
            foreach (UIWebAttribute thsAtt in attributes)
            {
                buildText += thsAtt.uiText;
            } 
            if (styleAttributes.Count > 0)
            {
                buildText += " style=\"";
                 foreach (UIWebAttribute thsAtt in styleAttributes)
                {
                    buildText += thsAtt.uiText + ":" + thsAtt.Value + ";";
                }
                buildText += "\"";
            }
            if (classAttributes.Count > 0)
            {
                buildText += " class=\"";
                foreach (UIWebAttribute thsAtt in classAttributes)
                {
                    buildText += thsAtt.Value + " ";
                }
                buildText += "\"";
            }
            if (ContentAttribute != null)
            {
                buildText +=  " value=\"" + ContentAttribute.Value + "\" >";
            }
            else
            {
                buildText += " />";
            }
            return buildText;
        }
        public override string elementNM { get { return Elements.textarea; } }
    }
}
