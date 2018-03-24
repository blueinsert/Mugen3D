using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public enum MoveType
    {
        Attack,
        Idle,
        Defence,
        BeingHitted,
    }

    public enum PhysicsType
    {
        N = -1,
        S,
        C,
        A,
    }

    public class Status
    {
        public MoveType moveType = MoveType.Idle;
        public PhysicsType physicsType = PhysicsType.S;
        public bool pushTest = true;
        public bool ctrl = true;
        public int animNo = -1;
    }

}