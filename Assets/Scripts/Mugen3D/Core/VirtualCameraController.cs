using System;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{   
    public delegate Number[] DelegateGetScreenBound();

    public class VirtualCameraController 
    {
        public static DelegateGetScreenBound funcGetScreenBound;

        public static Number[] GetScreenBound()
        {
            if (funcGetScreenBound != null)
            {
                return funcGetScreenBound();
            }
            else
            {
                return null;
            }
        }
    }

}