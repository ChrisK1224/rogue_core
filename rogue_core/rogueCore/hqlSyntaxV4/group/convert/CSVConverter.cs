using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group.convert
{
    class CSVConverter : SplitSegment, IGroupConvert
    {
        public const string codeMatchConst = "CSV";
        public static string codeMatchName { get { return codeMatchConst; } }
        public IORecordID tableId {get { return 2567217; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public CSVConverter(string hql, QueryMetaData metaData) : base(hql, metaData) { }
        public override string PrintDetails()
        {
            return ", CSV Converter, ";
        }
        public IEnumerable<IReadOnlyRogueRow> Transform(List<HQLLevel> topLevels)
        {
            string path = @"C:\Users\chris\Desktop\blah.csv";
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("\"" + String.Join("\",\"", topLevels[0].columnNames)  + "\"");
            foreach(var row in topLevels[0].rows)
            {
                csvBuilder.AppendLine("\"" + String.Join("\",\"", row.GetValueList().ToList().Select(x => x.Value)) + "\"");
            }
            File.WriteAllText(path, csvBuilder.ToString());
            var csvRow = new ManualBinaryRow();
            csvRow.AddPair(2567223, path);
            yield return csvRow;
            //csvRow.Add("CSV_Path", path);
            //topLevels[0].rows = new List<IMultiRogueRow>() { csvRow };
        }
        //public virtual IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        //{
        //    int rowCount = 0;
        //    int snapshotRowAmount = parentLvl.rows.Count;
        //    foreach (IRogueRow testRow in tableId.ToTable().StreamDataRows().TakeWhile(x => rowCount != limit.limitRows))
        //    {
        //        foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
        //        {
        //            if (whereClause.CheckWhereClause(idName, testRow, parentRow))
        //            {
        //                yield return NewRow(idName, testRow, parentRow);
        //            }
        //        }
        //        rowCount++;
        //    }
        //}
    }
}
