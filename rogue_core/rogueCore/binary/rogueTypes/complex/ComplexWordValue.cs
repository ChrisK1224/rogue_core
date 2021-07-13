using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace rogue_core.rogueCore.binary.word.complex
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct ComplexWordValue
    {
        public byte typ;
        public Int64 value;  
        public ComplexWordValue(IValueReference valueReference)
        {
            this.typ = valueReference.dataTypeID;
            this.value = valueReference.valueID;
        }
    }
}
