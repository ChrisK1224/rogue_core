using FilesAndFolders;
using rogue_core.rogueCore.hqlFilter;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments.selects
{
    public class LevelColumns : Dictionary<String, FullColumnLocations>
    {
        public String GetHQLText()
        {
            //*Problem if etting text for anything with a merged row
            var e = this.GetEnumerator();
            e.MoveNext();
            var anElement = e.Current;
            return anElement.Value.GetHQLText();
        }
        public String GetFullHQLText(String tableRefName)
        {
            String hql = " SELECT ";
            Boolean hasResults = false;
            if (this.ContainsKey(tableRefName))
            {
                hql += this[tableRefName].GetFullHQLText() + ",";
                hasResults = true;
                if (hasResults)
                {
                    hql = hql.Substring(0, hql.Length - 1);
                }
            }
            return hql;
        }
        //public static LevelColumns FromEncodedText(String encodedHQL)
        //{
        //    String selectText = stringHelper.get_string_between_2(encodedHQL, "SELECT", "WHERE");
        //    String[] columnSegments = selectText.Split(',');
        //    //new LevelColumns();
        //    foreach (String col in columnSegments)
        //    {
        //        this.Add(0, new FullColumnLocations());
        //    }
        //    return 
        //}
        public static LevelColumns RowToColumns(String tableRefName, int columnRowID)
        {
            LevelColumns rowToCols = new LevelColumns();
            rowToCols.Add(tableRefName, FullColumnLocations.RowToColumnCols(tableRefName, columnRowID));
            return rowToCols;
        }
        public List<SelectColumn> Columns()
        {
            List<SelectColumn> allCols = new List<SelectColumn>();
            foreach(FullColumnLocations cols in this.Values)
            {
                foreach(SelectColumn thsCol in cols.allColumns.Values)
                {
                    allCols.Add(thsCol);
                }
            }
            return allCols;
        }
        public static String HumanToEncodedHQL(String humanHQL, Dictionary<String, int> tableRefNameIDs, String tableRefName)
        {
            return FullColumnLocations.HumanToEncodedHQL(humanHQL, tableRefNameIDs, tableRefName);
        }
    }
}
