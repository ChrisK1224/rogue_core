using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Text;
using files_and_folders;
using FilesAndFolders;
using System.Linq;

namespace rogue_core.rogueCore.hqlSyntaxV4.classify
{
    class Classify : SplitSegment, IClassify
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.colSeparator}; } }
        List<IColumn> classifyCols = new List<IColumn>();
        Dictionary<string, List<IMultiRogueRow>> classifiedRows = new Dictionary<string, List<IMultiRogueRow>>();
        public IEnumerable<IEnumerable<IMultiRogueRow>> classifiedRows2 { get { return classifiedRows.Values.ToList(); } }
        public Classify(string hql, QueryMetaData metaData) : base(hql, metaData)
        {
            splitList.ForEach(x => classifyCols.Add(BaseColumn.ParseColumn(x.Value, metaData)));
        }
        public void ClassifyRow(IMultiRogueRow row)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IColumn col in classifyCols)
            {
                stringBuilder.Append(col.RetrieveStringValue(row.tableRefRows.ToSingleEnum()));
            }
            classifiedRows.FindAddIfNotFound(stringBuilder.ToString(), row);
        }
        public override string PrintDetails()
        {
            return "Classify";
        }
    }
}
