using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesAndFolders
{
    /// <summary>
    /// This class has various common methods to be used with csv files
    /// </summary>
  public static  class CSV
    {/// <summary>
        public static System.Data.DataTable ConvertCSVtoDataTableFromString(String FullPath)
        {
            //This sub takes in a fileupload control (csv ONLY) and converst it to a datatable and returns the dt
            StreamReader sr = new StreamReader(FullPath);

            string[] headers = sr.ReadLine().Split(',');
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = System.Text.RegularExpressions.Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    string currRecord = rows[i];
                    if (currRecord.Length >= 250)
                    {
                        currRecord = currRecord.Substring(0, 250);
                    }
                    dr[i] = currRecord;
                }
                dt.Rows.Add(dr);
            }
            sr.Close();
            return dt;
        }

        /// <summary>
        /// This method takes in a DataTable and path and creates a new csv file in the form of the Datatable in the path provided
        /// </summary>
        /// <param name="thsTable"></param>
        public static void DataTableToCSV(DataTable thsTable, string fullPath)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = thsTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in thsTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(fullPath, sb.ToString());
        }
        /// <summary>
        /// This method takes in a dataRow and a path. It appends the datarow to the csv file path 
        /// </summary>
        /// <param name="thsRow"></param>
        /// <param name="fullPath"></param>
        public static void appendCSVRow(DataRow thsRow, string fullPath)
        {
            var cols = thsRow.ItemArray.ToArray();
            var csvRow = string.Join(",", cols);
            List<String> thsList = new List<string>();
            thsList.Add(csvRow);
            File.AppendAllLines(fullPath, thsList);
        }
        public static List<string> splitCSVRow(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(','));
            }

            return list;
        }
        public static DataTable csv_to_datatable(String csv_path, Boolean has_headers)
        {
            StreamReader sr = new StreamReader(csv_path);
            System.Data.DataTable dt = new System.Data.DataTable();
            Boolean first_line = true;
            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                if (has_headers && first_line)
                {
                   foreach(String ths_col in splitCSVRow(line))
                   {
                        dt.Columns.Add(ths_col);
                   }
                    sr.ReadLine();
                }
                else if(!has_headers && first_line)
                {
                    foreach (String ths_col in splitCSVRow(line))
                    {
                        dt.Columns.Add();
                    }
                }
                first_line = false;
                DataRow new_row = dt.NewRow();
                new_row.ItemArray = splitCSVRow(line).ToArray();
                dt.Rows.Add(new_row);
            }
            return dt;
        }
    }
}
