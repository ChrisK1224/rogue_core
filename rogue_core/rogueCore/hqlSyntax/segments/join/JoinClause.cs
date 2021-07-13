using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using files_and_folders;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.select;
using static rogueCore.hqlSyntax.segments.table.TableStatement;
using FilesAndFolders;
using rogueCore.hqlSyntax.segments.human;

namespace rogueCore.hqlSyntax.segments.join
{
    // public class JoinClause : OptionalSplitSegment
    public class JoinClause : OptionalSplitSegment
    {
        //*Mightn ot need this at all and just use TableGroup to send in top level info when needed for just one public function probss get datarows
        //*not done yet
        public const String splitKey = "JOIN";
        internal JoinTypes joinType { get; private set; } = JoinTypes.inner;
        public LocationColumn parentColumn { get; protected set; }
        public LocationColumn localColumn { get; protected set; }
        //HQLMetaData metaData;
        //public String tableRefName { private get; set; }
        //public Func<IRogueRow, FilledTable, IEnumerable<FilledSelectRow>> IndexedParentGroup;
        //public Func<IRogueRow, FilledSelectRow, FilledSelectRow> NewRow;
        internal Boolean joinAll { get { return localColumn.isStar; } }
        public EvaluationTypes evaluationType { get; protected set; }
        internal JoinClause(String segment, HQLMetaData metaData) : base(segment, metaData) { }
        public static JoinClause JoinAll(string headerName, HQLMetaData metaData){
            return new JoinClause(EvaluationTypes.equal, new LocationColumn(true, headerName), metaData);
        }
        JoinClause(EvaluationTypes evaluationType, LocationColumn parentColumn, HQLMetaData metaData, LocationColumn localColumn = null){
            this.isSet = true;
            //this.metaData = metaData;
            this.evaluationType = evaluationType;
            this.localColumn = new LocationColumn(true, "");
            this.parentColumn = parentColumn;
        }
        protected override void SetVariables(HQLMetaData metaData)
        {
            //String joinType = segment.BeforeFirstSpace();
            switch (segment.BeforeFirstSpace().ToUpper())
            {
                case "ON":
                    evaluationType = EvaluationTypes.equal;
                    break;
                case "MERGE":
                    evaluationType = EvaluationTypes.merge;
                    break;
                case "ROWTOCOLUMN":
                    evaluationType = EvaluationTypes.valuePair;
                    break;
                case "MERGEFULL":
                   evaluationType = EvaluationTypes.merge;
                    joinType = JoinTypes.full;
                    break;
                default:
                    evaluationType = EvaluationTypes.equal;
                    break;
            }
            String joinPortion = segment.AfterFirstSpace().Trim();
            String[] parts = joinPortion.Split('=');

            localColumn = new LocationColumn(parts[0].Trim(), metaData);
            if (!localColumn.isStar)
            {
                parentColumn = new LocationColumn(parts[1].Trim(), metaData);
            }
            else
            {
                parentColumn = new LocationColumn(true, parts[1].Split('.')[0].Trim());
            }
        }
        internal enum JoinTypes
        {
            inner, full, outer
        }
        /*protected void SetParentRowJoinClause()
        {
            if (isSet == false)
            {
                IndexedParentGroup = JoinAllFilter;
                NewRow = EqualSelectRow;
            }
            else
            {
                switch (evaluationType)
                {
                    case EvaluationTypes.equal:
                        NewRow = EqualSelectRow;
                        break;
                    case EvaluationTypes.merge:
                        NewRow = MergeSelectRow;
                        break;
                }
                if (joinAll)
                {
                    IndexedParentGroup = JoinAllFilter;
                }
                else
                {
                    IndexedParentGroup = StandardFilter;
                }
            }
        }
        /*IEnumerable<FilledSelectRow> JoinAllFilter(IRogueRow row, DataRows parents)
        {
            return parents.Rows();
        }
        IEnumerable<FilledSelectRow> StandardFilter(IRogueRow row, DataRows parents)
        {
            //*might need to do and TryFindReturn and return empty list if not found. I think will need this for a valid situation but leaving until situation arises
            return parents.GetIndexedRows(parentColumn.columnRowID, localColumn.CalcValue(row));
            //return parents[localColumn.CalcValue(row)];
        }
        FilledSelectRow EqualSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return new FilledSelectRow(tableRefName, selectRow, baseRow, parentRow);
        }
        FilledSelectRow MergeSelectRow(IRogueRow baseRow, FilledSelectRow parentRow)
        {
            return parentRow.MergeRow(tableRefName, selectRow, baseRow);
        }
        List<FilledSelectRow> EqualSelect(SelectRow rowOutline, IRogueRow baseRow, DataRows parents)
        {
            List<FilledSelectRow> rows = new List<FilledSelectRow>();
            foreach (FilledSelectRow parentRow in FilterRows(baseRow, parents))
            {
                rows.Add(new FilledSelectRow(tableRefName, rowOutline, baseRow, parentRow));
            }
            return rows;
        }
        List<FilledSelectRow> MergeSelect(SelectRow rowOutline, IRogueRow baseRow, DataRows parents)
        {
            List<FilledSelectRow> rows = new List<FilledSelectRow>();
            foreach (FilledSelectRow parentRow in FilterRows(baseRow, parents))
            {
                FilledSelectRow newRow = parentRow.MergeRow(tableRefName, rowOutline, baseRow);
                rows.Add(newRow);
            }
            return rows;
        }
        //*this is for top join and parents are assumed null in this case
        List<FilledSelectRow> NoJoinSelect(SelectRow rowOutline, IRogueRow baseRow, DataRows parents)
        {
            List<FilledSelectRow> rows = new List<FilledSelectRow>();
            foreach (FilledSelectRow parentRow in JoinAllFilter(baseRow, parents))
            {
                rows.Add(new FilledSelectRow(tableRefName, rowOutline, baseRow, parentRow));
            }
            return rows;
        }*/
        /*protected void JoinToParent()
        {
            JoinClause parentJoin = HQLQueryTwo.allTables[parentColumn.colTableRefName].joinClause;
            level = parentJoin.level +1;
            parentJoin.SetChildJoin(this);
        }
        
        public void SetChildJoin(JoinClause childJoin)
        {
            childJoins.Add(childJoin);
            //*May need to do addiional things here like check join type and add columns accordingly
        }*/
        //public JoinClause(Dictionary<String,String> tableSegments)
        //public static JoinClause FromEncodedHQL(String humanHQL, Dictionary<String, int> tableRefNameToIDs)
        //{
        //    //String joinPortion = stringHelper.get_string_between_first_occurs(humanHQL, "JOIN", "SELECT");
        //    EvaluationTypes thsType;
        //    String joinType = humanHQL.BeforeFirstSpace();
        //    switch (joinType.ToUpper())
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
        //    String joinPortion = humanHQL.AfterFirstSpace().Trim();
        //    String[] parts = joinPortion.Split('=');
        //    LocationColumn localColumn;
        //    parts[0] = parts[0].Trim();
        //    parts[1] = parts[1].Trim();
        //    if (parts[0].Trim().Equals("*"))
        //    {
        //        localColumn = new LocationColumn(parts[0]);
        //    }
        //    else
        //    {
        //        localColumn = LocationColumn.HumanToEncodedHQL(parts[0], tableRefNameToIDs);
        //    }
        //    LocationColumn parentColumn = LocationColumn.HumanToEncodedHQL(parts[1], tableRefNameToIDs);
        //    return new JoinClause(localColumn, parentColumn, thsType);
        //}
        //public String GetHQLText()
        //{
        //    if (joinAll)
        //    {
        //        return "*" + (char)evaluationType + parentColumnRef.GetHQLText();
        //    }
        //    else
        //    {
        //        return localColumn.GetHQLText() + (char)evaluationType + parentColumnRef.GetHQLText();
        //    }
        //}
        //public String GetFullHQLText()
        //{
        //    String hql = "";
        //    switch (evaluationType)
        //    {
        //        case EvaluationTypes.equal:
        //            hql += " JOIN ON ";
        //            break;
        //        case EvaluationTypes.merge:
        //            hql += " JOIN MERGE ";
        //            break;
        //        case EvaluationTypes.valuePair:
        //            hql += " JOIN ROWTOCOLUMN ";
        //            break;
        //    }
        //    if (joinAll)
        //    {
        //        hql += "  *" + " = " + parentColumnRef.GetHumanHQLText();
        //    }
        //    else
        //    {
        //        hql +=  localColumn.GetHumanHQLText() + " = " + parentColumnRef.GetHumanHQLText();
        //    }
        //    return hql;
        //}
        //public IEnumerable<IRogueRow> ChildRowSet(HQLTable hqlTable, Dictionary<String,int> parentMatchValues)
        //{
        //    if (joinAll)
        //    {
        //        return hqlTable.Values;
        //    }
        //    else
        //    {
        //        int parentValueToMatchOn = parentMatchValues[hqlTable.tableSegment.tableRefName];
        //        List<IRogueRow> foundList;
        //        if (hqlTable.indexedParentValueMap.TryGetValue(parentValueToMatchOn, out foundList))
        //        {
        //            return foundList;
        //        }
        //        else
        //        {
        //            return new List<IRogueRow>();
        //        }
        //    }
        //}
        //public DecodedRowID CalcLocalValue(IRogueRow thsRow)
        //{
        //    if (this.joinAll)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return this.localColumn.CalcValue(thsRow);
        //    }
        //}
    }
}