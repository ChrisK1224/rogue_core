using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    class StarColumn : SplitSegment, ISelectColumn, ISelectColOrStar
    {
        public string columnName => throw new NotImplementedException();
        public string upperColumnName => throw new NotImplementedException();
        public ColumnRowID baseColumnID => throw new NotImplementedException();
        public List<ISelectColumn> generatedColumns => throw new NotImplementedException();
        public override List<SplitKey> splitKeys => throw new NotImplementedException();
        public StarColumn(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            if (txt == "*")
            {
                var cols = metaData.CurrentLevelColumns();
                foreach (var col in cols)
                {
                    col.Value.ForEach(x => generatedColumns.Add(new DirectColumn(col.Key, x.ColumnIDName(), x.rowID)));
                }
            }
            else if (txt.Contains("*"))
            {
                string tblName = txt.BeforeFirstChar('.');
                var cols = metaData.CurrentLevelSingleTableColumns(tblName);
                cols.ForEach(x => generatedColumns.Add(new DirectColumn(tblName, x.ColumnIDName(), x.rowID)));
            }
        }
        public string GetValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> tableRefRows)
        {
            throw new NotImplementedException();
        }
        public void ResetColName(string colName)
        {
            throw new NotImplementedException();
        }
        public override string PrintDetails()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<string> SyntaxSuggestions()
        {
            return new List<string>();
        }
    }
}
