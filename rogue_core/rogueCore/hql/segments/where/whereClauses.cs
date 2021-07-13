using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.RogueCode.hql;
using rogue_core.RogueCode.hql.hqlSegments.where;
using rogue_core.rogueCore.hql.segments;

namespace rogue_core.rogueCore.hql.hqlSegments.where
{
    public class WhereClauses : List<WhereClause> 
    {
        const char startWhere = '[';
        const char endWhere = ']';
        const char orSplitter = ',';
        const char andSplitter = '&';
        public const String humanHQLSplitter = "WHERE ";
        public CompareTypes compareType;
        public WhereClauses(String tableSegment){
            String whereClausePortion = stringHelper.GetStringBetween(tableSegment,startWhere.ToString(), endWhere.ToString());
            if (whereClausePortion.Contains("&"))
            {
                compareType = CompareTypes.and;
            }
            else
            {
                compareType = CompareTypes.or;
            }
            String[] clauses = whereClausePortion.Split(new[] { orSplitter, andSplitter }, StringSplitOptions.RemoveEmptyEntries);
            foreach(String clause in clauses){
                this.Add(new WhereClause(clause));
            }
        }
        public static String HumanToEncodedHQL(String humanHQL, Dictionary<String,int> tableRefIDs, String tableRefName)
        {
            String[] wheres = humanHQL.Split(new char[] { orSplitter, andSplitter }, StringSplitOptions.RemoveEmptyEntries);
            WhereClauses whereClauses = new WhereClauses();
            foreach (String thsWhere in wheres)
            {
                whereClauses.Add(WhereClause.HumanToEncodedHQL(thsWhere, tableRefIDs, tableRefName));
            }
            return whereClauses.GetHQLText();
        }
        public WhereClauses(){} 
        public String GetHQLText(){
            String hql = "[";
            foreach(WhereClause thsClause in this){
                hql += thsClause.GetHQLText() + ",";
            }
            if (this.Count > 0)
            {
                hql = hql.Substring(0, hql.Length - 1);
            }
            //hql = hql.Substring(0, hql.Length-1);
            hql += "]";
            return hql;
        }
        public String GetHQLFullText()
        {
            String encodedHQL = " WHERE ";
            foreach(WhereClause thsClause in this)
            {
                encodedHQL += thsClause.GetHQLFullText();
            }
            return encodedHQL;
        }
        public static String RootWhereClauses()
        {
            //* FIXME SOO BAD
            //* THis is for HUMANHQL to encoded when there is no join clause
            return "{*=Root.-1012}";
        }
        public enum CompareTypes
        {
            and = '&', or = ','
        }
    }
}