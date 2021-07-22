using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    public class SentimentClassification : CommandLocation, IColumn
    {
        public static string CodeMatchName { get { return "SENTIMENT_CLASSIFICATION"; } }
        public string columnName { get { return name; } }
        public SentimentClassification(string colTxt, QueryMetaData metaData)  : base(colTxt, metaData)
        {

        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            string val = commandParams[0].GetValue(rows);
            if (val.Length > 100)
            {
                val = val.Substring(0, 100);
            }
            try
            {
                return SentimentAnalysis.sentimentClassifier.Classify(val).BestClassName;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}
