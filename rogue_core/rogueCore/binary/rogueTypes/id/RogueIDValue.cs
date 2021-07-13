using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.word.id
{
    struct RogueIDValue : IValueReference
    {
        public long valueID { get; }
        string value { get; }
        public byte dataTypeID { get { return BinaryDataPair.dtNumber; } }
        public int complexWordCount { get { return 0; } }
        public RogueIDValue(string val)
        {
            this.valueID = new UnKnownID(val);
            this.value = val;
        }
        public RogueIDValue(long id)
        {
            string idStr = id.ToString();
            this.valueID = new UnKnownID(idStr);
            this.value = idStr;
        }
        public string WriteValue()
        {
            return value;
        }
        public string StringValue(ComplexWordTable complexTbl) { return value; }
    }
}
