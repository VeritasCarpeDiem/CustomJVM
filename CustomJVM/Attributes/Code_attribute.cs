using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Attributes
{
    public class Code_attribute: Attribute_Info
    {
        public Attribute_Info[] attributes { get; private set; }

       
        public Code_attribute(ushort Name_Index) :base(Name_Index)
        {
            
        }

        //public static byte[] Code { get; set; }
        public ushort Max_Stack{ get; private set; }
        public ushort Max_Locals { get; private set; }
        public uint Code_Length { get; private set; }
        public ushort Exception_Table_Length { get; private set; }
        public Exception_Table[] exception_Table{ get; private set; }
        public ushort Attributes_Count { get; private set; }

        //public ushort Execute(Method_Info main_info)
        //{
        //    uint?[] machineCode = new uint?[Code_Length];


        //    main_info.Attributes[0].
        //}

        public override void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            //Attribute_Name_Index = hexdump.Read2();
            Attribute_Length = hexdump.Read4();
            Max_Stack = hexdump.Read2();
            Max_Locals = hexdump.Read2();
            Code_Length = hexdump.Read4();
            
            Code = new byte[Code_Length];

            for (int i = 0; i < Code_Length; i++)
            {
                Code[i] = hexdump.Read1();
            }

            Exception_Table_Length = hexdump.Read2();
            exception_Table = new Exception_Table[Exception_Table_Length];

            for (int i = 0; i < Exception_Table_Length; i++)
            {
                exception_Table[i] = new Exception_Table();
                exception_Table[i].Parse(ref hexdump);
            }
            
            Attributes_Count = hexdump.Read2();
            attributes = new Attribute_Info[Attributes_Count];

            for (int i = 0; i < Attributes_Count; i++)
            {
                var info = Program.CreateAttribute(ref hexdump, pool);
                info.Parse(ref hexdump, pool);
                attributes[i] = info;
            }
        }
    }
}
