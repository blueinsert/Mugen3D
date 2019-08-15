using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 按键修饰符
    /// </summary>
    public enum KeyModifier
    {
        /// <summary>
        /// 必须被按下
        /// </summary>
        KeyMode_Must_Be_Held,
        /// <summary>
        /// 模糊侦测，有周围按键按下也可
        /// </summary>
        KeyMode_Detect_As_4Way,
        /// <summary>
        /// 必须没有其他键码的输入
        /// </summary>
        KeyMode_Ban_Other_Input,
        /// <summary>
        /// 松开键码的时候成立
        /// </summary>
        KeyMode_On_Release,
    }

    /// <summary>
    /// 按键名枚举
    /// </summary>
    public enum KeyNames
    {
        KEY_UP = 0,
        KEY_DOWN,
        KEY_LEFT,
        KEY_RIGHT,
        KEY_BUTTON_A,
        KEY_BUTTON_B,
        KEY_BUTTON_C,
        KEY_BUTTON_X,
        KEY_BUTTON_Y,
        KEY_BUTTON_Z,
        KEY_BUTTON_START,
        KEY_BUTTON_PAUSE,
        KEY_COUNT
    };

    /// <summary>
    /// 指令系统
    /// </summary>
    public class CommandSystem : SystemBase
    {
        private List<CmdManager> cmdMgrs = new List<CmdManager>();

       public CommandSystem(BattleWorld world):base(world)
       {

       }

        protected override void OnAddEntity(Entity e)
        {
            if(e is Character)
            {
                var c = e as Character;
                this.cmdMgrs.Add(c.cmdMgr);
            }
        }

        protected override void OnRemoveEntity(Entity e)
        {
            if (e is Character)
            {
                var c = e as Character;
                this.cmdMgrs.Remove(c.cmdMgr);
            }
        }

        public override void Update()
        {
            foreach(var cmdMgr in this.cmdMgrs)
            {
                cmdMgr.Update(cmdMgr.owner.input);
            }
        }

        protected override void ProcessEntity(IComponentOwner componentOwner)
        {
            var commandComponent = componentOwner.GetComponent<CommandComponent>();
            if (commandComponent == null)
                return;
        }
    }
}
