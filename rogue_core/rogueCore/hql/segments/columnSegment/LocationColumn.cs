using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hql.segments.columnSegment;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments.selects
{

    public class LocationColumn : ILocationColumn
    {
        public String tableRefName { get; set; }
        public ColumnRowID columnRowID { get; set; }
        public Boolean IsStar = false;
        //public String constValue = "";
        public LocationColumn(String snippet)
        {
            if(snippet == "*")
            {
                IsStar = true;
                columnRowID = -1012;
            }
            else
            {
                String[] snips = snippet.Split('.');
                tableRefName = snips[0].Trim();
                columnRowID = new ColumnRowID(snips[1].Trim());
            }
        }
        public LocationColumn(String tableRefName, ColumnRowID columnRowID)
        {
            this.tableRefName = tableRefName.Trim();
            this.columnRowID = columnRowID;
        }
        public static LocationColumn HumanToEncodedHQL(String humanHQL, Dictionary<String, int> tableRefNameIDs)
        {
            //*IF star
            //if (humanHQL.Trim().Equals("*"))
            //{
            //    return new LocationColumn("*");
            //}
            String[] parts = humanHQL.Split('.');
            String tableRefName = parts[0].ToUpper();
            ColumnRowID columnID;
            //*This is current problem lopokig for ROOT but was renamed to curr talbe
            if(parts.Length == 1)
            {
                columnID = -1012;
            }
            else
            {
                int parentID = tableRefNameIDs[tableRefName];
                //***TERRRIBLE**
                if(parts[1].ToUpper() == "ROGUECOLUMNID")
                {
                    columnID = -1012;
                }
                else
                {
                    columnID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(parts[1], parentID));
                }
            }
            return new LocationColumn(tableRefName, columnID);
        }
        public String GetHQLText()
        {
             return tableRefName + "." + columnRowID;
        }
        public String GetHumanHQLText()
        {
            return tableRefName + "." + HQLEncoder.GetColumnNameByID(columnRowID);
        }
        public virtual DecodedRowID CalcValue(IRogueRow thsRow)
        {
            return int.Parse(thsRow.IGetBasePair(columnRowID).WriteValue());
        }
        public String CalcStringValue(IRogueRow thsRow)
        {
            //if (constValue != "")
            //{
            //    return constValue;
            //}
            //else
            //{
                if (thsRow.ITryGetValue(columnRowID) != null)
                {
                    return thsRow.IGetBasePair(columnRowID).DisplayValue();
                }
                else
                {
                    return "";
                }
            //}
        }

        //public string CalcValue()
        //{
        //    if (thsRow.ITryGetValue(columnRowID) != null)
        //    {
        //        return thsRow.IGetBasePair(columnRowID).DisplayValue();
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }
}
