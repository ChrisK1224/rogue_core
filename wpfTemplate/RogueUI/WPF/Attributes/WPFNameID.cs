using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFNameID : WPFUIAttribute
    {
        public WPFNameID(String nameID) { Value = nameID; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            thsElement.Tag = Value;
        }

        public override void SetAttribute(TextElement thsElement)
        {
            thsElement.Tag = Value;
        }
    }
}
