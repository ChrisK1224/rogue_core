using rogue_core.rogueCore.UI.UIControls;
using rogueCore.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using wpfTemplate.RogueUI.WPF.Attributes;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    class WPFDisplayCell : ColumnDefinition, IUIControl
    {
        //public UISegmentBuilder.ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UISegmentBuilder.ParentRelationships parentRelation { get; set; }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is WPFUIAttribute)
            {
                //((WPFUIAttribute)thsElement).SetAttribute((FrameworkElement)this);
                return thsElement;
            }
            else
            {
                return null;
            }
        }
    }
}
