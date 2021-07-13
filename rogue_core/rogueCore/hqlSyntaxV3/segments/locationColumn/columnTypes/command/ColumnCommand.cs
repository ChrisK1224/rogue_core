using FilesAndFolders;
using hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable
{ 
    //*DONT THINK THIS IS READY OR USED YET
    abstract class ColumnCommand : CommandLocation, ILocationColumn
    {
        public string columnName { get { return base.name; } }
        public ColumnRowID columnRowID { get { return -1012; } }
        public string colTableRefName { get { return base.name; } }
        public bool isConstant { get { return false; } }
        public bool isEncoded { get { return false; } }
        public bool isStar { get { return false; } }
        SelectRow selectRow { get; }
        protected List<ISelectColumn> paramColumns { get { return selectRow.columnList; } }
        public string upperColTableRefName { get { return base.upperName; } }
        public ColumnCommand(string execTxt) : base(execTxt) 
        {
            string colTxt = FilesAndFolders.stringHelper.get_string_between_2(execTxt, "(", ")");
            selectRow = new SelectRow(colTxt);
                //MultiSymbolSegment<PlainList<ISelectColOrStar>, ISelectColOrStar>(SymbolOrder.symbolafter,colTxt, keys, GetSelectColType).segmentItems;
            //BaseLocation.LocationType(colTxt);
        }
        public static ColumnCommand GetCommandColumn(string colTxt)
        {
            Type parentType = typeof(ColumnCommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString(), x));            
            string parseTxt = colTxt.ToUpper();
            string idName = colTxt.BeforeFirstChar('(').ToUpper();
            switch (idName)
            {
                case "CURRENT_DATE":
                    return new CurrentDate(colTxt);
                case "FILE_CONTENT":
                    return new FileContent(colTxt);
                case "DATE_TO_EPOCH":
                    return new DateToEpoch(colTxt);
                case "EPOCH_TO_DATE":
                    return new EpochToDate(colTxt);
                case "SENTIMENT_ANALYSIS":
                    return new SentimentAnalysis(colTxt);
                case "SENTIMENT_CLASSIFICATION":
                    return new SentimentClassification(colTxt);
                default:
                    throw new Exception("Unknown command location type");
            }
                    
            //Select(x => x.GetProperties().Where(p => p.Name.ToUpper() == "IDNAME")
        }
        public abstract string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows);
        //{
        //    string result = base.RunExecProcedure(rows).ToArray()[0].GetValueAt(0);
        //    return result;
        //}
        //public void SecondaryLoad(string thsLevelName, Dictionary<string, IFrom> tableList)
        //{
        //    throw new Exception("NOT FINISHED EXEC COLUMN");
        //}
        public void PreFill(QueryMetaData metaData, string assumedTblName)
        {
            //throw new Exception("NOT FINISHED EXEC COLUMN");
            selectRow.PreFill(metaData, assumedTblName);
        }
        public IEnumerable<string> UnsetParams()
        {
            throw new Exception("NOT FINISHED EXEC COLUMN");
        }
    }
}
