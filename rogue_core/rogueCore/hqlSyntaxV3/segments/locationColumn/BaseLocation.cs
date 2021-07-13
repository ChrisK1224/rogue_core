using hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using System;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn
{
    class BaseLocation
    {
        protected string name { get; private set; }
        //**Can namedlocation be added here so all columns can be named?? Or do some not have a named option????
        public static ILocationColumn LocationType (string locationTxt)
        {
            if(locationTxt.Contains("{"))
            {
                return new EncodedColumn(locationTxt);
            }
            else if (locationTxt.StartsWith("\""))
            {
                return new ConstantColumn(locationTxt);
            }
            else if (locationTxt.ToUpper().StartsWith("EXECUTE("))
            {
                throw new Exception("No code for executeColumn");
            }
            else if (new NamedLocation(locationTxt).remainingTxt.EndsWith(")"))
            {
                return ColumnCommand.GetCommandColumn(locationTxt);
            }
            //else if(locationTxt.StartsWith("(") && locationTxt.EndsWith(")"))
            //{
            //    return new CommandLocation(locationTxt, queryStatement);
            //}
            else if (locationTxt.Equals("*"))
            {
                return new StarColumn();
            }
            else
            {
                return new StandardColumn(locationTxt);
            }
        }
        public static string GetValue(IRogueRow thsRow, ColumnRowID columnRowID)
        {
            if (thsRow.ITryGetValue(columnRowID) != null)
            {
                return thsRow.GetValueByColumn(columnRowID);
                //return thsRow.IGetBasePair(columnRowID).DisplayValue();
            }
            else
            {
                return "";
            }
        }
    }
}
