using FilesAndFolders;
using Microsoft.ML;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.insert.insertMethods;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.misc.reflector;
using rogueCore.hqlSyntaxV4.location.column.command.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from.command
{
    class RunMLCommand : CommandFrom
    {
        public string columnName { get { return name; } }
        public const string commandNameIDConst = "RUN_ML_COMMAND";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override string commandNameID { get { return CodeMatchName; } }
        public override IORecordID tableId { get { return 2770122; } }
        public RunMLCommand(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
        protected override IEnumerable<IReadOnlyRogueRow> RunProcedure(IMultiRogueRow parentRow)
        {
            string path = commandParams[0].GetValue(parentRow);
            //string calcLabelName = commandParams[1].GetValue(rows);
            //string algorithmName = commandParams[2].GetValue(rows);
            //string algorithmDesc = commandParams[3].GetValue(rows);
            var mlContext = new MLContext();
            // Load Trained Model
            DataViewSchema predictionPipelineSchema;
            
            //var stream = new FileStream(@"C:\Users\chris\Documents\RogueDataBase\Pure\MlModels\2770118\SampleRegression\SampleRegression.Model\MLModel.zip", FileMode.Open);
            //ITransformer predictionPipeline = mlContext.Model.Load(stream, out predictionPipelineSchema);
            ITransformer predictionPipeline = mlContext.Model.Load(@"C:\Users\chris\Documents\RogueDataBase\Pure\MlModels\2795445\SampleRegression\SampleRegression.Model\MLModel.zip", out predictionPipelineSchema);
            PredictionEngine<CryptoLagInput, CryptoLagOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<CryptoLagInput, CryptoLagOutput>(predictionPipeline);
            // Input Data
            //var paramForInput = new List<KeyValuePair<string, string>>();
            //foreach (var pair in commandParams)
            //{
            //    paramForInput.Add(new KeyValuePair<string, string>("", pair.GetValue(rows)));
            //}
            //Reflector.SetModelProperties("Name", parentRow);
            CryptoLagInput inputData = (CryptoLagInput)Reflector.SetModelProperties("CryptoLagInput", parentRow);
            //JsonConvert.DeserializeObject<CryptoLagInput>("");
            //var insertQry = new ManualInsert(2770054);
            //insertQry.NewRow();
            var outPut = predictionEngine.Predict(inputData);
            //insertQry.AddKeyValuePair(2770127, outPut.Score.ToString());
            //insertQry.Execute();
            var row = new ManualBinaryRow();
            row.AddPair(2770127, outPut.Score.ToString());
            return row.ToSingleEnum();
        }
    }
}
