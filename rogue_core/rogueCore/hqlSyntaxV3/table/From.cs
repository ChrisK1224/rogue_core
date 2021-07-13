using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rogueCore.hqlSyntaxV3.segments.from
{
    //*THISIS BROKEN NOW> SHOULD GET RID OF
    class From : Location<IORecordID>, IFrom
    {
        public const String splitKey = "FROM";
        public const String splitTableName = " AS ";
        public IORecordID tableID { get { return base.ID; } }
        public string tableRefName { get { return base.name; } }
        Func<IMultiRogueRow, IEnumerable<IRogueRow>> StreamType;
        Func<string, IORecordID> EncodedToTableStream;
        public From(string colTxt, SelectHQLStatement qry) : base(colTxt, qry) 
        {
            //StreamType = (isEncoded) ? StreamType = EncodedStreamIRows : StandardStreamIRows;
            //EncodedToTableStream = (isDirectID) ? EncodedToTableStream = EncodedDirectToTableID : EncodedNonDirectToTableID;
        }
        protected override IORecordID NameToID(string[] ids)
        {
            if(ids.Length == 1)
            {
                return new IORecordID(BinaryDataTable.ioRecordTable.GuessTableIDByName(ids[0]));
            }
            else 
            {
                return BinaryDataTable.ioRecordTable.DecodeTableName(ids);
            }
        }
        protected override IORecordID NameToID(string directID)
        {
            return new IORecordID(directID);
        }
        protected override string IDToName(IORecordID directID)
        {
            return directID.TableName();
        }
        public IEnumerable<IRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            return StreamType(parentRow);
        }
        IEnumerable<IReadOnlyRogueRow> StandardStreamIRows(IMultiRogueRow parentRow)
        {
            return tableID.ToTable().StreamDataRows();
        }
        //IEnumerable<IRogueRow> EncodedStreamIRows(IMultiRogueRow parentRow)
        //{
        //    string tableIdStr = encodedCol.RetrieveStringValue(parentRow.tableRefRows);
        //    IORecordID encodedTblID = EncodedToTableStream(tableIdStr);
        //    foreach (var row in encodedTblID.ToTable().StreamIRows())
        //    {
        //        yield return row;
        //    }
        //}
        IORecordID EncodedDirectToTableID(string val)
        {
            return new IORecordID(val);
        }
        IORecordID EncodedNonDirectToTableID(string val)
        {
            return NameToID(val.Split('.').ToArray());
        }
        protected override void SetNameAndID()
        {
            throw new NotImplementedException();
        }
    }
}
