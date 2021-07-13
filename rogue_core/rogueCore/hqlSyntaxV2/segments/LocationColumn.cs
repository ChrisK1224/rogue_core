
using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.rogueUI;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.table.encoded;
using rogueCore.hqlSyntaxV2.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments
{
    public class LocationColumn : ILocationColumn
    {
        public Boolean isStar { get; set; } = false;
        internal const string fullColumnSplitter = ".";
        public bool isConstant { get { return false; } }
        public ColumnRowID columnRowID { get; set; }
        bool isEncoded = false;
        internal string origTxt;
        public string colTableRefName { get { return colTblName.ToUpper(); }  }
        string colTblName;
        bool isExecutable = false;
        string execName;
        internal bool isDirectID = false;
        HQLMetaData metaData;
        public LocationColumn(String humanHQL, HQLMetaData metaData)
        {
            this.origTxt = humanHQL;
            this.metaData = metaData;
            if (humanHQL.ToUpper().StartsWith("EXECUTE("))
            {
                execName = stringHelper.get_string_between_2(humanHQL, "(\"", ",");
                humanHQL = stringHelper.get_string_between_2(humanHQL, ",", "\")").Trim();
                isExecutable = true;
            }
            string[] parts = humanHQL.Split('.');
            //* AWFULE CODE
            if (parts.Length == 2)
            {
                isEncoded = (parts[1].StartsWith("{") || parts[1].StartsWith("[{")) ? true : false;
                isDirectID = (parts[1].StartsWith("[")) ? true : false;
                
            }
            if (humanHQL == "*")
            {
                isStar = true;
                columnRowID = -1012;
                colTblName = metaData.currTableRefName;
            }
            //else if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{") || humanHQL.StartsWith("\"{")) { metaData.encodedTableStatements.Add(metaData.currTableRefName); }
            else if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{")) { isEncoded = true; }
            //* TODO Horrible code but easily fixed when clean this up
            else if (parts.Length > 1 && isEncoded)
            {
                isEncoded = true; colTblName = humanHQL.Split('.')[0];
            }
            else if (isDirectID)
            {
                colTblName = parts[0];
                columnRowID = new ColumnRowID(stringHelper.GetStringBetween(parts[1], "[", "]"));
            }
            else
            {
                String colNm;
                if (parts.Length == 1)
                {
                    colTblName = metaData.currTableRefName;
                    colNm = parts[0];
                }
                else
                {
                    colTblName = parts[0];
                    if (humanHQL.StartsWith("{") || humanHQL.StartsWith("[{")) { isEncoded = true; }
                    colNm = parts[1];
                }
                //* TODO terrible code TryGetValue is only here to handle hierarchytable format when this coltablerefanem doesn't exist yet for roguevalue columnID. need to inclu this column in generic rouge columns that all get queried
                int parentID = 0;
                // HumanHQLStatement.tableRefIDs.TryGetValue(colTblName.ToUpper(), out parentID);
                metaData.tableRefIDs.TryGetValue(colTblName.ToUpper(), out parentID);
                columnRowID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(colNm, parentID));
            }
        }
        public LocationColumn(Boolean isStar, string colTblName, ColumnRowID columnRowID = null)
        {
            if (isStar)
            {
                columnRowID = new ColumnRowID(-1012);
            }
            else
            {
                this.columnRowID = columnRowID;
            }
            this.colTblName = colTblName;
            this.isStar = isStar;
        }
        public LocationColumn(bool fakeConstructor, string colTblName)
        {
            this.colTblName = colTblName;
            columnRowID = new ColumnRowID(0);
        }
        internal LocationColumn(ColumnRowID columnRowID)
        {
            this.columnRowID = columnRowID;
            //this.colTblName = colTblName;
        }
        LocationColumn(ColumnRowID columnRowID, string colTblName)
        {
            this.columnRowID = columnRowID;
            this.colTblName = colTblName;
        }
        public LocationColumn(int colID, String tableName, HQLMetaData metaData)
        {
            if (tableName == "")
            {
                colTblName = metaData.currTableRefName;
            }
            else
            {
                colTblName = tableName;
            }
            columnRowID = new ColumnRowID(colID);
        }
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
                if(rows.ContainsKey(colTableRefName))
                {
                    return CalcStringValue(rows[colTableRefName]);
                }
                else
                {
                    return "";
                }
            }
        }
        string RunExecutable(string name, string paramVal)
        {
            //*BRINGBACK*************************
            //var statement = new FilledHQLQuery(FilledHQLQuery.StoredProcByID(8552).Replace("@EXECUTABLE_NM", name));
            //statement.IterateRows(null);
            //FilledSelectRow execRow = statement.hierarchyGrid[0].Value;
            //switch (execRow.values["CODE_LANG_NM"].Value.ToLower())
            //{
            //    case "c#":
            //        return CodeCaller.RunProcedure(execName, new string[] { paramVal });
            //}
            return "";
        }
        internal LocationColumn ModifyEncodedColumn(string calcedVal, string tableRefName)
        {
            ColumnRowID thsColumnRowID;
            if (isDirectID)
            {
                thsColumnRowID = new ColumnRowID(calcedVal);
            }
            else
            {
                int parentID = 0;
                metaData.tableRefIDs.TryGetValue(colTblName.ToUpper(), out parentID);
                thsColumnRowID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(calcedVal, parentID));
            }
            var newLocCol = new LocationColumn(thsColumnRowID);
            newLocCol.isDirectID = this.isDirectID;
            newLocCol.colTblName = tableRefName;
            return newLocCol;
        }
        public List<UIDecoratedTextItem> txtItems()
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem(origTxt, "black", "normal"));
            return items;
        }
    }
}
