using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public static class UtilityFuncs 
    {
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
            //todo
            return Vector.zero;
        }
    }

    
}
