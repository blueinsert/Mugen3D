using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class CameraController : Entity
    {
        private Character m_target1;
        private Character m_target2;  
        private Number dumpRatio = 10;
        private Number yOffset = 1;
        public Number fieldOfView;
        public Number depth;
        public Number aspect;

        public Rect viewportRect;

        public CameraController(CameraConfig config, Character char1, Character char2)
        {
            this.yOffset = config.yOffset;
            this.fieldOfView = config.fieldOfView;
            this.depth = config.depth;
            this.aspect = config.aspect;

            m_target1 = char1;
            m_target2 = char2;
            viewportRect = new Rect(Vector.zero, 1, 1);
            position = new Vector((m_target1.position.x + m_target2.position.x) / 2, (m_target1.position.y + m_target2.position.y) / 2 + yOffset, depth);
            CalcViewportRect();
        }

        public override void OnUpdate(Number deltaTime)
        {
            if (m_target1 == null || m_target2 == null)
                return;
            Vector newPos = new Vector((m_target1.position.x + m_target2.position.x) / 2, (m_target1.position.y + m_target2.position.y) / 2 + yOffset, depth);
            position = Vector.Lerp(position, newPos, deltaTime * dumpRatio);
            CalcViewportRect();
        }

        private void CalcViewportRect()
        {
            Number h = Math.Tan(fieldOfView / 2 / 180 * Math.Pi) * Math.Abs(depth) * 2;
            Number w = aspect * h;
            viewportRect.position = new Vector(position.x, position.y, 0);
            viewportRect.width = w;
            viewportRect.height = h;
        }
       
    }
}
