using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;
namespace Mugen3D
{
    public class World
    {  
        public static World mInstance;
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                }
                return mInstance;
            }
        }

        private World()
        {
            Init();
        }

        private void Init() {
            collisionWorld = new CollisionWorld();
        }

        public CollisionWorld collisionWorld;
        public int gameTime = -1;
        public float deltaTime;
        public List<Entity> entities
        {
            get{
                return m_entities;
            }
        }

        private List<Entity> m_addedEntities = new List<Entity>();
        private List<Entity> m_destroyedEntities = new List<Entity>();
        private List<Entity> m_entities = new List<Entity>();

        private List<Character> m_players = new List<Character>();

        public void AddEntity(Entity e)
        {
            m_addedEntities.Add(e);
        }

        private void DoAddEntity(Entity e){
            m_entities.Add(e);
            collisionWorld.AddCollider((e as Unit).GetCollider());
            if (e is Character)
            {
                m_players.Add(e as Character);
            }
        }

        private void DoRemoveEntity(Entity e)
        {
            m_entities.Remove(e);
            //collisionWorld.RemoveCollideable(e);
            if (e is Character)
            {
                m_players.Remove(e as Character);
            }
            GameObject.Destroy(e.gameObject);
        }

        public void Clear()
        {
            m_entities.Clear();
            m_players.Clear();
        }

        public Character GetPlayer(PlayerId id)
        {
            var players = m_players.FindAll((p) => { return p.id == id; });
            Utility.Assert(players.Count == 1, "more than one player of id:" + id.ToString());
            return players[0];
        }

        public List<Character> GetAllPlayers()
        {
            return m_players;
        }

        public void Update(float _deltaTime)
        {
            gameTime++;
            deltaTime = _deltaTime;

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

            //UpdateFacing();
           
        }

        /*
        private void UpdateFacing()
        {
            var p1 = GetPlayer(PlayerId.P1);
            var p2 = GetPlayer(PlayerId.P2);
            if (p1 == null || p2 == null)
                return;
            if (p1.transform.position.x > p2.transform.position.x)
            {
                p1.ChangeFacing(-1);
                p2.ChangeFacing(1);
            }
            else
            {
                p1.ChangeFacing(1);
                p2.ChangeFacing(-1);
            }
        }
         */
    }
}
