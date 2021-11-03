using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level
{
    public abstract class CommandLevel : CommandFrom, IHQLLevel
    {
        //public override List<SplitKey> splitKeys { get { return new List<SplitKey>() {y }; } }
        List<HQLTable> tables { get; } = new List<HQLTable>(); 
        //public abstract string commandNameID { get; }
        //public abstract IORecordID tableId { get; }
        public abstract List<IMultiRogueRow> filteredRows { get; set; }
        public string parentLvlName => throw new NotImplementedException();
        List<HQLTable> IHQLLevel.tables => throw new NotImplementedException();
        public SelectRow selectRow => throw new NotImplementedException();
        public List<IMultiRogueRow> rows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string dataSetName => throw new NotImplementedException();
        public Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>> indexedRows => throw new NotImplementedException();
        public CommandLevel(string groupTxt, QueryMetaData metaData) : base(groupTxt, metaData)
        {

        }
        public void Fill()
        {

        }
        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            throw new NotImplementedException();
        }

        public void AddChildLevel(IHQLLevel lvl)
        {
            throw new NotImplementedException();
        }
    }
}
