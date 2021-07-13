using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV2.segments.join;
using rogueCore.hqlSyntaxV2.segments.level;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.hqlSyntaxV2.filledSegments
{
    class FilledGroup : TableGroupStatement
    {
        Dictionary<string, FilledLevel> filledLevels = new Dictionary<string, FilledLevel>();
        internal FilledGroup(string hqlTxt, HQLMetaData metaData) : base(hqlTxt, metaData)
        {
            
        }
        internal override FilledGroup Fill()
        {
            //levelStatements.Where(v => v.joinClause.isSet == false).ToList().ForEach(x => SetTableLevels(x));
            foreach (LevelStatement fillLvl in levelStatements.OrderBy(x => x.levelNum))
            {
                if(fillLvl.levelNum == 0)
                {
                    filledLevels.Add(fillLvl.levelName, fillLvl.Fill(metaData.rootLevel));
                }
                else
                {
                    filledLevels.Add(fillLvl.levelName, fillLvl.Fill(metaData.levelStatements[fillLvl.parentLevelRefName]));
                }
            }
            //var transformedRows = groupInfo.Transformer(groupRows, existingPieces, levelStatements);
            //PrintTopTableGroup(transformedRows);
            //Parallel.ForEach(filledLevels.Values.ToList(), command => command.LoadDataIntoRows());
            //foreach( var dataRow in filledLevels.Values)
            //{
            //    dataRow.LoadDataIntoRows();
            //}
            return this;
        }
        void SetTableLevels(LevelStatement topTbl)
        {
            foreach (LevelStatement tbl in levelStatements.Where(x => x.joinClause.isSet == true && x.joinClause.parentColumn.colTableRefName == topTbl.levelName))
            {
                tbl.levelNum = topTbl.levelNum + 1;
                SetTableLevels(tbl);
            }
        }
        //internal void PrintGroup()
        //{
        //    foreach (FilledLevel fillLvl in filledLevels.Values.OrderBy(x => x.levelNum))
        //    {
        //        fillLvl.PrintLevel();
        //    }
        //}
    }
}
