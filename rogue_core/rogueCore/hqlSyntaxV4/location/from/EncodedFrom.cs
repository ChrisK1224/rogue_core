using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV4.join;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    class EncodedFrom : EncodedLocation<IORecordID>, IFrom, ICalcableFromId
    {
        public bool IsIdable { get { return false; } }
        public string idName { get { return base.name.ToUpper(); } }
        Dictionary<IORecordID, List<IReadOnlyRogueRow>> indexedEncodedRows = new Dictionary<IORecordID, List<IReadOnlyRogueRow>>();
        public EncodedFrom(string tableTxt, QueryMetaData metaData) : base(tableTxt, metaData)
        {
            //encodedTableLocation = new EncodedTableLocation(tableTxt, queryStatement);
            //EncodedToTableStream = (encodedColumn.isDirectID) ? EncodedToTableStream = EncodedDirectToTableID : EncodedNonDirectToTableID;
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            List<IMultiRogueRow> encodedParentRows = new List<IMultiRogueRow>(parentLvl.rows);
            foreach (IMultiRogueRow parentRow in encodedParentRows)
            {
                foreach (IRogueRow testRow in StreamIRows(parentRow).TakeWhile(x => rowCount != limit.limitRows))
                {
                    if (whereClause.CheckWhereClause(idName, testRow, parentRow))
                    {
                        yield return NewRow(idName, testRow, parentRow);
                    }
                    rowCount++;
                }
            }
        }
        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT READY");
        }
        //**CONFUSING the indexedRows just saves the rows from a table so that the table only has to be read once. Purely for speed.**
        protected IEnumerable<IReadOnlyRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            IORecordID encodedTblID = CalcTableID(parentRow);
            if (indexedEncodedRows.ContainsKey(encodedTblID))
            {
                foreach (var row in indexedEncodedRows[encodedTblID])
                {
                    yield return row;
                }
            }
            else
            {
                indexedEncodedRows.Add(encodedTblID, new List<IReadOnlyRogueRow>());
                foreach (var row in encodedTblID.ToTable().StreamDataRows())
                {
                    yield return row;
                    indexedEncodedRows[encodedTblID].Add(row);
                }
            }
        }
        public IORecordID CalcTableID(IMultiRogueRow row)
        {
            string ColOrID = row.GetValue(encodedColumn);
            bool isDirect = (this.IsDirectID(ColOrID));
            EncodedIDPull = (isDirect) ? EncodedIDPull = DirectToID : NameToID;
            return EncodedIDPull(ColOrID);
        }
        protected override IORecordID NameToID(string ids)
        {
            return BinaryDataTable.ioRecordTable.DecodeTableName(ids);
        }
        protected override IORecordID DirectToID(string directID)
        {
            return new IORecordID(directID);
        }
        public override string PrintDetails()
        {
            return "idName:" + idName;
        }
    }
}
