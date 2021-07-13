using rogue_core.rogueCore.UI.UIControls;
using rogueCore.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using wpfTemplate.RogueUI.WPF.Attributes;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    class WPFDisplayTable : Grid, IUIControl
    {
        int maxRows = -1;
    int maxCols = -1;
    public int currRowNum { get; private set; } = -1;
    public int currColNum { get; private set; } = -1;
    public UISegmentBuilder.ParentRelationships parentRelation { get; set; }

        public void AddRow()
        {
        currRowNum++;
        if (currRowNum > maxRows)
        {
            this.RowDefinitions.Add(new RowDefinition());
            maxRows++;
        }
        currColNum = -1;
        }
        public void AddColumn()
        {
        currColNum++;
        if (currColNum > maxCols)
        {
            this.ColumnDefinitions.Add(new ColumnDefinition());
            maxCols++;
        }
        }
        public void SetAttribute(string attName, string attVal)
        {
        //this.parentRelation = ParentRelationships.attribute;
        }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is WPFUIAttribute)
            {
                ((WPFUIAttribute)thsElement).SetAttribute(this);
                return thsElement;
            }
            else
            {
                //FrameworkElement element = (FrameworkElement)thsElement;
                if (thsElement is WPFDisplayRow)
                {
                    AddRow();
                    return this;
                }
                else if (thsElement is WPFDisplayCell)
                {
                    AddColumn();
                    return this;
                }
                else
                {
                    FrameworkElement element = (FrameworkElement)thsElement;
                    Grid.SetRow(element, currRowNum);
                    Grid.SetColumn(element, currColNum);
                    Children.Add(element);
                }
                return thsElement;
            }
        }
    }
}
