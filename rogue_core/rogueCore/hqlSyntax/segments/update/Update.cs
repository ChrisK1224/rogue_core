using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.table;
using rogueCore.hqlSyntax.segments.from;
using rogueCore.hqlSyntax.segments.from.human;
using rogueCore.hqlSyntax.segments.update.updateFields;
using rogueCore.hqlSyntax.segments.where;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.update
{
    //public abstract class UpdateStatement : MultiSymbolSegment<StringMyList, string>, ISplitSegment
    public class UpdateStatement
    {
        protected string[] keys { get { return new string[3] { "UPDATE", "SET", WhereClauses.splitKey }; } }
        protected From fromInfo;
        protected WhereClauses whereClauses;
        protected UpdateFields updateFields;
        public UpdateStatement(string txt)
        {
            HQLMetaData metaData = new HQLMetaData();
            var segmentItems = new MultiSymbolSegmentNew<StringMyList, string>(SymbolOrder.symbolbefore, txt, keys, metaData).segmentItems;
            fromInfo = new From(segmentItems["UPDATE"], metaData);
            //HumanHQLStatement.tableRefIDs = new Dictionary<string, int>();
            //HumanHQLStatement.tableRefIDs.Add(fromInfo.tableRefName, fromInfo.tableID);
            //HumanHQLStatement.currTableRefName = fromInfo.tableRefName;
            metaData.tableRefIDs.Add(fromInfo.tableRefName, fromInfo.tableID);
            metaData.currTableRefName = fromInfo.tableRefName;
            updateFields = new UpdateFields(segmentItems["SET"], metaData);
            whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey), metaData);
            ExecuteUpdate();
        }
        /// <summary>
        /// This is for a direct update in code 
        /// </summary>
        /// <param name="tableID"></param>
        public UpdateStatement(IORecordID tableID)
        {
            fromInfo = new From(tableID);
            updateFields = new UpdateFields();
            //updateFields = codeUpdateFields;
            whereClauses = new WhereClauses();
            //whereClauses = codeWheres;
        }
        public void ExecuteUpdate()
        {
            StringBuilder fileRebuilder = new StringBuilder();
            IRogueTable writeTbl = fromInfo.tableID.ToTable();
            foreach (IRogueRow thsRow in writeTbl.StreamIRows())
            {
                if (whereClauses.CheckRow(fromInfo.tableRefName, thsRow))
                {
                    foreach (UpdateField thsField in updateFields.Fields())
                    {
                        thsRow.SetValue(thsField.setColumn.columnRowID, thsField.setValue);
                    }
                }
                writeTbl.AddWriteRow(thsRow);
            }
            writeTbl.EraseAndRewrite();
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
             new UpdateStatement(statement);
        }
    }
}