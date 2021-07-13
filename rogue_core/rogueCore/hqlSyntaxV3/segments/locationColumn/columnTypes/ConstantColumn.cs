using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes
{
    class ConstantColumn : ILocationColumn
    {
        public string columnName { get; private set; }
        public string upperColumnName { get; private set; }
        public ColumnRowID columnRowID { get { return -1012; } }
        public string colTableRefName { get; private set; }
        public bool isConstant { get { return true; } }
        public bool isEncoded { get { return true; } }
        public bool isStar { get { return true; } }
        public string constValue { get { return constVal; } }
        string constVal;
        string origTxt { get; }
        NamedLocation aliasName;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public ConstantColumn(string colTxt) 
        {
            origTxt = colTxt;
            try
            {
                aliasName = new NamedLocation(colTxt);
                constVal = aliasName.remainingTxt.TrimFirstAndLastChar();
                columnName = aliasName.Name;
                upperColumnName = columnName.ToUpper();
                //*colTableRefName = queryStatement.tableRefIDs.currTableRef.ToUpper();
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void PreFill(QueryMetaData metaData,string assumedTableName)
        {
            colTableRefName = assumedTableName;
            if (constVal.Contains("@"))
            {
                metaData.AddUnsetParams(new List<string>() { constVal });
            }
        }
        public string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows){ return constVal; }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            if (constVal.StartsWith("@"))
            {
                syntaxCommands.GetLabel(parentRow, "\"" + constVal + "\"", IntellsenseDecor.MyColors.yellow);
            }
            else
            {
                syntaxCommands.GetLabel(parentRow, "\"" + constVal + "\"", IntellsenseDecor.MyColors.green);
            }
            aliasName.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            var directNmLbl = syntaxCommands.GetLabel(parentRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.none, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //aliasName.LoadSyntaxParts(parentRow);
        }
        public IEnumerable<string> UnsetParams()
        {
            if (constVal.Trim().Contains("@"))
            {
                yield return constVal;
            }
        }
    }
}
