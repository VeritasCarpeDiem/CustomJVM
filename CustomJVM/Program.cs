using CustomJVM.Attributes;
using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using CustomJVM.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static SharedLibrary.SharedLibrary;

namespace CustomJVM
{
    class Program
    {
        public static T[] GetAllTypesThatInheritT<T>(Type type) => Assembly.GetAssembly(typeof(T)).GetTypes().
                                                                Where(x => x.IsSubclassOf(typeof(T))).
                                                                Select(x => (T)Activator.CreateInstance(x)).
                                                                ToArray();

        public static Dictionary<AttributeNames, Func<ushort, Attribute_Info>> map = new Dictionary<AttributeNames, Func<ushort, Attribute_Info>>()
        {
            [AttributeNames.Code] = new Func<ushort, Attribute_Info>((name_index) => new Code_attribute(name_index)),
            [AttributeNames.SourceFile] = new Func<ushort, Attribute_Info>((name_index) => new SourceFile(name_index)),
            [AttributeNames.LineNumberTable] = new Func<ushort, Attribute_Info>((name_index) => new LineNumberTable_attribute(name_index))
        };

        public static Attribute_Info CreateAttribute(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            var name_index = hexdump.Read2();
            string attribute_name = (pool[name_index-1] as ConstantPoolItems.UTF8_Info).UTF8ToString();
            AttributeNames attributeName = (AttributeNames)Enum.Parse(typeof(AttributeNames), attribute_name);

            return map[attributeName](name_index);
        }

        static void Main(string[] args)
        {
            byte[] machineCode = System.IO.File.ReadAllBytes("Program.class");
            Memory<byte> hexdump = machineCode.AsMemory();
           
            //start of program:
            uint magic = hexdump.Read4();
            ushort major_version = hexdump.Read2();
            ushort minor_version = hexdump.Read2();

            #region ConstantPool
            Constant_Pool pool = new Constant_Pool();
            pool.Parse(ref hexdump);

            ushort access_flags = hexdump.Read2();
            ushort this_class = hexdump.Read2();
            ushort super_class = hexdump.Read2();
            ushort interfaces_count = hexdump.Read2();
            #endregion

            #region Interfaces
            ushort[] interfaces = new ushort[interfaces_count];

            for (int i = 0; i < interfaces_count; i++)
            {
                interfaces[i] = hexdump.Read2();
                
            }

            #endregion

            #region Fields
            FieldManager fieldManager = new FieldManager();
            fieldManager.Parse(ref hexdump, pool);

            //ushort field_count = hexdump.Read2();
            //Field_Info[] fields = new Field_Info[field_count];

            //for (int i = 0; i < field_count; i++)
            //{
            //    Field_Info info = new Field_Info();
            //    info.Parse(ref hexdump);
            //    fields[i] = info; 
            //}

            #endregion
            ;
            #region Methods
            MethodManager methodManager = new MethodManager();
            methodManager.Parse(ref hexdump, pool);

            #endregion

            #region Attributes
            ushort attribute_count = hexdump.Read2(); 
            Attribute_Info[] attributes = new Attribute_Info[attribute_count];

            for (int i = 0; i < attribute_count; i++)
            {
                Attribute_Info cur = Program.CreateAttribute(ref hexdump, pool);
                cur.Parse(ref hexdump, pool);
                attributes[i] = cur;

                //UTF8_Info attribute_name = (UTF8_Info)pool[attributes[i].Attribute_Name_Index - 1];
                //string attribute_title = attribute_name.UTF8ToString();

                //switch (attribute_title)
                //{
                //    case "Code":
                //        Code_attribute code_Attribute = new Code_attribute(name_index);
                //        code_Attribute.Parse(ref hexdump);
                //        attributes[i] = code_Attribute;
                //        break;
                //    case "SourceFile":
                //        SourceFile sourceFile = new SourceFile();
                //        sourceFile.Parse(ref hexdump);
                //        attributes[i] = sourceFile;
                //         break;
                //    case "LineNumberTable":
                //        LineNumberTable_attribute line_Number_Table = new LineNumberTable_attribute();
                //        line_Number_Table.Parse(ref hexdump);
                //        attributes[i] = line_Number_Table;
                //        break;
                //}
            }
            #endregion
            ;

            #region Main method
            Method_Info main_info=null;
            foreach (var methodInfo in methodManager)
            {
                UTF8_Info info = (UTF8_Info)pool[methodInfo.Name_Index - 1];
                string method_name = info.UTF8ToString();
                UTF8_Info descriptor = (UTF8_Info)pool[methodInfo.Descriptor_Index - 1];
                string descriptorString = descriptor.UTF8ToString();
               
                if(method_name == "main" && descriptorString == "([Ljava/lang/String;)V" && methodInfo.Access_Flags ==0x0009)
                {
                    main_info = methodInfo;
                    break;
                }
            }
            #endregion

            #region Execute main method
            main_info.Execute(pool, methodManager);
            #endregion

            //#region Execute Add method
            //Method_Info add_info = null;
            //foreach (var methodInfo in methodManager)
            //{
            //    UTF8_Info info = (UTF8_Info)pool[methodInfo.Name_Index - 1];
            //    string method_name = info.UTF8ToString();
            //    if(method_name != "Add")
            //    {
            //        continue;
            //    }
            //    if (method_name == "Add")
            //    {
            //        add_info = methodInfo;
            //        break;
            //    }
            //}
            //Console.WriteLine(add_info.Execute(pool, methodManager));
            //#endregion

        }
    }
}
