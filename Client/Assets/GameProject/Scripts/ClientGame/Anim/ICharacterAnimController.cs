using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public interface ICharacterAnimController
    {
        void UpdateAnimSample(string animName, float normalizedTime);//for 3D
        void UpdateAnimSample(string animName, int frameNo);//for 2D
    }

}
