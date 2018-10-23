using System.Collections;
using System.Collections.Generic;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class CharacterMoveCtrl : MoveCtrl
    {
        private Character m_character;

        public CharacterMoveCtrl(Character c):base(c)
        {
            m_character = c;
        }

        protected override void PushTest()
        {
            Number maxPlayerDist = m_character.world.config.stageConfig.cameraConfig.maxPlayerDist;

            var pos = m_character.position;
            var newPos = pos + m_deltaPos;
            var p2Dist = m_character.GetP2Dist();
            var worldConfig = m_owner.world.config;

            //map border
            if(newPos.y < worldConfig.stageConfig.borderYMin)
                justOnGround = true;
            newPos.x = Math.Clamp(newPos.x, worldConfig.stageConfig.borderXMin, worldConfig.stageConfig.borderXMax);
            newPos.y = Math.Clamp(newPos.y, worldConfig.stageConfig.borderYMin, worldConfig.stageConfig.borderYMax);
            //max player dist limit
            var center = m_character.position + p2Dist / 2;
            newPos.x = Math.Clamp(newPos.x, center.x - maxPlayerDist / 2, center.x + maxPlayerDist / 2);
           
            m_deltaPos = newPos - pos;

            var enemy = m_owner.world.teamInfo.GetEnemy(m_character);
            bool findIntersect = false;
            foreach (var clsn in m_owner.animCtr.curActionFrame.clsns)
            {
                foreach (var clsn2 in enemy.animCtr.curActionFrame.clsns)
                {
                    if (clsn.type == 1 && clsn2.type == 1)
                    {
                        Vector clsnCenter = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2, 0);
                        clsnCenter.x = clsnCenter.x * m_owner.GetFacing();
                        clsnCenter += m_owner.position;
                        Core.Rect rect1 = new Core.Rect(clsnCenter, Math.Abs(clsn.x1 - clsn.x2), Math.Abs(clsn.y1 - clsn.y2));

                        clsnCenter = new Vector((clsn2.x1 + clsn2.x2) / 2, (clsn2.y1 + clsn2.y2) / 2, 0);
                        clsnCenter.x = clsnCenter.x * enemy.GetFacing();
                        clsnCenter += enemy.position;
                        Core.Rect rect2 = new Core.Rect(clsnCenter, Math.Abs(clsn2.x1 - clsn2.x2), Math.Abs(clsn2.y1 - clsn2.y2));
                        if (rect1.IsOverlap(rect2))
                        {
                            Vector dir = (rect1.position - rect2.position).normalized;
                            Number distX = (rect1.width + rect2.width) / 2 - Math.Abs(rect1.position.x - rect2.position.x);
                            m_deltaPos += new Vector(distX * new Number(9) / new Number(10) * (dir.x > 0 ? 1 : -1), 0, 0);
                        }
                        findIntersect = true;
                        break;
                    }
                }
                if (findIntersect)
                    break;
            }
        }

    }
}
