using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public static class CommandHelper
    {
        public static int GetKeycode(KeyNames key)
        {
            return 1 << ((int)key);
        }
    }
}
