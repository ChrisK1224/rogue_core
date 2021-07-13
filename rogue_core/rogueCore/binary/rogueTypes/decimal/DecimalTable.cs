using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.id;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes.dec
{
    public class DecimalTable : RogueTypeTable<DecimalValue>
    {
        protected override Encoding encodingType { get { return Encoding.UTF8; } }
        public DecimalTable() : base("decimal") { }
        protected override ISimpleValueReference NewReadValue(string line)
        {
            return new DecimalValue(line);
        }
        protected override ISimpleValueReference NewWriteValue(RowID rowID, string value)
        {
            return new DecimalValue(rowID, value);
        }
    }
}
