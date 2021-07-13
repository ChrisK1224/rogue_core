using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rogueCore.hqlSyntax.segments.table;

namespace rogueCore.hqlSyntax
{
    public abstract class HQLQueryTwo
    {
        protected HQLMetaData metaData = new HQLMetaData();
        internal TableGroups tableGroups { set; get; }
        Dictionary<String, FilledTable> dataTables = new Dictionary<string, FilledTable>();
        internal List<KeyValuePair<int, FilledSelectRow>> hierarchyGrid = new List<KeyValuePair<int, FilledSelectRow>>();
        //public List<KeyValuePair<int, FilledSelectRow>> grid() { return hierarchyGrid}
        protected HQLQueryTwo(){ }
        protected void LoadData()
        {
            foreach (TableGroup thsGroup in tableGroups.groups)
            {
                thsGroup.LoadGroupData(dataTables);
            }
            
        }
        public StringBuilder PrintResults()
        {
            //*print from top down
            //foreach (var currTbl in dataTables.Values.Where(x => x.level == 0))
            //{
            //    //IterateTopRowSet(currTbl.Rows());
            //    foreach (var row in currTbl.Rows())
            //    {
            //        LoopPrintHierachy(row, currTbl.level);
            //    }
            //}
            StringBuilder strBuild = new StringBuilder();
            foreach(FilledSelectRow topRow in TopRows())
            {
                strBuild = LoopPrintHierachy(topRow, 0, strBuild);
            }
            return strBuild;
            //return metaData.rootTable.Rows().First().childRows.ForEach(x => LoopPrintHierachy(x, 0, strBuild));
            // return LoopPrintHierachy(metaData.rootTable.Rows().First(), -1, new StringBuilder());
        }
        StringBuilder LoopPrintHierachy(FilledSelectRow topRow, int currLvl, StringBuilder stringBuild)
        {
            stringBuild.Append(topRow.PrintRow(currLvl));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl, stringBuild);
            }
            return stringBuild;
        }
        void IterateTopRowSet(IEnumerable<FilledSelectRow> topRows, Action<rowstatus, FilledSelectRow> finalOutput = null)
        {
            if(finalOutput == null){
                finalOutput = (rowstatus stat, FilledSelectRow row) => { };
            } 
            foreach (var topRow in topRows)
            {
                LoopHierachy(topRow, 0, finalOutput);
            }
        }
        void LoopHierachy(FilledSelectRow topRow, int currLvl, Action<rowstatus, FilledSelectRow> finalOutput)
        {
            //topRow.PrintRow(currLvl);
            finalOutput(rowstatus.open,topRow);
            hierarchyGrid.Add(new KeyValuePair<int,FilledSelectRow>(currLvl,topRow));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopHierachy(childRow, currLvl, finalOutput);
            }
            finalOutput(rowstatus.close,topRow);
        }
        internal List<KeyValuePair<int, FilledSelectRow>> IterateRows(Action<rowstatus, FilledSelectRow> finalOutput){
            //foreach (var currTbl in dataTables.Values.Where(x => x.level == 0))
            //{
            IterateTopRowSet(TopRows(), finalOutput);
            //}
            return hierarchyGrid;
        }
        public void LoadRows(Action<rowstatus, FilledSelectRow> finalOutput)
        {
            IterateTopRowSet(TopRows(), finalOutput);
            //foreach (var currTbl in dataTables.Values.Where(x => x.level == 0))
            //{
            //    IterateTopRowSet(currTbl.Rows(), finalOutput);
            //}
        }
        public IEnumerable<FilledSelectRow> TopRows()
        {
            //List<FilledSelectRow> topRows = new List<FilledSelectRow>();
            //foreach (var currTbl in dataTables.Values.Where(x => x.level == 0))
            //{
            //    topRows.AddRange(currTbl.Rows());
            //}
            return metaData.rootTable.rows.First().childRows;
        }
        public enum rowstatus
        {
            open, close
        }
    }
}