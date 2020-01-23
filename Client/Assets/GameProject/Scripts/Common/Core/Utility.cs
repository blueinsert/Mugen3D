using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace bluebean.Mugen3D.Core
{
    public class Utility
    {
        public static int GetKeycode(KeyNames key)
        {
            return 1 << ((int)key);
        }
 
    }
}
