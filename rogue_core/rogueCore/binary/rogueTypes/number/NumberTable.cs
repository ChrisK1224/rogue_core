using rogue_core.rogueCore.binary.rogueTypes;
using rogue_core.rogueCore.binary.rogueTypes.number;
using rogue_core.rogueCore.id;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.word.number
{
    public class NumberTable : RogueTypeTable<NumberValue>
    {
        protected override Encoding encodingType { get { return Encoding.UTF8; } }
        public NumberTable() : base("number") { }
        protected override ISimpleValueReference NewReadValue(string line)
        {
            return new NumberValue(line);
        }
        protected override ISimpleValueReference NewWriteValue(RowID rowID, string value)
        {
            return new NumberValue(rowID, value);
        }
    }
}
