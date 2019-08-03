using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Net
{

    public class ClientNetMgr : MonoBehaviour
    {
        private static ClientNetMgr m_instance;
        public static ClientNetMgr Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject go = new GameObject("__ClientNetMgr");
                    m_instance = go.AddComponent<ClientNetMgr>();
                }
                return m_instance;
            }
        }

        public static void Destroy()
        {
            GameObject.Destroy(m_instance.gameObject);
            m_instance = null;
        }

        public Connection conn = new Connection();

        public void Update()
        {
            conn.Update();
        }

        public bool Connect(string host, int port)
        {
            return conn.Connect(host, port);
        }
        
    }

}