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
        public CommandSystem(WorldBase world) : base(world) { }

        protected override bool Filter(Entity e)
        {
            return e.GetComponent<CommandComponent>() != null && e.GetComponent<PlayerComponent>()!=null;
        }

        protected override void ProcessEntity(List<Entity> entities)
        {
            //Debug.Log("CommandSystem:ProcessEntity");
            var inputComponent = World.GetSingletonComponent<InputComponent>();
            foreach(var entity in entities)
            {
                var playerComponent = entity.GetComponent<PlayerComponent>();
                int inputCode = inputComponent.GetInputCode(playerComponent.Slot);
                var commandComponent = entity.GetComponent<CommandComponent>();
                commandComponent.Update(inputCode);
                //Debug.Log(string.Format("inputCode:{0} activeCommand:{1}", inputCode, commandComponent.GetActiveCommandName()));
            }
        }

    }
}
