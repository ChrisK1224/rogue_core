using files_and_folders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.delete
{
    public class HQLDelete : BaseLocation, IIdableFrom, IFrom
    {    
        public string idName { get; }
        protected ICalcableFromId tableFrom { get; }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.AsKey }; } }
        public IORecordID tableId { get { return ((IIdableFrom)tableFrom).tableId; } }
        Dictionary<IORecordID, IRogueTable> writeTables = new Dictionary<IORecordID, IRogueTable>();
        public HQLDelete(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            tableFrom = ((ICalcableFromId)HQLTable.ParseFromClause(splitList.Where(x => x.Key == KeyNames.startKey).FirstOrDefault(), metaData));
            if (tableFrom is StandardFrom)
            {
                idName = (GetAliasName() == "") ? ((StandardFrom)tableFrom).idName : GetAliasName();
            }
            else
            {
                idName = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            }
        }
        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IMultiRogueRow parentRow in parentLvl.rows)
            {
                var writeTable = tableFrom.CalcTableID(parentRow).ToTable();
                writeTables.FindAddIfNotFound(writeTable.ioItemID, writeTable);
                List<IReadOnlyRogueRow> deletedRows = new List<IReadOnlyRogueRow>();
                foreach (IRogueRow thsRow in writeTable.StreamDataRows())
                {
                    foreach (IMultiRogueRow joinParentRow in joinClause.JoinRows(parentLvl, thsRow, snapshotRowAmount))
                    {
                        if (whereClause.CheckWhereClause(idName, thsRow, joinParentRow))
                        {
                            rowCount++;
                            yield return NewRow(idName, thsRow, joinParentRow);
                            deletedRows.Add(thsRow);
                        }
                        else
                        {
                            writeTable.AddWriteRow(thsRow);
                        }
                    }                    
                }
                writeTable.DeleteRewrite(deletedRows);
            }
        }
    }
}
