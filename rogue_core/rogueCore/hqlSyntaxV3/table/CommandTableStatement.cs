using hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.table.commandTables;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.hqlSyntaxV3.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.table
{
    abstract class CommandTableStatement : CoreTableStatement, ITableStatement, IFrom
    {
        protected CommandLocation commandLocation { get; private set; }
        public bool isEncoded { get { return false; } }
        public override string tableRefName { get { return commandLocation.name; } }
        public string upperTableRefName { get { return commandLocation.upperName; } }
        public abstract string commandNameID { get; }
        protected List<ILocationColumn> parameters { get; private set; } = new List<ILocationColumn>();
        protected abstract IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow);
        //abstract public override IORecordID tableID { get { return 2076411; } }
        //public string execName;
        public override string displayTableRefName => throw new NotImplementedException();
        public CommandTableStatement(string tableTxt) : base(tableTxt)
        {
            //execName = commandLocation.upperName;
            //commandLocation = new CommandLocation(tableTxt);
            foreach (string thsParam in commandLocation.commandParams)
            {
                parameters.Add(BaseLocation.LocationType(thsParam));
            }
        }
        //public  IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow);
        //{
        //    int rowCount = 0;
        //    int snapshotRowAmount = parentLvl.rows.Count;
        //    foreach (IMultiRogueRow topRow in parentLvl.rows)
        //    {
        //        foreach (IReadOnlyRogueRow testRow in commandLocation.RunExecProcedure(topRow).TakeWhile(x => rowCount != limit.limitRows))
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
            //foreach (IMultiRogueRow parentRow in parentLvl.rows)
            //{
            //    foreach (IMultiRogueRow calcRow in commandLocation.RunExecProcedure(parentRow))
            //    {
            //        yield return calcRow;
            //    }
            //}
        //}
        protected override void InitializeFrom(string txt)
        {
            commandLocation = new CommandLocation(txt);
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            commandLocation.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IMultiRogueRow topRow in parentLvl.rows)
            {
                foreach (IReadOnlyRogueRow testRow in commandLocation.RunExecProcedure(RunProcedure, topRow).TakeWhile(x => rowCount != limit.limitRows))
                {
                    //**SHIT CODE to handle the situation of multiple matching with join all since it needs the parent row to make the child row so when matchingto all rows you get dups
                    if (joinClause.joinAll)
                    {
                        if (WhereClauseCheck(tableRefName, testRow, topRow))
                        {
                            yield return NewRow(tableRefName, testRow, topRow);
                        }                        
                    }
                    else
                    {
                        foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                        {
                            if (WhereClauseCheck(tableRefName, testRow, parentRow))
                            {
                                yield return NewRow(tableRefName, testRow, parentRow);
                            }
                        }
                    }
                    
                    rowCount++;
                }
            }
        }
        public override void PreFill(QueryMetaData metaData)
        {
            foreach(var thsParam in parameters)
            {
                thsParam.PreFill(metaData, tableRefName);
            }
            base.PreFill(metaData);
        }
        public static ITableStatement GetCommandTableType(string commandName, string fullTxt)
        {
            switch (commandName)
            {
                case DateRange.commandNameIDConst:
                    return new DateRange(fullTxt);
                case RunApi.commandNameIDConst:
                    return new RunApi(fullTxt);
                default:
                    throw new Exception("Unknown table command");
            }
        }
    }
}
