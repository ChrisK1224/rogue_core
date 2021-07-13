﻿using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using wpfTemplate.RogueUI.WPF.Attributes;
using static rogueCore.UI.UISegmentBuilder;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    class WPFTableCell : TableCell, IUIControl
    {
        //public ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParentRelationships parentRelation { get; set; }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is WPFUIAttribute)
            {
                ((WPFUIAttribute)thsElement).SetAttribute((TextElement)this);
                return thsElement;
            }
            else
            {
                this.Blocks.Add((Block)thsElement);
                return thsElement;
            }
        }
    }
}
