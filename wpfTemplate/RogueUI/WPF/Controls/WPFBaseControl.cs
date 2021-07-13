using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using wpfTemplate.RogueUI.WPF.Attributes;
using static rogueCore.UI.UISegmentBuilder;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    abstract class WPFBaseControl : FrameworkElement, IUIControl
    {
        public ParentRelationships parentRelation { get; set; }
        //public void SetAttribute(string attName, string attVal)
        //{
        //    //throw new NotImplementedException();
        //}
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if(thsElement is WPFUIAttribute)
            {
                ((WPFUIAttribute)thsElement).SetAttribute((FrameworkElement)this);
                return thsElement;
            }
            else
            {
                return SetChildControl(thsElement);
            }
        }
        protected abstract IUIElement SetChildControl(IUIElement thsElement);
    }
}
