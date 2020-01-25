using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 打击系统
    /// </summary>
    public class HitSystem : SystemBase
    {
        public HitSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<CollideComponent>() != null && e.GetComponent<FSMComponent>() != null && e.GetComponent<HitComponent>() != null;
        }

        /// <summary>
        /// 判断攻击者的攻击框和防御者的防御框是否重合
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static bool IsIntersect(ComplexCollider attacker, ComplexCollider defender)
        {
            for (int i = 0; i < attacker.AttackClsnsLength; i++)
            {
                var attackClsn = attacker.AttackClsns[i];
                for (int j = 0; j < defender.DefenceClsnsLength; j++)
                {
                    var defenceClsn = defender.DefenceClsns[j];
                    ContactInfo contactInfo;
                    if (PhysicsUtils.RectColliderIntersectTest(attackClsn, defenceClsn, out contactInfo))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
        private static bool CanBeHit(HitDefData hitDef, HitBy hitBy, NoHitBy noHitBy, PhysicsType defenderPhysicsType)
        {
            if (hitBy != null && !hitBy.Check(hitDef))
                return false;
            if (noHitBy != null && noHitBy.Check(hitDef))
                return false;
            int hitFlag = hitDef.hitFlag;
            if ((hitFlag & (int)HitFlag.H) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Stand)
                    return true;
            }
            if ((hitFlag & (int)HitFlag.L) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Crouch)
                    return true;
            }
            
            if ((hitFlag & (int)HitFlag.A) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Air && (this.fsmMgr.stateNo != 5050))
                    return true;
            }
            if ((hitFlag & (int)HitFlag.F) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Air && (fsmMgr.stateNo == 5050))
                    return true;
            }
            if ((hitFlag & (int)HitFlag.D) != 0)
            {
            }
            
            return false;
        }
   */

            /*
        private static bool CanBeGuard(HitDefData hitDef, PhysicsType defenderPhysicsType)
        {
            int guardFlag = hitDef.guardFlag;
            if ((guardFlag & (int)GuardFlag.H) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Stand)
                {
                    return true;
                }
            }
            if ((guardFlag & (int)GuardFlag.L) != 0)
            {
                if (defenderPhysicsType == PhysicsType.Crouch)
                {
                    return true;
                }
            }
            return false;
        }
        */

        /// <summary>
        /// 处理角落的力反馈：攻击角落的敌人，攻击者向远离方向运动
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        public static void ProcessCornerPush(Entity attacker)
        {
            var physics = attacker.GetComponent<PhysicsComponent>();
            var hit = attacker.GetComponent<HitComponent>();
            var hitDef = attacker.GetComponent<HitComponent>().HitDef;
            //只要敌人靠墙站着，每次打击都会产生对于自身的反作用，拉开距离避免无限连
            if (UtilityFuncs.GetP2BackStageDist(attacker)< new Number(1)/2 && hit.MoveContact)
            {
                Number velX = 0;
                if (hit.MoveGuarded)
                {
                    velX = hitDef.guardVel.x;
                }
                else if (hit.MoveHit)
                {
                    if (physics.PhysicsType == PhysicsType.Air)
                        velX = hitDef.airVel.x;
                    else
                        velX = hitDef.groundVel.x;
                }
                var moveComponent = attacker.GetComponent<MoveComponent>();
                if (physics.PhysicsType == PhysicsType.Air)
                    moveComponent.VelAdd(velX* hitDef.airCornerPush, 0);
                else
                    moveComponent.VelAdd(velX * hitDef.groundCornerPush, 0);
            }

        }

        private static void OnHitSuccessForAttacker(Entity attacker)
        {
            var hit = attacker.GetComponent<HitComponent>();
            hit.OnMoveHit();
            var hitDef = hit.HitDef;
            var fsm = attacker.GetComponent<FSMComponent>();
            if (hitDef.hitType == HitType.Throw)
            {  
                fsm.ChangeState(hitDef.p1StateNo);
            }
            else
            {
                fsm.PushLayer(StateConst.StateNO_HitPause);
            }
        }

        private static void OnHitSuccessForTarget(Entity target)
        {
            //处理被攻击者
            var fsm = target.GetComponent<FSMComponent>();
            var move = target.GetComponent<MoveComponent>();
            var hit = target.GetComponent<HitComponent>();
            var basic = target.GetComponent<BasicInfoComponent>();
            var hitDef = hit.BeHitData;
            if (hitDef.hitType == (int)HitType.Attack)
            {
                //var healthComponent = target.GetComponent<HealthComponent>();
                //healthComponent.AddHP(-hitDef.hitDamage);
                //设置被连击计数
                if (basic.MoveType == MoveType.BeingHitted)
                {
                    hit.AddBeHitCount();
                }
                else
                {
                    hit.ClearBeHitCount();
                    hit.AddBeHitCount();
                }
                fsm.ChangeState(5000);
                /*
                if (hitDef1.knockAwayType == -1)
                {
                    if (moveComponent2.PhysicsType == PhysicsType.Stand)
                    {
                        fsmComponent2.ChangeState(5000);
                    }
                    else if (moveComponent2.PhysicsType == PhysicsType.Crouch)
                    {
                        fsmComponent2.ChangeState(5010);
                    }
                    else if (moveComponent2.PhysicsType == PhysicsType.Air)
                    {
                        fsmComponent2.ChangeState(5020);
                    }
                }
                else
                {
                    fsmComponent2.ChangeState(5030);
                }
                */
            }
            else if (hitDef.hitType == HitType.Throw)
            {
                fsm.ChangeState(hitDef.p2StateNo);
            }
        }

       
         private static void OnHitGuardedForAttacker(Entity attacker)
        {
            var hit = attacker.GetComponent<HitComponent>();
            hit.OnMoveGuarded();
            var fsm = attacker.GetComponent<FSMComponent>();
            fsm.PushLayer(StateConst.StateNO_HitPause);
        }   
       
        private static void OnHitGuardedForTarget(Entity target)
        {
            var fsm = target.GetComponent<FSMComponent>();
            var hit = target.GetComponent<HitComponent>();
            var hitdef = hit.BeHitData;
            //var health = target.GetComponent<HealthComponent>();
            fsm.ChangeState(StateConst.StateNo_GuardingShake);
            //health.AddHP(-hitdef.guardDamage);
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                var hit = e.GetComponent<HitComponent>();
                hit.Update();
            }
            //遍历查找hit的产生
            //key:打击者 value:承受者
            Dictionary<Entity, Entity> hitResults = new Dictionary<Entity, Entity>(10);
            for (int m = 0; m < entities.Count; m++)
            {
                var e1 = entities[m];
                var collideComponent1 = e1.GetComponent<CollideComponent>();
                var hitComponent1 = e1.GetComponent<HitComponent>();
                var basic = e1.GetComponent<BasicInfoComponent>();
                for (int n = 0; n < entities.Count; n++)
                {
                    var e2 = entities[n];
                    if (e1 == e2)
                        continue;
                    var collideComponent2 = e2.GetComponent<CollideComponent>();
                    if (basic.MoveType != MoveType.Attack
                        || !hitComponent1.IsActive()
                        )
                        continue;
                    //检查攻击框与受击框是否重合
                    if (IsIntersect(collideComponent1.Collider, collideComponent2.Collider))
                    {
                        hitResults[e1] = e2;
                    }
                }
            }
            foreach (var hitResult in hitResults)
            {
                var attacker = hitResult.Key;
                var target = hitResult.Value;
                var hit1 = attacker.GetComponent<HitComponent>();
                var hitDef = hit1.HitDef;
                var hit2 = target.GetComponent<HitComponent>();
                var basic2 = target.GetComponent<BasicInfoComponent>();

                if (true)
                {
                    if (hitDef.guardFlag == GuardFlag.Normal && (basic2.MoveType == MoveType.Defence))
                    {
                        OnHitGuardedForAttacker(attacker);
                        hit2.SetBeHitDef(hitDef);
                        OnHitGuardedForTarget(target);
                    }
                    else
                    {
                        OnHitSuccessForAttacker(attacker);
                        hit2.SetBeHitDef(hitDef);
                        OnHitSuccessForTarget(target);
                    }
                    //ProcessCornerPush(attacker);
                }
                
            }

        }
    }
}
