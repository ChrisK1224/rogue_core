using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class SentimentClassification : ColumnCommand
    {
        public static string CodeMatchName { get { return "SENTIMENT_CLASSIFICATION"; } }
        public SentimentClassification(string colTxt) : base(colTxt)
        {

        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            string val = paramColumns[0].GetValue(rows);
            if(val.Length > 100)
            {
                val = val.Substring(0, 100);
            }
            try
            {
                return SentimentAnalysis.sentimentClassifier.Classify(val).BestClassName;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
            
        }
    }
}
