using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum DecisionBoxType
    {
        Attack,
        Defence,
        Collide,
    }

    public enum HitBoxLocation
    {
        Hand_L,
        Hand_R,
        Foot_L,
        Foot_R,
    }

    public enum DefenceBoxLocdation
    {
        Default,
    }

    [System.Serializable]
    public class DecisionBox
    {
        public string name;
        public DecisionBoxType type;
        public Cuboid cuboid;
        public bool visualable;
    }

    [System.Serializable]
    public class HitBox : DecisionBox
    {
        public HitBoxLocation location;   
    }

    [System.Serializable]
    public class DefenceBox : DecisionBox
    {
        public DefenceBoxLocdation location = DefenceBoxLocdation.Default;
    }

    [System.Serializable]
    public class CollideBox : DecisionBox
    {

    }

}