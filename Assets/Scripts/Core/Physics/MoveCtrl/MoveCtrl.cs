using System;
using System.Collections;
using System.Collections.Generic;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{

    public abstract class MoveCtrl
    {
        public Vector gravity
        {
            get
            {
                return m_gravity;
            }
        }

        public Vector velocity
        {
            get
            {
                return m_velocity;
            }
        }
       
        public bool isOnGround = true;
        public bool justOnGround = false;

        protected Unit m_owner;

        protected Vector m_velocity = Vector.zero;
        protected Vector m_acceleratedVelocity = Vector.zero;
        protected Vector m_deltaPos = Vector.zero;
        protected Vector m_gravity = new Vector(0, -10, 0);
        protected Number mass = 70;
        protected Vector m_externalForce = Vector.zero;
        protected Number groundFrictionFactor = 3;

        private static Number TINY = new Number(2) / new Number(10);

        public MoveCtrl(Unit unit) {
            m_owner = unit;
        }

        public virtual void Update(Number deltaTime)
        {
            if (justOnGround)
            {
                isOnGround = true;
                justOnGround = false;
            }
            //Log.Info("force:" + m_externalForce.ToString());
            if (m_owner.status.physicsType == PhysicsType.S || m_owner.status.physicsType == PhysicsType.C)
            {
                m_acceleratedVelocity = (m_gravity.magnitude * mass + m_externalForce.y) / mass * groundFrictionFactor * (-velocity.normalized) + m_externalForce / mass;
            }
            else if (m_owner.status.physicsType == PhysicsType.A)
            {
                m_acceleratedVelocity = m_gravity + m_externalForce / mass;
            }
            //Log.Info("accler:" + m_acceleratedVelocity.ToString());
         
            m_velocity += deltaTime * m_acceleratedVelocity;
            if (m_owner.status.physicsType == PhysicsType.S || m_owner.status.physicsType == PhysicsType.C)
            {
                m_velocity = StabilizeVel(velocity);
            }
            AddPos(m_velocity * deltaTime);
        }

        private Vector StabilizeVel(Vector v)
        {
            Number x, y, z;
            x = Math.Abs(v.x) < TINY ? 0 : v.x;
            y = Math.Abs(v.y) < TINY ? 0 : v.y;
            z = Math.Abs(v.z) < TINY ? 0 : v.z;
            return new Vector(x, y, z);
        }

        public Vector AddPos(Vector deltaPos)
        {
            m_deltaPos = deltaPos;
            CollideTest();
            var pos = m_owner.position;
            pos += m_deltaPos;
            m_owner.position = pos;
            return m_deltaPos;
        }

        public void VelSet(Number velx, Number vely)
        {
            this.m_velocity = new Vector(velx * m_owner.facing, vely, 0);
        }

        public void VelAdd(Number deltaX, Number deltaY)
        {
            this.m_velocity.x += deltaX;
            this.m_velocity.y += deltaY;
        }

        public void PosSet(Number x, Number y)
        {
            m_owner.position = new Vector(x, y, 0);
        }

        public void PosAdd(Number deltaX, Number deltaY)
        {
            var pos = m_owner.position;
            pos.x += deltaX;
            pos.y += deltaY;
            m_owner.position = pos;
        }

        public void SetGravity(Number x, Number y)
        {
            m_gravity = new Vector(x, y, 0);
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

        private void CollideTest()
        {
            Number playerWidth = new Number(7)/new Number(10);
            var pos = this.m_owner.position;
            var newPos = pos + m_deltaPos;
            var viewportRect = m_owner.world.camCtl.viewportRect;
            if (newPos.x < viewportRect.xMin + playerWidth)
            {
                newPos.x = viewportRect.xMin + playerWidth;
            }
            if (newPos.x > viewportRect.xMax - playerWidth)
            {
                newPos.x = viewportRect.xMax - playerWidth;
            }
            if (newPos.x < m_owner.world.config.stageConfig.borderXMin)
           {
               newPos.x = m_owner.world.config.stageConfig.borderXMin;
           }
            if (newPos.x > m_owner.world.config.stageConfig.borderXMax)
           {
               newPos.x = m_owner.world.config.stageConfig.borderXMax;
           }
            if (newPos.y < m_owner.world.config.stageConfig.borderYMin)
           {
               justOnGround = true;
               newPos.y = m_owner.world.config.stageConfig.borderYMin;
           }
           m_deltaPos = newPos - pos;
           var enemy = m_owner.world.teamInfo.GetEnemy(m_owner as Character);
           bool findIntersect = false;
           foreach (var clsn in m_owner.animCtr.curActionFrame.clsns)
           {
               foreach (var clsn2 in enemy.animCtr.curActionFrame.clsns)
               {
                   if (clsn.type == 1 && clsn2.type == 1)
                   {
                       Rect rect1 = new Rect(new Vector(clsn.x1, clsn.y1, 0), new Vector(clsn.x2, clsn.y2, 0));
                       rect1.position += new Vector(m_owner.position.x, m_owner.position.y, 0);
                       Rect rect2 = new Rect(new Vector(clsn2.x1, clsn2.y1, 0), new Vector(clsn2.x2, clsn2.y2, 0));
                       rect2.position += new Vector(enemy.position.x, enemy.position.y, 0);
                       if (PhysicsUtils.RectRectTest(rect1, rect2))
                       {
                           Vector dir = (rect1.position - rect2.position).normalized;
                           Number distX = (rect1.width + rect2.width) / 2 - Math.Abs(rect1.position.x - rect2.position.x);
                           m_deltaPos += new Vector(distX * new Number(1) / new Number(4) * (dir.x > 0 ? 1 : -1), 0, 0);
                       }
                       findIntersect = true;
                       break;
                   }
               }
               if (findIntersect)
                   break;
           }
        }

    }
}
