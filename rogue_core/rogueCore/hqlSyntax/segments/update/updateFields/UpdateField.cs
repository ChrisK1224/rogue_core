using rogue_core.rogueCore.id.rogueID;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.update.updateFields
{
    //public abstract class UpdateField : MultiSymbolSegment<StringMyList, string>, ISplitSegment
    public class UpdateField
    {
        protected string[] keys {get;} = new string[1] {"="};
        public LocationColumn setColumn {get;protected set;}
        public string setValue {get;protected set;}
        public UpdateField(string txt, HQLMetaData metaData)
        {
            var segmentItems = new MultiSymbolSegmentNew<StringMyList, string>(SymbolOrder.symbolbefore, txt, keys, metaData).segmentItems;
            setColumn = new LocationColumn(segmentItems[MutliSegmentEnum.firstEntrySymbol], metaData);
            //setValue = segmentItems["="];
            setValue = segmentItems["="].Substring(1, segmentItems["="].Length - 2);
        }
        internal UpdateField(ColumnRowID columnRowID, string updateValue) : base()
        {
            setColumn = new LocationColumn(columnRowID);
            setValue = updateValue;
        }
    }
}