using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments.select
{
    public class ConstLocationColumn : ILocationColumn
    {
        protected String constValue {private get; set;}
        public ColumnRowID columnRowID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string colTableRefName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        internal ConstLocationColumn(){}
        internal ConstLocationColumn(bool directValue, string value){ constValue = value;}
        public ConstLocationColumn(String constTxt)
        {
            constValue = constTxt.Substring(1, constTxt.Length - 2);
        }
        public string CalcStringValue(IRogueRow thsRow)
        {
            return constValue;
        }
        public string CalcStringValue(Dictionary<string, IRogueRow> rows)
        {
            return constValue;
        }
    }
}
