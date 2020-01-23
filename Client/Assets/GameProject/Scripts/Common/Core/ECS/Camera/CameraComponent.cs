using System.Collections;
using System.Collections.Generic;
using bluebean.UGFramework.ConfigData;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class CameraComponent : ComponentBase
    {
        public Vector Position { get { return m_position; } }
        public Number FieldOfView { get { return m_fieldOfView; } }
        public Number Aspect { get { return m_aspect; } }
        public Number ZValue { get { return m_zValue; } }
        public Rect ViewPort { get { return m_viewPort; } }
        /// <summary>
        /// 当前的视口
        /// </summary>
        private Rect m_viewPort = new Rect();
        /// <summary>
        /// 当前的摄像机位置
        /// </summary>
        private Vector m_position = Vector.zero;
        /// <summary>
        /// 当前的视角
        /// </summary>
        private Number m_fieldOfView;

        #region 配置字段
        /// <summary>
        /// yOffset
        /// </summary>
        private Number m_yOffset;
        /// <summary>
        /// 宽高比
        /// </summary>
        private Number m_aspect;
        /// <summary>
        /// 最小视角
        /// </summary>
        private Number m_minFieldOfView;
        /// <summary>
        /// 最大视角
        /// </summary>
        private Number m_maxFieldOfView;
        /// <summary>
        /// 摄像机坐标z值
        /// </summary>
        private Number m_zValue;
        /// <summary>
        /// 摄像机跟随平滑值
        /// </summary>
        private Number m_dumpRatio;
        #endregion

        public CameraComponent Init(ConfigDataCamera config)
        {
            //初始化配置
            m_yOffset = config.Yoffset * Number.EN4;
            m_aspect = config.Aspect*Number.EN4;
            m_minFieldOfView = config.MinFieldOfView * Number.EN4;
            m_maxFieldOfView = config.MaxFieldOfView * Number.EN4;
            m_zValue = config.Depth * Number.EN4;
            m_dumpRatio = config.DumpRatio * Number.EN4;


            m_position = Vector.zero;
            m_fieldOfView = m_maxFieldOfView;

            CalcViewportRect();

            return this;
        }

      
        private Vector GetCenter(Vector[] targetPosArray)
        {
            Vector sum = Vector.zero;
            int count = 0;
            foreach (var pos in targetPosArray)
            {
                sum += pos;
                count++;
            }
            return sum / count;
        }

        /*
        /// <summary>
        /// 支持随人物距离变化动态变化视口大小
        /// </summary>
        /// <returns></returns>
        private Number CalcFieldOfView()
        {
            var dist = GetCharacterDist();
            Number filedOfView = Math.Lerp(m_minFieldOfView,m_maxFieldOfView, Math.Abs(dist) / m_maxCharacterDist);
            return filedOfView;
        }
        */

        public void Update(Vector[] targetPosArray)
        {
            var targetCenter = GetCenter(targetPosArray);
            targetCenter.y = targetCenter.y + m_yOffset;
            m_position = Vector.Lerp(m_position, targetCenter, m_dumpRatio);
            CalcViewportRect();
        }

        private Rect CalcViewportRect()
        {
            Number h = Math.Tan(m_fieldOfView / 2 / 180 * Math.Pi) * Math.Abs(m_zValue) * 2;
            Number w = Aspect * h;
            m_viewPort.position = new Vector(m_position.x, m_position.y);
            m_viewPort.width = w;
            m_viewPort.height = h;
            return m_viewPort;
        }
    }
}
