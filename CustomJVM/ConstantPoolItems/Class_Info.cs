using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.ConstantPoolItems
{
    public class Class_Info :CP_Info
    {
        public override byte Tag => 0x07; //7
        public ushort Name_Index { get; private set; }
        
        public override void Parse(ref Memory<byte> hexdump)
        {
            Name_Index= hexdump.Read2();
        }
    }
}
