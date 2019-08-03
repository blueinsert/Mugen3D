using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{

    [Serializable]
    public class AssetObject : ScriptableObject
    {
        [SerializeField]
        public Byte[] m_bytes;

        [SerializeField]
        public string m_MD5;
    }

}
