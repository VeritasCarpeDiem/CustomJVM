using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Attributes
{
    public class Exception_Table
    {
        public ushort Start_PC { get; private set; }
        public ushort End_PC { get; private set; }
        public ushort Handler_PC { get; private set; }
        public ushort Catch_Type { get; private set; }

        public Exception_Table() 
        {

        }

        public void Parse(ref Memory<byte>hexdump)
        {
            Start_PC = hexdump.Read2();
            End_PC= hexdump.Read2();
            Handler_PC = hexdump.Read2();
            Catch_Type = hexdump.Read2();
        }
    }
}
