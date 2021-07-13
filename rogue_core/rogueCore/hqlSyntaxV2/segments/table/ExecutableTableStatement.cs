using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.from;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.limit;
using rogueCore.hqlSyntaxV2.segments.where;
using rogueCore.rogueUIV2;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.table
{
    class ExecutableTableStatement : BaseTableStatement
    {
        //protected string[] keys { get; } = new string[3] { JoinClause.splitKey, WhereClause.splitKey, Limit.splitKey };
        public override IFrom fromInfo { get { return fromExec; }  }
        FromExecute fromExec;
        //public string tableRefName { get { return fromExec.tableRefName; } }
        //public JoinClause joinClause { get; private set; }
        public override Func<IFilledLevel, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow>, IEnumerable<MultiRogueRow>> FilterAndStreamRows { get; protected set; }
        //public bool isEncoded { get; } = false;
        //HQLMetaData metaData { get; }
        //public string origStatement { get; private set; }
        //public List<ILocationColumn> IndexedWhereColumns { get { return new List<ILocationColumn>(); } }
        public ExecutableTableStatement(string tablePortion, HQLMetaData metaData) : base(tablePortion, metaData)
        {
            FilterAndStreamRows = ExecFilterAndStreamRows;
            //this.metaData = metaData;
            //var segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, tablePortion, keys, metaData).segmentItems;
            //origStatement = tablePortion;
            //fromExec = new FromExecute(segmentItems[firstEntrySymbol], metaData);
            //FilterAndStreamRows = StandardFilterAndStreamRows;
            //metaData.currTableRefName = fromInfo.tableRefName;
            //joinClause = new JoinClause(segmentItems.GetValue(JoinClause.splitKey), metaData);
            //metaData.AddTableStatement(this);
        }
        protected override void SetFromInfo(string hqlTxt, HQLMetaData metaData)
        {
            fromExec = new FromExecute(segmentItems[firstEntrySymbol], metaData);
        }
        internal IEnumerable<MultiRogueRow> ExecFilterAndStreamRows(IFilledLevel parentLvl, Func<string, IRogueRow, MultiRogueRow, MultiRogueRow> NewRow)
        {
            foreach (MultiRogueRow parentRow in JoinAllFill(parentLvl, null, parentLvl.rows.Count))
            {
                object[] parameters = new object[2];
                parameters[0] = fromExec.execParams.ToArray();
                parameters[1] = parentRow;
                foreach (MultiRogueRow row in (List<MultiRogueRow>)CodeCaller.RunProcedure(fromExec.executableName, parameters))
                {
                    yield return row;
                    //yield return parentRow.ManualChildRow(row);
                }
            }
        }
    }
}
