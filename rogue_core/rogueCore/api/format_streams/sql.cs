using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace rogueCore.api.formats.sql
{
    /*public class Sql
    {
        public string con_str;
        public static string qry_tables;
        public static String get_tables_qry = "SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' AND table_schema='@db_name'";
        public static String get_all_results = "SELECT * FROM @table_name";
        public Sql(String connstr, String db_name)
        {
            //con_str = "Server=localhost\\sqlexpress;Database=entries;uid=root;Password=shallNotpass12;";
            //con_str = "Server=localhost\\sqlexpress;Database=entries;uid=root;password=shallNotpass12;";
            qry_tables = "show tables from " + db_name;
            //String connStr;
            String server = "localhost";
            String uid = "root";
            String DBName = "entries";
            String password = "shallNotpass12";
            this.con_str = "SERVER=" + server + ";" + "DATABASE=" + DBName + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";ConnectionTimeout=600;";
        }
        public void mass_load_db(Database rogue_db, String sql_db_name, String parent_id)
        {
            String tbl_qry = get_tables_qry.Replace("@db_name", sql_db_name);
            foreach (DataRow ths_tbl in base_qry(tbl_qry).to_datatable().Rows)
            {
                RogueDataTable ths_writer = rogue_db.GetWriteChildTable(ths_tbl[0].ToString(), "");
                DataTable full_tbl = base_qry(get_all_results.Replace("@table_name", ths_tbl[0].ToString())).to_datatable();
                //ths_writer.open_writer();
                int p = 0;
                //foreach (Tuple<DataTable, DataRow> ths_row in base_qry(get_all_results.Replace("@table_name", ths_tbl[0].ToString())).read_by_row())
                foreach(DataRow ths_row in full_tbl.Rows)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    RogueRow new_row = ths_writer.newRow();//ths_writer.write_header(0);
                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    stopwatch.Reset();
                    Stopwatch stopwatch_full = Stopwatch.StartNew();
                    //Data_Pairs all_pairs = new Data_Pairs();
                    foreach (DataColumn ths_col in full_tbl.Columns)
                    {
                        ColumnRow ths_rogue_col = ths_writer.columnTable.get_write_record(column_key_types.column, ths_col.ColumnName, ths_row[ths_col].ToString());
                        new_row.add_write_pair(ths_rogue_col, ths_row[ths_col].ToString());
                        //String col = ths_col.ColumnName;
                        //String value = ths_row[col].ToString();
                        //.write_value_pair(col, tables.auto_tables.columns.column_key_types.column, value);                       
                    }
                    stopwatch_full.Stop();
                    Console.WriteLine(stopwatch_full.ElapsedMilliseconds);
                    Stopwatch stopwatch_close = Stopwatch.StartNew();
                    //ths_writer.close_container();
                    stopwatch_close.Stop();
                    Console.WriteLine(stopwatch_close.ElapsedMilliseconds);
                    p++;
                }
                ths_writer.write();
            }
        }
        private DataTable get_table_results(String query)
        {
            using (MySqlConnection con = new MySqlConnection(con_str))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter returnVal = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                returnVal.Fill(dt);
                return dt;
            }
        }
        private MySqlDataReader base_qry(String query)
        {
            MySqlConnection con = new MySqlConnection(con_str);
                con.Open();
                try
                {
                    MySqlCommand command = new MySqlCommand(query, con);
                    
                    MySqlDataReader reader = command.ExecuteReader();
                    return reader;
                }
                catch
                {
                    Console.WriteLine("Something went wrong");
                }
            return null;
        }
    }
    public static class extender{
        public static DataTable to_datatable(this MySqlDataReader reader)
        {
            DataTable table = new DataTable();

            var schemaTable = reader.GetSchemaTable();
            if(schemaTable != null)
            {
                foreach (DataRowView row in schemaTable.DefaultView)
                {
                    var columnName = (string)row["ColumnName"];
                    var type = (Type)row["DataType"];
                    table.Columns.Add(columnName, type);
                }
            }
            
            table.Load(reader);
            reader.Close();
            return table;
        }
        public static IEnumerable<Tuple<DataTable, DataRow>> read_by_row(this MySqlDataReader ths_reader)
        {
            DataTable tbl_shell = new DataTable();
            String[] columns = new String[ths_reader.FieldCount];
            var schemaTable = ths_reader.GetSchemaTable();
            foreach (DataRowView row in schemaTable.DefaultView)
            {
                var columnName = (string)row["ColumnName"];
                var type = (Type)row["DataType"];
                tbl_shell.Columns.Add(columnName, type);
            }
            while (ths_reader.Read())
            {
                DataRow new_row = tbl_shell.NewRow();
                for (int i = 0; i < ths_reader.FieldCount; i++)
                    new_row[ths_reader.GetName(i)] = ths_reader[i];
                yield return new Tuple<DataTable, DataRow>(tbl_shell, new_row);
            }
            ths_reader.Close();
        }
    }*/
}

