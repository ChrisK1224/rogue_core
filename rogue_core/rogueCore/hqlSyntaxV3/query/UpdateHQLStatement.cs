using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.update.updateFields;
using rogueCore.hqlSyntaxV3.segments.where;
using rogueCore.hqlSyntaxV3.segments.from;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.hqlSyntaxV3.table;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.query;

namespace rogueCore.hqlSyntaxV3.segments.update
{
    public class UpdateHQLStatement : SelectHQLStatement, IHQLStatement
    {
        protected string[] keys { get { return new string[3] { "UPDATE", "SET", WhereClauses.splitKey }; } }
        protected ITableStatement tblInfo;
        protected WhereClauses whereClauses;
        protected UpdateFields updateFields;
        public UpdateHQLStatement(string txt)
        {
            var segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, txt, keys).segmentItems;
            tblInfo = LevelStatement.NewTableStatement(segmentItems["UPDATE"]);
            //queryMetaData.AddTable(tblInfo);
            //currTableRefName = fromInfo.tableRefName;
            updateFields = new UpdateFields(segmentItems["SET"], this);
            whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey));
            Execute();
        }
        /// <summary>
        /// This is for a direct update in code 
        /// </summary>
        /// <param name="tableID"></param>
        public UpdateHQLStatement(IORecordID tableID)
        {
            tblInfo = new DirectTable(tableID);
            updateFields = new UpdateFields();
            whereClauses = new WhereClauses();
        }
        public new void Execute()
        {
            StringBuilder fileRebuilder = new StringBuilder();
            IRogueTable writeTbl = tblInfo.tableID.ToTable();
            foreach (IRogueRow thsRow in tblInfo.tableID.ToTable().StreamDataRows())
            {
                if (whereClauses.CheckRow(tblInfo.tableRefName, thsRow))
                {
                    foreach (UpdateField thsField in updateFields.Fields())
                    {
                        thsRow.SetValue(thsField.setColumn.columnRowID, thsField.setValue);
                    }
                }
                writeTbl.AddWriteRow(thsRow);
            }
            writeTbl.UpdateRewrite();
        }
        public void AddWhereClause(ColumnRowID columnRowID, string value)
        {
            whereClauses.AddDirectWhereClause(columnRowID, value);
        }
        public void AddUpdateField(ColumnRowID columnRowID, string value)
        {
            updateFields.AddField(columnRowID, value);
        }
    }
    public static class UpdateStatementRunner
    {
        public static void RunUpdateStatement(string statement)
        {
             new UpdateHQLStatement(statement);
        }
    }
    
}