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
        private static bool IsIntersect(ComplexCollider attacker,ComplexCollider defender)
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
            /*
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
            */
            return false;
        }

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


        /// <summary>
        /// 处理角落的力反馈：攻击角落的敌人，攻击者向远离方向运动
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        private static void ProcessCornerPush(Entity attacker, Entity target)
        {
            
            var hitDef = attacker.GetComponent<HitComponent>().HitDef;
            if (UtilityFuncs.GetFrontStageDist(attacker) < new Number(5) / new Number(10) && hitDef.moveContact)
            {
                Number velX = 0;
                if (hitDef.moveGuarded)
                {
                    velX = hitDef.guardVel.X();
                }else if (hitDef.moveHit)
                {
                    var targetPhysics = target.GetComponent<PhysicsComponent>();
                    if (targetPhysics.PhysicsType == PhysicsType.Air)
                        velX = hitDef.airVel.X();
                    else
                        velX = hitDef.groundVel.X();
                }
                var physics = attacker.GetComponent<PhysicsComponent>();
                var moveComponent = attacker.GetComponent<MoveComponent>();
                if (physics.PhysicsType == PhysicsType.Air)
                    moveComponent.VelAdd(-Number.Abs(velX) * hitDef.airCornerPush, 0);
                else
                    moveComponent.VelAdd(-Number.Abs(velX) * hitDef.groundCornerPush, 0);
            }
            
        }

        private static void ProcessHitSuccess(Entity attacker,Entity target)
        {
            
            //处理攻击者
            var hitComponent1 = attacker.GetComponent<HitComponent>();
            var hitDef1 = hitComponent1.HitDef;
            hitDef1.moveContact = true;
            hitDef1.moveGuarded = false;
            hitDef1.moveHit = true;
            if (hitDef1.hitType == (int)HitType.Throw)
            {
                var fsmComponent = attacker.GetComponent<FSMComponent>();
                fsmComponent.ChangeState(hitDef1.p1StateNo);
            }
            //处理被攻击者
            var fsmComponent2 = target.GetComponent<FSMComponent>();
            var moveComponent2 = target.GetComponent<MoveComponent>();
            var hitComponent2 = target.GetComponent<HitComponent>();
            hitComponent2.SetBeHitDef(hitDef1);
            if (hitDef1.hitType == (int)HitType.Attack)
            {
                var healthComponent = target.GetComponent<HealthComponent>();
                healthComponent.AddHP(-hitDef1.hitDamage);
                
                if (hitComponent2.MoveType == MoveType.BeingHitted)
                {
                    hitComponent2.AddBeHitCount();
                }
                else
                {
                    hitComponent2.ClearBeHitCount();
                    hitComponent2.AddBeHitCount();
                }
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
            }
            else if (hitDef1.hitType == (int)HitType.Throw)
            {
                fsmComponent2.ChangeState(hitDef1.p2StateNo);
            }
            //处理角落的力反馈：攻击角落的敌人，攻击者向远离方向运动
            ProcessCornerPush(attacker, target);
            
        }

        private static void ProcessHitBeGuard(Entity attacker,Entity target)
        {
            
            //处理攻击者
            var hitComponent1 = attacker.GetComponent<HitComponent>();
            var hitDef1 = hitComponent1.HitDef;
            hitDef1.moveContact = true;
            hitDef1.moveGuarded = true;
            hitDef1.moveHit = false;
            //处理防御者
            var fsmComponent2 = target.GetComponent<FSMComponent>();
            var moveComponent2 = target.GetComponent<MoveComponent>();
            var hitComponent2 = target.GetComponent<HitComponent>();
            var healthComponent2 = target.GetComponent<HealthComponent>();
            hitComponent2.SetBeHitDef(hitDef1);
            fsmComponent2.ChangeState(moveComponent2.PhysicsType == PhysicsType.Stand ? 150 : 156);
            healthComponent2.AddHP(-hitDef1.guardDamage);
            //处理角落的力反馈：攻击角落的敌人，攻击者向远离方向运动
            ProcessCornerPush(attacker, target);
            
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
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
                        || hitComponent1.HitDef == null 
                        || hitComponent1.HitDef.moveContact == true//已经接触了的话这个hitdef已生效，不重复使用
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
                var attackHitComponent = hitResult.Key.GetComponent<HitComponent>();
                var hitDef = attackHitComponent.HitDef;
                var targetMoveComponet = hitResult.Value.GetComponent<MoveComponent>();
                var targetHitComponent = hitResult.Value.GetComponent<HitComponent>();

                targetHitComponent.SetBeHitDef(hitDef);
                var fsm2 = hitResult.Value.GetComponent<FSMComponent>();
                fsm2.ChangeState(StateConst.StateNo_GetHitStandShake);
                /*
                if (CanBeHit(hitDef, targetHitComponent.HitBy, targetHitComponent.NoHitBy, targetMoveComponet.PhysicsType))
                {
                    if (CanBeGuard(hitDef, targetMoveComponet.PhysicsType) && (targetHitComponent.MoveType == MoveType.Defence))
                    {
                        //防御成功
                        ProcessHitBeGuard(attacker, target);
                    }
                    else
                    {
                        ProcessHitSuccess(attacker, target);
                    }
                    //todo
                    //if (!hitResults.ContainsKey(attacker))
                    //    attacker.Pause(isBeGuarded ? hitDef.guardPauseTime[0] : hitDef.hitPauseTime[0]);
                }
                */
            }
            
        }
    }
}
