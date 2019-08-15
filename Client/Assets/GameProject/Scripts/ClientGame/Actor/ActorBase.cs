using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class ActorBase
    {
        protected GameObject m_prefabInstance;

        public void SetPosition(float x, float y)
        {
            m_prefabInstance.transform.position = new Vector3(x, y, 0);
        }

        public void SetFacing(int facing)
        {
            m_prefabInstance.transform.localScale = new Vector3(facing > 0 ? 1 : -1, 1, 1);
        }
    }
}
