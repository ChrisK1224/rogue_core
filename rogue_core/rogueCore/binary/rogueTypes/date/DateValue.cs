using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;

namespace rogue_core.rogueCore.binary.rogueTypes.date
{
    public class DateValue : ISimpleValueReference
    {
        public long valueID { get; private set; }
        public string value { get; private set; }
        public int complexWordCount { get { return 0; } }
        public byte dataTypeID { get { return BinaryDataPair.dtDate; } }
        public DateValue(string line)
        {
            var split = line.Split(RogueTypeTable.valueSeparatorChar);
            this.valueID = new UnKnownID(split[0]);
            this.value = split[1];
        }
        public DateValue(RowID rowID, string value)
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