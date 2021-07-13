using rogue_core.rogueCore.id.rogueID;
using System.Collections.Generic;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.update.updateFields
{
    //public abstract class UpdateFields : MultiSymbolSegment<PlainList<UpdateField>, UpdateField>
    public class UpdateFields
    {
        protected string[] keys { get; } = new string[1] { "," };
        PlainList<UpdateField> segmentItems;
        public UpdateFields(string txt, HQLMetaData metaData) 
        {
            segmentItems = new MultiSymbolSegment<PlainList<UpdateField>, UpdateField>(SymbolOrder.symbolafter, txt, keys,(x,y) => new UpdateField(x,y), metaData).segmentItems;
        }
        internal UpdateFields() { segmentItems = new PlainList<UpdateField>(); }
        internal void AddField(ColumnRowID changeCol, string newVal)
        {
            segmentItems.Add(new UpdateField(changeCol, newVal));
        }
        internal IEnumerable<UpdateField> Fields(){return segmentItems;}
    }
}