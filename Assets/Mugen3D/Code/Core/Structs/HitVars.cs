using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum HitAttr
    {
        A,//普通攻击
        T,//抓投
        P,//飞行道具
    }

    //此参数说明如果p1要集中p2,p2必须处于的状态类型
    public enum HitFlag{
        H,
        L,
        A,
        M,
        F,
        D,
    }

    //此参数决定p2要如何防御住这次攻击
    public enum GrardFlag
    {
        H,
        L,
        A,
        M,
    }

    public enum HittedAnimType
    {
        light,
        medium,
        hard,
        back,
        up,
        diagup,
    }

    public class HitVars
    {
        public HitAttr attr;
        //This determines what type of state P2 must be in for P1 to hit. 
        public HitFlag hitFlag;
        //This determines how P2 may guard the attack. 
        public GrardFlag guardFlag;
        public HittedAnimType animType;

        public int hitDamage;
        public int guardDamage;

        public int p1HitPauseTime;
        public int p2HitShakeTime;
        public int p2HitSlideTime;

        public int p1GuardPauseTime;
        public int p2GuardShakeTime;
        public int p2GuardSlideTime;

        private Dictionary<string, string> param;
        private Dictionary<int, int> dic = new Dictionary<int,int>();

        private void SetValue(string id)
        {
            if (param.ContainsKey(id))
            {
                Type type = this.GetType();
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(id);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    int value;
                    if (!int.TryParse(param[id], out value))
                    {
                        Log.Error(id + " can't be recognized as int");
                        return;
                    }
                    propertyInfo.SetValue(this, value, null);
                    dic[id.GetHashCode()] = value;
                }
                else if (propertyInfo.PropertyType == typeof(Enum))
                {
                    var value = Enum.Parse(propertyInfo.PropertyType, param[id]);
                    propertyInfo.SetValue(this, value, null);
                }
            }
            else
            {
                Log.Error("hitdef's " + id +" can't be null");
            }
        }

        public HitVars(Dictionary<string, string> param)
        {
            this.param = param;
            SetValue("attr");
            SetValue("hitFlag");
            SetValue("guardFlag");
            SetValue("animType");
            SetValue("hitDamage");
            SetValue("guardDamage");
            SetValue("p1HitPauseTime");
            SetValue("p2HitShakeTime");
            SetValue("p2HitSlideTime");
            SetValue("p1GuardPauseTime");
            SetValue("p2GuardShakeTime");
            SetValue("p2GuardSlideTime");
            /*
            if (param.ContainsKey("attr"))
            {
                switch (param["attr"])
                {
                    case "A":
                        this.attr = HitAttr.A; break;
                    case "T":
                        this.attr = HitAttr.T; break;
                    case "P":
                        this.attr = HitAttr.P; break;
                    default:
                        Log.Error("hitdef's attr can't be recongnized"); break;
                    
                }
            }
            else
            {
                Log.Error("hitdef's attr can't be null");
            }
            if (param.ContainsKey("hitFlag"))
            {
                switch (param["hitFlag"])
                {
                    case "H":
                        this.hitFlag = HitFlag.H; break;
                    case "L":
                        this.hitFlag = HitFlag.L; break;
                    case "A":
                        this.hitFlag = HitFlag.A; break;
                    case "M":
                        this.hitFlag = HitFlag.M; break;
                    case "F":
                        this.hitFlag = HitFlag.F; break;
                    case "D":
                        this.hitFlag = HitFlag.D;break;
                    default :
                        Log.Error("hitdef's hitFlag can't be recongnized"); break;
                }
            }
            else
            {
                Log.Error("hitdef's hitFlag can't be null");
            }
            if (param.ContainsKey("guardFlag"))
            {
                switch (param["guardFlag"])
                {
                    case "H":
                        this.guardFlag = GrardFlag.H; break;
                    case "L":
                        this.guardFlag = GrardFlag.L; break;
                    case "A":
                        this.guardFlag = GrardFlag.A; break;
                    case "M":
                        this.guardFlag = GrardFlag.M; break;
                    default:
                        Log.Error("hitdef's guardFlag can't be recongnized"); break;
                }
            }
            else
            {
                Log.Error("hitdef's guardFlag can't be null");
            }
            if (param.ContainsKey("animType"))
            {
                switch (param["animType"])
                { 
                    case "light":
                        this.animType = HittedAnimType.light; break;
                    case "medium":
                        this.animType = HittedAnimType.medium; break;
                    case "hard":
                        this.animType = HittedAnimType.hard; break;
                    case "up":
                        this.animType = HittedAnimType.up; break;
                    case "diagup":
                        this.animType = HittedAnimType.diagup;break;
                    default:
                         Log.Error("hitdef's animType can't be recongnized"); break;
                }
            }
            else
            {
                Log.Error("hitdef's animType can't be null");
            }
            if (param.ContainsKey("hitDamage"))
            {
                int value;
                if (int.TryParse(param["hitDamage"], out value))
                {
                    this.hitDamage = value;
                }
                else
                {
                    Log.Error("hitDamage can't be recognized as int");
                }
            }
            if (param.ContainsKey("guardDamage"))
            {
                int value;
                if (int.TryParse(param["guardDamage"], out value))
                {
                    this.guardDamage = value;
                }
                else
                {
                    Log.Error("guardDamage can't be recognized as int");
                }
            }
            if (param.ContainsKey("p1PauseTime"))
            {
                int value;
                if (int.TryParse(param["p1PauseTime"], out value))
                {
                    this.p1HitPauseTime = value;
                }
                else
                {
                    Log.Error("p1PauseTime can't be recognized as int");
                }
            }
            if (param.ContainsKey("p2ShakeTime"))
            {
                int value;
                if (int.TryParse(param["p2ShakeTime"], out value))
                {
                    this.p2HitShakeTime = value;
                }
                else
                {
                    Log.Error("p2ShakeTime can't be recognized as int");
                }
            }
            if (param.ContainsKey("slideTime"))
            {
                int value;
                if (int.TryParse(param["slideTime"], out value))
                {
                    this.p2HitSlideTime = value;
                }
                else
                {
                    Log.Error("slideTime can't be recognized as int");
                }
            }
             */
        }

        public int GetHitVar(int key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                return -1;
            }
        }
    }
}
