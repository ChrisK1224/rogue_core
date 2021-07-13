using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.table.commandTables
{
    class DateRange : CommandTableStatement
    {
        public override IORecordID tableID { get { return 2100792; } }

        public const string commandNameIDConst = "DATE_RANGE";
        public override string commandNameID { get { return commandNameIDConst; } }
        public DateRange(string tblTxt) : base(tblTxt)
        {
            
        }
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {            
            DateTime baseDate = DateTime.Parse(parentRow.GetValue(parameters[0]));
            string diffType = parentRow.GetValue(parameters[1]);
            double diffNum = double.Parse(parentRow.GetValue(parameters[2]));
            int interval = int.Parse(parentRow.GetValue(parameters[3]));
            int currInterval = 0;
            DateTime finalDate;
            while(interval > currInterval)
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
