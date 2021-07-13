using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes
{
    public abstract class RogueTypeTable<DataValueType> where DataValueType : ISimpleValueReference
    {
        static readonly string fileFolder = install.RootVariables.sharedDataPath + Path.DirectorySeparatorChar;
        Dictionary<Int64, int> readLookup = new Dictionary<Int64, int>();
        Dictionary<string, int> writeLookup = new Dictionary<string, int>();
        List<ISimpleValueReference> values = new List<ISimpleValueReference>();
        //abstract protected byte dataTypeID { get; }
        protected string filePath { get; }
        abstract protected Encoding encodingType { get; }
        ValueRefInstance valueRefInstance { get; }
       // Func<string, ISimpleValueReference> NewReadValue { get; }
       // Func<RowID, string, ISimpleValueReference> NewWriteValue { get; }
        public RogueTypeTable(string tableName)
        {
            filePath = fileFolder + tableName + ".rogueValue";
            //NewReadValue = SetReadValue();
            //NewWriteValue = SetWriteValue();
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            foreach (var row in StreamRows())
            {
                AddValue(row);
            }
            valueRefInstance = new ValueRefInstance(fileFolder + tableName + "_Instance.rogueInstance");
        }
        protected void AddValue(ISimpleValueReference word)
        {
            values.Add(word);
            readLookup.Add(word.valueID, values.Count - 1);
            //*Temp check for unrecognized char
            //if (!writeLookup.ContainsKey(word.value))
            //{
                writeLookup.Add(word.value, values.Count - 1);
            //}
            //else
            //{
            //    string l = word.value;
            //}
        }
        ISimpleValueReference WriteValue(string value)
        {
            DecodedRowID rowID = IDIncrement.NextID();           
            ISimpleValueReference val = NewWriteValue(rowID, value);
            File.AppendAllText(filePath, val.WriteValue() + RogueTypeTable.lineSeparatorChar, encodingType);
            values.Add(val);
            writeLookup.Add(value, values.Count - 1);
            readLookup.Add(val.valueID, values.Count - 1);
            return val;
        }
        public ISimpleValueReference GetValue(string strValue)
        {
            int foundID;
            //string checkVal = strValue.Replace(",", "@RCOMMA").Replace(":", "@ROGUECOLON").Replace(";", "@ROGUESEMICOLON").Replace(Environment.NewLine, "@RNEWLINE").Replace("|", "@RBAR").Replace("\n", "@RNEWLINE");
            if (writeLookup.TryGetValue(strValue, out foundID))
            {
                return values[foundID];
            }
            else
            {
                return WriteValue(strValue);                            
            }
        }
        public ISimpleValueReference GetValue(long rowID)
        {
            return values[readLookup[rowID]];
        }
        IEnumerable<ISimpleValueReference> StreamRows()
        {
            //string currLine = "";
            using (StreamReader sr = new StreamReader(filePath, encodingType))
            {
                foreach (var line in sr.ReadLines('~'))
                    yield return NewReadValue(line);
            }
        }
        protected abstract ISimpleValueReference NewReadValue(string line);
        protected abstract ISimpleValueReference NewWriteValue(RowID rowID, string value);
        public void WriteValueReference(long valueID, IORecordID tableId, long rowID, long colId, long rowPosition, int complexWordIndex)
        {
            valueRefInstance.WriteValueReference(valueID, tableId, colId, rowID, rowPosition, complexWordIndex);
        }
        //Func<string, ISimpleValueReference> SetReadValue()
        //{
        //    switch (dataTypeID)
        //    {
        //        case BinaryDataPair.dtSimpleString:
        //            return (x => new SimpleWordValue(x));
        //        case BinaryDataPair.dtNumber:
        //            return (x => new NumberValue(x));
        //        case BinaryDataPair.dtDecimal:
        //            return (x => new DecimalValue(x));
        //        case BinaryDataPair.dtDate:
        //            return (x => new DateValue(x));
        //        case BinaryDataPair.dtEmoji:
        //            return (x => new EmojiValue(x));
        //        default:
        //            throw new Exception("Unkown dataTypeID: " + dataTypeID.ToString());
        //    }
        //}
        //Func<RowID,string, ISimpleValueReference> SetWriteValue()
        //{
        //    switch (dataTypeID)
        //    {
        //        case BinaryDataPair.dtSimpleString:
        //            return ((x,y) => new SimpleWordValue(x,y));
        //        case BinaryDataPair.dtNumber:
        //            return ((x,y) => new NumberValue(x,y));
        //        default:
        //            throw new Exception("Unkown dataTypeID: " + dataTypeID.ToString());
        //    }
        //}
    }
    static class RogueTypeTable
    {
        internal const char lineSeparatorChar = '~';
        internal const char valueSeparatorChar = '|';
        public static IEnumerable<string> ReadLines(this TextReader reader, char delimiter)
        {
            List<char> chars = new List<char>();
            while (reader.Peek() >= 0)
            {
                char c = (char)reader.Read();

                if (c == delimiter)
                {
                    yield return new String(chars.ToArray());
                    chars.Clear();
                    continue;
                }
                chars.Add(c);
            }
        }
    }
}
