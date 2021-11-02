using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.group.convert
{
    class JsonConverter : SplitSegment, IGroupConvert
    {
        public const string codeMatchConst = "JSON";
        public static string codeMatchName { get { return codeMatchConst; } }
        public IORecordID tableId { get { return 2567217; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>(); } }
        public JsonConverter(string hql, QueryMetaData metaData) : base(hql, metaData) { }
        public override string PrintDetails()
        {
            return ", Json Converter, ";
        }
        public IEnumerable<IReadOnlyRogueRow> Transform(List<IHQLLevel> topLevels)
        {
            //string path = @"C:\Users\chris\Desktop\blah.csv";
            string path = @"C:\Users\chris\Desktop\jsonTest.json";
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("\"" + String.Join("\",\"", topLevels[0].ColumnNames()) + "\"");
            List<IMultiRogueRow> topRows = new List<IMultiRogueRow>();
            topLevels.ForEach(x => topRows.AddRange(x.rows));
            string jsonTxt = AsJsonResult(topRows);            
            File.WriteAllText(path, jsonTxt);
            var csvRow = new ManualBinaryRow();
            csvRow.AddPair(2567223, path);
            yield return csvRow;
        }
        public string AsJsonResult(List<IMultiRogueRow> topRows)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            int currLevel = 0;
            int itCount = 0;
            TimeSpan totalGetValTime = new TimeSpan(0);
            TimeSpan totalEndTime = new TimeSpan(0);
            TimeSpan totalIfEndTime = new TimeSpan(0);
            Action<IMultiRogueRow> openJson = (row) => {
                itCount++;
                if (row.levelNum > currLevel)
                {
                    json.Append("\"" + row.levelName + "\" : [");
                }
                currLevel = row.levelNum;
                if (row.GetValueList().ToList().Count == 1 && row.childRows.Count == 0)
                {
                    json.Append("\"" + row.GetValueAt(0).Replace("\\", "\\\\").Replace("\"", "\\\"") + "\",");
                }
                else
                {
                    json.Append("{");
                    foreach (var pair in row.GetValueList())
                    {
                        json.Append("\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\",");
                        //json += "\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\"", "\\\"").Replace("\\", "\\\\") + "\",";
                        //json += "\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\"", "\\\"") + "\",";
                    }
                }               
            };
            Action<IMultiRogueRow> closeJson = (row) =>
            {
                
                if (row.GetValueList().ToList().Count == 1 && row.childRows.Count == 0)
                {
                    json.Append("");
                }
                else
                {
                    json.Length--;
                    if (row.levelNum < currLevel)
                    {
                        json.Append("]");
                    }
                    json.Append("},");
                }
            };
            HQLQuery.IterateRows(topRows, openJson, closeJson);
            json.Length--;
                //(0, json.Length - 1);
            json.Append("]}");
            return json.ToString();
        }
    }
}
