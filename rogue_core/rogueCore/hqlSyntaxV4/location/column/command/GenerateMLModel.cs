using files_and_folders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.insert.insertMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class GenerateMLModel : CommandLocation, IColumn
    {
        public string columnName { get { return name; } }
        public static string CodeMatchName { get { return "GENERATE_ML_MODEL"; } }
        public GenerateMLModel(string colTxt, QueryMetaData metaData) : base(colTxt, metaData) { }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            string path = commandParams[0].GetValue(rows);
            string calcLabelName = commandParams[1].GetValue(rows);            
            string algorithmName = commandParams[2].GetValue(rows);
            string algorithmDesc = commandParams[3].GetValue(rows);
            var insertQry = new ManualInsert(2770054);
            var id = insertQry.NewRow();
            string outputPath = install.RootVariables.mlModelPath + id;
            Directory.CreateDirectory(outputPath);
            string zipPath = outputPath + "SampleRegression" + Path.DirectorySeparatorChar + "SampleRegression.Model" + Path.DirectorySeparatorChar + "MLModel.zip";
            //model name
            insertQry.AddKeyValuePair(2770059, algorithmName);
            //Model desc
            insertQry.AddKeyValuePair(2770062, algorithmDesc);
            //LabelName
            insertQry.AddKeyValuePair(2770065, calcLabelName);
            //Model Path
            insertQry.AddKeyValuePair(2770069, outputPath);
            //Zip Path
            insertQry.AddKeyValuePair(2770072, zipPath);                       
            string cmd = String.Format("mlnet regression --dataset \"{0}\" --label-col \"{1}\" -o \"{2}\"", path, calcLabelName, outputPath);
            //string cmd = String.Format("mlnet regression --dataset \"{0}\" --label -col \"{1}\" -o \"{2}\" ", path, calcLabelName, outputPath);
            var info =  MyCli.RunCommand(cmd);
            insertQry.Execute();
            return info;
        }
    }
}
