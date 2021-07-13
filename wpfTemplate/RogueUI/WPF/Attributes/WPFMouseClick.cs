using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFMouseClick : WPFUIAttribute
    {
        public WPFMouseClick(String click) { Value = click; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            Control btnDoubleClick = (Control)thsElement;
            //btnDoubleClick.MouseDoubleClick += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.insert, (UISegmentQuery)thsQry, btnDoubleClick.Tag.ToString()); };
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
