using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public class SelectRow : SplitSegment, ISelectRow
    {
        public List<ISelectColumn> selectColumns { get; } = new List<ISelectColumn>();
        public Dictionary<string, ISelectColumn> columnsByName { get; } = new Dictionary<string, ISelectColumn>();
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.colSeparator }; } }
        public SelectRow(string rowTxt, QueryMetaData metaData) : base(rowTxt, metaData)
        {
            splitList.ToList().ForEach(x => AddColumn(x.Value, metaData));
            selectColumns.ForEach(x => columnsByName.Add(x.upperColumnName, x));
        }
        void AddColumn(string rowTxt, QueryMetaData metaData)
        {
            rowTxt = rowTxt.Trim();
            if (rowTxt == "*")
            {
                var cols = metaData.CurrentLevelColumns();
                foreach(var col in cols)
                {
                    col.Value.ForEach(x => selectColumns.Add(new DirectColumn(col.Key, x.ColumnIDName(), x.rowID)));
                }                
            }
            else if(rowTxt.Contains(".*"))
            {
                string tblName = rowTxt.BeforeFirstChar('.');
                var cols = metaData.CurrentLevelSingleTableColumns(tblName);
                cols.ForEach(x => selectColumns.Add(new DirectColumn(tblName, x.ColumnIDName(), x.rowID)));
            }
            else
            {
                selectColumns.Add(new SelectColumn(rowTxt, metaData));
            }            
        }
        public override string PrintDetails()
        {
            return "";
        }
        public IEnumerable<string> SyntaxSuggestions()
        {
            return new List<string>();
        }
    }
}