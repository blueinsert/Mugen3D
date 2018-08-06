using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fsoul.Net
{
    public class Msg
    {
         public string Type;
         public int SessionId;
         public virtual byte[] Encode() { return null; }
         public virtual void Decode(byte[] data) { }
    }
}
