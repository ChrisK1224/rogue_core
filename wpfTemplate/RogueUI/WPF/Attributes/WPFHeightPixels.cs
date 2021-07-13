using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFHeightPixels : WPFUIAttribute
    {
        public WPFHeightPixels(String height) { Value = height; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            thsElement.Height = double.Parse(Value);
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
