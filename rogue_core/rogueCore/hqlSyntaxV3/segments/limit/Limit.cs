using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.limit
{
    public class Limit
    {
        public const String splitKey = "LIMIT";
        public Boolean isSet { get; protected set; }
       // protected String segment { get; private set; }
        string origTxt { get; }
        public int limitRows { get; protected set; } = -1;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public Limit(String limitSegment)
        {
            try
            {
                origTxt = limitSegment;
                if (limitSegment != "")
                {
                    isSet = true;
                    limitRows = int.Parse(limitSegment);
                }
                else
                {
                    isSet = false;
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if (isSet)
            {
                syntaxCommands.GetLabel(parentRow, "&nbsp;" + splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue);
                syntaxCommands.GetLabel(parentRow, limitRows.ToString(), IntellsenseDecor.MyColors.black);
            }
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
    }
}
