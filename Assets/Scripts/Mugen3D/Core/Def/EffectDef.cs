using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Core
{
    public class EffectDef
    {
        public string name;
        public string posType;
        public Number[] pos;
        public int facing;
        public int bindTime;
        public Number[] vel;
        public Number[] accel;
        public int removeTime;
        public int superPauseMoveTime;
        public int pauseMoveTime;

        public static EffectDef ConstructNormal(string name, Number[] pos, int facing)
        {
            EffectDef effect = new EffectDef()
            {
                name = name,
                posType = "p1",
                pos = pos,
                facing = facing,
                bindTime = 1,
            };
            return effect;
        }
    }
}
