using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Attributes
{
    public class LineNumberTable_attribute :Attribute_Info
    {

        public LineNumberTable_attribute(ushort attribute_name_index):base(attribute_name_index)
        {
            
        }

        protected ushort Line_Number_Table_Length { get; private set; }
        public Line_Number_Table[] line_Number_Tables { get; private set; }

        public override void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
           // Attribute_Name_Index = hexdump.Read2();
            Attribute_Length = hexdump.Read4();
            Line_Number_Table_Length = hexdump.Read2();

            line_Number_Tables = new Line_Number_Table[Line_Number_Table_Length];

            for (int i = 0; i < Line_Number_Table_Length; i++)
            {
                line_Number_Tables[i] = new Line_Number_Table(Attribute_Name_Index);
                line_Number_Tables[i].Parse(ref hexdump, pool);
             }
        }
    }
}
