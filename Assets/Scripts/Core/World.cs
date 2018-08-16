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
        public int gameTime = -1;
        public Number deltaTime;
        public WorldConfig config;
        private int m_maxEntityId = 0;
        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        public List<Entity> entities  { get; private set; }
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
            this.deltaTime = new Number(1000/logicFPS) / new Number(1000);
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

        private void DoAddEntity(Entity e){
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

        public void Update()
        {
            Time.Update(deltaTime);
            gameTime++;
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

    }
}
