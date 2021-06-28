using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Attributes
{
    public class SourceFile : Attribute_Info
    {
        public SourceFile(ushort name_index):base(name_index)
        {

        }
        public ushort Source_File_Index { get; private set; }

        public override void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            //Attribute_Name_Index = hexdump.Read2();
            Attribute_Length = hexdump.Read4();

            Source_File_Index = hexdump.Read2();
        }
    }
}
