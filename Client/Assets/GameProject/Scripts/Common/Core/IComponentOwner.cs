using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public interface IComponentOwner
    {
        T GetComponent<T>() where T : ComponentBase;

        T AddComponent<T>() where T : ComponentBase,new();
    }
}
