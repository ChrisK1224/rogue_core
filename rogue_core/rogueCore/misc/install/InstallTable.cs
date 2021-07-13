//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using FilesAndFolders;
//using rogue_core.rogueCore.id;
//using rogue_core.rogueCore.id.idableItem;
//using rogue_core.rogueCore.pair;
//using rogue_core.rogueCore.row;
//using rogue_core.rogueCore.row.encoded.column;
//using rogue_core.rogueCore.table;

//namespace rogue_core.rogueCore.install
//{
//    public class InstallTable<R,D, RowIDType> where R : RogueRow<D, RowIDType> where RowIDType : RowID
//    {
//        String fullPath;
//        public List<R> writeRows = new List<R>();
//        public InstallTable(String fullPath) {this.fullPath = fullPath; DirectoryHelper.createFileCheckExists(fullPath);}
//        public void Write(){
//            StringBuilder writeText = new StringBuilder();
//            foreach (R ths_row in writeRows)
//            {
//                writeText.Append(ths_row.ID);
//                foreach(IRoguePair thsPair in ths_row.Values)
//                {
//                    writeText.Append(RogueTable<R, D, RowIDType>.PairWriteText(thsPair));
//                }
//                writeText.AppendLine();
//            }
//            //String tempPat = Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "Pure" + Path.DirectorySeparatorChar + "test.txt";
//            String bla = writeText.ToString();
//            File.AppendAllText(fullPath, writeText.ToString());
//        }
//    }
//}