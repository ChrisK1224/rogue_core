using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.rogueUIV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hqlSyntaxV3.segments.locationColumn.columnTypes
{
    class ExecutableLocation : CommandLocation
    {
        public string execName { get { return base.commandParams[0]; } }
        string[] execParams { get { return base.commandParams.Skip(1).ToArray(); } }
        public ExecutableLocation(string execTxt) : base(execTxt) { }
        public IEnumerable<IMultiRogueRow> RunExecProcedure(IMultiRogueRow parentRow)
        {
            object[] parameters = new object[2];
            parameters[0] = execParams;
            parameters[1] = parentRow;
            foreach (IMultiRogueRow row in (List<IMultiRogueRow>)CodeCaller.RunProcedure(execName, parameters))
            {
                yield return row;
            }
        }
        public IEnumerable<IMultiRogueRow> RunExecProcedure(Dictionary<string, IRogueRow> parentRow)
        {
            object[] parameters = new object[2];
            parameters[0] = execParams;
            parameters[1] = parentRow;
            foreach (IMultiRogueRow row in (List<IMultiRogueRow>)CodeCaller.RunProcedure(execName, parameters))
            {
                yield return row;
            }
        }
        public void PreFill(QueryMetaData metaData, string assumedName) {  }
    }
}
