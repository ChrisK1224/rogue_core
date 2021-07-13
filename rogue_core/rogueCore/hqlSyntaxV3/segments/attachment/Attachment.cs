using FilesAndFolders;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.attachment
{
    //class Attachment
    //{
    //    internal const string attachSplitter = "ATTACH";
    //    internal const string attachSelectSplitter = "MODCOLUMNS";
    //    string attachTxt;
    //    internal Attachment(string attachTxt, HQLMetaData metaData)
    //    {
    //        var  attachmentSplits = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, attachTxt, new string[] { attachSelectSplitter }, metaData).segmentItems;
    //        var tbls = FilledLevel.ConvertToTableStatements(stringHelper.getStringBetweenFirstLastOccurance(attachmentSplits[MutliSegmentEnum.firstEntrySymbol],'"').Trim(), metaData);
            
    //        //List<TableStatement> attachTbls = FilledLevel.ConvertToTableStatements(attachTxt, metaData);
    //        string levelName = tbls[0].joinClause.parentColumn.colTableRefName;
    //        SelectRow attachSelectRow = new SelectRow(stringHelper.getStringBetweenFirstLastOccurance(attachmentSplits[attachSelectSplitter], '"'), metaData);
    //        metaData.levelStatements[levelName].AttachTables(tbls, attachSelectRow);
    //        //foreach (var tbl in attachTbls)
    //        //{
    //        //    metaData.levelStatements[levelName].AttachTable();
    //        //}
    //    }
    //}
}
