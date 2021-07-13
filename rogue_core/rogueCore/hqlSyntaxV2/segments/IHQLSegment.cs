using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.split;
using rogueCore.rogueUIV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntaxV2.segments
{
    public interface IHQLSegment
    {
        public List<UIDecoratedTextItem> txtItems();
        public string origText { get; }
    }
    public static class HQLSegmentProc
    {
        //public static MultiRogueRow QueryUIResults(IHQLSegment hqlSegment, MultiRogueRow masterRow)
        //{
        //    var divRow = UISection.ManualUIElementRow(masterRow, "groupbox");
        //    var marginleftRow = UISection.ManualUIAttributeRow(divRow,  "margin-left", "25");
        //    var marginTopRow = UISection.ManualUIAttributeRow(divRow, "margin-top", "20");
        //    foreach(var txtLine in hqlSegment.txtItems())
        //    {
        //        var txtRow = UISection.ManualUIElementRow(divRow, "label", UISection.ParentRelationships.child);
        //        var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "font-weight", txtLine.boldFont);
        //        var colorRow = UISection.ManualUIAttributeRow(txtRow, "color", txtLine.textColor);
        //        var textRow = UISection.ManualUIAttributeRow(txtRow, "text", txtLine.splitKey);
        //    }
        //    return divRow;
        //}
        public static MultiRogueRow QueryUIResults(IHQLSegment hqlSegment, MultiRogueRow masterRow)
        {
            var divRow = UISection.ManualUIElementRow(masterRow, "groupbox");
            var marginleftRow = UISection.ManualUIAttributeRow(divRow, "margin-left", "25");
            var marginTopRow = UISection.ManualUIAttributeRow(divRow, "margintop", "20");
            var regPattern = MultiSymbolSegment<StringMyList, string>.GetOutsideQuotesPattern(hqlSegment.txtItems().Select(x => x.splitKey).ToArray());
            foreach (string txtLine in Regex.Split(hqlSegment.origText,regPattern))
            {
                var uiSeg = hqlSegment.txtItems().Where(x => x.splitKey == txtLine.Trim().ToUpper()).First();
                if(uiSeg == null)
                {
                    uiSeg = UIDecoratedTextItem.Default(txtLine);
                }
                var txtRow = UISection.ManualUIElementRow(divRow, "label", UISection.ParentRelationships.child);
                var fontweightRow = UISection.ManualUIAttributeRow(txtRow, "font-weight", uiSeg.boldFont);
                var colorRow = UISection.ManualUIAttributeRow(txtRow, "color", uiSeg.textColor);
                var textRow = UISection.ManualUIAttributeRow(txtRow, "text", uiSeg.splitKey);
            }
            return divRow;
        }
    }
    public class UIDecoratedTextItem
    {
        public string splitKey { get; }
        public string text { get { return splitKey + " &nbsp;"; } }
        public string textColor { get; }
        public string boldFont { get; }
        public string fontStyle { get; }
        public bool formatText { get; } = false;
        public bool breakLineAfter { get; } = false;
        public static UIDecoratedTextItem Default(string txt)
        {
            return new UIDecoratedTextItem(txt, "black", "normal", "normal", false);
        }
        public UIDecoratedTextItem(string text, string textColor, string boldFont, bool formatText = false, bool breakLineAfter = false) { this.splitKey = text;this.textColor = textColor; this.boldFont = boldFont;this.fontStyle = "normal"; this.formatText = formatText; this.breakLineAfter = breakLineAfter; }
        public UIDecoratedTextItem(string text, string textColor, string boldFont, string italic, bool breakLineAfter) { this.splitKey = text; this.textColor = textColor; this.boldFont = boldFont;this.fontStyle = italic; this.breakLineAfter = breakLineAfter; }
    }
}
