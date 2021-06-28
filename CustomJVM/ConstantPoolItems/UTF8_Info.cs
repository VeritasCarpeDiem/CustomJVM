using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.ConstantPoolItems
{
    public class UTF8_Info : CP_Info
    {
        public override byte Tag => 0x01; //1
        public ushort Length { get; private set; }
        public byte[] bytes { get; private set; }

        public override void Parse(ref Memory<byte> hexdump)
        {
            Length = hexdump.Read2();
            bytes = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                bytes[i]= hexdump.Read1();
            }
        }
    }
}
