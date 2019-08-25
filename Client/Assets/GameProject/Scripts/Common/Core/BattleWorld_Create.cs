using bluebean.UGFramework.ConfigData;
using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public partial class BattleWorld : WorldBase
    {
        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="configDataCharacter"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Entity AddCharacter(ConfigDataCharacter configDataCharacter, int index)
        {
            var entity = AddEntity();
            entity.AddComponent<MoveComponent>();
            entity.AddComponent<PlayerComponent>().Init(index, configDataCharacter);
            entity.AddComponent<CommandComponent>().Init();
            return entity;
        }
    }
}
