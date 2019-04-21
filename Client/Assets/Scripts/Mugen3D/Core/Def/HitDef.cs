using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class HitDef
    {
        public enum HitType
        {
            Attack = 0,
            Throw,
        }
        public enum GuardFlag
        {
            H = 1 << 0,
            L = 1 << 1,
        }
        public enum HitFlag
        {
            H = 1 << 0,
            L = 1 << 1,
            A = 1 << 2,
            F = 1 << 3,
            D = 1 << 4,
        }

        public enum GroundType
        {
            High = 0,
            Low,
        }

        public Unit owner;
        public Unit target;
        public int hitFlag;
        public int guardFlag;

        public int hitType;
        //params for attack, knock back
        public int forceLevel;
        public int groundType;
        //params for attack, knock away 
        public int knockAwayType = -1;

        public int hitDamage;
        public int[] hitPauseTime;
        public int hitSlideTime;
        public Number[] groundVel;
        public Number[] airVel;

        public int guardDamage;
        public int[] guardPauseTime;
        public int guardSlideTime;
        public Number[] guardVel;

        public Number groundCornerPush;
        public Number airCornerPush;

        //params for throw
        public int p1StateNo;
        public int p2StateNo;

        public string spark;
        public string guardSpark;
        public Number[] sparkPos;

        public string hitSound;
        public string guardSound;

        public bool moveHit = false;
        public bool moveGuarded = false;
        public bool moveContact = false;

        public HitDef()
        {
        }
    }
}
