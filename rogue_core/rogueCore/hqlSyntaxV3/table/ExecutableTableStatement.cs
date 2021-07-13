using FilesAndFolders;
using hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.table;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.table
{
    //*SHOULD PROBABLY NOT BE BASE Table statement
    class ExecutableTableStatement : CoreTableStatement, ITableStatement, IFrom
    {
        ExecutableLocation executableLocation;
        public bool isEncoded { get { return false; } }
        public override string tableRefName { get { return executableLocation.upperName; } }
        public string upperTableRefName { get { return executableLocation.upperName; } }
        public override IORecordID tableID{ get { return -1012; } }
        public override string displayTableRefName { get { return executableLocation.name; } }
        public ExecutableTableStatement(string tableTxt) : base(tableTxt)
        {
            executableLocation = new ExecutableLocation(tableTxt);
            //var items = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, tableTxt, new string[1] { From.splitTableName }, queryStatement).segmentItems;
            //executableName = stringHelper.GetStringBetween(tableTxt, "(", ",").Trim();
            //tableRefName = items[From.splitTableName];
            //string paramPortion = tableTxt.Substring(tableTxt.IndexOf(",") + 1, tableTxt.Length - tableTxt.IndexOf(",") - 1);
            //paramPortion = paramPortion.Substring(0, paramPortion.LastIndexOf(")"));
            //execParams = new MultiSymbolString<PlainList<string>>(SymbolOrder.symbolbefore, paramPortion, new string[1] { From.splitTableName }, queryStatement).segmentItems;
            //tableID = -1011;
        }
        public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            foreach (IMultiRogueRow parentRow in parentLvl.rows)
            {
                foreach(IMultiRogueRow calcRow in executableLocation.RunExecProcedure(parentRow))
                {
                    yield return calcRow;
                }
            }
        }
        protected override void InitializeFrom(string txt)
        {
            executableLocation = new ExecutableLocation(txt);
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            executableLocation.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void PreFill(QueryMetaData metaData, string assumedTblNm)
        {
            executableLocation.PreFill(metaData, assumedTblNm);
        }
    }
}
