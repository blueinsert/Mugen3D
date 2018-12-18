using System;
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
        private int m_maxEntityId = 0;
        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        private List<Entity> entities = new List<Entity>();
        public Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public TeamManager teamInfo = new TeamManager();
        public System.Action<Entity> onAddEntity;
        public System.Action<Entity> onRemoveEntity;
        public Character localPlayer { get; private set; }
        public WorldConfig config { get; private set; }
        public CameraController cameraController { get; private set; }
        public Action<Event> onEvent;
        private bool isPause = false;
        private int m_pauseTime = 0;
        public MatchManager matchManager;

        private PhysicsEngine m_physicsEngine;
        private ScriptEngine m_scriptEngine;
        private AnimEngine m_animEngine;
        private CommandEngine m_commandEngine;

        public World(WorldConfig cfg)
        {
            config = cfg;
            entities = new List<Entity>();
            cameraController = new CameraController(cfg.stageConfig.cameraConfig);
            m_physicsEngine = new PhysicsEngine(this);
            m_scriptEngine = new ScriptEngine(this);
            m_animEngine = new AnimEngine(this);
            m_commandEngine = new CommandEngine(this);
        }

        public bool IsPause()
        {
            return m_pauseTime > 0;
        }

        public void Pause(int time)
        {
            m_pauseTime = time;
        }

        public void Continue()
        {
            isPause = false;
        }

        public void FireEvent(Event evt)
        {
            if (onEvent != null)
            {
                onEvent(evt);
            }
        }

        public void AddEntity(Entity e)
        {
            m_addedEntities.Add(e);
            e.SetEntityId(m_maxEntityId++);
            e.SetWorld(this);
        }

        public void AddCharacter(Character c)
        {
            this.characters.Add(c.slot, c);
            teamInfo.AddCharacter(c);
            //if (c.isLocal)
            //{
            //    this.localPlayer = c;
            // }
            cameraController.SetFollowTarget(c.slot, c);
            AddEntity(c);
        }

        private void RemoveCharacter(Character c)
        {
            characters.Remove(c.slot);
            //entities.Remove(c);
            cameraController.RemoveFollowTarget(c.slot);
            teamInfo.RemoveCharacter(c);
        }

        private void DoAddEntity(Entity e)
        {
            if (onAddEntity != null)
            {
                onAddEntity(e);
            }
            entities.Add(e);
        }

        private void DoRemoveEntity(Entity e)
        {
            if (onRemoveEntity != null)
                onRemoveEntity(e);
            entities.Remove(e);
            if (e is Projectile)
            {
                var proj = e as Projectile;
                proj.owner.RemoveProj(proj);
            }
            else if (e is Helper)
            {
                var h = (e as Helper);
                h.owner.RemoveHelper(h);
            }
            else if (e is Character)
            {
                RemoveCharacter(e as Character);
            }
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
                e.OnUpdate();
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

        private Dictionary<Unit, Unit> hitResults = new Dictionary<Unit, Unit>(10);
        private void GetHitResults()
        {
            hitResults.Clear();
            for (int m = 0; m < entities.Count; m++)
            {
                var e1 = entities[m];
                for (int n = 0; n < entities.Count; n++)
                {
                    var e2 = entities[n];
                    if (e1 == e2)
                        continue;
                    if (!(e1 is Unit) || !(e2 is Unit))
                        continue;
                    var attacker = e1 as Unit;
                    var target = e2 as Unit;
                    if (attacker.GetMoveType() != MoveType.Attack || attacker.GetHitDefData() == null || attacker.GetHitDefData().moveContact == true)
                        continue;
                    if (attacker is Helper && (attacker as Helper).owner == target)
                        continue;
                    var collider1 = attacker.moveCtr.collider;
                    var collider2 = target.moveCtr.collider;
                    for (int i = 0; i < collider1.attackClsnsLength; i++)
                    {
                        var attackClsn = collider1.attackClsns[i];
                        for (int j = 0; j < collider2.defenceClsnsLength; j++)
                        {
                            var defenceClsn = collider2.defenceClsns[j];
                            ContactInfo contactInfo;
                            if (PhysicsUtils.RectColliderIntersectTest(attackClsn, defenceClsn, out contactInfo))
                            {
                                hitResults[target] = attacker;
                            }
                        }
                    }

                }
            }
        }

        private void HitResolve()
        {
            GetHitResults();
            foreach (var hitResult in hitResults)
            {
                var attacker = hitResult.Value;
                var target = hitResult.Key;
                var hitDef = attacker.GetHitDefData();
                if (target.CanBeHit(hitDef))
                {
                    bool isBeGuarded = false;
                    if (target.CanBeGuard(hitDef) && (target.IsGuarding()))
                    {
                        isBeGuarded = true;
                    }
                    if (isBeGuarded && target.GetHP() - hitDef.guardDamage >= 0)
                    {
                        attacker.OnMoveGuarded(target);
                        target.OnGuardHit(hitDef);
                    }
                    else
                    {
                        attacker.OnMoveHit(target);
                        target.OnBeHitted(hitDef);
                    }
                    if (!hitResults.ContainsKey(attacker))
                        attacker.Pause(isBeGuarded ? hitDef.guardPauseTime[0] : hitDef.hitPauseTime[0]);
                }
            }
        }

        void UpdateView()
        {
            foreach (var e in entities)
            {
                e.SendEvent(new Event() { type = EventType.SampleAnim, data = null });
            }
        }
  
        void PrepareForNextFrame()
        {
            foreach (var e in entities)
            {
                if (e is Unit)
                {
                    var u = e as Unit;
                    if (u.IsPause())
                    {
                        u.AddPauseTime(-1);
                    }
                }
            }
        }

        void PushTest()
        {

        }

        void Debug()
        {
            foreach (var e in this.entities)
            {
                if (e is Character || e is Helper)
                {
                    (e as Unit).PrintDebugInfo();
                }
            }
        }

        public void Update()
        {
            if (IsPause())
            {
                m_pauseTime--;
                return;
            }
            cameraController.Update();
            m_commandEngine.Update();
            m_scriptEngine.PreUpdate();
            EntityUpdate();
            m_animEngine.Update();
            m_scriptEngine.Update();     //change state, change anim, so on...  
            m_physicsEngine.Update();
            HitResolve();
            Debug();
            UpdateView();
            PrepareForNextFrame();
        }

    }
}
