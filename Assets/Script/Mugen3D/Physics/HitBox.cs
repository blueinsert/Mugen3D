using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum HitBoxType
    {
        Attack_Hand_L,
        Attack_Hand_R,
        Defence_Body,
        Defence_Thigh_L,
        Defence_Thigh_R,
        Defence_Calf_L,
        Defence_Calf_R,
        Defence_Head,
        Defence_UpperArm_L,
        Defence_UpperArm_R,
        Defence_ForeArm_L,
        Defence_ForeArm_R,
    }

    [System.Serializable]
    public class HitBox
    {
        public string name;
        public HitBoxType type;
        public Cuboid cuboid;
        public bool visualable;
    }
}