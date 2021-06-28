using CustomJVM;
using CustomJVM.Attributes;
using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Reflection;
using static SharedLibrary.SharedLibrary;
using System.Linq;
namespace JVMFunction
{
    public class Program
   {
        public static T[] GetAllTypesThatInheritT<T>() => Assembly.GetAssembly(typeof(T)).GetTypes()
                                                                                         .Where(x => x.IsSubclassOf(typeof(T)))
                                                                                         .Select(x => (T)Activator.CreateInstance(x))
                                                                                         .ToArray();
                                                                                           

        public static Dictionary<AttributeNames, Func<ushort, Attribute_Info>> map = new Dictionary<AttributeNames, Func<ushort, Attribute_Info>>()
        {
            [AttributeNames.Code] = new Func<ushort, Attribute_Info> ((attribute_name_index) => new Code_attribute(attribute_name_index)),
            [AttributeNames.LineNumberTable]= new Func<ushort, Attribute_Info>((attribute_name_index => new LineNumberTable_attribute(attribute_name_index))),
            [AttributeNames.SourceFile]= new Func<ushort,Attribute_Info>((attribute_name_index =>new SourceFile(attribute_name_index)))
        };

        public Attribute_Info CreateAttribute(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            var attribute_name_index = hexdump.Read2();
            string name_index = (pool[attribute_name_index - 1] as CustomJVM.ConstantPoolItems.UTF8_Info).UTF8ToString();

            AttributeNames attributeName = (AttributeNames)Enum.Parse(typeof(AttributeNames), name_index);

            return map[attributeName](attribute_name_index);
        }

        static void Main(string[] args)
        {
            byte[] bytes = System.IO.File.ReadAllBytes("Program.class");

            Memory<byte> hexdump = bytes.AsMemory();

            #region Magic
            uint magic = hexdump.Read4();
            ushort major_version = hexdump.Read2();
            ushort minor_version = hexdump.Read2();
            #endregion

            #region ConstantPool
            Constant_Pool pool = new Constant_Pool();
            pool.Parse(ref hexdump);

            ushort access_flags = hexdump.Read2();
            ushort this_class = hexdump.Read2();
            ushort super_class = hexdump.Read2();
            ushort interfaces_count = hexdump.Read2();
            #endregion


        }
    }
}
