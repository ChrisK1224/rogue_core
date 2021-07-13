using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using wpfTemplate.RogueUI.WPF.Controls;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFWidthPercent : WPFUIAttribute
    {
        public WPFWidthPercent(String width) { Value = width; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            //thsElement.Width = double.Parse(Value);
            if (thsElement is Grid)
            {
                Grid thsGrid = ((Grid)thsElement);
                thsGrid.ColumnDefinitions[thsGrid.ColumnDefinitions.Count - 1].Width = new GridLength(double.Parse(Value), GridUnitType.Star);
            }
        }
        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
