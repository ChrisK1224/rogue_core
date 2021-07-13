using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using System.Collections.Generic;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.update.updateFields
{
    //public abstract class UpdateFields : MultiSymbolSegment<PlainList<UpdateField>, UpdateField>
    public class UpdateFields
    {
        protected string[] keys { get; } = new string[1] { "," };
        PlainList<UpdateField> segmentItems;
        public UpdateFields(string txt, SelectHQLStatement queryStatement) 
        {
            segmentItems = new MultiSymbolSegment<PlainList<UpdateField>, UpdateField>(SymbolOrder.symbolafter, txt, keys,(x) => new UpdateField(x)).segmentItems;
        }
        internal UpdateFields() { segmentItems = new PlainList<UpdateField>(); }
        internal void AddField(ColumnRowID changeCol, string newVal)
        {
            segmentItems.Add(new UpdateField(changeCol, newVal));
        }
        internal IEnumerable<UpdateField> Fields(){return segmentItems;}
    }
}