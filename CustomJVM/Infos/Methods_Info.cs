using CustomJVM.Attributes;
using CustomJVM.ConstantPoolItems;
using CustomJVM.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static SharedLibrary.SharedLibrary;

namespace CustomJVM.Infos
{
    public class Method_Info
    {
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
                newInfo.Parse(ref hexdump, pool);
                //Attribute_Info info = new Attribute_Info();
                //info.Parse(ref hexdump);
                Attributes[i] = newInfo;

                if (newInfo is Code_attribute)
                {
                    code_Attribute = (Code_attribute)newInfo;
                }
            }

            locals = new uint?[code_Attribute.Max_Locals];
        }
        private Code_attribute code_Attribute;
        public uint?[] locals;
        public static Stack<uint?> stack = new Stack<uint?>();

        public uint? Execute(Constant_Pool pool, MethodManager manager)
        {
            byte[] code = code_Attribute.Code;
            
            for (int i = 0; i < code.Length; i++)
            {
                var opCode = (InstructionType)code[i];

                switch (opCode)
                {
                    case InstructionType.iconst_5:
                        stack.Push(5);
                        break;
                    case InstructionType.istore_1:
                        locals[1] = stack.Pop();
                        break;
                    case InstructionType.bipush:
                        stack.Push(code[++i]);
                        break;
                    case InstructionType.istore_2:
                        locals[2] = stack.Pop();
                        break;
                    case InstructionType.iload_1:
                        stack.Push(locals[1]);
                        break;
                    case InstructionType.iload_2:
                        stack.Push(locals[2]);
                        break;
                    case InstructionType.iload_0:
                        stack.Push(locals[0]);
                        break;
                    case InstructionType.iadd:
                        var result = stack.Pop() + stack.Pop();
                        stack.Push(result);
                        break;
                    case InstructionType.istore_3:
                        locals[3] = stack.Pop();
                        break;
                    case InstructionType.@return:
                        return null;
                        
                    case InstructionType.invokestatic:
                        byte indexByte1 = code[++i];
                        byte indexByte2 = code[++i];
                        ushort constant_pool_index = (ushort)(indexByte1 >> 8 | indexByte2);
                        
                        var method_ref = (CP_MethodRef_Info)pool[constant_pool_index - 1];
                        var name_and_type__index = (NameAndType_Info)pool[method_ref.Name_And_Type_Index - 1];

                        var method_name = ((UTF8_Info)pool[name_and_type__index.Name_Index - 1]).UTF8ToString();
                        var descriptor_name = ((UTF8_Info)pool[name_and_type__index.Descriptor_Index - 1]).UTF8ToString();

                        var name_and_type = (NameAndType_Info)pool[method_ref.Name_And_Type_Index - 1];
                        var string_method_name = ((UTF8_Info)pool[name_and_type.Name_Index - 1]).UTF8ToString();
                        var string_descriptor_name = ((UTF8_Info)pool[name_and_type.Descriptor_Index - 1]).UTF8ToString();

                        var parsedDescriptor= ParseDescriptor(string_descriptor_name);

                        var numberOfParameters = parsedDescriptor.parameters.Count; //2

                        Method_Info method_to_execute = null;
                        foreach (var  method_Info in manager)
                        {
                            UTF8_Info info = (UTF8_Info)pool[method_Info.Name_Index - 1];
                            string method_name_ = info.UTF8ToString();
                            if (method_name == method_name_)
                            {
                                method_to_execute = method_Info;
                                break;
                            }
                        }
                        //locals = new uint?[method_to_execute.code_Attribute.Max_Locals];
                        for (int j = 0; j < numberOfParameters; j++)
                        {
                            method_to_execute.locals[j] = stack.Pop();
                        }
                        stack.Push(method_to_execute.Execute(pool, manager));
                        break;
                    case InstructionType.iconst_2:
                        stack.Push(2);
                        break;
                    case InstructionType.iconst_3:
                        stack.Push(3);
                        break;
                    case InstructionType.pop:
                        stack.Pop();
                        break;
                    case InstructionType.ireturn:
                        return stack.Pop();
                    //break;
                    default:
                        throw new NotImplementedException($"OpCode missing: 0x{opCode:X}");
                }
            }
            return null;
        }

        public static (List<Type> parameters, Type returnType) ParseDescriptor(string descriptor)
        {
            List<Type> parameters = new List<Type>();
            Type returnType =null;
            string pattern = @"^\((.*)\)(.*)$";

            var map = new Dictionary<char, Type>()
            {
                ['I']= typeof(int),
                ['C']= typeof(char),
                ['S']=typeof(string),
                ['B']=typeof(byte),
                ['U']= typeof(ushort),
                ['V']=typeof(void)
            };
            
            var match = Regex.Match(descriptor, pattern);
            string insideParanthesis = match.Groups[1].Value;
            string outsideParanthesis = match.Groups[2].Value;

            foreach (var @char in insideParanthesis)
            {
                parameters.Add(map[@char]);
            }

            var returnTypeValue = map[outsideParanthesis[0]];

            return (parameters,returnTypeValue);
        }

        //public uint? Add(Constant_Pool pool, MethodManager methodManager)
        //{
        //    byte[] code = code_Attribute.Code;

        //    Stack<uint?> stack = new Stack<uint?>();
        //    for (int i = 0; i < code.Length; i++)
        //    {
        //        InstructionType opCode = (InstructionType)code[i];

        //        switch (opCode)
        //        {
        //            case InstructionType.iload_0:
        //                stack.Push(locals[0]);
        //                break;
        //            case InstructionType.iload_1:
        //                stack.Push(locals[1]);
        //                break;
        //            case InstructionType.iadd:
        //                var result = stack.Pop() + stack.Pop();
        //                stack.Push(result);
        //                break;
        //            case InstructionType.ireturn:
        //                return stack.Pop();
        //                //break;
        //            default:
        //                throw new NotImplementedException("yeet");
        //                break;
        //        }
        //    }
        //    return null;
        //}
    }
   
}
        

