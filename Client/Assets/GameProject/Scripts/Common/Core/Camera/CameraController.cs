using System.Collections;
using System.Collections.Generic;
using bluebean.UGFramework.ConfigData;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class CameraController 
    {
        public ConfigDataCamera config { get; private set; }

        private Dictionary<int, Character> targets = new Dictionary<int, Character>();
        private Number m_maxCharacterDist;
        private Vector m_targetCenter;
        public Vector targetCenter { get { return m_targetCenter; } }
        private Vector m_position = Vector.zero;
        public Vector position { get { return m_position; } }
        public Number m_fieldOfView;
        public Number fieldOfView { get { return m_fieldOfView; } }
        public Number m_rotationX;
        public Number rotationX { get { return m_rotationX; } }
        public Number aspect { get; private set; }
        public Rect viewPort { get; private set; }

        public CameraController(ConfigDataCamera config)
        {
            this.config = config;
            m_position.y = config.Yoffset;
            //m_position.z = config.Depth;
            aspect = config.Aspect;
            viewPort = new Rect();
            m_rotationX = 0;
            m_targetCenter = Vector.zero;
            m_maxCharacterDist = Math.Tan(config.MaxFieldOfView / 2 / 180 * Math.Pi) * Math.Abs(config.Depth) * 2 * config.Aspect;
            m_fieldOfView = config.MaxFieldOfView;
            CalcViewportRect();
        }

        public void SetFollowTarget(int slot, Character character)
        {
            targets[slot] = character;
        }

        public void RemoveFollowTarget(int slot)
        {
            targets.Remove(slot);
        }

        Vector GetCenter()
        {
            Vector sum = Vector.zero;
            int count = 0;
            foreach (var pair in targets)
            {
                sum += pair.Value.Position;
                count++;
            }
            return sum / count;
        }

        Number GetCharacterDist()
        {
            Number xMax = Number.MinValue;
            Number xMin = Number.MaxValue;
            foreach (var pair in targets)
            {
                var character = pair.Value;
                if (character.Position.x > xMax)
                {
                    xMax = character.Position.x;
                }
                if (character.Position.x < xMin)
                {
                    xMin = character.Position.x;
                }
            }
            return xMax - xMin;
        }

        private Number CalcFieldOfView()
        {
            var dist = GetCharacterDist();
            Number filedOfView = Math.Lerp(config.MinFieldOfView, config.MaxFieldOfView, Math.Abs(dist) / m_maxCharacterDist);
            return filedOfView;
        }

        public void Update()
        {
            m_targetCenter = GetCenter();
            m_position.x = m_targetCenter.x;
            m_fieldOfView = CalcFieldOfView();
            CalcViewportRect();
        }
        
        private Rect CalcViewportRect()
        {
            Number h = Math.Tan(m_fieldOfView / 2 / 180 * Math.Pi) * Math.Abs(config.Depth) * 2;
            Number w = aspect * h;
            viewPort.position = new Vector(m_position.x, m_position.y);
            viewPort.width = w;
            viewPort.height = h;
            return viewPort;
        }      
        
    
    }
}