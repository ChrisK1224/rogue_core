//using System;
//using System.Collections.Generic;
//using System.Text;
//using FilesAndFolders;

//namespace rogue_core.rogueCore.hqlSyntaxV4
//{
//    class NamedLocation : SegmentBase
//    {
//        bool isNameSet { get; }
//        string Name { get; set; } = "";
//        public string displayName;
//        public string remainingTxt { get; private set; }        
//        public override string[] splitters { get { return new string[1] { "AS" }; } }
//        protected override bool includeWhitespace { get { return true; } }
//        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;        
//        public NamedLocation(string txt, QueryMetaData metaData) : base(txt, metaData)
//        {
//            try
//            {
//                var lst = txt.Split(splitters, StringSplitOptions.None);
//                remainingTxt = lst[0].Trim();
//                if (lst.Length > 1)
//                {
//                    isNameSet = true;
//                    displayName = lst[1].BeforeFirstSpace().ToUpper();
//                    Name = displayName.ToUpper();
//                }
//                else
//                {
//                    isNameSet = false;
//                }
//                LocalSyntaxParts = StandardSyntaxParts; 
//            }
//            catch(Exception ex)
//            {
//                //LocalSyntaxParts = ErrorSyntaxParts;
//            }
//        }
//        public string GetName(string defaultName)
//        {
//            return isNameSet ? Name : defaultName;
//        }
//        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
//        {
//            LocalSyntaxParts(parentRow, syntaxCommands);
//        }
//        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
//        {
//            if (isNameSet)
//            {
//                syntaxCommands.GetLabel(parentRow, "&nbsp;" + "AS" + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
//                syntaxCommands.GetLabel(parentRow, Name);
//            }
//        }
//    }
//}