using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.ConstantPoolItems
{
    public class NameAndType_Info : CP_Info
    {
        public override byte Tag => 0x0C; //12
        public ushort Name_Index { get; private set; }
        public ushort Descriptor_Index { get; private set; }

        public override void Parse(ref Memory<byte> hexdump)
        {
            Name_Index = hexdump.Read2();
            Descriptor_Index = hexdump.Read2();
        }
    }
}
