using System.Collections;
using System.Collections.Generic;
using FixPointMath;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    public class StageComponent : ComponentBase
    {
        #region 单例模式
        public static StageComponent Instance { get { return m_instance; } }
        private static StageComponent m_instance;
        public static StageComponent CreateInstance() { m_instance = new StageComponent(); return m_instance; }
        #endregion

        public Number BorderXMin { get { return m_BorderXMin; } }

        public Number BorderXMax { get { return m_BorderXMax; } }

        public Number BorderYMin { get { return m_BorderYMin; } }

        public Number BorderYMax { get { return m_BorderYMax; } }

        public Vector P1InitPos { get { return m_P1InitPos; } }

        public Vector P2InitPos { get { return m_P2InitPos; } }

        private Number m_BorderXMin;

        private Number m_BorderXMax;

        private Number m_BorderYMin;

        private Number m_BorderYMax;

        private Vector m_P1InitPos;

        private Vector m_P2InitPos;

        public StageComponent(WorldBase world) : base(world)
        {

        }

        public void Init(ConfigDataStage configDataStage)
        {
            m_BorderXMin = configDataStage.BorderXMin * Number.EN4;
            m_BorderXMax = configDataStage.BorderXMax * Number.EN4;
            m_BorderYMin = configDataStage.BorderYMin * Number.EN4;
            m_BorderYMax = configDataStage.BorderYMax * Number.EN4;
            m_P1InitPos = new Vector(configDataStage.P1InitPos[0] * Number.EN4, configDataStage.P1InitPos[1] * Number.EN4);
            m_P2InitPos = new Vector(configDataStage.P2InitPos[0] * Number.EN4, configDataStage.P2InitPos[1] * Number.EN4);
        }
    }
}
