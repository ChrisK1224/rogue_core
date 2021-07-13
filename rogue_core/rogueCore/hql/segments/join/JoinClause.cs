using System;
using System.Collections.Generic;
using System.Diagnostics;
using FilesAndFolders;
using rogue_core.RogueCode.hql.hqlSegments.table;
using rogue_core.RogueCode.hql.hqlSegments.where;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hql.segments;
using rogue_core.rogueCore.hql.segments.selects;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.hqlOLD;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.id.rogueID.hqlIDs;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using static rogue_core.RogueCode.hql.hqlSegments.where.WhereClause;

namespace rogue_core.rogueCore.hql.hqlSegments.join
{
    public class JoinClause
    {
        public LocationColumn parentColumnRef;
        public LocationColumn localColumn;
        public Boolean joinAll { get { return localColumn.IsStar; } }
        public int level;
        public EvaluationTypes evaluationType {get;}
        public JoinClause(LocationColumn localTableRefCol, LocationColumn parentColumnRef, EvaluationTypes evalType)
        {
            //this.joinAll = joinAll;
            this.localColumn = localTableRefCol;
            this.parentColumnRef = parentColumnRef;
            this.evaluationType = evalType;
        }
        public JoinClause(String fullTxt)
        {
            string[] joinParts = fullTxt.Split(WhereClause.evalChars(), StringSplitOptions.None);
            int evalIndex = fullTxt.IndexOfAny(WhereClause.evalChars());
            evaluationType = (EvaluationTypes)fullTxt[evalIndex];
            parentColumnRef = new SelectColumn(joinParts[1].Trim());
            localColumn = new SelectColumn(joinParts[0].Trim());
        }
        public static JoinClause FromEncodedHQL(String humanHQL, Dictionary<String, int> tableRefNameToIDs)
        {
            //String joinPortion = stringHelper.get_string_between_first_occurs(humanHQL, "JOIN", "SELECT");
            EvaluationTypes thsType;
            String joinType = humanHQL.BeforeFirstSpace();
            switch (joinType.ToUpper())
            {
                case "ON":
                    thsType = EvaluationTypes.equal;
                    break;
                case "MERGE":
                    thsType = EvaluationTypes.merge;
                    break;
                case "ROWTOCOLUMN":
                    thsType = EvaluationTypes.valuePair;
                    break;
                default:
                    thsType = EvaluationTypes.equal;
                    break;
            }
            String joinPortion = humanHQL.AfterFirstSpace().Trim();
            String[] parts = joinPortion.Split('=');
            LocationColumn localColumn;
            parts[0] = parts[0].Trim();
            parts[1] = parts[1].Trim();
            if (parts[0].Trim().Equals("*"))
            {
                localColumn = new LocationColumn(parts[0]);
            }
            else
            {
                localColumn = LocationColumn.HumanToEncodedHQL(parts[0], tableRefNameToIDs);
            }
            LocationColumn parentColumn = LocationColumn.HumanToEncodedHQL(parts[1], tableRefNameToIDs);
            return new JoinClause(localColumn, parentColumn, thsType);
        }
        public String GetHQLText()
        {
            if (joinAll)
            {
                return "*" + (char)evaluationType + parentColumnRef.GetHQLText();
            }
            else
            {
                return localColumn.GetHQLText() + (char)evaluationType + parentColumnRef.GetHQLText();
            }
        }
        public String GetFullHQLText()
        {
            String hql = "";
            switch (evaluationType)
            {
                case EvaluationTypes.equal:
                    hql += " JOIN ON ";
                    break;
                case EvaluationTypes.merge:
                    hql += " JOIN MERGE ";
                    break;
                case EvaluationTypes.valuePair:
                    hql += " JOIN ROWTOCOLUMN ";
                    break;
            }
            if (joinAll)
            {
                hql += "  *" + " = " + parentColumnRef.GetHumanHQLText();
            }
            else
            {
                hql +=  localColumn.GetHumanHQLText() + " = " + parentColumnRef.GetHumanHQLText();
            }
            return hql;
        }
        public IEnumerable<IRogueRow> ChildRowSet(HQLTable hqlTable, Dictionary<String,int> parentMatchValues)
        {
            if (joinAll)
            {
                return hqlTable.Values;
            }
            else
            {
                //indexedLocalJoinValues[matrixTableID][topRow.parentMatchValue];
                //return hqlTable.indexedLocalJoinValues[parentMatrixTableID][parentMatchValue];
               // Trace.WriteLine(parentMatchValues[hqlTable.tableSegment.matrixTableID].ToDecodedRowID().ToString());
               // Trace.WriteLine(hqlTable.indexedParentValueMap[parentMatchValues[hqlTable.tableSegment.matrixTableID]].ToString());
                int parentValueToMatchOn = parentMatchValues[hqlTable.tableSegment.tableRefName];
                List<IRogueRow> foundList;
                if (hqlTable.indexedParentValueMap.TryGetValue(parentValueToMatchOn, out foundList))
                {
                    return foundList;
                    //return hqlTable.indexedParentValueMap[parentMatchValues[hqlTable.tableSegment.matrixTableID]];
                }
                else
                {
                    return new List<IRogueRow>();
                }
            }
        }
        public DecodedRowID CalcLocalValue(IRogueRow thsRow)
        {
            if (this.joinAll)
            {
                return 0;
            }
            else
            {
                return this.localColumn.CalcValue(thsRow);
            }
        }
        //public static String HumanToEncodedHQL(String humanHQL)
        //{
        //    // JOIN ON   * = Root.RogueColumnID
        //    String[] parts = humanHQL.Split(new char[0]);
        //    String hql = "";
        //    EvaluationTypes thsType;
        //    switch (parts[0].ToUpper())
        //    {
        //        case "ON":
        //            thsType = EvaluationTypes.equal;
        //            break;
        //        case "MERGE":
        //            thsType = EvaluationTypes.merge;
        //            break;
        //        case "ROWTOCOLUMN":
        //            thsType = EvaluationTypes.valuePair;
        //            break;
        //        default:
        //            thsType = EvaluationTypes.equal;
        //            break;
        //    }
        //    return thsType.ToString();
        //}
    }
}