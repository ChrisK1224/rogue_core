using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.order
{
    class OrderBy : SplitSegment, IOrderBy
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.colSeparator }; } }
        public int limitRows { get; protected set; } = -1;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        List<KeyValuePair<IColumn, string>> orderColumns { get; } = new List<KeyValuePair<IColumn, string>>();
        public OrderBy(String orderSegment, QueryMetaData metaData) : base(orderSegment, metaData)
        {
            try
            {
                foreach(var line in splitList)
                {
                    var split = line.Value.Split(' ');
                    if(split.Length > 1)
                    {
                        orderColumns.Add(new KeyValuePair<IColumn,string>(BaseColumn.ParseColumn(split[0], metaData), split[1]));
                    }
                    else
                    {
                        orderColumns.Add(new KeyValuePair<IColumn, string>(BaseColumn.ParseColumn(split[0], metaData), "ASC"));
                    }
                }
            }
            catch (Exception ex)
            {
                //LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void OrderRows(List<IMultiRogueRow> rows)
        {
            if(orderColumns.Count == 0)
            {
                //return rows;
            }
            else
            {
                //string bbl = rows[0].GetValue(orderColumns[0].Key);
                rows.Sort((x, y) => double.Parse(x.GetValue(orderColumns[0].Key)).CompareTo(double.Parse(y.GetValue(orderColumns[0].Key))));
                //return rows.OrderBy(x => x.GetValue(orderColumns[0].Key)).ToList();
            }
            
                //rows.Sort((x, y) => x.GetValue(orderColumns[0].Key).CompareTo(y.GetValue(orderColumns[0].Key)));
                //string.Compare(, y.GetValue(orderColumns[0].Key)));
            //rows.Reverse();
        }
        public override string PrintDetails()
        {
            return "ORDER BY: ";
        }
    }
}
