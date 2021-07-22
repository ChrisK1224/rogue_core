using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class CalcGroup : SplitSegment, ICalcable
    {
        List<KeyValuePair<string, ICalcable>> allCalcs = new List<KeyValuePair<string, ICalcable>>();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { }; } }
        //QueryMetaData metaData { get; }
        public CalcGroup(QueryMetaData metaData) : base("", metaData)
        {
            //this.metaData = metaData;
        }
        public void AddCalcable(string key, ICalcable calcable)
        {
            allCalcs.Add(new KeyValuePair<string, ICalcable>(key, calcable));
        }
        Func<string, string, string> GetSymbolFunc(string symbol)
        {
            switch (symbol)
            {
                case KeyNames.addKey:
                    return AddCalc;
                case KeyNames.minusKey:
                    return MinusCalc;
                case KeyNames.concat:
                    return ConcatCalc;
                case KeyNames.multiplyKey:
                    return MultiplyCalc;
                case KeyNames.divideKey:
                    return DivideCalc;
                default:
                    throw new Exception("Unknown Calc Group Symbol");
            }
        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        {
            string lastValue = allCalcs[0].Value.RetrieveStringValue(parentRows);
            for (int i = 1; i < allCalcs.Count; i++)
            {
                lastValue = GetSymbolFunc(allCalcs[i].Key)(lastValue, allCalcs[i].Value.RetrieveStringValue(parentRows));
            }
            return lastValue;
        }
        string AddCalc(string first, string second)
        {
            return (double.Parse(first) + double.Parse(second)).ToString();
        }
        string MinusCalc(string first, string second)
        {
            return (double.Parse(first) - double.Parse(second)).ToString();
        }
        string ConcatCalc(string first, string second)
        {
            return first + second;
        }
        string DivideCalc(string first, string second)
        {
            return ((double.Parse(first))/(double.Parse(second))).ToString();
        }
        string MultiplyCalc(string first, string second)
        {
            return (double.Parse(first) * (double.Parse(second))).ToString();
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
