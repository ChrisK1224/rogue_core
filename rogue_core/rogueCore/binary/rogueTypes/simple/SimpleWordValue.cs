using rogue_core.rogueCore.binary.rogueTypes;
using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace rogue_core.rogueCore.binary
{
    //[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SimpleWordValue : ISimpleValueReference
    {
        //int length;
        public long valueID { get; private set; }
        public string value { get; private set; }
        public int complexWordCount { get { return 0; } }
        public byte dataTypeID { get { return BinaryDataPair.dtSimpleString; } }
        static char[] splitter = new[] { RogueTypeTable.valueSeparatorChar };
        public SimpleWordValue(RowID rogueID, string value)
        {
            this.valueID = rogueID.ToInt();
            this.value = value;
        }   
        public SimpleWordValue(string fullValue)
        {
            var split = fullValue.Split(splitter, 2);
            //var split = fullValue.Split(RogueTypeTable<SimpleWordValue>.valueSeparatorChar);
            this.valueID = new UnKnownID(split[0]);
            this.value = split[1].Replace(SimpleWordTable.lineSeparatorEncoded, RogueTypeTable.lineSeparatorChar.ToString());
        }
        public string WriteValue()
        {
            //File.WriteAllText(@"Y:\RogueDatabase\Pure\Shared\test3.txt", value);
            //string txt = File.ReadAllText(@"Y:\RogueDatabase\Pure\Shared\test3.txt");
            //if(txt == "�")
            //{
            //    string ll = "SDF";
            //}
            return valueID.ToString() + RogueTypeTable.valueSeparatorChar + value.Replace(RogueTypeTable.lineSeparatorChar.ToString(), SimpleWordTable.lineSeparatorEncoded);
        }
        public string StringValue(ComplexWordTable complexTbl) { return value; }
    }
}
