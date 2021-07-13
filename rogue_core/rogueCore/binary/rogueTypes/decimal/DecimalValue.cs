using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes.dec
{
    public class DecimalValue : ISimpleValueReference
    {
        public long valueID { get; private set; }
        public string value { get; private set; }
        public int complexWordCount { get { return 0; } }
        public byte dataTypeID { get { return BinaryDataPair.dtDecimal; } }
        public DecimalValue(string line)
        {
            var split = line.Split(RogueTypeTable.valueSeparatorChar);
            this.valueID = new UnKnownID(split[0]);
            this.value = split[1];
        }
        public DecimalValue(RowID rowID, string value)
        {
            this.valueID = rowID.ToInt();
            this.value = value;
        }
        public string WriteValue()
        {
            return valueID.ToString() + RogueTypeTable.valueSeparatorChar + value;
        }
        public string StringValue(ComplexWordTable complexTbl) { return value; }
    }
}
