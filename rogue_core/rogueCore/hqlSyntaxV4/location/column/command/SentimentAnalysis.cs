using GroupDocs.Classification;
using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class SentimentAnalysis : CommandLocation, IColumn
    {
        public static SentimentClassifier sentimentClassifier = new SentimentClassifier();
        public string columnName { get { return name; } }
        public static string CodeMatchName { get { return "SENTIMENT_ANALYSIS"; } }
        public SentimentAnalysis(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {

        }
        public string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows)
        {
            string val = commandParams[0].GetValue(rows);
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
