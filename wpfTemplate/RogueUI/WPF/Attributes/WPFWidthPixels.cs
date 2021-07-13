using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFWidthPixels : WPFUIAttribute
    {
        public WPFWidthPixels (String width) { Value = width; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            thsElement.Width = double.Parse(Value);
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
