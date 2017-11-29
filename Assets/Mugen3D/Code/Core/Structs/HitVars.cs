using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class HitVars
    {
        public string attr;
        //This determines what type of state P2 must be in for P1 to hit. 
        public string hitFlag;
        //This determines how P2 may guard the attack. 
        public string guardFlag;
        public string animType;

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

        private void SetValue(string id)
        {
            if (param.ContainsKey(id))
            {
                Type type = this.GetType();
                System.Reflection.FieldInfo propertyInfo = type.GetField(id);
                Log.Info(id);
                Log.Info(propertyInfo.ToString() + ":" + id);
                if (propertyInfo.FieldType == typeof(int))
                {
                    int value;
                    if (!int.TryParse(param[id].asStr, out value))
                    {
                        Log.Error(id + " can't be recognized as int");
                        return;
                    }
                    propertyInfo.SetValue(this, value);
                    dic[id.GetHashCode()] = value;
                }
                else if (propertyInfo.FieldType == typeof(Enum))
                {
                    var value = Enum.Parse(propertyInfo.FieldType, param[id].asStr);
                    propertyInfo.SetValue(this, value);
                }
            }
            else
            {
                Log.Error("hitdef's " + id +" can't be null");
            }
        }

        private void SetAttr()
        {
            if (param.ContainsKey("attr"))
            {
                
                this.attr = param["attr"].asStr;
                dic["attr".GetHashCode()] = this.attr.GetHashCode();
                
            }
            else
            {
                Log.Error("hitdef's attr can't be null");
            }
        }
        private void SetHitFlag()
        {
            if (param.ContainsKey("hitFlag"))
            {
                this.hitFlag = param["hitFlag"].asStr;
                dic["hitFlag".GetHashCode()] = this.hitFlag.GetHashCode();
            }
            else
            {
                Log.Error("hitdef's hitFlag can't be null");
            }
        }
        private void SetGuardFlag()
        {
            if (param.ContainsKey("guardFlag"))
            {
                this.guardFlag = param["guardFlag"].asStr;
                dic["guardFlag".GetHashCode()] = this.guardFlag.GetHashCode();
            }
            else
            {
                Log.Error("hitdef's guardFlag can't be null");
            }
        }
        private void SetAnimType()
        {
            if (param.ContainsKey("animType"))
            {
                this.animType = param["animType"].asStr;
                dic["animType".GetHashCode()] = this.animType.GetHashCode();
            }
            else
            {
                Log.Error("hitdef's animType can't be null");
            }
        }
        private void SetHitDamage()
        {
            if (param.ContainsKey("hitDamage"))
            {
                int value;
                if (int.TryParse(param["hitDamage"].asStr, out value))
                {
                    this.hitDamage = value;
                }
                else
                {
                    Log.Error("hitDamage can't be recognized as int");
                }
            }
        }
        private void SetGuardDamage()
        {
            if (param.ContainsKey("guardDamage"))
            {
                int value;
                if (int.TryParse(param["guardDamage"].asStr, out value))
                {
                    this.guardDamage = value;
                }
                else
                {
                    Log.Error("guardDamage can't be recognized as int");
                }
            }
        }
        private void SetP1HitPauseTime()
        {
            if (param.ContainsKey("p1HitPauseTime"))
            {
                int value;
                if (int.TryParse(param["p1HitPauseTime"].asStr, out value))
                {
                    this.p1HitPauseTime = value;
                    dic["p1HitPauseTime".GetHashCode()] = value;
                }
                else
                {
                    Log.Error("p1HitPauseTime can't be recognized as int");
                }
            }
            else
            {
                Log.Error("p1HitPauseTime can't be null");
            }
        }
        private void SetP2HitShakeTime()
        {
            if (param.ContainsKey("p2HitShakeTime"))
            {
                int value;
                if (int.TryParse(param["p2HitShakeTime"].asStr, out value))
                {
                    this.p2HitShakeTime = value;
                    dic["p2HitShakeTime".GetHashCode()] = value;
                }
                else
                {
                    Log.Error("p2HitShakeTime can't be recognized as int");
                }
            }
            else
            {
                Log.Error("p2HitShakeTime can't be null");
            }
        }
        private void SetP2HitSlideTime()
        {
            if (param.ContainsKey("p2HitSlideTime"))
            {
                int value;
                if (int.TryParse(param["p2HitSlideTime"].asStr, out value))
                {
                    this.p2HitSlideTime = value;
                    dic["p2HitSlideTime".GetHashCode()] = value;
                }
                else
                {
                    Log.Error("p2HitSlideTime can't be recognized as int");
                }
            }
            else
            {
                Log.Error("p2HitSlideTime can't be null");
            }
        }
        private void SetGroundVelocityX()
        {
            if (param.ContainsKey("groundVelocityX"))
            {
                int value;
                if (int.TryParse(param["groundVelocityX"].asStr, out value))
                {
                    this.groundVelocityX = value;
                    dic["groundVelocityX".GetHashCode()] = value;
                }
                else
                {
                    Log.Error("groundVelocityX can't be recognized as int");
                }
            }
            else
            {
                Log.Error("groundVelocityX can't be null");
            }
        }
        private void SetGroundVelocityY()
        {
            if (param.ContainsKey("groundVelocityY"))
            {
                int value;
                if (int.TryParse(param["groundVelocityY"].asStr, out value))
                {
                    this.groundVelocityY = value;
                    dic["groundVelocityY".GetHashCode()] = value;
                }
                else
                {
                    Log.Error("groundVelocityY can't be recognized as int");
                }
            }
            else
            {
                Log.Error("groundVelocityY can't be null");
            }
        }

        public HitVars(Dictionary<string, TokenList> param)
        {
            this.param = param;
            //use reflection
            /*
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
           */
            SetAttr();
            SetHitFlag();
            SetGuardFlag();
            SetAnimType();
            SetHitDamage();
            SetGuardDamage();
            SetP1HitPauseTime();
            SetP2HitShakeTime();
            SetP2HitSlideTime();
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
