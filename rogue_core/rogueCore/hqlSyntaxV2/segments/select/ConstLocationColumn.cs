using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments.select
{
    public class ConstLocationColumn : ILocationColumn
    {
        protected String constValue { private get; set; }
        public ColumnRowID columnRowID { get { return new ColumnRowID(-1012); } set => throw new NotImplementedException(); }
        public string colTableRefName { get; set; }
        public bool isConstant { get{return true;} }
        internal ConstLocationColumn(){}
        /// <summary>
        /// This is for a const column coming from a where clause so directly the value
        /// </summary>
        /// <param name="directValue"></param>
        /// <param name="value"></param>
        internal ConstLocationColumn(bool directValue, string value){ constValue = value;}
        public ConstLocationColumn(String constTxt, HQLMetaData metaData)
        {
            colTableRefName = metaData.currTableRefName.ToUpper();
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
