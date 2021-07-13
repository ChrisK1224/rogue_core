using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RLabel : WebBaseControl
    {
        public void Text(String text) { this.attributes.Add(new Text(text)); }
        public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        protected override string uiText { get { return "label "; } }
        public override string endTag { get { return "</label>"; } }
        public override string elementNM { get { return Elements.label; } }
        //public override String StartTagText()
        //{
        //    String buildText = "<" + uiText;
        //    Text thsText = null;
        //    foreach (IUIAttribute thsAtt in attributes)
        //    {
        //        if(thsAtt is Text)
        //        {
        //            thsText = (Text)thsAtt;
        //        }
        //        else
        //        {
        //            buildText += thsAtt.uiText;
        //        }
        //    }
        //    if(thsText != null)
        //    {
        //        return buildText += ">" + thsText.Value;
        //    }
        //    else
        //    {
        //        return buildText + ">";
        //    }
        //}
    }
}
