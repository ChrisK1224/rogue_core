using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    public class StandardFrom : StandardLocation, IIdableFrom, ICalcableFromId, IOptionalDirect
    {
        public string idName { get { return tableRefName.ToUpper(); } }
        public bool IsIdable { get { return true; } }
        public IORecordID tableId { get; }
        public string tableRefName { get; }
        public StandardFrom(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {
            List<string> periodParts = splitList.Where(x => x.Key == KeyNames.period).Select(x => x.Value).ToList();
            Stopwatch bl = new Stopwatch();
            bl.Start();
            if (this.IsDirectID(tblTxt))
            {
                tableId = new IORecordID(this.GetDirectID(tblTxt));
            }
            else
            {
                bl.Stop();
                bl.Restart();
                tableId = (periodParts.Count == 1) ? new IORecordID(BinaryDataTable.ioRecordTable.GuessTableIDByName(periodParts[0])) : BinaryDataTable.ioRecordTable.DecodeTableName(periodParts.ToArray());
                bl.Stop();
                Console.WriteLine("WITHIN GUESSID:" + bl.ElapsedMilliseconds);
            }            
            tableRefName = (base.GetAliasName() == "" ) ? tableId.TableName() : GetAliasName();
            bl.Stop();
            Console.WriteLine("WITHIN STANDARDROM:" + bl.ElapsedMilliseconds);
        }
        public virtual IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, HQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IRogueRow testRow in tableId.ToTable().StreamDataRows().TakeWhile(x => rowCount != limit.limitRows))
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
        public override string PrintDetails()
        {
            return "idName:" + idName + ",tableID:"+ tableId.ToString();
        }
        public IORecordID CalcTableID(IMultiRogueRow row)
        {
            return tableId;
        }

        public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows, ILimit limit, IJoinClause joinClause)
        {
            throw new Exception("NOT DONE");
        }
    }
}
