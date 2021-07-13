using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using wpfTemplate.RogueUI.WPF.Attributes;
using static rogueCore.UI.UISegmentBuilder;

namespace wpfTemplate.RogueUI.WPF.Controls
{
    class WPFTable : FlowDocumentReader, IUIControl
    {
        //public ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParentRelationships parentRelation { get; set; }
        FlowDocumentReader topFlow = new FlowDocumentReader();
        FlowDocument doc = new FlowDocument();
        WPFRealTable tbl = new WPFRealTable();
        TableRowGroup rowGroup = new TableRowGroup();
        public WPFTable()
        {
            this.Document = doc;
            doc.Blocks.Add(tbl);
            tbl.RowGroups.Add(rowGroup);
        }
        public IUIElement SetChildContent(IUIElement thsElement)
        {
            if (thsElement is WPFUIAttribute)
            {
                ((WPFUIAttribute)thsElement).SetAttribute(tbl);
                return thsElement;
            }
            else if(thsElement is WPFTableRow)
            {
                rowGroup.Rows.Add((WPFTableRow)thsElement);
                return thsElement;
            }
            else
            {
                return thsElement;
            }
        }
    }
    class WPFRealTable : Table, IUIControl
    {
        public ParentRelationships parentRelation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IUIElement SetChildContent(IUIElement thsElement)
        {
            throw new NotImplementedException();
        }
    }
}
