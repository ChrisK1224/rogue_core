using System;
using System.Collections.Generic;
using System.Text;
using static rogue_core.rogueCore.hqlSyntaxV4.IntellsenseDecor;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public interface ISyntaxPartCommands
    {
        public IMultiRogueRow GetLabel(IMultiRogueRow parentRow, string txt, MyColors myColor = MyColors.black, Boldness boldness = Boldness.none, FontSize fontSize = FontSize.regular, Underline isUnderlined = Underline.none);
        public IMultiRogueRow IndentedGroupBox(IMultiRogueRow parentRow, int levelNum);
        public void BreakLine(IMultiRogueRow parentRow);
    }
    public class SyntaxCommands : ISyntaxPartCommands
    {
        public IMultiRogueRow GetLabel(IMultiRogueRow parentRow, string txt, MyColors myColor = MyColors.black, Boldness boldness = Boldness.none, FontSize fontSize = FontSize.regular, Underline isUnderlined = Underline.none)
        {
            return IntellsenseDecor.MyLabel(parentRow, txt, myColor, boldness, fontSize, isUnderlined);
        }
        public IMultiRogueRow IndentedGroupBox(IMultiRogueRow parentRow, int levelNum)
        {
            return IntellsenseDecor.IndentedGroupbox(parentRow, levelNum);
        }
        public void BreakLine(IMultiRogueRow parentRow)
        {
            IntellsenseDecor.BreakLine(parentRow);
        }
    }
}
