using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public static class UtilityFuncs 
    {
        public static Number GetP2BackStageDist(Entity target)
        {
            var enemy = (target.World as BattleWorld).GetEnemy(target);
            var dist = GetBackStageDist(enemy);
            return dist;
        }

        /// <summary>
        /// 获得距离背后的舞台边界的距离
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Number GetBackStageDist(Entity target)
        {
            var stateComponet = target.World.GetSingletonComponent<StageComponent>();
            var playerComponent = target.GetComponent<BasicInfoComponent>();
            var transform = target.GetComponent<TransformComponent>();
            if (transform.Facing > 0)//向右
            {
                return transform.Position.x - stateComponet.BorderXMin;
            }
            else
            {
                return stateComponet.BorderXMax - transform.Position.x;
            }
        }

        /// <summary>
        /// 获得距离前方的舞台边界的距离
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Number GetFrontStageDist(Entity target)
        {
            var stateComponet = target.World.GetSingletonComponent<StageComponent>();
            var playerComponent = target.GetComponent<BasicInfoComponent>();
            var moveComponent = target.GetComponent<MoveComponent>();
            var transform = target.GetComponent<TransformComponent>();

            if (transform.Facing > 0)//向右
            {
                return stateComponet.BorderXMax - transform.Position.x;
            }
            else
            {
                return transform.Position.x - stateComponet.BorderXMin;
            }
        }

        /// <summary>
        /// 获取距离前方的屏幕边界的距离
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Number GetFrontEdgeDist(Entity target)
        {
            var moveComponent = target.GetComponent<MoveComponent>();
            var cameraComponet = target.World.GetSingletonComponent<CameraComponent>();
            var viewPort = cameraComponet.ViewPort;
            var playerComponent = target.GetComponent<BasicInfoComponent>();
            var transform = target.GetComponent<TransformComponent>();

            if (transform.Facing > 0)
            {
                return viewPort.XMax - transform.Position.x;
            }
            else
            {
                return transform.Position.x - viewPort.XMin;
            }
        }

        /// <summary>
        /// 获取距离身后的屏幕边界的距离
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Number GetBackEdgeDist(Entity target)
        {
            var moveComponent = target.GetComponent<MoveComponent>();
            var cameraComponet = target.World.GetSingletonComponent<CameraComponent>();
            var viewPort = cameraComponet.ViewPort;
            var playerComponent = target.GetComponent<BasicInfoComponent>();
            var transform = target.GetComponent<TransformComponent>();

            if (transform.Facing > 0)
            {
                return transform.Position.x - viewPort.XMin;
            }
            else
            {
                return viewPort.XMax - transform.Position.x;
            }
        }

        /// <summary>
        /// 获取距离敌人的距离
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector GetP2Dist(Entity target)
        {
            var transform1 = target.GetComponent<TransformComponent>();
            var w = target.World as BattleWorld;
            var enemy = w.GetEnemy(target);
            var transform2 = enemy.GetComponent<TransformComponent>();
            return transform2.Position - transform1.Position;
        }

        public static int GetP2StateNo(Entity e)
        {
            var w = e.World as BattleWorld;
            var enemy = w.GetEnemy(e);
            var fsm = enemy.GetComponent<FSMComponent>();
            return fsm.StateNo;
        }

        public static MoveType GetP2MoveType(Entity e)
        {
            var w = e.World as BattleWorld;
            var enemy = w.GetEnemy(e);
            var basic = enemy.GetComponent<BasicInfoComponent>();
            return basic.MoveType;
        }
    }

    
}
