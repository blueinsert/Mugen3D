using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core {
    /// <summary>
    /// 角色状态基类
    /// </summary>
    public class StateBase
    {
        public const int StateNo_Stand = 0;
        public const int StateNo_Walk = 1;
        public const int StateNo_JumpStart = 2;
        public const int StateNo_JumpUp = 3;
        public const int StateNo_JumpDown = 4;
        public const int StateNo_JumpLand = 5;

        public virtual void OnEnter(Entity e) { 
        }

        public virtual void OnExit(Entity e) { 
        }

        public virtual void OnUpdate(Entity e)
        {

        }
    }
}
