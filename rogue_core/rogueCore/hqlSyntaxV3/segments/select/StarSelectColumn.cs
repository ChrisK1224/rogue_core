using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    class StarSelectColumn : ISelectColOrStar
    {
        public List<ISelectColumn> generatedColumns { get; private set; } = new List<ISelectColumn>();
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        string origTxt;
        public StarSelectColumn(string colTxt)
        {
            try
            {
                origTxt = colTxt;
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void PreFill(QueryMetaData queryMetaData,string levelName)
        {
            try
            {
                foreach (IFrom from in queryMetaData.GetLevelByRefName(levelName).allTableStatements)
                {
                    foreach (ColumnRowID thsColID in BinaryDataTable.columnTable.AllColumnsPerTable(from.tableID).Select(x => x.rowID))
                    {
                        var genCol = new DirectSelectColumn(thsColID, from.tableRefName);
                        //genCol.PreFill(queryMetaData, levelName);
                        generatedColumns.Add(genCol);
                    }
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public List<string> UnsetParams() { return new List<string>(); }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;\"*\"&nbsp;", IntellsenseDecor.MyColors.black);
        }
    }
}
