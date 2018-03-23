using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public interface IHealth
    {
        int GetHP();
        int GetMaxHP();
        void AddHP(int hpAdd);
        void SetHP(int hp);
    }
}
