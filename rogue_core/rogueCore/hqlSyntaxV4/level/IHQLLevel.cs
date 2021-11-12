using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.level
{
    public interface IHQLLevel : IQueryableDataSet
    {
        public List<IMultiRogueRow> filteredRows { get; set; }
        public string parentLvlName { get; }
        List<HQLTable> tables { get; }
        public void Fill();
        public ISelectRow selectRow { get; }
        public Dictionary<IColumn, Dictionary<string, List<IMultiRogueRow>>> indexedRows { get; }
        public void AddChildLevel(IHQLLevel lvl);
    }
    public static class LevelExtensions
    {
        public static List<string> ColumnNames(this IHQLLevel lvl) { return lvl.selectRow.selectColumns.Select(x => x.columnName).ToList();  }
    }
}
