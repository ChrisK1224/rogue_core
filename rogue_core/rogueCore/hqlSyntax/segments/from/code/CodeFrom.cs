using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;

namespace rogueCore.hqlSyntax.segments.from.code
{
    public class CodeFrom : From
    {
        public const string splitKey = "FROM";
       // protected override string[] keys { get { return new string[1] { " AS " }; } }
        public CodeFrom(IORecordID tableID) : base(tableID) { }
       // protected override string ItemParse(string txt) { return txt; }
    }
        
}