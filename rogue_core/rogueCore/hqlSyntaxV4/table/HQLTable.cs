using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.delete;
using rogue_core.rogueCore.hqlSyntaxV4.insert;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using static rogueCore.hqlSyntaxV4.join.IJoinClause;

namespace rogue_core.rogueCore.hqlSyntaxV4.table
{
    public class HQLTable : SplitSegment
    {
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { TableSplitters.whereKey, TableSplitters.limitKey, TableSplitters.joinKey, TableSplitters.combineKey, TableSplitters.fromKey, TableSplitters.insertKey, TableSplitters.deleteKey, TableSplitters.updateKey }; } }
        public string idName { get { return from.idName.ToUpper(); } }
        public string parentTableName { get { return joinClause.parentTableName; } }
        public IJoinClause joinClause { get; }
        //IWhereClause whereClause { get; }
        ILimit limit { get; }
        IFrom from { get; }
        internal List<Dictionary<string, IReadOnlyRogueRow>> rows { get; } = new List<Dictionary<string, IReadOnlyRogueRow>>();
        //public List<IColumn> IndexedWhereColumns { get { return whereClause.evalColumns.Where(iCol => !(iCol is ConstantColumn)).ToList(); } }
        //Dictionary<IColumn, IReadOnlyRogueRow> indexedRows = new Dictionary<IColumn, IReadOnlyRogueRow>();
        public IORecordID potentialTableID { get { return ((IIdableFrom)from).tableId; } }
        public HQLTable(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {
            //var stopwatch3 = new Stopwatch();
            //stopwatch3.Start(); 
            metaData.AddTable(this);
            //stopwatch3.Stop();
            //Console.WriteLine("Metaddat:" + stopwatch3.ElapsedMilliseconds);
            //stopwatch3.Restart();
            from = ParseFromClause(splitList.Where(x => x.Key == KeyNames.from || x.Key == KeyNames.combine || x.Key == KeyNames.insert || x.Key == KeyNames.delete || x.Key == KeyNames.update).FirstOrDefault(), metaData);
            //stopwatch3.Stop();
            //Console.WriteLine("FROM:" + stopwatch3.ElapsedMilliseconds);
            //whereClause = ParseWhereClause(splitList.Where(x => x.Key == KeyNames.where).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            joinClause = ParseJoinClause(splitList.Where(x => x.Key == KeyNames.join).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            limit = ParseLimitClause(splitList.Where(x => x.Key == KeyNames.limit).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
            //stopwatch3.Stop();
            //Console.WriteLine("TableTime " + idName + " : " + stopwatch3.ElapsedMilliseconds);
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(HQLLevel parentLvl, IWhereClause whereClause, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> AddRow)
        {
            foreach (var row in from.FilterAndStreamRows(limit, joinClause, whereClause, parentLvl, AddRow))
            {
                yield return row;
            }
        }
        //public IEnumerable<Dictionary<string, IReadOnlyRogueRow>> LoadTableRows(List<Dictionary<string, IReadOnlyRogueRow>> parentRows)
        //{
        //    foreach (var row in from.LoadTableRows(parentRows, limit, joinClause))
        //    {
        //        rows.Add(row);                
        //        yield return row;
        //    }
        //}
        public bool DoesColumnBelong(string columnName)
        {
            if (from is IIdableFrom)
            {
                var lst = BinaryDataTable.columnTable.AllColumnsPerTable(((IIdableFrom)from).tableId).Where(x => x.ColumnIDNameID() == columnName.ToUpper()).ToList();
                if (lst.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        IWhereClause ParseWhereClause(string whereTxt, QueryMetaData metaData)
        {
            whereTxt = whereTxt.Trim();
            if (whereTxt == "")
            {
                return new EmptyWhereClause(whereTxt, metaData);
            }
            else
            {
                return new WhereClause(whereTxt, metaData);
            }
        }
        IJoinClause ParseJoinClause(string joinTxt, QueryMetaData metaData)
        {
            joinTxt = joinTxt.Trim();
            //*Delete if clause eventually
            if (joinTxt.Contains("*"))
            {
                throw new Exception("UPDATE JOIN * CLAUSE DEPRECIATED");
            }
            if (joinTxt.Equals(""))
            {
                return new EmptyJoinClause(joinTxt, metaData);
            }
            else if (joinTxt.StartsWith(JoinTypes.to.GetStringValue()))
            {
                return new JoinToClause(joinTxt, metaData);
            }
            else
            {
                return new JoinClause(joinTxt, metaData);
            }
        }
        ILimit ParseLimitClause(string limitTxt, QueryMetaData metaData)
        {
            limitTxt = limitTxt.Trim();
            if (limitTxt.Equals(""))
            {
                return new EmptyLimit();
            }
            else
            {
                return new Limit(limitTxt, metaData);
            }
        }
        public static IFrom ParseFromClause(KeyValuePair<string, string> hqlPair, QueryMetaData metaData)
        {
            string hql = hqlPair.Value.Trim();
            //string afterFrom = hql.AfterFirstSpace().ToUpper();
            IFrom newTable;
            //**Weird logic. Group fufills IFrom to act like regular from when being referenced. 
            var testName = hql.BeforeFirstSpace();
            var fromGroup = metaData.GetGroupByName(hql.BeforeFirstSpace());
            if (fromGroup != null)
            {
                newTable = fromGroup;
            }
            else if (hql.StartsWith("EXECUTE("))
            {
                newTable = new ExecutableFrom(hql, metaData);
            }
            else if (hql.StartsWith("CONVERT("))
            {
                newTable = new InvertFrom(hql, metaData);
            }
            else if (hqlPair.Key == KeyNames.insert)
            {
                //newTable = new HQLInsert(hql, metaData);
                newTable = HQLInsert.GetInsertType(hql, metaData);
            }
            else if (hqlPair.Key == KeyNames.delete)
            {
                newTable = new HQLDelete(hql, metaData);
            }
            else if (hql.StartsWith("\""))
            {
                newTable = new ConstantFrom(hql, metaData);
            }//***FIXHERE***
            else if (hql.Contains("{"))
            {
                newTable = new EncodedFrom(hql, metaData);
            }
            else if (hql.Contains("(") && hql.Contains(")"))
            {
                newTable = CommandFrom.GetCommandTableType(hql.BeforeFirstChar('('), hql, metaData);
            }
            else
            {
                newTable = new StandardFrom(hql, metaData);
            }
            //lvl.tables.Add(newTable.idName, newTable);
            return newTable;
        }
        public override string PrintDetails()
        {
            return "idName:" + idName + ",ParentTableName:" + parentTableName;
        }
    }
}
