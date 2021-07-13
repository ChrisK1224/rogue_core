using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FilesAndFolders
{
    /// <summary>
    /// This class has various help funtions around datatables
    /// </summary>
   public static class DT
   {/// <summary>
    /// This function creates a datatable as a subset of a datatable passed to it based on rules from the paramters. Was used for getting mini
    /// tables out of excel files that were not csv format
    /// </summary>
    /// <param name="headerDefineLst"></param>
    /// <param name="dtTbl"></param>
    /// <param name="isValid"></param>
    /// <param name="foundNumForValid"></param>
    /// <param name="TableCompleteAtEnd"></param>
    /// <param name="colNameForEnd"></param>
    /// <returns></returns>
        public static System.Data.DataTable createSubTableFromHeaderRow(List<string> headerDefineLst, System.Data.DataTable dtTbl, ref Boolean isValid, int foundNumForValid, Boolean TableCompleteAtEnd, string colNameForEnd = "")
        {
            int currFoundCount = 0;

            int headerRowNum = 0;
            for (int p = 0; p < dtTbl.Rows.Count; p++)
            {
                for (int i = 0; i < dtTbl.Rows[p].ItemArray.Count(); i++)
                {
                    if (headerDefineLst.Contains(dtTbl.Rows[p][i].ToString().ToUpper().Replace(" ", "")))
                    {
                        // dtTbl.Columns[i].ColumnName = dtTbl.Rows[p][i].ToString().ToUpper().Replace(" ", "");
                        currFoundCount++;
                    }
                }
                if (currFoundCount >= foundNumForValid)
                {
                    isValid = true;
                    headerRowNum = p;
                    break;
                }
                currFoundCount = 0;
            }

            //Rename columns to match header row
            for (int i = 0; i < dtTbl.Rows[headerRowNum].ItemArray.Count(); i++)
            {
                if (headerDefineLst.Contains(dtTbl.Rows[headerRowNum][i].ToString().ToUpper().Replace(" ", "")))
                {
                    dtTbl.Columns[i].ColumnName = dtTbl.Rows[headerRowNum][i].ToString().ToUpper().Replace(" ", "");
                }
            }
            System.Data.DataTable retTbl = dtTbl.Clone();
            if (isValid == true)
            {
                //Make sure there are rows to read after header row
                if ((dtTbl.Rows.Count - headerRowNum) > 1)
                {
                    if (TableCompleteAtEnd == true)
                    {
                        for (int r = headerRowNum + 1; r < dtTbl.Rows.Count; r++)
                        {
                            retTbl.Rows.Add(dtTbl.Rows[r].ItemArray);
                        }
                    }
                    else
                    {
                        for (int r = headerRowNum + 1; r < dtTbl.Rows.Count; r++)
                        {
                            if (retTbl.Rows[r][colNameForEnd].ToString() != "")
                            {
                                retTbl.Rows.Add(dtTbl.Rows[r].ItemArray);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                //end of isValid IF
            }
            return retTbl;
        }
        /// <summary>
        /// This method takes in a referenced datarow and based on match conditions sets a new column value with a sub value the line sent to it. it
        /// returns true if the test is passed and value changed
        /// </summary>
        /// <param name="thsLine"></param>
        /// <param name="testVal"></param>
        /// <param name="dtColName"></param>
        /// <param name="startVal"></param>
        /// <param name="endVal"></param>
        /// <param name="tillEnd"></param>
        /// <param name="thsDataRow"></param>
        /// <returns></returns>
        public static Boolean setDataRowOnCondition(string thsLine, string testVal, string dtColName, string startVal, string endVal, Boolean tillEnd, ref DataRow thsDataRow)
        {

            if (thsLine.ToLower().Contains(testVal))
            {
                if (tillEnd == true)
                {
                    thsDataRow[dtColName] = stringHelper.GetStringBetween(thsLine, startVal, "", true).ToString().Trim();
                }
                else
                {
                    thsDataRow[dtColName] = stringHelper.GetStringBetween(thsLine, startVal, endVal, false).ToString().Trim();
                }

                return true;
            }

            else
            {
                return false;
            }

        }
    }
}
