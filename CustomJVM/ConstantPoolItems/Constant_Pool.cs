using System;
using System.Collections.Generic;
using System.Text;

namespace CustomJVM.ConstantPoolItems
{
    public class Constant_Pool
    {
        CP_Info[] cp_info;
        int index = 0;
        public CP_Info this[int index]
        {
            get
            {
                return cp_info[index];
            }
        }
        private void Set(CP_Info info)
        {
            cp_info[index] = info;
            index++;
        }

        public void Parse(ref Memory<byte> hexdump)
        {
            CP_Info[] allConstantPoolTypes = Program.GetAllTypesThatInheritT<CP_Info>(typeof(CP_Info));

            Dictionary<byte, Func<CP_Info>> map = new Dictionary<byte, Func<CP_Info>>();
            foreach (var item in allConstantPoolTypes)
            {
                map.Add(item.Tag, new Func<CP_Info>(() =>
                {
                    return (CP_Info)Activator.CreateInstance(item.GetType());
                }));
            }
            ushort constant_pool_count = hexdump.Read2();
            cp_info = new CP_Info[constant_pool_count - 1];

            for (int i = 0; i < constant_pool_count -1; i++)
            {
                byte tag = hexdump.Read1();
                CP_Info currentInfo = map[tag]();

                currentInfo.Parse(ref hexdump);

                Set(currentInfo);
            }
        }
    }
}
