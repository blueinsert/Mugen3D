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
        public CameraController cameraController {get; private set;}
        public Action<Event> onEvent;
        private bool isPause = false;
        public MatchManager matchManager;

        public World(WorldConfig cfg)
        {
            config = cfg; 
            entities = new List<Entity>();
            cameraController = new CameraController(cfg.stageConfig.cameraConfig);
            
        }

        public void Pause()
        {
            isPause = true;
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
            if (c.isLocal)
            {
                this.localPlayer = c;
            }
            cameraController.SetFollowTarget(c.slot, c);
            AddEntity(c);
        }

        public void RemoveCharacter(int slot)
        {
            var c = characters[slot];
            characters.Remove(slot);
            entities.Remove(c);
        }

        public void ChangeCharacter(Character c)
        {
            RemoveCharacter(c.slot);
            AddCharacter(c);
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
                e.OnUpdate(Time.deltaTime);
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
        private void GetHitResults() {
            hitResults.Clear();
            foreach (var e1 in entities)
            {
                foreach (var e2 in entities)
                {
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
                    var clsns1 = attacker.animCtr.curActionFrame.clsns;
                    var clsns2 = target.animCtr.curActionFrame.clsns;
                    if (clsns1 != null && clsns2 != null && clsns1.Count != 0 && clsns2.Count != 0)
                    {
                        foreach (var attackClsn in clsns1)
                        {
                            foreach (var defenseClsn in clsns2)
                            {
                                if (attackClsn.type == 2 && defenseClsn.type == 1)
                                {
                                    Vector center = new Vector((attackClsn.x1 + attackClsn.x2) / 2, (attackClsn.y1 + attackClsn.y2) / 2, 0);
                                    center.x = center.x * attacker.GetFacing();
                                    center += attacker.position;
                                    Core.Rect rect1 = new Core.Rect(center, Math.Abs(attackClsn.x1 - attackClsn.x2), Math.Abs(attackClsn.y1 - attackClsn.y2));
                                    center = new Vector((defenseClsn.x1 + defenseClsn.x2) / 2, (defenseClsn.y1 + defenseClsn.y2) / 2, 0);
                                    center.x = center.x * target.GetFacing();
                                    center += target.position;
                                    Core.Rect rect2 = new Core.Rect(center, Math.Abs(defenseClsn.x1 - defenseClsn.x2), Math.Abs(defenseClsn.y1 - defenseClsn.y2));
                                    if (rect1.IsOverlap(rect2))
                                    {
                                        hitResults[target] = attacker;
                                    }
                                }
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
                    if (isBeGuarded)
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

        void UpdateLuaScripts()
        {
            foreach (var e in entities)
            {
                if (e is Unit)
                {
                    var u = e as Unit;
                    u.UpdateScript();
                }
            }
        }
        void PushTest() { 

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

        void UpdateAfterScriptUpdate()
        {
            foreach (var e in entities)
            {
                if (e is Unit)
                {
                    var u = e as Unit;
                    u.UpdateAfterScriptUpdate();
                }
            }
        }
       
        public void Update()
        {
            if (isPause)
                return;
            cameraController.Update();
            EntityUpdate();
            UpdateLuaScripts();     //change state, change anim, so on...  
            HitResolve();

            Debug();
            UpdateView();

            UpdateAfterScriptUpdate();
        }
        
    }
}
