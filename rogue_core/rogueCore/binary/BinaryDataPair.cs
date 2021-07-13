using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace rogue_core.rogueCore.binary
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct BinaryDataPair : IGenericValue
    {
        public Int16 rowCount;
        public byte dataTypeID { get; private set; }
        public Int64 colID;
        public Int64 valueID { get; private set; }
        public byte isEnd { get; private set; }
        const byte endRow = 1;
        const byte nonEndRow = 0;
        public const byte dtNumber = 1;
        public const byte dtSimpleString = 2;
        public const byte dtComplexString = 3;
        public const byte dtDate = 4;
        public const byte dtDecimal = 5;
        //public const byte dtRogueID = 6;
        //public const byte dtSepChar = 7;
        public const byte dtEmoji = 8;
        public bool IsRowEnd()
        {
            return (isEnd == 1) ? true : false;
        }
        /// <summary>
        /// This should be called when writing new values. Complex Word Table values will always be written here.
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colID"></param>
        /// <param name="complexFieldCount"></param>
        /// <param name="value"></param>
        /// <param name="complexWordTable"></param>
        public BinaryDataPair(Int16 rowCount, Int64 colID, string value, byte isEnd, ComplexWordTable complexWordTable)
        {
            this.rowCount = rowCount;
            this.colID = colID;
            IValueReference val = GetValueOID(value, complexWordTable);
            this.dataTypeID = val.dataTypeID;
            this.valueID = val.valueID;
            this.isEnd = isEnd; 
            //this.complexFieldCount = complexFieldCount;
        }
        /// <summary>
        /// This should be called when writing new values where you don't yet know the rowCount or if its the last pair in a row. Those values are set on WriteBytes method before writing.
        /// </summary>
        /// <param name="colID"></param>
        /// <param name="value"></param>
        /// <param name="complexWordTable"></param>
        public BinaryDataPair(Int64 colID, string value, ComplexWordTable complexWordTable)
        {
            this.colID = colID;
            IValueReference val = GetValueOID(value, complexWordTable);
            this.dataTypeID = val.dataTypeID;
            this.valueID = val.valueID;
            this.isEnd = 0;
            this.rowCount = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colID"></param>
        /// <param name="valueID"></param>
        /// <param name="dataTypeID"></param>
        /// <param name="complexWordTable"></param>
        public BinaryDataPair(Int64 colID, long valueID,byte dataTypeID, ComplexWordTable complexWordTable)
        {
            this.colID = colID;
            this.dataTypeID = dataTypeID;
            //if (dataTypeID == dtRogueID)
            //{
            //    string bll = "DSF:";
            //}
            this.valueID = valueID;
            this.isEnd = 0;
            this.rowCount = 1;
        }
        public string StringValue(ComplexWordTable complexWordTable)
        {
            //*Unsure of this logic and if rowCount is used right
            return GetValueOID(dataTypeID, valueID, complexWordTable, this.rowCount).StringValue(complexWordTable);
        }
        //*This is for making update statements
        //public void ResetValue(string value,ComplexWordTable complexWordTable)
        //{
        //    IValueReference val = GetValueOID(value, complexWordTable);
        //    this.dataTypeID = val.dataTypeID;
        //    this.valueID = val.valueID;
        //}
        //*Bad code but needed from when t his is called from ComplexWordRow which will never need to use the complexWordTable
        public static IValueReference GetValueOID(string value, ComplexWordTable complexWordTable = null)
        {
            switch (GetDataType(value))
            {
                //case dtRogueID:
                //    return new RogueIDValue(value);
                case dtSimpleString:
                    return BinaryDataTable.simpleTable.GetValue(value);
                case dtComplexString:
                    return complexWordTable.NewValue(value);
                case dtNumber:
                    return BinaryDataTable.numberTable.GetValue(value);
                case dtEmoji:
                    return BinaryDataTable.emojiTable.GetValue(value);
                case dtDecimal:
                    return BinaryDataTable.decimalTable.GetValue(value);
                case dtDate:
                    return BinaryDataTable.dateTable.GetValue(value);
                default:
                    return BinaryDataTable.simpleTable.GetValue(value);
            }
        }
        public static IValueReference GetValueOID(byte dataType, long rowID, ComplexWordTable complexWordTable = null, int complexWordCount = 0)
        {
            switch (dataType)
            {
                case dtSimpleString:
                    return BinaryDataTable.simpleTable.GetValue(rowID);
                case dtComplexString:
                    return complexWordTable.GetValue(rowID);
                case dtNumber:
                    return BinaryDataTable.numberTable.GetValue(rowID);
                case dtDecimal:
                    return BinaryDataTable.decimalTable.GetValue(rowID);
                case dtEmoji:
                    return BinaryDataTable.emojiTable.GetValue(rowID);
                case dtDate:
                    return BinaryDataTable.dateTable.GetValue(rowID);
                default:
                    return BinaryDataTable.simpleTable.GetValue(rowID);
            }
        }
        public IEnumerable<IValueReference> GetReferenceList(ComplexWordTable complexWordTable)
        {            
            switch (this.dataTypeID)
                {
                case dtSimpleString:
                        yield return BinaryDataTable.simpleTable.GetValue(valueID);
                        break;
                case dtComplexString:
                    foreach(var valRef in complexWordTable.GetValue(valueID).values)
                    {
                        yield return GetValueOID(valRef.typ, valRef.value, complexWordTable);
                    }
                    break;
                case dtNumber:
                    yield return BinaryDataTable.numberTable.GetValue(valueID);
                    break;
                case dtDecimal:
                    yield return BinaryDataTable.decimalTable.GetValue(valueID);
                    break;
                case dtEmoji:
                    yield return BinaryDataTable.emojiTable.GetValue(valueID);
                    break;
                case dtDate:
                    yield return BinaryDataTable.dateTable.GetValue(valueID);
                    break;
                default:
                    yield return BinaryDataTable.simpleTable.GetValue(valueID);
                    break;
            }
        }
        public static byte GetDataType(string value)
        {
            long result;
            Decimal decTest;
            DateTime dateTest;
            if (long.TryParse(value, out result))
                return dtNumber;
            else if (Decimal.TryParse(value, out decTest))
                return dtDecimal;
            else if (value.Length == 1 && ComplexWordRow.complexSeparators.Contains(value[0]))
                return dtSimpleString;
            else if (DateTime.TryParse(value, out dateTest))
                return dtDate;
            else if (ComplexWordRow.IsSingleEmoji(value))
                return dtEmoji;
            else if (value.IndexOfAny(ComplexWordRow.complexSeparators) != -1 || ComplexWordRow.ContainsEmoji(value))
                return dtComplexString;
            else
                return dtSimpleString;
        }
        public void SetEnd()
        {
            this.isEnd = 1;
        }
        public void UnSetEnd()
        {
            this.isEnd = 0;
        }
    }
}
