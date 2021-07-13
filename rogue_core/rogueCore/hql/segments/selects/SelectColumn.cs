using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hql.segments;
using rogue_core.rogueCore.hql.segments.columnSegment;
using rogue_core.rogueCore.hql.segments.selects;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.id.rogueID.hqlIDs;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;

namespace rogue_core.rogueCore.hql
{
    public class SelectColumn : LocationColumn
    {
        public String columnAliasName;
        public DecodedRowID columnName;
        public String constValue = "";
        public String colOrigName = "";
        public Boolean maintainOnHeadRow = false;
        public Boolean hidden = false;
        public List<ILocationColumn> comboColumns = new List<ILocationColumn>();
        public SelectColumn(String tableRefName, ColumnRowID columnRowID, String columnAliasName = "", String columnStaticValue = "", List<ILocationColumn> comboCols = null) : base(tableRefName, columnRowID)
        {
            if(constValue == "")
            {
                colOrigName = columnRowID.ToColumnName().ToUpper();
            }
            if(columnAliasName == ""){
                this.columnAliasName = colOrigName;
            }else{
                this.columnAliasName = columnAliasName.ToUpper();
            }
            this.columnName = this.columnAliasName.ToDecodedRowID();
            this.constValue = columnStaticValue;
            if(comboCols != null)
            {
                this.comboColumns = comboCols;
            }
        }
        public SelectColumn(String colSnippet) : base(colSnippet)
        {
            String[] columnParts = colSnippet.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            String[] snips = columnParts[0].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            colOrigName = columnRowID.ToColumnName().ToUpper();
            if (snips.Length >= 3 && snips[2] != "")
                {
                    columnAliasName = snips[2].ToUpper();
                }
                else
                {
                    columnAliasName = colOrigName;
                }
                if (snips.Length == 4)
                {
                    constValue = snips[3];
                }
                this.columnName = this.columnAliasName.ToDecodedRowID();
            //}
            for(int i = 1; i < columnParts.Length; i++)
            {
                if (columnParts[i].Trim().StartsWith("\""))
                {
                    comboColumns.Add(new ConstantValue(columnParts[i].TrimFirstAndLastChar(), tableRefName));
                }
                else
                {
                    comboColumns.Add(new LocationColumn(columnParts[i]));
                }
            }
            SetIndicators(colSnippet);
        }
        public static SelectColumn ColumnAsRowColumnName(String tableRefName, int columnRowID)
        {
            return new SelectColumn(tableRefName, columnRowID, "RogueKeyName");
        }
        public static SelectColumn ColumnAsRowColumnValue(String tableRefName, int columnRowID)
        {
            return new SelectColumn(tableRefName, columnRowID, "RogueValue");
        }
        public static SelectColumn FromEncodedText(String encodedHQL)
        {
            String[] names = encodedHQL.Split(new string[] { " AS " }, StringSplitOptions.None);
            String columnAliasName = "";
            String columnStaticValue = "";
            ColumnRowID colRowID = null;
            String tableRefName = "";
            //String[] baseColInfo = names[0].Split('.');
            //LocationColumn blah = new LocationColumn(baseColInfo[0],);
            if (names.Length > 0)
            {
                columnAliasName = names[1];
            }
            if (names[0].StartsWith("\""))
            {
                columnStaticValue = names[0].Substring(1, names[0].Length - 2);
            }
            else
            {
                String[] baseColInfo = names[0].Split('.');
                tableRefName = baseColInfo[0];
                colRowID = new ColumnRowID(HQLEncoder.GetColumnIDByFullName(baseColInfo[1]).ToInt());
                
            }
            return new SelectColumn(tableRefName, colRowID, columnAliasName, columnStaticValue);
        }
        private void SetIndicators(String colSnippet)
        {
            if (colSnippet.Contains("<"))
            {
                String commandSnip = stringHelper.GetStringBetween(colSnippet, "<", ">");
                string[] commands = commandSnip.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String command in commands)
                {
                    switch (command.ToLower())
                    {
                        case "maintainonrow":
                            maintainOnHeadRow = true;
                            break;
                        case "hide":
                            hidden = true;
                            break;
                    }
                }
            }
        }
        public new String GetHQLText(){
            String finalValue ;
            if(constValue != "")
            {
                finalValue = base.GetHQLText() + "." + columnAliasName + "." + constValue;
            }
            else
            {
                finalValue = base.GetHQLText() + "." + columnAliasName;
            }
            foreach(ILocationColumn thsCol in comboColumns)
            {
                finalValue += "&" + thsCol.GetHQLText();
            }
            
            return finalValue;
        }
        public String GetFullHQLText()
        {
            String encodedHQL = "";
            if(constValue != "")
            {
                encodedHQL += "\"" + constValue + "\"" + " AS " + columnAliasName;
            }
            else
            {
                if(colOrigName != columnAliasName)
                {
                    encodedHQL += tableRefName + "." + colOrigName + " AS " + columnAliasName;
                }
                else
                {
                    encodedHQL += tableRefName + "." + colOrigName;
                }
            }
            foreach (ILocationColumn thsCol in comboColumns)
            {
                encodedHQL += thsCol.GetHumanHQLText();
            }
            return encodedHQL;
        }
        public override DecodedRowID CalcValue(IRogueRow thsRow)
        {
            if(constValue != "")
            {
                return constValue.ToDecodedRowID();
            }
            else
            {
                return int.Parse(thsRow.IGetBasePair(columnRowID).WriteValue());
            }
        }
        public String CalcValueAsDisplay(IRogueRow thsRow)
        {
            if (constValue != "")
            {
                return constValue;
            }
            else
            {
                if(thsRow.ITryGetValue(columnRowID) != null)
                {
                    return thsRow.IGetBasePair(columnRowID).DisplayValue();
                }
                else
                {
                    return "";
                }
            }
        }
        public static SelectColumn HumanToEncodedHQL(String columnHQL, Dictionary<String,int> tableRefNameIDs, String tableRefName)
        {
            //HeaderLabel.CONTROLNAME,\"child\" AS PARENTRELATION";
            columnHQL = columnHQL.Trim();
            string[] additionalParts = columnHQL.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            String constValue = "";
            String columnAliasName = "";
            String[] columnParts = additionalParts[0].Trim().Split(new char[0]);
            columnParts[0] = columnParts[0].Trim();
            ILocationColumn thsCol;
            if (columnParts[0].StartsWith("\""))
            {
                constValue = columnParts[0].Substring(1, columnParts[0].Length - 2);
                thsCol = new ConstantValue(constValue, tableRefName);
            }
            else
            {
                if (!columnParts[0].Contains("."))
                {
                    thsCol = new LocationColumn(tableRefName, HQLEncoder.GuessColumnIDByName(columnParts[0], tableRefNameIDs[tableRefName]));
                }
                else
                {
                    thsCol = LocationColumn.HumanToEncodedHQL(columnParts[0], tableRefNameIDs);
                }
            }
            if(columnParts.Length > 1)
            {
                columnAliasName = columnParts[2];
            }
            List<ILocationColumn> comboColumns = new List<ILocationColumn>();
            for(int i = 1; i < additionalParts.Length; i++)
            {
                additionalParts[i] = additionalParts[i].Trim();
                if (additionalParts[i].Trim().StartsWith("\""))
                {
                    comboColumns.Add(new ConstantValue(additionalParts[i].TrimFirstAndLastChar(), tableRefName));
                }
                else
                {
                    comboColumns.Add(LocationColumn.HumanToEncodedHQL(additionalParts[i], tableRefNameIDs));
                }
            }
            return new SelectColumn(thsCol.tableRefName,thsCol.columnRowID , columnAliasName, constValue, comboColumns);
        }
    }
}