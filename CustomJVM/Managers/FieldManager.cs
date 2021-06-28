using CustomJVM.ConstantPoolItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Managers
{
    public class FieldManager : IEnumerable<Field_Info>
    {
        public IEnumerator<Field_Info> GetEnumerator()
        {
            for (int i = 0; i < fields.Length; i++)
            {
                yield return fields[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Field_Info[] fields;

        public void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            ushort method_Count = hexdump.Read2();
            fields = new Field_Info[method_Count];
            for (int i = 0; i < method_Count; i++)
            {
                Field_Info cur = new Field_Info();
                cur.Parse(ref hexdump, pool);
                fields[i] = cur;
            }
        }
    }
}
