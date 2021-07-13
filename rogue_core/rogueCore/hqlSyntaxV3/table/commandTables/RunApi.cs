using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.apiV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.table.commandTables
{
    class RunApi : CommandTableStatement
    {
        public override string commandNameID { get { return commandNameIDConst; } }
        public const string commandNameIDConst = "RUN_API";

        public override IORecordID tableID { get { return 2076411; } }

        public RunApi(string tblTxt) : base(tblTxt)
        {

        }
       
        //public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        //{
        //    int rowCount = 0;
        //    int snapshotRowAmount = parentLvl.rows.Count;
        //    foreach (IMultiRogueRow topRow in parentLvl.rows)
        //    {
        //        foreach (IReadOnlyRogueRow testRow in commandLocation.RunExecProcedure(RunApiProcedure, topRow).TakeWhile(x => rowCount != limit.limitRows))
        //        {
        //            foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
        //            {
        //                if (WhereClauseCheck(tableRefName, testRow, parentRow))
        //                {
        //                    yield return NewRow(tableRefName, testRow, parentRow);
        //                }
        //            }
        //            rowCount++;
        //        }
        //    }
        //}
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            var apiRun = new APIConnect(parentRow);
            var newRow = new ManualBinaryRow();
            newRow.AddPair(2076417, apiRun.resultPath + "data.json");
            newRow.AddPair(2076445, apiRun.resultPath + "metadata.json");
            newRow.AddPair(2076472, apiRun.resultPath);
            newRow.AddPair(2094214, apiRun.segment_NM);
            newRow.AddPair(2094252, apiRun.database_ID);
            return new List<IReadOnlyRogueRow>() { newRow };
        }
    }
}
