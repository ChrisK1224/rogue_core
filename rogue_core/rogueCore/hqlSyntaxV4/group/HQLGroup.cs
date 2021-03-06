using rogue_core.rogueCore.hqlSyntaxV4.level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group
{
    public class HQLGroup : SplitSegment
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LevelSplitters.fromKey, LevelSplitters.insertKey, LevelSplitters.deleteKey }; } }
        List<HQLLevel> levels { get; } = new List<HQLLevel>(); 
        List<HQLLevel> topLevels { get { return levels.Where(x => x.parentLvlName == "").ToList(); } }
        public HQLGroup(string line, QueryMetaData metaData) : base(line, metaData)
        {
            //*This is weird that i Have to add key first because level should have own split nameeee
            this.splitList.ForEach(x => levels.Add(new HQLLevel(x.Value, metaData)));
            metaData.AddGroup(this);
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
    }
}
