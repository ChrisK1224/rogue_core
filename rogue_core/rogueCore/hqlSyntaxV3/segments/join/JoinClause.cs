using System;
using System.Collections.Generic;
using files_and_folders;
using FilesAndFolders;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.segments.join
{
    public class JoinClause : IJoinClause
    {
        //*Mightn ot need this at all and just use TableGroup to send in top level info when needed for just one public function probss get datarows
        //*not done yet
        public bool isJoinSet { get { return true; } }
        public const String splitKey = "JOIN";
        public JoinTypes joinType { get; set; } = JoinTypes.inner;
        public ILocationColumn parentColumn { get; protected set; }
        StandardColumn localColumn { get; set; }
        public Boolean joinAll { get { return false; } }
        EvaluationTypes evaluationType { get; set; }
        public string parentTableRef { get { return isJoinSet ? parentColumn.colTableRefName.ToUpper() : ""; } }
        public string textColor { get { return "red"; } }
        string origText;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxLoad;
        public JoinClause(String segment) 
        {
            try
            {
                //isSet = (segment == "") ? JoinAll() : SetVariables(segment, queryStatement);
                origText = segment;
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
                        joinType = JoinTypes.to;
                        break;
                    default:
                        evaluationType = EvaluationTypes.equal;
                        break;
                }
                String joinPortion = segment.AfterFirstSpace().Trim();
                char test = segment[2];
                String[] parts = joinPortion.Split('=');
                localColumn = new StandardColumn(parts[0].Trim());
                parentColumn = new StandardColumn(parts[1].Trim());
                LocalSyntaxLoad = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxLoad = ErrorSyntaxParts; 
            }
            //localColumn = BaseLocation.LocationType(parts[0].Trim(), queryStatement);
            //parentColumn = BaseLocation.LocationType(parts[1].Trim(), queryStatement);
        }
        public IEnumerable<IMultiRogueRow> JoinRows(ILevelStatement filledLevel, IReadOnlyRogueRow testRow, int currRowCount)
        {
            //foreach (IMultiRogueRow row in filledLevel.indexedRows[parentColumn][parentTableRef].TryFindReturn(localColumn.GetValue(testRow).ToDecodedRowID()))
            foreach (IMultiRogueRow row in filledLevel.indexedRows[parentColumn].TryFindReturn(localColumn.GetValue(testRow).ToDecodedRowID()))
            {
                yield return row;
            }
        }
        //bool JoinAll(){
        //    evaluationType = EvaluationTypes.equal;
        //    localColumn = new StarColumn();
        //    LocalJoinRows = JoinAllFill;
        //    return false;
        //}
        //JoinClause(EvaluationTypes evaluationType, LocationColumn parentColumn, HQLqueryStatement queryStatement, LocationColumn localColumn = null){
        //    this.isSet = true;
        //    //this.queryStatement = queryStatement;
        //    this.evaluationType = evaluationType;
        //    this.localColumn = new LocationColumn(true, "");
        //    this.parentColumn = parentColumn;
        //}
        //void SetVariables(string segment, CoreQueryStatement queryStatement)
        //{
        //    //String joinType = segment.BeforeFirstSpace();
        //    switch (segment.BeforeFirstSpace().ToUpper())
        //    {
        //        case "ON":
        //            evaluationType = EvaluationTypes.equal;
        //            break;
        //        case "MERGE":
        //            evaluationType = EvaluationTypes.merge;
        //            break;
        //        case "ROWTOCOLUMN":
        //            evaluationType = EvaluationTypes.valuePair;
        //            break;
        //        case "MERGEFULL":
        //           evaluationType = EvaluationTypes.merge;
        //            joinType = JoinTypes.full;
        //            break;
        //        default:
        //            evaluationType = EvaluationTypes.equal;
        //            break;
        //    }
        //    String joinPortion = segment.AfterFirstSpace().Trim();
        //    String[] parts = joinPortion.Split('=');
        //    localColumn = BaseLocation.LocationType(parts[0].Trim(), queryStatement);
        //    parentColumn = BaseLocation.LocationType(parts[1].Trim(), queryStatement);
        //    //parentColumn = BaseLocation.LocationType(parts[0].Trim(), queryStatement);
        //    //if (!localColumn.isStar)
        //    //{
        //    //    parentColumn = BaseLocation.LocationType(parts[1].Trim(), queryStatement);
        //    //    LocalJoinRows = StandardJoinFill;
        //    //}
        //    //else
        //    //{
        //    //    //** SHould fix to accept only a table name with no column
        //    //    parentColumn = BaseLocation.LocationType(parts[1].Trim(), queryStatement);
        //    //    //parentColumn = new ColumnDirect(parts[1].Split('.')[0].Trim());
        //    //    LocalJoinRows = JoinAllFill;
        //    //}
        //    //return true;
        //}
        //public List<UIDecoratedTextItem> txtItems()
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
        //    items.Add(new UIDecoratedTextItem(origText, "black", "normal"));
        //    return items;
        //}
        //public enum JoinTypes
        //{
        //    inner, full, outer
        //}
        public enum JoinTypes : int
        {
            [StringValue("ON")] inner = 1,
            [StringValue("OUTER")] outer = 2,
            [StringValue("TO")] to = 3
        }
        //public Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<IMultiRogueRow>>>> indexedRows { get; } = new Dictionary<ColumnRowID, Dictionary<string, Dictionary<int, List<IMultiRogueRow>>>>();
        //IEnumerable<IMultiRogueRow> StandardJoinFill(IFilledLevel filledLevel, IRogueRow testRow, int currRowCount)
        //{
        //    foreach (IMultiRogueRow row in filledLevel.indexedRows[parentColumn.columnRowID][parentTableRef].TryFindReturn(localColumn.RetrieveStringValue(new Dictionary<string, IRogueRow>() { { localColumn.colTableRefName, testRow } }).ToDecodedRowID()))
        //    {
        //        yield return row;
        //    }
        //}
        //IEnumerable<IMultiRogueRow> JoinAllFill(IFilledLevel filledLvl, IRogueRow testRow, int currRowCount)
        //{
        //    for (int i = 0; i < currRowCount; i++)
        //    {
        //        yield return filledLvl.rows[i];
        //    }
        //}
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
        //internal static EvaluationTypes ByName(String evalName)
        //{
        //    switch (evalName.ToLower())
        //    {
        //        case "=":
        //            return EvaluationTypes.equal;
        //        case "!=":
        //            return EvaluationTypes.notEqual;
        //        case "$":
        //            return EvaluationTypes.merge;
        //        case "?":
        //            return EvaluationTypes.valuePair;
        //        default:
        //            return EvaluationTypes.valuePair;
        //    }
        //}
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxLoad(parentRow, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinType.GetStringValue(), IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //localColumn.LoadSyntaxParts(parentRow);
            //syntaxCommands.GetLabel(parentRow, "&nbsp;=&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //parentColumn.LoadSyntaxParts(parentRow);
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(parentColumn.UnsetParams());
            unsets.AddRange(localColumn.UnsetParams());
            return unsets;
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + " " + joinType.GetStringValue() + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            localColumn.LoadSyntaxParts(parentRow, syntaxCommands);
            syntaxCommands.GetLabel(parentRow, "&nbsp;=&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            parentColumn.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + JoinClause.splitKey + "&nbsp;" + origText, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        public void PreFill(QueryMetaData metaData, string assumedTableName)
        {
            try
            {
                localColumn.PreFill(metaData, assumedTableName);
                parentColumn.PreFill(metaData, assumedTableName);
                LocalSyntaxLoad = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxLoad = ErrorSyntaxParts;
            }
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