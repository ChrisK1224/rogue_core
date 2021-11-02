using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using files_and_folders;

namespace rogue_core.rogueCore.hqlSyntaxV4.update
{
    public class HQLUpdate : SplitSegment, IFrom, IIdableFrom
    {
        Dictionary<ColumnRowID, string> updateFields { get; } = new Dictionary<ColumnRowID, string>();
        protected ICalcableFromId tableFrom { get; }
        public IORecordID tableId { get { return ((IIdableFrom)tableFrom).tableId; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public string idName { get; }
        //* TODO HQLINsert shouldn't have tableID avaialbe or cant be encoded***
        Dictionary<IORecordID, IRogueTable> writeTables = new Dictionary<IORecordID, IRogueTable>();
        public HQLUpdate(string txt, QueryMetaData metaData) : base(txt, metaData) 
        {
            
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            //**UNTESTED 
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach(IMultiRogueRow parentRow in parentLvl.rows)
            {
                var writeTable = tableFrom.CalcTableID(parentRow).ToTable();
                writeTables.FindAddIfNotFound(writeTable.ioItemID, writeTable);
                foreach (IRogueRow thsRow in writeTable.StreamDataRows())
                {
                    foreach (IMultiRogueRow joinParentRow in joinClause.JoinRows(parentLvl, thsRow, snapshotRowAmount))
                    {
                        if (whereClause.CheckWhereClause(idName, thsRow, joinParentRow))
                        {
                            rowCount++;
                            foreach (var thsField in updateFields)
                            {
                                thsRow.SetValue(thsField.Key, thsField.Value);
                            }
                            yield return NewRow(idName, thsRow, joinParentRow); 
                        }                        
                    }
                    writeTable.AddWriteRow(thsRow);
                }
                writeTable.UpdateRewrite();
            }
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
