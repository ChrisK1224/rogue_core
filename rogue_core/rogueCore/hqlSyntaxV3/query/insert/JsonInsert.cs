using rogue_core.rogueCore.hqlSyntaxV3.query.insert;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.apiV3.formats.json;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using rogue_core.rogueCore.binary;

namespace rogue_core.rogueCore.hqlSyntaxV3.query
{
    class JsonInsert : CommandLocation, IInsertCommand
    {
        List<ILocationColumn> parameters = new List<ILocationColumn>();
        public JsonInsert(string txt) : base(txt)
        {
            foreach(string thsParam in commandParams)
            {
                parameters.Add(BaseLocation.LocationType(thsParam));
            }
        }
        public IReadOnlyRogueRow Execute(IMultiRogueRow row, IORecordID recordID)
        {
            var jsonInsert = new jsonTester(row.GetValue(parameters[0]), row.GetValue(parameters[1]), recordID.ToString());
            return jsonInsert.topRow;
        }
        public void PreFill(QueryMetaData metaData, string defaultName)
        {
            foreach(var thsParam in parameters)
            {
                thsParam.PreFill(metaData, defaultName);
            }
        }
        public List<string> UnsetParams()
        {
            List<string> final = new List<string>();
            foreach (var thsParam in parameters)
            {
                final.AddRange(thsParam.UnsetParams());
            }
            return final;
        }
    }
}
