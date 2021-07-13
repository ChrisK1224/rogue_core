using GroupDocs.Classification;
using rogue_core.rogueCore.binary;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands
{
    class SentimentAnalysis : ColumnCommand
    {
        public static SentimentClassifier sentimentClassifier = new SentimentClassifier();
        public static string CodeMatchName { get { return "SENTIMENT_ANALYSIS"; } }
        public SentimentAnalysis(string colTxt) : base(colTxt)
        {

        }
        public override string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            string val = paramColumns[0].GetValue(rows);
            if (val.Length > 100)
            {
                val = val.Substring(0, 100);
            }
            try
            {
                return sentimentClassifier.PositiveProbability(val).ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }
    }
}
