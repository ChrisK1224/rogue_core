using rogue_core.rogueCore.id.rogueID;

namespace rogueCore.hqlSyntax.segments.select.code
{
    public class CodeSelectColumn: SelectColumn
    {
        //protected override string columnAliasSeparator {get;} = " AS ";
        //protected override string columnConcatSymbol {get;} = "&";
        //protected override string[] keys { get { return new string[0] { }; } }
        public CodeSelectColumn(ILocationColumn column) : base(column) {}
      //  protected override string ItemParse(string txt) { return txt;}
    }
}