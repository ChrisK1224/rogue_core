
using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.rogueUI;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.table.encoded;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments
{
    public class LocationColumn : ILocationColumn
    {
        public Boolean isStar { get; set; } = false;
        public ColumnRowID columnRowID { get; set; }
        bool isEncoded = false;
        public string colTableRefName { get; set; }
        bool isExecutable = false;
        string execName;
        //public string CalcStringValue(Dictionary<string, IRogueRow> rows);
        public virtual DecodedRowID CalcValue(IRogueRow thsRow)
        {
            return int.Parse(thsRow.IGetBasePair(columnRowID).WriteValue());
        }
        public String CalcStringValue(IRogueRow thsRow)
        {
            if (thsRow.ITryGetValue(columnRowID) != null)
            {
                return thsRow.IGetBasePair(columnRowID).DisplayValue();
            }
            else
            {
                return "";
            }
        }
        public string CalcStringValue(Dictionary<string, IRogueRow> rows)
        {
            //*Need to fix this to a delegate to avoid if statement for each calc
            if (isExecutable)
            {
                string initValue = CalcStringValue(rows[colTableRefName]);
                //string finalValue = CodeCaller.RunProcedure(execName, new string[] { initValue });
                return RunExecutable(execName, initValue);
            }
            else
            {
                return CalcStringValue(rows[colTableRefName]);
            }
        }
        public LocationColumn(String humanHQL, HQLMetaData metaData)
        {
            //*This can be simplified once i fix bug to get rid of RogueCOlumnID not showing up in regular column query
            if (humanHQL.ToUpper().StartsWith("EXECUTE("))
            {
                execName = stringHelper.get_string_between_2(humanHQL, "(\"", ",");
                humanHQL = stringHelper.get_string_between_2(humanHQL, ",", "\")").Trim();
                isExecutable = true;
                //humanHQL = stringHelper.get_string_between_2(humanHQL, "(", ")");
            }
            string[] parts = humanHQL.Split('.');
            bool testEncoded = false;
            bool isDirectCol = false;
            //* AWFULE CODE
            if (parts.Length == 2)
            {
                testEncoded = (parts[1].StartsWith("{") || parts[1].StartsWith("[{")) ? true : false;
                isDirectCol = (parts[1].StartsWith("[")) ? true : false;
            }
            if (humanHQL == "*")
            {
                isStar = true;
                columnRowID = -1012;
                //colTableRefName = HumanHQLStatement.currTableRefName;
                colTableRefName = metaData.currTableRefName;
            }
            //else if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{") || humanHQL.StartsWith("\"{")) { metaData.encodedTableStatements.Add(metaData.currTableRefName); }
            else if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{")) { isEncoded = true; }
            //* TODO Horrible code but easily fixed when clean this up
            else if (parts.Length > 1 && testEncoded)
            {
                isEncoded = true; colTableRefName = humanHQL.Split('.')[0];
            }
            else if (isDirectCol)
            {
                colTableRefName = parts[0];
                columnRowID = new ColumnRowID(stringHelper.GetStringBetween(parts[1], "[", "]"));
            }
            //else if (humanHQL.StartsWith("["))
            //{
            //    colTableRefName = metaData.currTableRefName;
            //    columnRowID = new ColumnRowID(humanHQL.Substring(1, humanHQL.Length-2));
            //}
            else
            {
                //String[] parts = humanHQL.Split('.');
                String colNm;
                if (parts.Length == 1)
                {
                    //colTableRefName = HumanHQLStatement.currTableRefName;
                    colTableRefName = metaData.currTableRefName;
                    colNm = parts[0];
                }
                else
                {
                    colTableRefName = parts[0];
                    if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{")) { isEncoded = true; }
                    colNm = parts[1];
                }
                //* TODO terrible code TryGetValue is only here to handle hierarchytable format when this coltablerefanem doesn't exist yet for roguevalue columnID. need to inclu this column in generic rouge columns that all get queried
                int parentID = 0;
                // HumanHQLStatement.tableRefIDs.TryGetValue(colTableRefName.ToUpper(), out parentID);
                metaData.tableRefIDs.TryGetValue(colTableRefName.ToUpper(), out parentID);
                columnRowID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(colNm, parentID));
            }
        }
        public LocationColumn(Boolean isStar, string colTableRefName, ColumnRowID columnRowID = null)
        {
            if (isStar)
            {
                columnRowID = new ColumnRowID(-1012);
            }
            else
            {
                this.columnRowID = columnRowID;
            }
            this.colTableRefName = colTableRefName;
            this.isStar = isStar;
        }
        public LocationColumn(bool fakeConstructor, string colTableRefName){
            this.colTableRefName = colTableRefName;
            columnRowID = new ColumnRowID(-1012);
        }
        internal LocationColumn(ColumnRowID columnRowID)
        {
            this.columnRowID = columnRowID;
            this.colTableRefName = colTableRefName;
        }
        public LocationColumn(int colID, String tableName, HQLMetaData metaData)
        {
            //HumanHQLStatement.tableRefIDs.TryGetValue(colTableRefName.ToUpper(), out parentID);
            if (tableName == "")
            {
                colTableRefName = metaData.currTableRefName;
            }
            else
            {
                colTableRefName = tableName;
            }
            columnRowID = new ColumnRowID(colID);
            //colNm = HQLEncoder.GetColumnNameByID(columnRowID);
        }
        string RunExecutable(string name, string paramVal)
        {

            var statement = new HumanHQLStatement(HumanHQLStatement.StoredProcByID(8552).Replace("@EXECUTABLE_NM", name));
            statement.LoadRows(null);
            FilledSelectRow execRow = statement.hierarchyGrid[0].Value;
            switch (execRow.values["CODE_LANG_NM"].Value.ToLower())
            {
                case "c#":
                    return CodeCaller.RunProcedure(execName, new string[] { paramVal });
            }
            return "";
        }
    }
}
