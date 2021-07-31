using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.group.convert;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.misc.reflector;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group
{
    public class HQLGroup : SplitSegment, IIdableFrom, IQueryableDataSet
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { GroupSplitters.convertKey, LevelSplitters.fromKey, LevelSplitters.insertKey, LevelSplitters.deleteKey }; } }
        List<HQLLevel> _levels { get; } = new List<HQLLevel>(); 
        public List<HQLLevel> topLevels { get { return _levels.Where(x => x.parentLvlName == "").ToList(); } }
        public IReadOnlyCollection<HQLLevel> levels { get { return _levels; } }
        IGroupConvert converter { get; }
        public List<IMultiRogueRow> rows { get; set; } = new List<IMultiRogueRow>();
        public string dataSetName { get; }
        public string idName { get { return dataSetName.ToUpper(); } }
        public IORecordID tableId{  get { return converter.tableId; }}
        public HQLGroup(string line, QueryMetaData metaData) : base(line, metaData)
        {
            dataSetName = splitList.Where(x => x.Key == KeyNames.startKey).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            this.splitList.Where(x=>x.Key != KeyNames.convert && x.Key != KeyNames.startKey).ToList().ForEach(x => _levels.Add(new HQLLevel(x.Value, metaData)));            
            metaData.AddGroup(this);
            string convertStr = this.splitList.Where(x => x.Key == KeyNames.convert).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
            string convertKey = convertStr.AfterFirstSpace();
            converter = Reflector.GetNewGroupConvert(convertKey, convertStr,dataSetName, metaData);              
        }
        public void Fill()
        {
            foreach(var lvl in topLevels)
            {
                lvl.Fill();
            }            
        }
        public override string PrintDetails()
        {
            return "";
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            converter.Transform(topLevels);
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IReadOnlyRogueRow testRow in converter.Transform(topLevels).TakeWhile(x => rowCount != limit.limitRows))
            {
                foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                {
                    if (whereClause.CheckWhereClause(idName, testRow, parentRow))
                    {
                        yield return NewRow(idName, testRow, parentRow);
                    }
                }
                rowCount++;
            }
        }
    }
}
