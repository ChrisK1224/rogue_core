using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.id;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes.emoji
{
    public class EmojiTable : RogueTypeTable<EmojiValue>
    {
        protected override Encoding encodingType { get { return Encoding.UTF8; } }
        public EmojiTable() : base("emoji") { }
        protected override ISimpleValueReference NewReadValue(string line)
        {
            return new EmojiValue(line);
        }
        protected override ISimpleValueReference NewWriteValue(RowID rowID, string value)
        {
            return new EmojiValue(rowID, value);
        }
    }
}
