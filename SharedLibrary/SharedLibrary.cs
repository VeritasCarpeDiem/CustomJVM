using System;
using System.Collections.Generic;

namespace SharedLibrary
{
    public class SharedLibrary
    {
        public enum InstructionType: byte
        {
            iconst_2=0x05,
            iconst_3= 0x06,
            pop =0x57,
            iload_0=0x1a,
            ireturn=0xac,
            iconst_5 = 0x08,
            istore_1=0x3C,
            bipush= 0x10,
            istore_2 = 0x3d,
            iload_1= 0x1b,
            iload_2=0x1c ,
            iadd= 0x60,
            istore_3 = 0x3e,
            @return = 0xb1,
            invokestatic = 0xb8,
        }
        public enum AttributeNames
        {
            Code,
            SourceFile,
            LineNumberTable
        }
        
    }
}
