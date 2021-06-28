using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Text;
using static SharedLibrary.SharedLibrary;

namespace CustomJVM
{
    public class Field_Info
    {
        Field_Info[] fields;

        public ushort Access_Flags { get; private set; }
        public ushort Name_Index { get; private set; }
        public ushort Descriptor_Index { get; private set; }
        public ushort Attributes_Count { get; private set; }

        public Attribute_Info[] Attributes { get; private set; }

        public void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            Access_Flags = hexdump.Read2();
            Name_Index = hexdump.Read2();
            Descriptor_Index = hexdump.Read2();
            Attributes_Count = hexdump.Read2();

            Attributes = new Attribute_Info[Attributes_Count];

            for (int i = 0; i < Attributes_Count; i++)
            {
                var newInfo = Program.CreateAttribute(ref hexdump, pool);
                //Attribute_Info info = new Attribute_Info();
                //info.Parse(ref hexdump);
                Attributes[i] = newInfo;
            }
        }
    }
}
