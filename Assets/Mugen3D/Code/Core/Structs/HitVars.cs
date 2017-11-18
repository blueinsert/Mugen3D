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
        public HitFlag hitFlag;
        public GrardFlag guardFlag;
        public int hitDamage;
        public int guardDamage;
        public int p1PauseTime;
        public int p2ShakeTime;
        public int slideTime;

        private Dictionary<int, int> dic = new Dictionary<int,int>();

        private void Prepare(string key, string v)
        {
            dic.Add(key.GetHashCode(), v.GetHashCode());
        }

        private void Prepare(string key, int v)
        {
            dic.Add(key.GetHashCode(), v);
        }

        public void Prepare()
        {
            Prepare("attr", attr.ToString());
            Prepare("hitFlag", hitFlag.ToString());
            Prepare("guardFlag", guardFlag.ToString());
            Prepare("hitDamage", hitDamage);
            Prepare("guardDamage", guardDamage);
            Prepare("p1PauseTime", p1PauseTime);
            Prepare("p2ShakeTime", p2ShakeTime);
            Prepare("slideTime", slideTime);
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
