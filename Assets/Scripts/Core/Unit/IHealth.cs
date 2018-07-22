using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public interface IHealth
    {
        int GetHP();
        int GetMaxHP();
        void AddHP(int hpAdd);
        void SetHP(int hp);
    }
}
