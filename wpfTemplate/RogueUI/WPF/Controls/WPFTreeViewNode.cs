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
    class WPFTreeViewNode :TreeViewItem, IUIControl
    {
        //public ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParentRelationships parentRelation { get; set; }
        StackPanel myHeader = new StackPanel();
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            myHeader.Orientation = Orientation.Horizontal;
            Header = myHeader;
            if (thsElement is WPFUIAttribute)
            {
                ((WPFUIAttribute)thsElement).SetAttribute((FrameworkElement)this);
                return thsElement;
            }
            else
            {
                if (thsElement.parentRelation == ParentRelationships.header)
                {
                    myHeader.Children.Add((UIElement)thsElement);
                }
                else
                {
                    Items.Add(thsElement);
                }
                return thsElement;
            }
        }
    
    }
}
