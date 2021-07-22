using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using System;
using System.Collections.Generic;
using System.Text;
using files_and_folders;

namespace rogue_core.rogueCore.hqlSyntaxV4.where
{
    public class WhereSegment : SplitSegment
    {
        IColumn localColumn { get; }
        public CalcableGroups foreignColumn { get; }
        string compareType { get; }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { WhereSplitters.whereEqual, WhereSplitters.whereNotEqual, WhereSplitters.whereGreaterThan, WhereSplitters.whereLessThan }; } }
        Dictionary<string, Func<String, string, Boolean>> compare;
        //public enum compareTypes : int {[StringValue("=")] equal = 1, [StringValue("!=")] notEqual = 2 }
        public WhereSegment(string txt, QueryMetaData metaData) : base(txt, metaData) 
        {
            compareType = splitList[1].Key;
            localColumn = BaseColumn.ParseColumn(splitList[0].Value, metaData);            
            foreignColumn = new CalcableGroups(splitList[1].Value.AfterFirstSpace(), metaData);
            SetCompareOptions();
        }
        void SetCompareOptions()
        {
            compare = new Dictionary<string, Func<String, string, Boolean>>();
            compare.Add(KeyNames.whereEqual, EqualCompare);
            compare.Add(KeyNames.whereNotEqual, NotEqualCompare);
            compare.Add(KeyNames.greatThanKey, GreaterThanCompare);
            compare.Add(KeyNames.lessThanKey, LessThanCompare);
        }
        public Boolean IsValid(string thsTableRef, IReadOnlyRogueRow thsRow, IMultiRogueRow parentRow)
        {
            var tempRows = new Dictionary<string, IReadOnlyRogueRow>(parentRow.tableRefRows);
            //**Garage code should know that this table ref doesn't exist here. only reason is for parent Rows that have already been merged and have last copy of row from this table. Need to fix row merging it blows
            tempRows.FindChangeIfNotFound(thsTableRef, thsRow);
            //tempRows.Add(thsTableRef, thsRow);
            string checkVal = localColumn.RetrieveStringValue(tempRows.ToSingleEnum());
            string evalCheck = foreignColumn.GetValue(tempRows.ToSingleEnum());
            return compare[compareType](checkVal, evalCheck);
        }
        public Boolean IsValid(IMultiRogueRow row)
        {
            //var tempRows = new Dictionary<string, IReadOnlyRogueRow>(parentRow.tableRefRows);
            //**Garage code should know that this table ref doesn't exist here. only reason is for parent Rows that have already been merged and have last copy of row from this table. Need to fix row merging it blows
            //tempRows.FindChangeIfNotFound(thsTableRef, thsRow);
            //tempRows.Add(thsTableRef, thsRow);
            string checkVal = localColumn.RetrieveStringValue(row.tableRefRows.ToSingleEnum());
            string evalCheck = foreignColumn.GetValue(row.tableRefRows.ToSingleEnum());
            return compare[compareType](checkVal, evalCheck);
        }
        Boolean EqualCompare(String value, string compareVal)
        {
            return compareVal.ToUpper().Equals(value.ToUpper());
        }
        Boolean NotEqualCompare(String value, string compareVal)
        {
            return !compareVal.ToUpper().Equals(value.ToUpper());
        }
        Boolean GreaterThanCompare(String value, string compareVal)
        {
            return (double.Parse(value) > double.Parse(compareVal));
        }
        Boolean LessThanCompare(String value, string compareVal)
        {
            return (double.Parse(value) < double.Parse(compareVal));
        }
        //compareTypes TempEnumCheck(string val)
        //{
        //    if (val.ToUpper() == KeyNames.whereEqual)
        //    {
        //        return compareTypes.equal;
        //    }
        //    else if (val.ToUpper() == KeyNames.whereNotEqual)
        //    {
        //        return compareTypes.notEqual;
        //    }
        //    else
        //    {
        //        throw new Exception("Unrecognized Compare type");
        //    }
        //    //if(val.ToUpper() == compareTypes.equal.GetStringValue())
        //    //{
        //    //    return compareTypes.equal;
        //    //}
        //    //else if (val.ToUpper() == compareTypes.notEqual.GetStringValue())
        //    //{
        //    //    return compareTypes.notEqual;
        //    //}
        //    //else
        //    //{
        //    //    throw new Exception("Unrecognized Compare type");
        //    //}
        //}
        public override string PrintDetails()
        {
            return "";
        }
    }
}
