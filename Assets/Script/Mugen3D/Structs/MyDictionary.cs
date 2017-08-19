using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class MyDictionary<T1, T2> : Dictionary<T1, T2>
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int count = this.Count;
            int index = 0;
            sb.Append("{");
            foreach (var k in this.Keys)
            {
                index++;
                sb.Append(k.ToString()).Append(":").Append(this[k].ToString());
                if (index != count)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }

    }
}
