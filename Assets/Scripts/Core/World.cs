using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class World
    {
        public Number deltaTime;
        public WorldConfig config;
        private int m_maxEntityId = 0;
        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        public List<Entity> entities { get; private set; }
        public List<Character> characters = new List<Character>();
        public CameraController camCtl { get; private set; }
        public TeamInfo teamInfo = new TeamInfo();
        public System.Action<Entity> onCreateEntity;
        public System.Action<WorldConfig> onCreateWorld;
        public Character localPlayer;
        public int logicFPS;

        public World(WorldConfig cfg, int logicFPS)
        {
            config = cfg;
            this.logicFPS = logicFPS;
            this.deltaTime = new Number(1000 / logicFPS) / new Number(1000);
            entities = new List<Entity>();
            Time.Clear();
        }

        public void CreateWorld()
        {
            if (onCreateWorld != null)
                onCreateWorld(config);
        }

        public void CreateCamera()
        {
            var cameraController = new CameraController(config.stageConfig.cameraConfig, teamInfo.GetCharacter(0), teamInfo.GetCharacter(1));
            AddEntity(cameraController);
            this.camCtl = cameraController;
        }

        public void AddEntity(Entity e)
        {
            m_addedEntities.Add(e);
            if (e is Character)
            {
                Character c = e as Character;
                teamInfo.AddCharacter(c);
                if (c.isLocal)
                {
                    this.localPlayer = c;
                }
                characters.Add(c);
            }
            e.SetEntityId(m_maxEntityId++);
            e.SetWorld(this);
            if (onCreateEntity != null)
            {
                onCreateEntity(e);
            }
        }

        private void DoAddEntity(Entity e)
        {
            entities.Add(e);
        }

        private void DoRemoveEntity(Entity e)
        {
            entities.Remove(e);
            //GameObject.Destroy(e.gameObject);
        }

        public void Clear()
        {
            entities.Clear();
        }

        private void EntityUpdate()
        {
            foreach (var e in m_addedEntities)
            {
                DoAddEntity(e);
            }
            m_addedEntities.Clear();
            foreach (var e in entities)
            {
                e.OnUpdate(deltaTime);
                if (e.isDestroyed)
                {
                    m_destroyedEntities.Add(e);
                }
            }
            foreach (var ent in m_destroyedEntities)
            {
                DoRemoveEntity(ent);
            }
            m_destroyedEntities.Clear();
        }

        private void HitResolve() 
        {
            foreach (var attacker in characters)
            {
                foreach (var target in characters)
                {
                    if (attacker == target)
                        continue;
                    if (attacker.status.moveType != MoveType.Attack)
                        continue;
                    var clsns1 = attacker.animCtr.curActionFrame.clsns;
                    var clsns2 = attacker.animCtr.curActionFrame.clsns;
                    if (clsns1 != null && clsns2 != null && clsns1.Count != 0 && clsns2.Count != 0)
                    {
                        foreach (var attackClsn in clsns1)
                        {
                            foreach (var defenseClsn in clsns2)
                            {
                                if (attackClsn.type == 2 && defenseClsn.type == 1)
                                {
                                    Vector center = new Vector((attackClsn.x1 + attackClsn.x2) / 2, (attackClsn.y1 + attackClsn.y2) / 2, 0);
                                    center.x = center.x * attacker.facing;
                                    center += attacker.position;
                                    Core.Rect rect1 = new Core.Rect(center, Math.Abs(attackClsn.x1 - attackClsn.x2), Math.Abs(attackClsn.y1 - attackClsn.y2));
                                    center = new Vector((defenseClsn.x1 + defenseClsn.x2) / 2, (defenseClsn.y1 + defenseClsn.y2) / 2, 0);
                                    center.x = center.x * target.facing;
                                    center += target.position;
                                    Core.Rect rect2 = new Core.Rect(center, Math.Abs(defenseClsn.x1 - defenseClsn.x2), Math.Abs(defenseClsn.y1 - defenseClsn.y2));
                                    if (rect1.IsOverlap(rect2))
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Update()
        {
            Time.Update(deltaTime);
            EntityUpdate();
            HitResolve();
        }
 
    }
}
