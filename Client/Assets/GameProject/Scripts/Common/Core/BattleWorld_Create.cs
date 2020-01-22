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
            entity.AddComponent<BasicInfoComponent>().Init(index, configDataCharacter);
            entity.AddComponent<CommandComponent>().Init();
            var actionDefStr = sFileReader(configDataCharacter.ActionDef);
            var acttionDefs = ConfigReader.Parse<List<ActionDef>>(actionDefStr);
            entity.AddComponent<AnimationComponent>().Init(acttionDefs.ToArray());
            entity.AddComponent<FSMComponent>().Initialize();
            entity.AddComponent<PhysicsComponent>();
            //暂时注释脚本系统，待最小可行模型验证通过后再逐步加入
            //entity.AddComponent<LuaScriptComponent>().Init(configDataCharacter.LuaMainModule, entity);
            //entity.AddComponent<AnimationComponent>().Init(); todo
            //entity.AddComponent<FSMComponent>();
            entity.AddComponent<DelayImpactComponent>();
            m_listener.OnCreateCharacter(entity);
            return entity;
        }
    }
}
