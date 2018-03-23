using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D {
public class Environment {
    private Unit m_unit;

    public Environment(Unit unit)
    {
        m_unit = unit;
    }

    public Vector2 pos
    {
        get
        {
            return Triggers.Instance.Pos(m_unit);
        }
    }
}
}
