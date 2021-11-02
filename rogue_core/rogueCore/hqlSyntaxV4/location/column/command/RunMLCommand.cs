//using files_and_folders;
//using Microsoft.ML;
//using Microsoft.ML.Data;
//using Newtonsoft.Json;
//using rogue_core.rogueCore.binary;
//using rogue_core.rogueCore.hqlSyntaxV4.insert.insertMethods;
//using rogue_core.rogueCore.misc.reflector;
//using rogueCore.hqlSyntaxV4.location.column.command.models;
//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
//{
//    class RunMLCommand : CommandLocation, IColumn
//    {
//        public string columnName { get { return name; } }
//        public static string CodeMatchName { get { return "RUN_ML_MODEL"; } }
//        public RunMLCommand(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
//        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
//        {
//            string path = commandParams[0].GetValue(rows);
//            var mlContext = new MLContext();
//            // Load Trained Model
//            DataViewSchema predictionPipelineSchema;
//            ITransformer predictionPipeline = mlContext.Model.Load(path, out predictionPipelineSchema);
//            PredictionEngine<CryptoLagInput, CryptoLagOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<CryptoLagInput, CryptoLagOutput>(predictionPipeline);
//            // Input Data
//            var paramForInput = new List<KeyValuePair<string, string>>();
//            foreach(var pair in commandParams)
//            {
//                paramForInput.Add(new KeyValuePair<string, string>("", pair.GetValue(rows)));
//            }
//            Reflector.SetModelProperties("SFD", parentRow);
//            CryptoLagInput inputData = JsonConvert.DeserializeObject<CryptoLagInput>("");
            
//            // Get Prediction
//            //CryptoLagOutput prediction = predictionEngine.Predict(inputData);
//            //ImmutableArray<RegressionMetricsStatistics> permutationFeatureImportance =
//            //mlContext.Regression.PermutationFeatureImportance(inputData, prediction, permutationCount: 3);

//            return info;
//        }
//    }
//}
