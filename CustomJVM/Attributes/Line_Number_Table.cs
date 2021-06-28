using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Attributes
{
    public class Line_Number_Table :Attribute_Info
    {

        public Line_Number_Table(ushort attribute_name_index):base(attribute_name_index)
        {
            
        }

        public ushort Start_PC { get; private set; }
        public ushort Line_Number { get; private set; }

        public override void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            Start_PC = hexdump.Read2();
            Line_Number = hexdump.Read2();
            
        }
    }
}
