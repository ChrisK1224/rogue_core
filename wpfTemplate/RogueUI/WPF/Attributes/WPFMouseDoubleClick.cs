using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFMouseDoubleClick : WPFUIAttribute
    {
        public WPFMouseDoubleClick(String dblClick) { Value = dblClick; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            //btnDoubleClick.MouseDoubleClick += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.insert, (UISegmentQuery)thsQry, btnDoubleClick.Tag.ToString()); };
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
