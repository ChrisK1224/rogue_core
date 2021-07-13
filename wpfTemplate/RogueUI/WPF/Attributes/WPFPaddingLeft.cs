using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFPaddingLeft : WPFUIAttribute
    {
        public WPFPaddingLeft(String paddingLeft) { Value = paddingLeft; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            //throw new NotImplementedException();
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
