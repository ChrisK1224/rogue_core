using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    class ConstantLocation : BaseLocation, ILocation
    {
        protected string constValue { get; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public ConstantLocation(string colTxt, QueryMetaData metaData)  : base(colTxt, metaData)
        {
            try
            {
                constValue = splitList[0].Value.TrimFirstAndLastChar();
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
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if (constValue.StartsWith("@"))
            {
                syntaxCommands.GetLabel(parentRow, "\"" + constValue + "\"", IntellsenseDecor.MyColors.yellow);
            }
            else
            {
                syntaxCommands.GetLabel(parentRow, "\"" + constValue + "\"", IntellsenseDecor.MyColors.green);
            }
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            var directNmLbl = syntaxCommands.GetLabel(parentRow, "", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //aliasName.LoadSyntaxParts(parentRow);
        }
        public IEnumerable<string> UnsetParams()
        {
            if (constValue.Trim().Contains("@"))
            {
                yield return constValue;
            }
        }

        public override string PrintDetails()
        {
            return "constValue:" + constValue;
        }
    }
}
