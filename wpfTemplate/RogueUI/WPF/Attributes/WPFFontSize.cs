using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFFontSize : WPFUIAttribute
    {
        public WPFFontSize(String font) { Value = font; }

        public override void SetAttribute(FrameworkElement parentElement)
        {
            ((Control)parentElement).FontSize = double.Parse(Value);
        }

        public override void SetAttribute(TextElement thsElement)
        {
            thsElement.FontSize = double.Parse(Value);
        }
    }
}
