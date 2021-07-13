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
            splitList.ToList().ForEach(x => selectColumns.Add(new SelectColumn(x.Value, metaData)));
            selectColumns.ForEach(x => columnsByName.Add(x.upperColumnName, x));
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}