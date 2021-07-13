using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.locationColumn;
using rogueCore.hqlSyntaxV3.segments.select;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.update.updateFields
{
    //public abstract class UpdateField : MultiSymbolSegment<StringMyList, string>, ISplitSegment
    public class UpdateField
    {
        protected string[] keys {get;} = new string[1] {"="};
        public ILocationColumn setColumn {get;protected set;}
        public string setValue {get;protected set;}
        public UpdateField(string txt)
        {
            var segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, txt, keys).segmentItems;
            setColumn = BaseLocation.LocationType(segmentItems[MutliSegmentEnum.firstEntrySymbol]);
            //setValue = segmentItems["="];
            setValue = segmentItems["="].Substring(1, segmentItems["="].Length - 2);
        }
        internal UpdateField(ColumnRowID columnRowID, string updateValue) : base()
        {
            setColumn = new UpdateDirect(columnRowID);
            setValue = updateValue;
        }
    }
}