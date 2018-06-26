using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerMoveCtrl : MoveCtrl
    {
        private Character m_collidePlayer;
        private bool m_intersectTest = false;
 
        public PlayerMoveCtrl(Unit u):base(u)
        {
        } 

        public override void Update()
        {
            base.Update();
        }

        protected  void OnHitCollider(RaycastHit hitResult)
        {
            /*
            Debug.Log("hit tar, tag:" + hitResult.collider.tag + " normal:" + hitResult.normal + " dis:" + hitResult.distance);
            if (hitResult.collider.owner != null && hitResult.collider.owner is Character)
            {   
                var collidePlayer = hitResult.collider.owner as Character;
                var normal = hitResult.normal;
                m_collidePlayer = collidePlayer;
                if (Mathf.Abs(normal.x) == 1)
                {
                    Vector3 deltaPos = new Vector3(m_deltaPos.x/2, 0, 0);
                    var realDeltaPos = collidePlayer.moveCtr.AddPos(deltaPos);
                    m_deltaPos.x = realDeltaPos.x;
                }
                else if (Mathf.Abs(normal.y) == 1)
                {
                    m_deltaPos.y = -normal.y * hitResult.distance;
                    m_intersectTest = true;
                }  
            }
            else if (hitResult.collider.owner == null)
            {
                m_deltaPos = m_velocity.normalized*hitResult.distance;
                if(hitResult.normal.y > 0){
                    justOnGround = true;
                }
                if (Mathf.Abs(hitResult.normal.x) > 0.99)
                {
                    m_velocity.x = 0;
                }
               
            }
             */
        }
       
    }
}
