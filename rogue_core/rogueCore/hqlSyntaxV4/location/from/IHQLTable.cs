//using FilesAndFolders;
//using rogue_core.rogueCore.binary;
//using rogue_core.rogueCore.hqlSyntaxV4.level;
//using rogue_core.rogueCore.hqlSyntaxV4.limit;
//using rogue_core.rogueCore.hqlSyntaxV4.where;
//using rogueCore.hqlSyntaxV4.join;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using static rogueCore.hqlSyntaxV4.join.IJoinClause;

//namespace rogue_core.rogueCore.hqlSyntaxV4.table
//{
//    public interface IHQLTable
//    {
//        string idName { get; }
//        string defaultName { get; }
//        IJoinClause joinClause { get; set; }
//        IWhereClauses whereClause { get; set; }
//        ILimit limit { get; set; }
//        string tablePortionTxt { get; set; }
//    }
//    static class TableExtender
//    {
//        const string where = "WHERE";
//        const string limit = "LIMIT";
//        const string join = "JOIN";
//        static string from { get { return HQLLevel.from; } }
//        static string combine { get { return HQLLevel.combine; } }
//        public static string[] Splitters(this IHQLTable tbl)
//        {
//            return new string[5] { where, limit, join, combine, from };
//        }
//        public static void Initialize(this IHQLTable thsTable, QueryMetaData metaData)
//        {
//            var segs = this.SplitList(thsTable.origTxt);
//            thsTable.tablePortionTxt = segs.Where(x => x.Key == from || x.Key == combine).Select(x => x.Value).FirstOrDefault();
//            var nameLoc = new NamedLocation(thsTable.tablePortionTxt, metaData);            
//            thsTable.joinClause = ParseJoinClause(thsTable, segs.Where(x => x.Key == join).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
//            thsTable.limit = ParseLimitClause(thsTable, segs.Where(x => x.Key == limit).Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault(), metaData);
//        }        
//        public static IJoinClause ParseJoinClause(this IHQLTable thsTable, string joinTxt, QueryMetaData metaData)
//        {
//            joinTxt = joinTxt.Trim().AfterFirstSpace();
//            //*Delete if clause eventually
//            if (joinTxt.Contains("*"))
//            {
//                throw new Exception("UPDATE JOIN * CLAUSE DEPRECIATED");
//            }
//            if (joinTxt.Equals(""))
//            {
//                return new EmptyJoinClause(joinTxt, metaData);
//            }
//            else if (joinTxt.StartsWith(JoinTypes.to.GetStringValue()))
//            {
//                return new JoinToClause(joinTxt, metaData);
//            }
//            else
//            {
//                return new JoinClause(joinTxt, metaData);
//            }
//        }
//        public static ILimit ParseLimitClause(this IHQLTable thsTable, string limitTxt, QueryMetaData metaData)
//        {
//            limitTxt = limitTxt.Trim();
//            if (limitTxt.Equals(""))
//            {
//                return new EmptyLimit();
//            }
//            else
//            {
//                return new Limit(limitTxt, metaData);
//            }
//        }
//    }
//}
