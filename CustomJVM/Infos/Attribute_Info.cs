using CustomJVM.ConstantPoolItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Infos
{
    public abstract class Attribute_Info
    {
        public Attribute_Info(ushort attribute_name_index)
        {
            Attribute_Name_Index = attribute_name_index;
        }

        protected ushort Attribute_Name_Index { get; set; }
        protected uint Attribute_Length { get; set; }
        public byte[] Code { get; set; }

        public abstract void Parse(ref Memory<byte> hexdum, Constant_Pool pool);
       
    }
}
