using rogueCore.hqlSyntaxV3;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using FilesAndFolders;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.rogueUIV3.web;
using rogue_core.rogueCore.syntaxCommand;

namespace rogueCore.hqlSyntaxV3.segments.namedLocation
{
    class NamedLocation
    {
        public bool isNameSet { get; private set; }
        public string Name { get; private set; } = "";
        public string displayName;
        public string remainingTxt { get; private set; }
        const string columnAliasSep = " AS ";
        string origTxt { get; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        static string[] selectKeys { get { return new String[1] { columnAliasSep }; } }
        public NamedLocation(string txt)
        {
            origTxt = txt;
            try
            {
                var lst = StripAliasName(txt);
                remainingTxt = lst[0].Trim();
                if (lst.Count > 1)
                {
                    isNameSet = true;
                    displayName = lst[1].BeforeFirstSpace().ToUpper();
                    Name = displayName.ToUpper();
                }
                else
                {
                    isNameSet = false;
                }
                LocalSyntaxParts = StandardSyntaxParts; 
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        static List<string> StripAliasName(string colTxt)
        {
            return new MultiSymbolString<PlainList<string>>(SymbolOrder.symbolbefore, colTxt, selectKeys).segmentItems;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //List<IMultiRogueRow> retRows = new List<IMultiRogueRow>();
            //if (isNameSet)
            //{
            //    syntaxCommands.GetLabel(parentRow, "&nbsp;" + columnAliasSep + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    syntaxCommands.GetLabel(parentRow, Name);
            //}
            //return retRows;
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            //List<IMultiRogueRow> retRows = new List<IMultiRogueRow>();
            if (isNameSet)
            {
                syntaxCommands.GetLabel(parentRow, "&nbsp;" + columnAliasSep + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
                syntaxCommands.GetLabel(parentRow, Name);
            }
            //return retRows;
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //List<IMultiRogueRow> retRows = new List<IMultiRogueRow>();
            //if (isNameSet)
            //{
            //    syntaxCommands.GetLabel(parentRow, "&nbsp;" + columnAliasSep + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    syntaxCommands.GetLabel(parentRow, Name);
            //}
            //return retRows;
        }
    }
}
