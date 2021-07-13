using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.binary
{
    class ManualBinaryRow : IReadOnlyRogueRow
    {
        readonly Dictionary<ColumnRowID, string> pairs = new Dictionary<ColumnRowID, string>();
        public RowID rowID { get { return new UnKnownID(0); } }
        public ManualBinaryRow()
        {

        }
        public string GetValueByColumn(ColumnRowID thsCol)
        {
            return pairs[thsCol];
        }
        public string ITryGetValueByColumn(ColumnRowID colRowID)
        {
            string val;
            return pairs.TryGetValue(colRowID, out val) ? val : "";
        }
        public void AddPair(ColumnRowID colId, string value)
        {
            pairs.Add(colId, value);
        }
        public IEnumerable<KeyValuePair<ColumnRowID, string>> GetPairs()
        {
            foreach(var pair in pairs)
            {
                yield return pair;
            }
        }
    }
}
