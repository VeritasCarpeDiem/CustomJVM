using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CustomJVM
{
    public static class Extensions
    {
        public static int Parse(this string MyString)
        {
            return int.Parse(MyString);
        }

        public static byte Read1(this ref Memory<byte> hexdump) //u1
        {
            byte ret = hexdump.Span[0]; //Read byte at top
            hexdump= hexdump.Slice(1); //advance memory by 1 byte

            return ret; //return value that need
        }

        public static ushort Read2(this ref Memory<byte> hexdump) //u2
        {
            Span<ushort> casted = MemoryMarshal.Cast<byte, ushort>(hexdump.Span);

            ushort ret = casted[0].ReverseBytes();
            
            hexdump = hexdump.Slice(2);

            return ret;
        }

        public static uint Read4(this ref Memory<byte> hexdump) //u3
        {
            var casted = MemoryMarshal.Cast<byte, uint>(hexdump.Span);
            uint ret = casted[0].ReverseBytes();  //swap

            hexdump = hexdump.Slice(sizeof(uint));

            return ret;
        }

        public static uint ReverseBytes(this uint item)
        {
            ushort lowByte =(ushort) item;
            ushort highByte = (ushort)(item >> 16);

            ushort reversedHighBytes = highByte.ReverseBytes();
            ushort reversedLowBytes = lowByte.ReverseBytes();

            return (uint)(reversedLowBytes<< 16 | reversedHighBytes);

        }
        public static ushort ReverseBytes(this ushort item)
        {

            byte lowByte = (byte)item;
            byte highByte = (byte)(item >> 8);

            return (ushort)(lowByte << 8 | highByte);

        }

        public static string UTF8ToString(this UTF8_Info info)
        {
            string returnString = new string(info.bytes.Select(x => (char)x).ToArray());
            // char[] array= Array.ConvertAll(info.Data, new Converter<byte, char>(x=> (char)x);
            return returnString;
        }
    }
}
