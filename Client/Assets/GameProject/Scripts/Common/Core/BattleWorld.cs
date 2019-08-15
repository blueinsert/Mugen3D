using System;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using FixPointMath;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 输入指令记录
    /// </summary>
    public class InputRecord
    {
        public int frameNum;
        public int playerIndex;
        public int inputCode;
    }

    /// <summary>
    /// 回合状态
    /// </summary>
    public enum RoundState
    {
        PreIntro = 0,
        Intro,//第一回合的出场白
        RoundDeclare,//回合开始
        Fight,//开始战斗
        PreOver,
        Over,
        PostOver,
    }

    /// <summary>
    /// 战斗、比赛状态
    /// </summary>
    public enum BattleState
    {
        None,
        Starting,
        Running,
        Stoping,
        Stoped
    }

    /// <summary>
    /// 战斗模式，玩法
    /// </summary>
    public enum BattleMode
    {
        SinglePlay,
        SingleVS,
        TeamPlay,
        TeamVS,
    }

    public class BattleWorld
    {
        public int BattleNo { get { return m_battleNo; } }
        public BattleState BattleState {get { return m_battleState; } }
        public int RoundNo { get { return m_roundNo; } }
        public RoundState RoundState { get { return m_roundState; } }

        private int m_maxEntityId = 0;
        private readonly List<Entity> m_addedEntities = new List<Entity>();
        private readonly List<Entity> m_destroyedEntities = new List<Entity>();
        private readonly List<Entity> m_entities = new List<Entity>();
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

        /// <summary>
        /// 胜利次数统计
        /// </summary>
        private Dictionary<int, int> winCount = new Dictionary<int, int>();
        private readonly int MAX_WIN_COUNT = 2;

        public Character m_p1;
        public Character m_p2;

        private PhysicsSystem m_physicsEngine;
        private ScriptSystem m_scriptEngine;
        private AnimSystem m_animEngine;
        private CommandSystem m_commandEngine;

        private ConfigDataStage m_stageConfig;
        private ConfigDataCamera m_cameraConfig;
        private ConfigDataCharacter m_p1Config;
        private ConfigDataCharacter m_p2Config;

        /// <summary>
        /// 战斗，比赛场次标号
        /// </summary>
        private int m_battleNo;
        /// <summary>
        /// 战斗模式
        /// </summary>
        private BattleMode m_battleMode;
        /// <summary>
        /// 战斗状态
        /// </summary>
        private BattleState m_battleState;
        /// <summary>
        /// 第几回合
        /// </summary>
        private int m_roundNo;
        /// <summary>
        /// 回合状态
        /// </summary>
        private RoundState m_roundState;

        public int[] m_cacheInputCodes;

        private IBattleWorldListener m_listener;

        private void InitSubSystem()
        {
            cameraController = new CameraController(m_cameraConfig);
            m_physicsEngine = new PhysicsSystem(this);
            m_scriptEngine = new ScriptSystem(this);
            m_animEngine = new AnimSystem(this);
            m_commandEngine = new CommandSystem(this);
        }

        public BattleWorld(List<ConfigDataCommand> configDataCommand, ConfigDataStage stageConfig, ConfigDataCamera cameraConfig, ConfigDataCharacter p1Config, ConfigDataCharacter p2Config, IBattleWorldListener listener)
        {
            m_stageConfig = stageConfig;
            m_cameraConfig = cameraConfig;
            m_p1Config = p1Config;
            m_p2Config = p2Config;
            m_cacheInputCodes = new int[2];
            m_listener = listener;
            InitSubSystem();
        }

        public void CreateCharacters()
        {
            //m_p1 = new Character(m_p1Config.Name, m_p1Config, 0, true);
            m_p1 = new Character(m_p1Config, 0, true, null, "", "", this);
            m_listener.OnCreateCharacter(m_p1);
        }

        #region 改变世界的方法
        /// <summary>
        /// 更新玩家输入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="inputCode"></param>
        public void UpdatePlayerInput(int index, int inputCode)
        {
            m_cacheInputCodes[index] = inputCode;
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        protected void StartBattle()
        {
            m_roundNo = 1;
            m_battleState = BattleState.Starting;
            ChangeRoundState(RoundState.PreIntro);
            m_p1 = new Character(m_p1Config,0,true,null, "","", this);
            m_listener.OnCreateCharacter(m_p1);
            m_p2 = new Character(m_p2Config,1, false, null, "", "", this);
            m_listener.OnCreateCharacter(m_p2);
            AddCharacter(m_p1);
            AddCharacter(m_p2);
            winCount[m_p1.slot] = 0;
            winCount[m_p2.slot] = 0;
            m_listener.OnBattleStart(m_battleNo);
        }

        protected void StopMatch()
        {
            m_battleState = BattleState.Stoping;
            m_listener.OnBattleEnd(m_battleNo);
        }

        #endregion

        #region roundState 更新

        //todo 写入配置表
        private readonly Number FADE_IN_TIME = new Number(1);
        private readonly Number FADE_OUT_TIME = new Number(1);
        private readonly Number ROUND_DECLARATION_TIME = new Number(3);
        private readonly Number PRE_OVER_TIME = new Number(3);
        private readonly Number OVER_TIME = new Number(3);

        private Number m_roundStateTimer = Number.Zero;

        private bool IsCharactersReady()
        {
            return m_p1.fsmMgr.stateNo == 0 && m_p2.fsmMgr.stateNo == 0;
        }

        private bool IsCharactersSteady()
        {
            return (!m_p1.IsAlive() || (m_p1.IsAlive() && m_p1.fsmMgr.stateNo == 0)) && (!m_p2.IsAlive() || (m_p2.IsAlive() && m_p2.fsmMgr.stateNo == 0));
        }

        private bool IsRoundEnd()
        {
            if (!m_p1.IsAlive() || !m_p2.IsAlive())
                return true;
            if (m_roundStateTimer <= 0)
                return true;
            return false;
        }

        private void UpdateRoundState()
        {
            switch (m_roundState)
            {
                case RoundState.PreIntro:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= FADE_IN_TIME)
                        ChangeRoundState(RoundState.Intro);
                    break;
                case RoundState.Intro:
                    if (IsCharactersReady())
                    {
                        ChangeRoundState(RoundState.RoundDeclare);
                    }
                    break;
                case RoundState.RoundDeclare:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= ROUND_DECLARATION_TIME)
                    {
                        ChangeRoundState(RoundState.Fight);
                    }
                    break;
                case RoundState.Fight:
                    m_roundStateTimer -= Time.deltaTime;
                    if (m_roundStateTimer < 0)
                        m_roundStateTimer = 0;
                    if (IsRoundEnd())
                    {
                        ChangeRoundState(RoundState.PreOver);
                    }
                    break;
                case RoundState.PreOver:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= PRE_OVER_TIME && IsCharactersSteady())
                        ChangeRoundState(RoundState.Over);
                    break;
                case RoundState.Over:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= OVER_TIME)
                    {
                        ChangeRoundState(RoundState.PostOver);
                    }
                    break;
                case RoundState.PostOver:
                    m_roundStateTimer += Time.deltaTime;
                    if (m_roundStateTimer >= FADE_OUT_TIME)
                    {
                        StopRound();
                    }
                    break;
            }
        }

        protected void StopRound()
        {
            m_listener.OnRoundEnd(m_roundNo);
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            if(m_roundState == roundState)
            {
                return;
            }
            m_roundStateTimer = 0;
            m_roundState = roundState;
            switch (m_roundState)
            {
                case RoundState.PreIntro:
                    //p1.fsmMgr.ChangeState(0, true);
                    //p2.fsmMgr.ChangeState(0, true);
                    break;
                case RoundState.Intro:
                    //p1.fsmMgr.ChangeState(5900);
                    //p2.fsmMgr.ChangeState(5900);
                    break;
                case RoundState.Fight:
                    //p1.SetCtrl(true);
                    //p2.SetCtrl(true);
                    break;
                case RoundState.PreOver:
                    //if (!p1.IsAlive() || !p2.IsAlive())
                    //{
                        //world.Pause(30);//pause 10 frame on ko
                    //}
                    break;
                case RoundState.Over:
                    /*
                    if (p1.IsAlive())
                    {
                        if (p1 == GetWiner())
                        {
                            p1.fsmMgr.ChangeState(180);
                        }
                        else
                        {
                            p1.fsmMgr.ChangeState(170);
                        }

                    }
                    if (p2.IsAlive())
                    {
                        if (p2 == GetWiner())
                        {
                            p2.fsmMgr.ChangeState(180);
                        }
                        else
                        {
                            p2.fsmMgr.ChangeState(170);
                        }

                    }
                    */
                    break;
            }
        }

        #endregion

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
            m_entities.Add(e);
        }

        private void DoRemoveEntity(Entity e)
        {
            if (onRemoveEntity != null)
                onRemoveEntity(e);
            m_entities.Remove(e);
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
            foreach (var e in m_entities)
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
            for (int m = 0; m < m_entities.Count; m++)
            {
                var e1 = m_entities[m];
                for (int n = 0; n < m_entities.Count; n++)
                {
                    var e2 = m_entities[n];
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
            foreach (var e in m_entities)
            {
                e.SendEvent(new Event() { type = EventType.SampleAnim, data = null });
            }
        }
  
        void PrepareForNextFrame()
        {
            foreach (var e in m_entities)
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
            foreach (var e in this.m_entities)
            {
                if (e is Character || e is Helper)
                {
                    (e as Unit).PrintDebugInfo();
                }
            }
        }

        public void Step()
        {
            if (IsPause())
            {
                m_pauseTime--;
                return;
            }
            for(int i = 0; i < m_cacheInputCodes.Length; i++)
            {
                //todo
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
