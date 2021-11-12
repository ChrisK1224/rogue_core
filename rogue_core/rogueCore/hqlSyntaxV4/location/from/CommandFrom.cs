using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from.command;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    public abstract class CommandFrom : CommandLocation, IIdableFrom
    {
        public string idName { get { return base.name; } }
        public bool IsIdable { get { return true; } }
        public string upperTableRefName { get { return base.name.ToUpper(); } }
        //public string defaultName => throw new NotImplementedException();
        public abstract string commandNameID { get; }        
        //public string displayTableRefName => throw new NotImplementedException();
        public abstract IORecordID tableId { get; }
        public CommandFrom(string tableTxt, QueryMetaData metaData) : base(tableTxt, metaData) 
        {

        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause,IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IMultiRogueRow topRow in parentLvl.rows.TakeWhile(x => rowCount != limit.limitRows))
            {
                foreach (IReadOnlyRogueRow testRow in RunExecProcedure(RunProcedure, topRow))
                {
                    //**SHIT CODE to handle the situation of multiple matching with join all since it needs the parent row to make the child row so when matchingto all rows you get dups
                    if (joinClause is JoinToClause)
                    {
                        //**This was set to JoinAllClause so might need to change somewhere
                        if (whereClause.CheckWhereClause(idName, testRow, topRow))
                        {
                            yield return NewRow(idName, testRow, topRow);
                            rowCount++;
                        }                        
                    }
                    else
                    {
                        foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                        {
                            if (whereClause.CheckWhereClause(idName, testRow, parentRow))
                            {
                                yield return NewRow(idName, testRow, parentRow);
                                rowCount++;
                            }
                        }
                    }
                    
                   
                }
            }
        }
        public static IFrom GetCommandTableType(string commandName, string fullTxt, QueryMetaData metaData)
        {
            switch (commandName)
            {
                case FromDateRange.commandNameIDConst:
                    return new FromDateRange(fullTxt, metaData);
                case FromRunApi.commandNameIDConst:
                    return new FromRunApi(fullTxt, metaData);
                case RunMLCommand.commandNameIDConst:
                    return new RunMLCommand(fullTxt, metaData);
                case RowToColumn.commandNameIDConst:
                    return new RowToColumn(fullTxt, metaData);
                case "UI_TABLE":
                    return new UIRow(fullTxt, metaData);
                default:
                    throw new Exception("Unknown table command");
            }
        }
        protected abstract IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow);
        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT READY");
        }
    }
}
