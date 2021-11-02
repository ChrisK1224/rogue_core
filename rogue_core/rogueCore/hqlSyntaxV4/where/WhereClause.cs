using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.where
{
   public class WhereClause : SplitSegment, IWhereClause
    {
        //**removed parent split keys WhereClauseSplitters.whereOpenParen, WhereClauseSplitters.whereCloseParen
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { WhereClauseSplitters.whereAnd, WhereClauseSplitters.whereOr }; } }
        List<List<KeyValuePair<string, WhereSegment>>> segmentItems = new List<List<KeyValuePair<string,WhereSegment>>>();
        //public override string[] splitters { get { return new string[4] { andMarker, orMarker, startMarker, endMarker }; } }
        public WhereClause(string whereTxt, QueryMetaData metaData) : base(whereTxt, metaData) 
        {
            //*Still needs updating to handle when groups within groups
            var activeGroup = new List<KeyValuePair<string, WhereSegment>>();
            foreach (var seg in splitList)
            {
                switch (seg.Key)
                {
                    case KeyNames.openParenthensis:
                    case KeyNames.startKey:
                        activeGroup = new List<KeyValuePair<string, WhereSegment>>();
                        segmentItems.Add(activeGroup);
                        activeGroup.Add(new KeyValuePair<string, WhereSegment>("", new WhereSegment(seg.Value, metaData)));
                        break;
                    case KeyNames.whereAndKey:
                    case KeyNames.whereOrKey:
                        activeGroup.Add(new KeyValuePair<string, WhereSegment>(seg.Key, new WhereSegment(seg.Value, metaData)));
                        break;
                    case KeyNames.closeParenthesis:
                        break;
                }
            }
        }
        //** NOT DONE
        public bool CheckWhereClause(string thsTblRef, IReadOnlyRogueRow thsRow, IMultiRogueRow fullRow)
        {
            bool valid = true;
            foreach(var lstSegs in segmentItems)
            {
                foreach(var seg in lstSegs)
                {
                    valid = seg.Value.IsValid(thsTblRef, thsRow, fullRow);
                    if (!valid)
                    {
                        return false;
                    }
                }
            }
            return valid;
        }
        public bool CheckWhereClause(IMultiRogueRow fullRow)
        {
            bool valid = true;
            foreach (var lstSegs in segmentItems)
            {
                foreach (var seg in lstSegs)
                {
                    valid = seg.Value.IsValid(fullRow);
                    if (!valid)
                    {
                        return false;
                    }
                }
            }
            return valid;
        }
        public override string PrintDetails()
        {
            return "";
        }
        public List<IColumn> evalColumns { get { List<IColumn> lstCols = new List<IColumn>(); segmentItems.ForEach(whereSeg => whereSeg.ForEach(single => lstCols.AddRange(single.Value.foreignColumn.columns))); return lstCols.Distinct().ToList(); } }
    }
}
