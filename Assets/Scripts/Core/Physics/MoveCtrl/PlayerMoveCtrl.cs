using System.Collections;
using System.Collections.Generic;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class PlayerMoveCtrl : MoveCtrl
    {
        private Character m_collidePlayer;
        private bool m_intersectTest = false;
 
        public PlayerMoveCtrl(Unit u):base(u)
        {
        } 

        public override void Update(Number deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
