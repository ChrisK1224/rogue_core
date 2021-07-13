using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.id;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes.date
{
    public class DateTable : RogueTypeTable<DateValue>
    {
        protected override Encoding encodingType { get { return Encoding.UTF8; } }
        public DateTable() : base("date") { }
        protected override ISimpleValueReference NewReadValue(string line)
        {
            return new DateValue(line);
        }
        protected override ISimpleValueReference NewWriteValue(RowID rowID, string value)
        {
            return new DateValue(rowID, value);
        }
    }
}
