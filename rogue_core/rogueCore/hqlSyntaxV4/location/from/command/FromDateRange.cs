using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from.command
{
    public class FromDateRange : CommandFrom
    {
        public override IORecordID tableId { get { return 2100792; } }
        public const string commandNameIDConst = "DATE_RANGE";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override string commandNameID { get { return CodeMatchName; } }
        public FromDateRange(string tblTxt, QueryMetaData metaData) : base(tblTxt, metaData)
        {
            
        }
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            DateTime baseDate = DateTime.Parse(commandParams[0].GetValue(parentRow.tableRefRows.ToSingleEnum()));
            string diffType = commandParams[1].GetValue(parentRow.tableRefRows.ToSingleEnum());
            double diffNum = double.Parse(commandParams[2].GetValue(parentRow.tableRefRows.ToSingleEnum()));
            int interval = int.Parse(commandParams[3].GetValue(parentRow.tableRefRows.ToSingleEnum()));
            int currInterval = 0;
            DateTime finalDate;
            var firstRow = new ManualBinaryRow();
            firstRow.AddPair(2100797, baseDate.ToString());
            yield return firstRow;
            currInterval++;
            while (interval > currInterval)
            {
                var newRow = new ManualBinaryRow();
                switch (diffType.ToUpper())
                {
                    case "DAY":
                        finalDate = baseDate.AddDays(diffNum);
                        break;
                    case "HOUR":
                        finalDate = baseDate.AddHours(diffNum);
                        break;
                    case "YEAR":
                        finalDate = baseDate.AddYears(int.Parse(diffNum.ToString()));
                        break;
                    case "MINUTE":
                        finalDate = baseDate.AddMinutes(diffNum);
                        break;
                    default:
                        finalDate = baseDate;
                        break;
                }
                newRow.AddPair(2100797, finalDate.ToString());                
                yield return newRow;
                currInterval++;
                baseDate = finalDate;
            }
        }
    }
}
