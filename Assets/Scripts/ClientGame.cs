using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum PlayMode
    {
        Training,
        SingleVS,
    }

    public class ClientGame : MonoBehaviour
    {
        public static ClientGame Instance;

        public PlayMode playMode;

        public Mugen3D.World world;
      
        private bool isInited = false;

        private static readonly List<Vector3> m_initPos = new List<Vector3> { 
            new Vector3(-1.5f, 0, 0),
            new Vector3(1.5f, 0, 0),
        };

        public void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
            world = Mugen3D.World.Instance;
        }

        public void OnDestroy()
        {
            Instance = null;
        }

        public void CreateGame(string p1CharacterName, string p2CharacterName, string stageName, PlayMode playMode = PlayMode.Training)
        {
            this.playMode = playMode;

            UnityEngine.Object prefabStage = Resources.Load<UnityEngine.Object>("Stage/" + stageName + "/" + stageName);
            GameObject goStage = GameObject.Instantiate(prefabStage, this.transform.Find("Stage")) as GameObject;
            var p1 = Mugen3D.EntityLoader.LoadPlayer(0, p1CharacterName, this.transform.Find("Players"), m_initPos[0]);
            var p2 = Mugen3D.EntityLoader.LoadPlayer(1, p2CharacterName, this.transform.Find("Players"), m_initPos[1]);
            p1.transform.localPosition = m_initPos[0];
            p2.transform.localPosition = m_initPos[1];
            var cameraController = new Mugen3D.CameraController(GetComponentInChildren<Camera>(), p1.transform, p2.transform);
            world.camCtl = cameraController;
            isInited = true;
        }

        void Update()
        {
            if (!isInited)
                return;
            Mugen3D.World.Instance.Update(Time.deltaTime);
        }
    }
}
