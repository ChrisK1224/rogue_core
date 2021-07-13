using rogue_core.rogueCore.binary.rogueTypes;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.word
{
    public interface IValueReference : IGenericValue
    {       
        Int32 complexWordCount { get; }
    }
    public interface ISimpleValueReference : IValueReference
    {
        string value { get; }
        string WriteValue();
    }
    public interface IGenericValue
    {
        byte dataTypeID { get; }
        long valueID { get; }
        string StringValue(ComplexWordTable complexTbl);
    }
    static class ValueRefExtensions
    {
        public static void WriteValueReference(this IGenericValue thisVal, IORecordID tableID, long rowID,long colID, long position, int complexWordIndex)
        {
            switch (thisVal.dataTypeID)
            {
                case BinaryDataPair.dtSimpleString:
                    BinaryDataTable.simpleTable.WriteValueReference(thisVal.valueID, tableID, rowID, colID, position, complexWordIndex);
                    break;
                case BinaryDataPair.dtNumber:
                    BinaryDataTable.numberTable.WriteValueReference(thisVal.valueID, tableID, rowID, colID, position, complexWordIndex);
                    break;
                case BinaryDataPair.dtDecimal:
                    BinaryDataTable.decimalTable.WriteValueReference(thisVal.valueID, tableID, rowID, colID, position, complexWordIndex);
                    break;
                case BinaryDataPair.dtEmoji:
                    BinaryDataTable.emojiTable.WriteValueReference(thisVal.valueID, tableID, rowID, colID, position, complexWordIndex);
                    break;
                case BinaryDataPair.dtDate:
                    BinaryDataTable.dateTable.WriteValueReference(thisVal.valueID, tableID, rowID, colID, position, complexWordIndex);
                    break;
                default:
                    throw new Exception("Unkown dataTypeID: " + thisVal.dataTypeID.ToString() + " from after writing row ValueRefExtensions.WriteValueReference");
            }
        }
    }
}
