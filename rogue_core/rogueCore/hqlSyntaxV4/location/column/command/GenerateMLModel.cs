using files_and_folders;
using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column.command
{
    class GenerateMLModel : CommandLocation, IColumn
    {
        public string columnName { get { return name; } }
        public static string CodeMatchName { get { return "GENERATE_ML_MODEL"; } }
        public GenerateMLModel(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {

        }
        public string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> rows)
        {
            string path = commandParams[0].GetValue(rows);
            string calcLabelName = commandParams[1].GetValue(rows);
            string outputPath = commandParams[2].GetValue(rows);
            string cmd = String.Format("mlnet regression --dataset \"{0}\" --label-col \"{1}\" -o \"{2}\"", path, calcLabelName, outputPath);
            //string cmd = String.Format("mlnet regression --dataset \"{0}\" --label -col \"{1}\" -o \"{2}\" ", path, calcLabelName, outputPath);
            return MyCli.RunCommand(cmd);
        }
    }
}
