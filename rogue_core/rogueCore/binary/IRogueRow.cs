using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;
using static rogue_core.rogueCore.binary.prefilled.ColumnTable;

namespace rogue_core.rogueCore.binary
{
    public interface IRogueRow : IReadOnlyRogueRow
    {
        IGenericValue ITryGetValue(ColumnRowID colRowID);        
        IEnumerable<IValueReference> GetDataTypeValList();
        void PrintRow();
        IGenericValue IGetBasePair(ColumnRowID colRowID);
        Dictionary<long, BinaryDataPair> pairs { get; }
        public byte[] WriteBytes();
        IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, String columnName, RowID value, byte dataTypeID);
        IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, String columnName, string value);
        IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, RowID value, byte dataTypeID, IORecordID parentTableID);
        IGenericValue NewWritePair(ColumnRowID colID, String colValue);
        IGenericValue NewWritePair(IORecordID ownerTblID, string colNM, String colValue);
        IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, string value, IORecordID parentTableID);
        void SetValue(ColumnRowID col, string value);
    }
    public interface IReadOnlyRogueRow
    {
        RowID rowID { get; }
        string GetValueByColumn(ColumnRowID thsCol);
        string ITryGetValueByColumn(ColumnRowID colRowID);
        IEnumerable<KeyValuePair<ColumnRowID, string>> GetPairs();
    }
}
