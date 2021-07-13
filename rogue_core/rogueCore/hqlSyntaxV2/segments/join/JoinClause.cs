using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using files_and_folders;
using rogue_core.rogueCore.row;
using static rogueCore.hqlSyntaxV2.segments.table.TableStatement;
using FilesAndFolders;

namespace rogueCore.hqlSyntaxV2.segments.join
{
    public class JoinClause
    {
        //*Mightn ot need this at all and just use TableGroup to send in top level info when needed for just one public function probss get datarows
        //*not done yet
        internal bool isSet;
        public const String splitKey = "JOIN";
        internal JoinTypes joinType { get; private set; } = JoinTypes.inner;
        public LocationColumn parentColumn { get; protected set; }
        public LocationColumn localColumn { get; protected set; }
        internal Boolean joinAll { get { return ((isSet == false) ? true: localColumn.isStar); } }
        public EvaluationTypes evaluationType { get; protected set; }
        public List<string> symbolMarkers { get { return new List<string>() { splitKey}; } }
        public string textColor { get { return "red"; } }
        string origText;
        internal JoinClause(String segment, HQLMetaData metaData) 
        {
            isSet = (segment == "") ? JoinAll() : SetVariables(segment, metaData);
            origText = segment;
        }
        bool JoinAll(){
            evaluationType = EvaluationTypes.equal;
            localColumn = new LocationColumn(true, "");
            return false;
        }
        //JoinClause(EvaluationTypes evaluationType, LocationColumn parentColumn, HQLMetaData metaData, LocationColumn localColumn = null){
        //    this.isSet = true;
        //    //this.metaData = metaData;
        //    this.evaluationType = evaluationType;
        //    this.localColumn = new LocationColumn(true, "");
        //    this.parentColumn = parentColumn;
        //}
        bool SetVariables(string segment, HQLMetaData metaData)
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
            return true;
        }
        public List<UIDecoratedTextItem> txtItems()
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
            items.Add(new UIDecoratedTextItem(origText, "black", "normal"));
            return items;
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