using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using System;
using System.Collections.Generic;
using rogueCore.hqlSyntaxV3.segments.where;
using rogueCore.hqlSyntaxV3.segments.limit;
using rogueCore.hqlSyntaxV3.segments;
using System.Linq;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogueCore.hqlSyntaxV3.segments.table;
using static rogueCore.hqlSyntaxV3.segments.join.JoinClause;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;

namespace rogueCore.hqlSyntaxV3.table
{
    abstract class CoreTableStatement : ITableStatement, IFrom       
    {
        protected string[] keys { get; } = new string[3] { JoinClause.splitKey, WhereClause.splitKey, Limit.splitKey };
        public IFrom fromInfo { get { return this; } }
        public String parentTableRefName { get { return joinClause.parentTableRef; } }
        public IJoinClause joinClause { get; protected set; }
        protected WhereClauses whereClauses { get; set; }
        internal Func<string, IReadOnlyRogueRow, IMultiRogueRow, bool> WhereClauseCheck { get { return whereClauses.CheckRow; } }
        public List<ILocationColumn> IndexedWhereColumns { get { return whereClauses.evalColumns.Where(iCol => iCol.isConstant == false).ToList(); } }
        protected Limit limit { get; set; }
        public abstract string tableRefName { get; }
        public abstract IORecordID tableID { get; }
        protected ComplexWordTable complexWordTable { get; }
        protected Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts { private get; set; }
        string origTxt { get;}
        public abstract string displayTableRefName { get; }
        public CoreTableStatement(string colTxt) 
        {
            try
            {
                var segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, colTxt, keys).segmentItems;
                InitializeFrom(segmentItems[MutliSegmentEnum.firstEntrySymbol]);
                joinClause = this.ParseJoinClause(segmentItems.GetValue(JoinClause.splitKey));
                whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey));
                limit = new Limit(segmentItems.GetValue(Limit.splitKey));
                //***Fix this shit from constant table that doesn't have complexwordtable
                if(fromInfo.tableID != 0)
                {
                    complexWordTable = new ComplexWordTable(fromInfo.tableID);
                }                
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public virtual List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(joinClause.UnsetParams());
            unsets.AddRange(whereClauses.UnsetParams());
            return unsets;
        }
        public virtual void PreFill(QueryMetaData metaData)
        {
            try
            {
                joinClause.PreFill(metaData, tableRefName);
                whereClauses.PreFill(metaData, tableRefName);
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        protected abstract void InitializeFrom(string txt);
        //IJoinClause ParseJoinClause(string joinTxt)
        //{
        //    joinTxt = joinTxt.Trim();
        //    if (joinTxt.Contains("*"))
        //    {
        //        return new JoinAllClause(joinTxt);
        //    }
        //    else if (joinTxt.Equals(""))
        //    {
        //        return new EmptyJoinClause();
        //    }
        //    else if (joinTxt.StartsWith(JoinTypes.to.GetStringValue()))
        //    {
        //        return new JoinToClause(joinTxt);
        //    }
        //    else
        //    {
        //        return new JoinClause(joinTxt);
        //    }
        //}
        protected virtual IEnumerable<IReadOnlyRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            return tableID.ToTable().StreamDataRows();
        }
        protected abstract void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //LoadFromSyntax(parentRow);
            //*JOIN CLAUSE
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinClause.joinType.ToString(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;*&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;=&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, joinClause.parentTableRef, IntellsenseDecor.MyColors.black);
            //joinClause.LoadSyntaxParts(parentRow);
            //whereClauses.LoadSyntaxParts(parentRow);
        }
        protected void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LoadFromSyntax(parentRow, syntaxCommands);
            //*JOIN CLAUSE
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinClause.joinType.ToString(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;*&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;=&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //syntaxCommands.GetLabel(parentRow, joinClause.parentTableRef, IntellsenseDecor.MyColors.black);
            joinClause.LoadSyntaxParts(parentRow, syntaxCommands);
            whereClauses.LoadSyntaxParts(parentRow, syntaxCommands);
            limit.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //LoadFromSyntax(parentRow);
            ////*JOIN CLAUSE
            ////syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinClause.joinType.ToString(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            ////syntaxCommands.GetLabel(parentRow, "&nbsp;*&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            ////syntaxCommands.GetLabel(parentRow, "&nbsp;=&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            ////syntaxCommands.GetLabel(parentRow, joinClause.parentTableRef, IntellsenseDecor.MyColors.black);
            //joinClause.LoadSyntaxParts(parentRow);
            //whereClauses.LoadSyntaxParts(parentRow);
        }
        public abstract IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow);
    }    
}
