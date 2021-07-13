using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFText : WPFUIAttribute
    {
        public WPFText(String text) { Value = text; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            ((ContentControl)thsElement).Content = Value;
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
