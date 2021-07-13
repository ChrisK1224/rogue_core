using rogue_core.rogueCore.id.rogueID;
using rogueCore.apiV3.formats.json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogueCore.hqlSyntaxV3;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;

namespace rogue_core.rogueCore.hqlSyntaxV4.insert
{
    class JsonInsert : HQLInsert // IHQLInsertType
    {
        public const string codeMatchName = "JSON_VALUE";
        public static string insertType { get { return codeMatchName; } }
        //public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { CommandSplitters.colSeparator, CommandSplitters.openCommand, CommandSplitters.closeCommand }; } }
        List<IColumn> insertParameters = new List<IColumn>();
        public JsonInsert(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            splitList.Where(x => x.Key == KeyNames.comma).ToList().ForEach(x => insertParameters.Add(BaseColumn.ParseColumn(x.Value, metaData)));
        }
        protected override IEnumerable<IReadOnlyRogueRow> Execute(IMultiRogueRow row, ICalcableFromId from)
        {
            var jsonInsert = new jsonTester(insertParameters[0].RetrieveStringValue(row.tableRefRows), insertParameters[1].RetrieveStringValue(row.tableRefRows), from.CalcTableID(row).ToString());
            yield return jsonInsert.topRow;
        }
        public void PreFill(QueryMetaData metaData, string defaultName)
        {
            throw new NotImplementedException();
        }
        public List<string> UnsetParams()
        {
            throw new NotImplementedException();
        }

        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }
    }
}
