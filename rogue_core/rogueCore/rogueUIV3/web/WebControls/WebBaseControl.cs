using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static rogueCore.rogueUIV3.UISection;

namespace rogueCore.rogueUIV3.web.element
{
    public abstract class WebBaseControl : IUIControl
    {
        public List<UIWebAttribute> attributes { get; set; } = new List<UIWebAttribute>();
        protected abstract String uiText { get; }
        internal List<IUIControl> childControls { get; set; } = new List<IUIControl>();
        public abstract String endTag { get; }    
        public abstract string elementNM { get; }
        public ParentRelationships parentRelation { get; set; }
        protected UIWebAttribute ContentAttribute = null;
        protected List<UIWebAttribute> styleAttributes = new List<UIWebAttribute>();
        protected List<UIWebAttribute> classAttributes = new List<UIWebAttribute>();
        internal List<IUIControl> Header { get; set; } = new List<IUIControl>();
        public void SetHeader(IUIElement thsHeader) { Header.Add((IUIControl)thsHeader); }
        internal void SetAttribute(UIWebAttribute thsAtt)
        {
            if (thsAtt is IStyleAttribute)
            {
                styleAttributes.Add(thsAtt);
            } 
            else if (thsAtt is CssClass)
            {
                classAttributes.Add(thsAtt);
            } 
            else if (thsAtt is Text)
            {
                ContentAttribute = thsAtt;
            } 
            else
            {
                attributes.Add(thsAtt);
            }
        }
        internal virtual String StartTagText()
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
                buildText += ">" + ContentAttribute.Value;
            }
            else
            {
                buildText += ">";
            }
            return buildText;
        }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is UIWebAttribute)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                SetAttribute((UIWebAttribute)thsElement);
                stopwatch.Stop();
                var tim = stopwatch.ElapsedMilliseconds;
            }
            else
            {
                if (thsElement.parentRelation == UISection.ParentRelationships.child)
                {
                    childControls.Add((IUIControl)thsElement);
                }
                else
                {
                    SetHeader((IUIControl)thsElement);
                }
            }
            return thsElement;
        }
    }
}
