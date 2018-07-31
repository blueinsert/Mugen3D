using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace Mugen3D.Core
{
    public static class LuaControllerLib 
    {
        public const string LIB_NAME = "controller.cs";

        public static int OpenLib(ILuaState lua)
        {
            var define = new NameFuncPair[] {
                new NameFuncPair("ChangeState", ChangeState),
                new NameFuncPair("ChangeAnim", ChangeAnim),
                new NameFuncPair("PhysicsSet", PhysicsSet),
                new NameFuncPair("MoveTypeSet", MoveTypeSet),
                new NameFuncPair("VelSet", VelSet),
                new NameFuncPair("CtrlSet", CtrlSet),
                new NameFuncPair("Pause", Pause),
            };
            lua.L_NewLib(define);
            return 1;
        }

        public static int ChangeState(ILuaState lua)
        {
            return 0;
        }

        public static int ChangeAnim(ILuaState lua)
        {
            return 0;
        }

        public static int PhysicsSet(ILuaState lua)
        {
            return 0;
        }

        public static int MoveTypeSet(ILuaState lua)
        {
            return 0;
        }

        public static int VelSet(ILuaState lua)
        {
            return 0;
        }

        public static int CtrlSet(ILuaState lua)
        {
            return 0;
        }

        public static int Pause(ILuaState lua)
        {
            return 0;
        }

        
    }
}
