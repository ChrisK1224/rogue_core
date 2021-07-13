using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using wpfTemplate.RogueUI.WPF.Attributes;
using static rogueCore.UI.UISegmentBuilder;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    class WPFDisplayRow : RowDefinition, IUIControl
    {
        //public ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParentRelationships parentRelation { get; set; }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is WPFUIAttribute)
            {
                //((WPFUIAttribute)thsElement).SetAttribute((FrameworkElement)this);
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
