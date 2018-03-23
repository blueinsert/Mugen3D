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
        None = -1,
        Stand,
        Crouch,
        Air,
    }

    public class Status
    {
        public MoveType moveType = MoveType.Idle;
        public PhysicsType physicsType = PhysicsType.Stand;
        public bool pushTest = true;
        public bool ctrl = true;
    }

}