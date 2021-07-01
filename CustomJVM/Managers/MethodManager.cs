using CustomJVM.ConstantPoolItems;
using CustomJVM.Infos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.Managers
{
    public class MethodManager :IEnumerable<Method_Info>
    {
        public IEnumerator<Method_Info> GetEnumerator()
        {
            for (int i = 0; i < methods.Length; i++)
            {
                yield return methods[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Method_Info[] methods;

        public void Parse(ref Memory<byte> hexdump, Constant_Pool pool)
        {
            ushort method_Count = hexdump.Read2();
            methods = new Method_Info[method_Count];
            for (int i = 0; i < method_Count; i++)
            {
                Method_Info cur = new Method_Info();
                cur.Parse(ref hexdump, pool);
                methods[i] = cur;

            }
        }

    }
}
