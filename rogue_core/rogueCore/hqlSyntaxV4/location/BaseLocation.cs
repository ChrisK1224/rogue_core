using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.column;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public abstract class BaseLocation : SplitSegment
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey> { LocationSplitters.colDivider, LocationSplitters.AsKey }; } }
        public BaseLocation(string txt, QueryMetaData metaData) : base(txt, metaData) 
        {
            
        }
        public static string GetValue(IRogueRow thsRow, ColumnRowID columnRowID)
        {
            if (thsRow.ITryGetValue(columnRowID) != null)
            {
                return thsRow.GetValueByColumn(columnRowID);
            }
            else
            {
                return "";
            }
        }
        public string GetAliasName()
        {
            string name = splitList.Where(x => x.Key == KeyNames.asKey).Select(x => x.Value).DefaultIfEmpty("").LastOrDefault();
            if (name.StartsWith("\""))
            {
                return name.Substring(1, name.Length - 2);
            }
            else
            {
                return name;
            }
        }
        
    }
}
