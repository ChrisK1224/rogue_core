using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class CalcableGroups : SplitSegment
    {        
        List<CalcGroup> calcGroups = new List<CalcGroup>();
        protected string name { get { return columns[columns.Count-1].columnName; } }
        public List<IColumn> columns { get; } = new List<IColumn>();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { CalcGroupSplitters.openGroup, CalcGroupSplitters.closeGroup, LocationSplitters.colConcat, LocationSplitters.colAdd, LocationSplitters.colMinus, LocationSplitters.colDivide, LocationSplitters.colMultiply }; } }
        public CalcableGroups(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            txt = txt.Trim();
            calcGroups.Add(new CalcGroup(metaData));
            for (int i = 0; i < splitList.Count; i ++)
            {
                switch (splitList[i].Key)
                {
                    case KeyNames.openParenthensis:
                        var newGrp = new CalcGroup(metaData);
                        calcGroups[calcGroups.Count - 1].AddCalcable(LastCalcKey(i), newGrp);
                        calcGroups.Add(newGrp);
                        if(splitList[i].Value.Trim() != "")
                        {
                            var newCol = BaseColumn.ParseColumn(splitList[i].Value, metaData);
                            columns.Add(newCol);
                            newGrp.AddCalcable(LastCalcKey(i), newCol);
                        }
                        break;
                    case KeyNames.closeParenthesis:
                        calcGroups.RemoveAt(calcGroups.Count - 1);
                        break;
                    default:
                        if(splitList[i].Value.Trim() != "")
                        {
                            var newCol = BaseColumn.ParseColumn(splitList[i].Value, metaData);
                            columns.Add(newCol);
                            calcGroups[calcGroups.Count - 1].AddCalcable(LastCalcKey(i), newCol);
                        }                        
                        break;
                }
            }
        }
        public string GetValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            return calcGroups[0].RetrieveStringValue(rows);
        }
        public string GetValue(IMultiRogueRow row)
        {
            return calcGroups[0].RetrieveStringValue(row.tableRefRows.ToSingleEnum());
        }
        string LastCalcKey(int currIndex)
        {
            if(currIndex == 0)
            {
                return KeyNames.startKey;
            }
           for(int i = currIndex; i > 0; i--)
           {
                if(splitList[i].Key != KeyNames.openBracket && splitList[i].Key != KeyNames.closeBracket)
                {
                    return splitList[i].Key;
                }
           }
            return KeyNames.startKey;
            //throw new Exception("No Calcable group key found when looking back");
        }
        public override string PrintDetails()
        {
            return "";
        }        
    }
}
