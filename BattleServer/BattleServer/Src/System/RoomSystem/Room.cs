using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleServer
{
    public enum RoomStatus
    {
        Wait,
        Loading,
        Battling,
    }

    public class Room
    {
        public const int MAX_PLAYER_NUM = 2;
        public const int FPS = 60;
        private int deltaTimeMS = 1000 / FPS;
        public int id { get; private set; }
        private List<Player> m_players = new List<Player>();
        private Dictionary<Player, int> m_gameLoadProgress = new Dictionary<Player, int>();
        public RoomStatus status { get; private set; }

        private int m_currentFrame = 0;
        private Dictionary<Player, Dictionary<int, int>> m_inputs = new Dictionary<Player, Dictionary<int, int>>();
        
        private System.Timers.Timer timer;

        public Room(int id)
        {
            this.id = id;
            status = RoomStatus.Wait;
        }

        public bool IsFull()
        {
            return m_players.Count >= MAX_PLAYER_NUM;
        }

        public void SetStatus(RoomStatus status){
            this.status = status;
        }

        public void AddPlayer(Player p)
        {
            m_players.Add(p);
            p.curRoom = this;
            if (m_players.Count >= MAX_PLAYER_NUM)
            {
                CreateMatch();
            }
        }

        public void RemovePlayer(Player p)
        {
            m_players.Remove(p);
            p.curRoom = null;
        }

        private void CreateMatch()
        {
            Console.WriteLine("room " + this.id + " CreateMatch");
            foreach (var p in m_players)
            {
                m_gameLoadProgress.Add(p, 0);
            }         
            Protocol.ProtocolBytes proto = new Protocol.ProtocolBytes();
            proto.AddString("MatchCreate");
            proto.AddInt(id);
            Broadcast(proto);
            this.status = RoomStatus.Loading;
            //listening to progress
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += ListenLoadProgress;
            timer.AutoReset = false;
            timer.Enabled = true; 
        }

        private bool CheckLoadProgress()
        {
            bool res = true;
            foreach (var p in m_gameLoadProgress)
            {
                if (p.Value < 100)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

        private void ListenLoadProgress(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Console.WriteLine("room " +this.id + "ListenLoadProgress");
            if(CheckLoadProgress()){
                timer.Elapsed -= ListenLoadProgress;
                timer = null;
                StartGame();
            }
            else
            {
                timer.Start();
            }
        }

        private void StartGame()
        {
            Console.WriteLine("room " +this.id + " StartGame");
            Protocol.ProtocolBytes proto = new Protocol.ProtocolBytes();
            proto.AddString("GameStart");
            Broadcast(proto);
            foreach (var p in m_players)
            {
                m_inputs.Add(p, new Dictionary<int, int>());
            }         
            this.status = RoomStatus.Battling;
           // timer = new System.Timers.Timer(1);
            //timer.Elapsed += HandleTimerGameUpdate;
            //timer.AutoReset = true;
            //timer.Enabled = true;
        }

        public void EndGame()
        {
            Console.WriteLine("room " + this.id + " EndGame");
            //this.timer.Stop();
            //this.timer = null;
            this.status = RoomStatus.Wait;
            Protocol.ProtocolBytes proto = new Protocol.ProtocolBytes();
            proto.AddString("GameEnd");
            Broadcast(proto);
            lock (m_players)
            {
                foreach (var player in m_players.ToArray())
                {
                    RemovePlayer(player);
                }
            }  
        }

        private void HandleTimerGameUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Console.WriteLine("room " +this.id + "HandleTimerGameUpdate");
            //GameUpdate();
        } 

        void GameUpdate()
        {
            //Console.WriteLine("GameUpdate");
            Protocol.ProtocolBytes proto = new Protocol.ProtocolBytes();
            proto.AddString("GameUpdate");
            proto.AddInt(m_currentFrame);
            lock (m_players)
            {
                for (int i = 0; i < m_players.Count; i++)
                {
                    var player = m_players[i];
                    proto.AddInt(player.curInput);
                    m_inputs[player].Add(m_currentFrame, player.curInput);
                }
                Broadcast(proto);
            }
            m_currentFrame++;    
        }

        long TickToMilliSec(long tick)
        {
            return tick / (10 * 1000);
        }

        long m_updateTime = 0;
        public void Update()
        {
            if (status == RoomStatus.Battling)
            {
                long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
                //Console.WriteLine("curTime:" + nCurTime);
                if (nCurTime - m_updateTime > deltaTimeMS)
                {
                    GameUpdate();
                    m_updateTime = nCurTime;
                }
            }
        }
        
        public void UpdateGameLoadProgress(Player p, int progress)
        {
            lock (m_gameLoadProgress)
            {
                m_gameLoadProgress[p] = progress;
            }
        }

        public void UpdateInput(Player p, int input)
        {
            p.curInput = input;
        }

        //广播
        private void Broadcast(Protocol.ProtocolBase protocol)
        {
            foreach (var p in m_players)
            {
                lock (p.conn)
                {
                    if (p.conn != null && p.conn.isUse)
                        p.conn.Send(protocol);
                }
            }
        }
    }
}