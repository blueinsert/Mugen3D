using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class HitVars
    {
        private enum AnimType
        {
            light = 0,
            medium,
            hard,
            back,
            up,
            diagup,
        }

        private enum GroundType
        {
            high = 0,
            low,
            trip,
        }

        public string attr;
        //This determines what type of state P2 must be in for P1 to hit. 
        public string hitFlag;
        //This determines how P2 may guard the attack. 
        public string guardFlag;

        public HitBoxLocation activeAttackBodyPart;
        private AnimType animType;
        private GroundType groundType;
        public int hitDamage;
        public int guardDamage;

        public int p1HitPauseTime;
        public int p2HitShakeTime;
        public int p2HitSlideTime;
        public int groundVelocityX;
        public int groundVelocityY;

        public int p1GuardPauseTime;
        public int p2GuardShakeTime;
        public int p2GuardSlideTime;

        private Dictionary<string, TokenList> param;
        private Dictionary<int, int> dic = new Dictionary<int,int>(); 

        public void SetInt(string id, ref int field, bool isRequired = true, int defaultValue = 0)
        {   
            if (param.ContainsKey(id))
            {
                int value;
                if (int.TryParse(param[id].asStr, out value))
                {
                    field = value;
                    dic[id.GetHashCode()] = value;
                }
                else
                {
                    Log.Error(id + " can't be recognized as int");
                }
            }
            else
            {
                if (isRequired)
                {
                    Log.Error(id + " can't be null");
                }
            }
        }

        public void SetString(string id, ref string field, bool isRequired = true, string defaultValue = "")
        {
            if (param.ContainsKey(id))
            {
                field = param[id].asStr; 
            }
            else
            {
                if (isRequired)
                {
                    Log.Error(id + " can't be null");
                }
            }
        }
        

        public HitVars(Dictionary<string, TokenList> param)
        {
            this.param = param;  
            SetString("attr", ref this.attr);
            SetString("hitFlag", ref this.hitFlag);
            SetString("guardFlag", ref this.guardFlag);
            if (param.ContainsKey("animType"))
            {
                this.animType = (AnimType)Enum.Parse(typeof(AnimType), param["animType"].asStr);
                this.dic["animType".GetHashCode()] = (int)this.animType;
            }
            if (param.ContainsKey("activeAttackBodyPart"))
            {
                this.activeAttackBodyPart = (HitBoxLocation)Enum.Parse(typeof(HitBoxLocation), param["activeAttackBodyPart"].asStr);
                this.dic["activeAttackBodyPart".GetHashCode()] = (int)this.activeAttackBodyPart;
            }
            if (param.ContainsKey("groundType"))
            {
                this.groundType = (GroundType)Enum.Parse(typeof(GroundType), param["groundType"].asStr);
                this.dic["groundType".GetHashCode()] = (int)this.groundType;
            }
            SetInt("hitDamage", ref this.hitDamage, false);
            SetInt("guardDamage", ref this.guardDamage, false);
            SetInt("p1HitPauseTime", ref this.p1HitPauseTime, true, 7);
            SetInt("p2HitShakeTime", ref this.p2HitShakeTime, true, 10);
            SetInt("p2HitSlideTime", ref this.p2HitSlideTime, true, 9);
            SetInt("groundVelocityX", ref this.groundVelocityX, true, -5);
            SetInt("groundVelocityY", ref this.groundVelocityY, true, 0);
           
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
