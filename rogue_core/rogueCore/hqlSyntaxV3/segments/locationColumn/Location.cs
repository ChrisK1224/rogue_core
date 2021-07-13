using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using System;
using System.Text.RegularExpressions;
using rogue_core.rogueCore.id;
using rogueCore.hqlSyntaxV3.segments.namedLocation;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn
{
    //*Need to get rid of this only used for degraded FROM
    abstract class Location<IDType> where IDType : RowID
    {
        internal const string fullColumnSplitter = ".";
        internal const string columnAliasSep = " AS ";
        protected bool isDirectID;
        public bool isEncoded { get;protected set; }
        //protected bool isDirectID;
        public bool isStar { get; private set; }
        bool isUnsetParam;
        public bool isConstant { get; private set; }
        protected bool isExecutable { get; private set; }
        //protected IDableColumn encodedCol { get; private set; }
        protected string executableName { get; private set; }
        protected string[] execParams { get; private set; }       
        //Func<string, IDType> DirectIDRetreival;
        protected QueryMetaData refTbls { get; private set; }
        SelectHQLStatement queryStatement;
        protected IDType ID { get; set; }
        //string colTableRefName { get; set; }
        //protected string[] items;
        protected NamedLocation aliasName { get; private set; }
        protected string directName { get; private set; }
        //protected string tableRefName;
        protected string constValue { get; private set; }
        protected string name;
        //string lastSeg { get { return items[items.Length - 1]; } }
        //*DELETE THIS ONE
        public Location(string locTxt, SelectHQLStatement qry)
        {
            this.queryStatement = qry;
            //this.refTbls = qry.queryMetaData;
            aliasName = new NamedLocation(locTxt);
            locTxt = aliasName.remainingTxt.Trim(); 
            //** Known probably can't handle a encoded col with a diredtID within brackets 
            //items = Regex.Split(locTxt, @"\.(?=([^(\]|\}|\"")]*(\[|\{|\"")[^(\[|\{|\"")]*(\]|\}|\""))*[^(\]|\}|\"")]*$)", RegexOptions.ExplicitCapture);
            //string lastSeg = items[items.Length - 1];
            //isStar = SetIsStar(locTxt);
            //isExecutable = SetExecute(locTxt); //Regex.IsMatch(locTxt.ToUpper(), "(EXECUTE)(?=(?:[^\"]|\"[^\"]*?\")*?$)");
            //isConstant = SetConstant(lastSeg); // TestAndSet(@"^"".*""?",ref lastSeg);            
            //isDirectID = SetDirectID(lastSeg);  //isDirectID = SetDirectID(@"^\[.*\]?", ref lastSeg);
            //isEncoded = SetEncoded(ref lastSeg);
            isUnsetParam = locTxt.StartsWith("@") ? true : false;
            //*Encoded must have an aliasName
            //if(!isDirectID && !isExecutable && !isStar)
            //{
            //    name = (aliasName.isNameSet) ? aliasName.Name : lastSeg;
            //    directName = lastSeg;
            //}
            //else if(isExecutable)
            //{
            //    name = aliasName.Name;
            //}
            //*Set details based on options
            //if (isDirectID && isEncoded) 
            //{ 
            //    //DirectIDRetreival = OptionEncodedDirectID;
            //    name = (aliasName.isNameSet) ? aliasName.Name : refTbls.currTableRef;
            //} 
            //else if (isDirectID && !isEncoded) 
            //{ 
            //    ID = NameToID(lastSeg);
            //    //DirectIDRetreival = OptionDirectNotEncoded;
            //    directName = IDToName(ID);
            //    name = (aliasName.isNameSet) ? aliasName.Name : directName;
            //} 
            //else if (!isDirectID && isEncoded)
            //{ 
            //    //DirectIDRetreival = OptionEncodedStandard;
            //} 
            //else if(!isConstant && !isStar)
            //{ 
            //    ID = NameToID(items);
            //    //DirectIDRetreival = OptionStandard;
            //}
            //DirectIDRetreival = (isDirectID) ? DirectIDRetreival = OptionEncodedDirectID : OptionEncodedStandard;                       
        }
        protected abstract void SetNameAndID();
        //{
        //    if (!isDirectID)
        //    {
        //        name = (aliasName.isNameSet) ? aliasName.Name : lastSeg;
        //    }
        //    else
        //    {
        //        name = (aliasName.isNameSet) ? aliasName.Name : IDToName(ID);
        //    }
        //}
        protected abstract string IDToName(IDType id);
        protected abstract IDType NameToID(string[] ids);
        protected abstract IDType NameToID(string directID);
        //protected void ResetEncodedID(IDType id)
        //{
        //    ID = id;
        //}
        //protected Location(ColumnRowID thsColID, bool isDirectID, string colTblRefName)
        //{
        //    this.columnRowID = thsColID;
        //    this.isDirectID = isDirectID;
        //    this.colTableRefName = colTblRefName;
        //    OptionValueRetrieval = OptionStandardRetrieval;
        //}
        //bool SetExecute(string humanHQL)
        //{
        //    if (humanHQL.ToUpper().StartsWith("EXECUTE("))
        //    {
        //        //***Need to account for when in quotes and if no parameters then its between 2 parenthesis**
        //        executableName = stringHelper.get_string_between_2(humanHQL, "(", ",");
        //        string execParamTxt = stringHelper.get_string_between_2(humanHQL, ",", "\")").Trim();
        //        execParams = Regex.Split(execParamTxt, MutliSegmentEnum.GetOutsideQuotesPattern(new string[1] { "," }));
        //        ID = NameToID("-1012");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //bool SetConstant(string humanHQL)
        //{
        //    if (Regex.IsMatch(humanHQL, @"^"".*""?"))
        //    {
        //        constValue = humanHQL.TrimFirstAndLastChar();
        //        ID = NameToID("-1012");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        
        //bool SetEncoded(ref string colTxt)
        //{
        //    //if (Regex.IsMatch(@"^\{.*\}?", colTxt))
        //    if(colTxt.StartsWith("{") && colTxt.EndsWith("}"))
        //    {
        //        colTxt = colTxt.TrimFirstAndLastChar();
        //        encodedCol = new IDableColumn(colTxt, queryStatement);
        //        ID = NameToID("0");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //bool SetIsStar(string colTxt)
        //{
        //    if (colTxt.Equals("*"))
        //    {
        //        ID = NameToID("-1012");
        //        //tableRefName = queryStatement.currTableRefName;
        //        return true;
        //    }
        //    else { return false; }
        //}
        //bool TestAndSet(string RegexPattern, ref string locTxt)
        //{
        //    if (Regex.IsMatch(locTxt, RegexPattern))
        //    {
        //        locTxt = locTxt.TrimFirstAndLastChar();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //*Below two methods are for directID. If directID just returns the val as ColumnRowID otherwise gets the column by column name
        //IDType OptionEncodedDirectID(string val)
        //{
        //    return NameToID(val);
        //}
        //IDType OptionEncodedStandard(string val)
        //{
        //    int parentID = refTbls.TryGetID(val);
        //    //queryStatement.tableRefIDs.TryGetValue(name.ToUpper(), out parentID);
        //    return NameToID(val);
        //}
        //IDType OptionStandard(string val)
        //{
        //    int parentID = refTbls.TryGetID(val);
        //    //queryStatement.tableRefIDs.TryGetValue(name.ToUpper(), out parentID);
        //    return NameToID(val);
        //}
        //IDType OptionDirectNotEncoded(string val)
        //{
        //    return ID;
        //}
    }
}
