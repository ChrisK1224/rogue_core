using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.limit
{
    public class Limit : SplitSegment,ILimit
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { }; } }
        public int limitRows { get; protected set; } = -1;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public Limit(String limitSegment, QueryMetaData metaData) : base(limitSegment, metaData)
        {
            try
            {
                limitRows = int.Parse(limitSegment.AfterFirstSpace());               
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                //LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
           // syntaxCommands.GetLabel(parentRow, "&nbsp;" + splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue);
            syntaxCommands.GetLabel(parentRow, limitRows.ToString(), IntellsenseDecor.MyColors.black);            
        }
        public override string PrintDetails()
        {
            return "";
        }
        public IEnumerable<string> SyntaxSuggestions()
        {
            return new List<string>();
        }
    }
}
