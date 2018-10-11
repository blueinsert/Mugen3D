using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public enum CameraMode
    {
        FollowTarget,
        Free,
    }

    public class CameraController
    {
        private Character m_target1;
        private Character m_target2;
        public CameraMode mode { get; private set; }
        public CameraConfig config {get; private set;}
        public Vector position { get; private set; }
        public Rect viewportRect { get; private set; }

        public CameraController(CameraConfig config, Character char1, Character char2)
        {
            this.config = config;
            m_target1 = char1;
            m_target2 = char2;
            SetMode(CameraMode.FollowTarget);
            position = new Vector((m_target1.position.x + m_target2.position.x) / 2, (m_target1.position.y + m_target2.position.y) / 2 + config.yOffset, config.depth);
            viewportRect = new Rect(Vector.zero, 1, 1); CalcViewportRect();
        }

        public void SetMode(CameraMode mode)
        {
            this.mode = mode;
        }

        public void Update(Number deltaTime)
        {
            if (this.mode == CameraMode.FollowTarget)
            {
                if (m_target1 == null || m_target2 == null)
                    return;
                Vector newPos = new Vector((m_target1.position.x + m_target2.position.x) / 2, (m_target1.position.y + m_target2.position.y) / 2 + config.yOffset, config.depth);
                position = Vector.Lerp(position, newPos, deltaTime * config.dumpRatio);
                viewportRect.position = new Vector(position.x, position.y, 0);
            }
        }

        private void CalcViewportRect()
        {
            Number h = Math.Tan(config.fieldOfView / 2 / 180 * Math.Pi) * Math.Abs(config.depth) * 2;
            Number w = config.depth * h;
            viewportRect.position = new Vector(position.x, position.y, 0);
            viewportRect.width = w;
            viewportRect.height = h;
        }    
       
    }
}
