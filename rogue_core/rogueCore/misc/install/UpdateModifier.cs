using files_and_folders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.install
{
    public static class UpdateModifier
    {
        public static void UpdateTableV2()
        {
            ArchiveRogueInstance();
            Dictionary<long, string> values = new Dictionary<long, string>();
            foreach(var line in File.ReadAllLines(@"Y:\RogueDatabase\Pure\-1005\-1008\-1003\-1003.rogue"))
            {
                string[] splitter= line.Split('|');
                var id = long.Parse(splitter[0]);
                string[] pairSplit = splitter[1].Split(':');
                string value = pairSplit[1].Replace("@RCOMMA", ",").Replace("@ROGUECOLON", ":").Replace("@ROGUESEMICOLON", ";").Replace("@RNEWLINE", Environment.NewLine).Replace("@RBAR", "|");
                values.Add(id, value);
            }
            bool isFirst = true;
            var file = @"Y:\RogueDatabase\Pure\-1005\-1009\-1010\-1010.rogue";
            //foreach(var file in Directory.GetFiles(@"Y:\RogueDatabase\Pure\", "*", SearchOption.AllDirectories).Where(x => x.EndsWith(".rogue") && !x.Contains("BIN") && !x.Contains("-1003") && !x.Contains("-1000")))
            //{
                IORecordID tableID;
                //if (isFirst)
                //{
                //    tableID = -1011;
                //    isFirst = false;
                //}
                //else {
                tableID = new IORecordID(Path.GetFileNameWithoutExtension(new FileInfo(file).Name));
            //}
            //string folder = new FileInfo(file).DirectoryName + Path.DirectorySeparatorChar;
            string folder = @"Y:\RogueDatabase\del\";
                string finalBINName = folder + BinaryDataTable.CreateTableName(tableID.ToString());
                
                var tbl = new BinaryDataTable(tableID, finalBINName);
                File.WriteAllText(tbl.filePath, "");
                File.WriteAllText(tbl.complexWordTable.filePath, "");
                //var tbl = new BinaryDataTable(tableID, @"Y:\RogueDatabase\Pure\-1005\-1009\-1011\BIN_-1011.rogue");
                foreach (var line in File.ReadAllLines(file))
                 {
                    var newRow = tbl.NewWriteRow();
                    String[] pair_lines = line.Split('|');
                newRow.pairs.Clear();
                    newRow.NewWritePair(-1012, pair_lines[0]);
                    Console.WriteLine("________________");
                     for(int i = 1; i < pair_lines.Length; i++)
                     {
                        var valColSplit = pair_lines[i].Split(':');                        
                        int valueID = int.Parse(valColSplit[1]);
                        string value;
                        if (values.ContainsKey(valueID))
                        {
                            value = values[valueID];
                        }
                        else
                        {
                            value = valueID.ToString();
                        }
                        newRow.NewWritePair(int.Parse(valColSplit[0]), value);
                     }
                    Console.WriteLine("________________");
                }
                tbl.Write();
            //}
        }
        public static void UpdateEachTable(Action<IRogueTable> changeTable)
        {
            //foreach(var tblRow in BinaryDataTable.ioRecordTable.stream_TableRows())
            //{
            //    if(tblRow.MetaRecordType().DisplayValue().ToUpper() == "TABLE")
            //    {
            //        IRogueTable tbl = tblRow.ID.ToTable();
            //        if (!(tbl is VarcharTable) && !(tbl is RogueIDTable))
            //        {
                        
            //            //Console.WriteLine(tbl.ioItemID);
            //            changeTable(tbl);
            //            //int rowCount = 0;
            //            //foreach (var row in tbl.StreamIRows().TakeWhile(stop => rowCount < 100))
            //            //{
            //            //    Console.WriteLine(row.rowID);
            //            //    rowCount++;
            //            //}
            //        }
            //        else
            //        {
            //            string ll = "SDF";
            //        }
            //    }                
            //}
        }
        public static void ArchiveRogueInstance()
        {
            string archivePath = RootVariables.basePath + Path.DirectorySeparatorChar + "Archives" + Path.DirectorySeparatorChar + DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Day.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Second.ToString() + Path.DirectorySeparatorChar + "Pure";
            Directory.CreateDirectory(archivePath);
            CopyAllContents(RootVariables.rootPath, archivePath);
        }
        public static void CopyAllContents(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
        //public static void ConvertToBinaryVOne()
        //{
        //    UpdateModifier.ArchiveRogueInstance();
        //    Action<IRogueTable> TranslateTable = x => {
        //        BinaryDataTable dataTable = new BinaryDataTable(x.ioItemID);
        //        foreach (var row in x.StreamDataRows())
        //        {
        //            var pairs = row.pairs.ToList();
        //            Dictionary<long,BinaryDataPair> currPairs = new Dictionary<long,BinaryDataPair>(pairs.Count);
        //            //var binRow = new BinaryDataRow(pairs.Count);                   
        //            for (int p =0; p < pairs.Count; p++)
        //            {
        //                int isEndInt = (p == pairs.Count-1) ? 1 : 0;
        //                byte isEnd = 0;
        //                if(isEndInt == 1)
        //                {
        //                    isEnd = 1;
        //                }
        //                //**Fx dictionary
        //                currPairs.Add(pairs[p].Key,new BinaryDataPair((short)pairs.Count, pairs[p].Key, pairs[p].Value.StringValue(dataTable.complexWordTable), isEnd, dataTable.complexWordTable));
        //                //currPairs[p] = new BinaryDataPair((short)pairs.Count, pairs[p].KeyColumnID, pairs[p].DisplayValue(), dataTable.complexWordTable);
        //                //currPairs[p] = new BinaryDataPair((short)pairs.Count, BinaryDataPair.GetDataType(pairs[p].DisplayValue()), pairs[p].KeyColumnID, pairs[p].WriteValue().ToDecodedRowID(), 0, (byte)isEnd);
        //            }
        //            dataTable.AddWriteRow(new BinaryDataRow(currPairs, dataTable.complexWordTable));
        //        }
        //        dataTable.Write();
        //    };
        //    UpdateModifier.UpdateEachTable(TranslateTable);
        //}
        //public static void ConvertBinaryString()
        //{
        //    var tbl = new SimpleWordTable();
        //    var stringTbl = new IORecordID(-1003).ToTable();
        //    foreach (var row in stringTbl.StreamDataRows())
        //    {
        //        string val = BinaryDataTable.simpleTable.GetValue(row.GetValueByColumn(-1025)).value;                
        //        //Console.WriteLine(val);
        //    }
        //}
        public static void ReadBinary()
        {
            var tbl = new BinaryDataTable(-1010);
            foreach(var tl in tbl.StreamDataRows())
            {
                var ll = tl;
            }
        }
        public static void TestTemp()
        {
            //SimpleWordTable fl = new SimpleWordTable();
            //IORecordTable ioRecordTable = new IORecordTable();
            //VarcharTable varcharTable = new VarcharTable();
           
            //string folderPath = RootVariables.rootPath + ioRecordTable.rows[-1010].FolderPath().DisplayValue();
            var tbl = new BinaryDataTable(new IORecordID("-1010"));
            foreach(var row in tbl.StreamDataRows())
            {
                foreach(var pair in row.GetPairs())
                {
                    Console.WriteLine(pair.Key.ToColumnName() + ":" + pair.Value);
                }
            }
            var bll = ComplexWordRow.SepSplit("👍 ’ hey 👍 jj   yo 👍👍");
            //BinaryDataPair.GetValueOID("👍 hey 👍 jj   yo 👍👍");
            new SimpleWordTable();
            //string blah =  File.ReadAllText(@"Y:\RogueDatabase\Pure\Shared\test2.txt");
            //new ComplexWordRow("klj ji kljlkj ssss");
            //new ComplexWordRow(blah);
            //string ll = "SDF";
        }
    }
}
