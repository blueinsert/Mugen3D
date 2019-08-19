using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 打击系统
    /// </summary>
    public class HitSystem : SystemBase
    {
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
        private static bool IsHitSuccess(ComplexCollider attacker,ComplexCollider defender)
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

        private static bool CanBeHit(HitDef hitDef, HitBy hitBy, NoHitBy noHitBy, PhysicsType defenderPhysicsType)
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

        private static bool CanBeGuard(HitDef hitDef, PhysicsType defenderPhysicsType)
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

        protected override void ProcessEntity(List<Entity> entities)
        {
            //获取成功产生打击的实体字典
            Dictionary<Entity, Entity> hitResults = new Dictionary<Entity, Entity>(10);
            for (int m = 0; m < entities.Count; m++)
            {
                var e1 = entities[m];
                var collideComponent1 = e1.GetComponent<CollideComponent>();
                var fsmComponent1 = e1.GetComponent<FSMComponent>();
                var hitComponent1 = e1.GetComponent<HitComponent>();
                for (int n = 0; n < entities.Count; n++)
                {
                    var e2 = entities[n];
                    if (e1 == e2)
                        continue;
                    var collideComponent2 = e2.GetComponent<CollideComponent>();
                    var fsmComponent2 = e2.GetComponent<FSMComponent>();
                    var hitComponent2 = e2.GetComponent<HitComponent>();
                    if (hitComponent1.MoveType != MoveType.Attack 
                        || hitComponent1.HitDef == null 
                        || hitComponent1.HitDef.moveContact == true//已经接触了的话这个hitdef已生效，不重复使用
                        )
                        continue;
                    //检查攻击框与受击框是否重合
                    if (IsHitSuccess(collideComponent1.Collider, collideComponent2.Collider))
                    {
                        hitResults[e1] = e2;
                    }
                }
            }
            foreach (var hitResult in hitResults)
            {
                var attackHitComponent = hitResult.Key.GetComponent<HitComponent>();
                var hitDef = attackHitComponent.HitDef;
                var targetMoveComponet = hitResult.Value.GetComponent<MoveComponent>();
                var targetHitComponent = hitResult.Value.GetComponent<HitComponent>();
                if (CanBeHit(hitDef, targetHitComponent.HitBy, targetHitComponent.NoHitBy, targetMoveComponet.PhysicsType))
                {
                    bool isBeGuarded = false;
                    if (CanBeGuard(hitDef, targetMoveComponet.PhysicsType) && (targetHitComponent.MoveType == MoveType.Defence))
                    {
                        isBeGuarded = true;
                    }
                    if (isBeGuarded && target.GetHP() - hitDef.guardDamage >= 0)
                    {
                        attackHitComponent.HitDef.moveContact = true;
                        attackHitComponent.HitDef.moveGuarded = true;
                        attackHitComponent.HitDef.moveHit = false;
                        //attackHitComponent.HitDef.target = target;
                        targetHitComponent.HitDef.moveContact = true;
                        targetHitComponent.HitDef.moveGuarded = true;
                        targetHitComponent.HitDef.moveHit = false;
                        //targetHitComponent.HitDef.target = target;
                    }
                    else
                    {
                        attackHitComponent.HitDef.moveContact = true;
                        attackHitComponent.HitDef.moveGuarded = false;
                        attackHitComponent.HitDef.moveHit = true;
                        //attackHitComponent.HitDef.target = target;
                        targetHitComponent.HitDef.moveContact = true;
                        targetHitComponent.HitDef.moveGuarded = false;
                        targetHitComponent.HitDef.moveHit = true;
                        //targetHitComponent.HitDef.target = target;
                        attacker.OnMoveHit(target);
                        target.OnBeHitted(hitDef);
                    }
                    if (!hitResults.ContainsKey(attacker))
                        attacker.Pause(isBeGuarded ? hitDef.guardPauseTime[0] : hitDef.hitPauseTime[0]);
                }
            }
        }
    }
}
