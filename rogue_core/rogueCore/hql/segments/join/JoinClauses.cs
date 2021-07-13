using System;
using System.Collections.Generic;
using FilesAndFolders;
using rogue_core.rogueCore.hql;
using rogue_core.rogueCore.hql.segments;

namespace rogue_core.rogueCore.hql.hqlSegments.join
{
    public class JoinClauses : List<JoinClause>
    {
        public const String humanHQLSplitter = "JOIN ";
        public JoinClauses(String allClauses){
            String clausePortion = stringHelper.GetStringBetween(allClauses, "{","}");
            String[] clauses = clausePortion.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(String clause in clauses){
                this.Add(new JoinClause(clause));
            }
        }
        public JoinClauses(){

        }
        public String GetHQLText(){
            String hql = "{";
            foreach(JoinClause thsClause in this){
                hql += thsClause.GetHQLText();
            }
            hql+="}";
            return hql;
        }
        public static String HumanToEncodedHQL(String humanHQL, Dictionary<String, int> tableRefIDs)
        {
                // JOIN ON   * = Root.RogueColumnID
                
            JoinClauses thsClauses = new JoinClauses();
            thsClauses.Add(JoinClause.FromEncodedHQL(humanHQL, tableRefIDs));
            return thsClauses.GetHQLText();
            //return thsClauses.GetHQLText();
        }
    }
}